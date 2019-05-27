using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Linq;

namespace DbFillingTool
{
    public class Helper
    {

        public static string[] GetPropertyValues(object obj)
        {
            if (Check(obj))
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
            try
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
            catch
            {
                throw new Exception($"Unable to get id from {tableName}");
            }
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

        public static ExcelFileRow[] GetExcel(string path)
        {
            try
            {
                ExcelQueryFactory factory;
                if (path.Contains(".xlsx")) factory = new ExcelQueryFactory(path);
                else factory = new ExcelQueryFactory(path + @"\" + Path.GetFileName(path) + ".xlsx");
                 
                ExcelFileRow[] file = (from row in factory.Worksheet<ExcelFileRow>(0)
                                       orderby row.Name
                                       select row).ToList().DeleteNulls();
                return file;
            }
            catch
            {
                throw new Exception($"Unable to get file {path}");
            }
        }

        public static FileInfo[] GetPictures(string path)
        {
            FileInfo[] pictures;
            try
            {
                string currentPath = path;
                DirectoryInfo picturesInfo = new DirectoryInfo(currentPath += @"\pictures");
                pictures = picturesInfo.GetFiles();
            }
            catch
            {
                Console.WriteLine("Directory has no pictures");
                pictures = Array.Empty<FileInfo>();
            }
            return pictures;
        }

        public static DirectoryInfo[] GetSubDirs(string path)
        {
            try
            {
                DirectoryInfo rootDir = new DirectoryInfo(path);
                DirectoryInfo[] subDirEntries = rootDir.GetDirectories();
                subDirEntries = subDirEntries.Where(dir => Path.GetFileName(dir.FullName) != "pictures").ToArray();
                return subDirEntries;
            }
            catch
            {
                throw new Exception($"Unable to get subdirectories from {path}");
            }
        }
    }
}
