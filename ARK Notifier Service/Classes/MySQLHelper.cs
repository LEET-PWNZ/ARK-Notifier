using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ARKNotifierService.Classes
{
    public class MySQLHelper
    {
        public static CustomDataTable ExecuteQueryReader(string Query,string[] paramNames=null,object[] paramValues=null,bool isStoredProcedure=false)
        {
            CustomDataTable result = new CustomDataTable();
            MySqlConnection conn =null;
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;
            try
            {
                conn = new MySqlConnection(ConfigurationManager.AppSettings["MySqlConnectionString"]);
                conn.Open();
                cmd = new MySqlCommand(Query, conn);
                cmd.CommandTimeout = 0;
                if (isStoredProcedure) cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (paramNames != null && paramValues != null)
                {
                    if (paramNames.Length != paramValues.Length)
                    {
                        throw new Exception("Command parameter names and values have different lengths.");
                    }
                    else
                    {
                        for (int i = 0; i < paramNames.Length; i++)
                        {
                            cmd.Parameters.AddWithValue(paramNames[i], paramValues[i]);
                        }
                    }
                }
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CustomDataRow row = new CustomDataRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            result.ColumnNames.Add(reader.GetName(i));
                            CustomDataColumn column = new CustomDataColumn();
                            column.Value = reader[i];
                            row.Columns.Add(column);
                        }
                        result.Rows.Add(row);
                    }
                }
            }
            catch (Exception){}
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return result;
        }

        public static bool ExecuteQuery(string query, string[] paramNames = null, object[] paramValues = null, bool isStoredProcedure = false)
        {
            bool result = false;
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            try
            {
                conn = new MySqlConnection(ConfigurationManager.AppSettings["MySqlConnectionString"]);
                conn.Open();
                cmd = new MySqlCommand(query, conn);
                cmd.CommandTimeout = 0;
                if (isStoredProcedure) cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (paramNames != null && paramValues != null)
                {
                    if (paramNames.Length != paramValues.Length)
                    {
                        throw new Exception("Command parameter names and values have different lengths.");
                    }
                    else
                    {
                        for (int i = 0; i < paramNames.Length; i++)
                        {
                            cmd.Parameters.AddWithValue(paramNames[i], paramValues[i]);
                        }
                    }
                }
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception){}
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }

    }

    public class CustomDataTable
    {
        private int CurrentRowIndex = 0;
        private CustomDataRow CurrentRow;
        public bool HasRows { get { return (Rows.Count > 0); } }
        public int FieldCount { get { return ColumnNames.Count; } }
        public int RowCount { get { return Rows.Count; } }

        public object this[string name] { get { return CurrentRow.Columns[ColumnNames.IndexOf(ColumnNames.Find(o => o.Equals(name)))].Value; } }
        public object this[int i] { get { return CurrentRow.Columns[i].Value; } }
        public List<CustomDataRow> Rows = new List<CustomDataRow>();
        public List<string> ColumnNames = new List<string>();

        public bool Read()
        {
            bool result = false;
            if (CurrentRowIndex < RowCount)
            {
                CurrentRow = Rows[CurrentRowIndex];
                result = true;
                CurrentRowIndex++;
            }
            return result;
        }

        public int? GetInt(int i)
        {
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (int?)CurrentRow.Columns[i].Value;
        }
        public int? GetInt(string name)
        {
            int i = ColumnNames.IndexOf(name);
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (int?)CurrentRow.Columns[i].Value;
        }

        public long? GetLong(int i)
        {
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (long?)CurrentRow.Columns[i].Value;
        }
        public long? GetLong(string name)
        {
            int i = ColumnNames.IndexOf(name);
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (long?)CurrentRow.Columns[i].Value;
        }

        public float? GetFloat(int i)
        {
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (float?)CurrentRow.Columns[i].Value;
        }
        public float? GetFloat(string name)
        {
            int i = ColumnNames.IndexOf(name);
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (float?)CurrentRow.Columns[i].Value;
        }

        public double? GetDouble(int i)
        {
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (double?)CurrentRow.Columns[i].Value;
        }
        public double? GetDouble(string name)
        {
            int i = ColumnNames.IndexOf(name);
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (double?)CurrentRow.Columns[i].Value;
        }

        public decimal? GetDecimal(int i)
        {
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (decimal?)CurrentRow.Columns[i].Value;
        }
        public decimal? GetDecimal(string name)
        {
            int i = ColumnNames.IndexOf(name);
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (decimal?)CurrentRow.Columns[i].Value;
        }

        public string GetString(int i)
        {
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (string)CurrentRow.Columns[i].Value;
        }
        public string GetString(string name)
        {
            int i = ColumnNames.IndexOf(name);
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (string)CurrentRow.Columns[i].Value;
        }

        public byte?[] GetBytes(int i)
        {
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (byte?[])CurrentRow.Columns[i].Value;
        }
        public byte?[] GetBytes(string name)
        {
            int i = ColumnNames.IndexOf(name);
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (byte?[])CurrentRow.Columns[i].Value;
        }

        public bool? GetBoolean(int i)
        {
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (bool?)CurrentRow.Columns[i].Value;
        }
        public bool? GetBoolean(string name)
        {
            int i = ColumnNames.IndexOf(name);
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (bool?)CurrentRow.Columns[i].Value;
        }

        public DateTime? GetDateTime(int i)
        {
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (DateTime?)CurrentRow.Columns[i].Value;
        }
        public DateTime? GetDateTime(string name)
        {
            int i = ColumnNames.IndexOf(name);
            if (CurrentRow.Columns[i].Value == DBNull.Value) return null;
            else return (DateTime?)CurrentRow.Columns[i].Value;
        }

    }

    public class CustomDataRow
    {
        public List<CustomDataColumn> Columns = new List<CustomDataColumn>();
    }

    public class CustomDataColumn
    {
        public object Value;
    }

}