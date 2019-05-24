using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DbFillingTool
{
    public class Helper
    {

        public static string[] GetPropertyValues(object obj)
        {
            if (obj != null)
            {
                Type type = obj.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                string[] result = new string[props.Count];
                int i = 0;
                foreach (PropertyInfo prop in props)
                {
                    result[i] = prop.GetValue(obj, null).ToString();
                    i++;
                }

                return result;
            }
            else
                throw new Exception("Object can not be null");

        }
        
        public static int GetId(string tableName, string value, SqlConnection connection)
        {
            int Id = -1;
            using (SqlCommand readerCommand = new SqlCommand())
            {
                readerCommand.Connection = connection;
                readerCommand.CommandText = $"select id from {tableName} where name = @name";
                readerCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = value;
                Id = (int)readerCommand.ExecuteScalar();
                readerCommand.Parameters.Clear();
            }

            return Id;
        }
        
        public static bool Check(params object[] objects)
        {
            bool result = true;
            foreach (object obj in objects)
            {
                if (obj == null) result = false;
            }
            return result;
        }
    }
}
