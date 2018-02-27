using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARKWebNotifier.Classes
{
    public class SecurityTokenManager
    {
        public string DeviceUID;
        public DateTime? LastAccessDate;
        private static string SplitCharacter = "|";

        public string GenerateTokenString()
        {
            string result = DeviceUID + SplitCharacter;
            result += LastAccessDate.ToString();
            result = Globals.Encrypt(result);
            return result;
        }

        public static SecurityTokenManager DecompileTokenString(string compiledToken)
        {
            SecurityTokenManager result = new SecurityTokenManager();
            compiledToken = Globals.Decrypt(compiledToken);
            string[] tempTokenData = compiledToken.Split(SplitCharacter.ToCharArray()[0]);
            result.DeviceUID = tempTokenData[0];
            result.LastAccessDate = DateTime.Parse(tempTokenData[1]);
            return result;
        }
    }
}