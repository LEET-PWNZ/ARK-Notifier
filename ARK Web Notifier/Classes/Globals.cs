using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ARKWebNotifier.Classes
{
    public class Globals
    {
        private static byte[] cryptoSalt = Encoding.ASCII.GetBytes("change_this_salt");
        private static string cryptoSecurityKey = "and_change_this_key";

        public static string Encrypt(string strToEncrypt)
        {
            string result = "";
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(cryptoSecurityKey, cryptoSalt);
            byte[] bytesToEncrypt = Encoding.ASCII.GetBytes(strToEncrypt);
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.KeySize = 256;
                rijndael.BlockSize = 256;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.Key = key.GetBytes(rijndael.KeySize / 8);
                using (MemoryStream x = new MemoryStream())
                {
                    x.Write(BitConverter.GetBytes(rijndael.IV.Length), 0, sizeof(int));
                    x.Write(rijndael.IV, 0, rijndael.IV.Length);
                    using (CryptoStream cstr = new CryptoStream(x, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cstr))
                        {
                            sw.Write(strToEncrypt);
                        }
                        result = Convert.ToBase64String(x.ToArray());
                    }
                }
            }

            return result;
        }

        public static string Decrypt(string strToDecrypt)
        {
            string result = "";
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(cryptoSecurityKey, cryptoSalt);
            byte[] bytesToDecrypt = Convert.FromBase64String(strToDecrypt);
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.KeySize = 256;
                rijndael.BlockSize = 256;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.Key = key.GetBytes(rijndael.KeySize / 8);
                using (MemoryStream x = new MemoryStream(bytesToDecrypt))
                {
                    rijndael.IV = ReadByteArray(x);
                    using (CryptoStream cstr = new CryptoStream(x, rijndael.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cstr))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }
            return result;
        }

        private static byte[] ReadByteArray(Stream stream)
        {
            byte[] rawLength = new byte[sizeof(int)];
            stream.Read(rawLength, 0, rawLength.Length);
            byte[] result = new byte[BitConverter.ToInt32(rawLength, 0)];
            stream.Read(result, 0, result.Length);
            return result;
        }

        public static void ApplyCulture()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-za");
            ci.DateTimeFormat.ShortTimePattern = "HH:mm";
            ci.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            ci.NumberFormat.NumberDecimalSeparator = ".";
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;
        }

        public static string ApplicationPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
    }
}