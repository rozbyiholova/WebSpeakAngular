using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel;

namespace DbFillingTool
{
    public static class FilesHandler
    {
        public static void LoadMainEntities()
        {
            string categoriesPath = @"D:\LanguageSite\DictionaryForFullStack";
            string languagesPath = @"D:\LanguageSite\DictionaryForFullStack\Languages.xlsx";
            string testsPath = @"D:\LanguageSite\DictionaryForFullStack\TestNames.xlsx";


            using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                db.Open();
                LoadLanguages(db, languagesPath);
                LoadCategories(db, categoriesPath);



            }
        }

        private static ExcelFileRow[] DeleteNulls(this List<ExcelFileRow> list)
        {
            return list.Where(r => r.Name != null).ToArray();
        }
        private static string[] GetPropertyValues(object obj)
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
        private static int 

        private static int parentCategoryId { get; set; } = 0;

        private static void LoadLanguages(SqlConnection connection, string path)
        {
            ExcelQueryFactory factory = new ExcelQueryFactory(path);
            ExcelFileRow[] languages = (from row in factory.Worksheet<ExcelFileRow>(0)
                                        select row).ToList().DeleteNulls();
            string[] names = factory.GetColumnNames("Categories").ToArray();

            //table Languages
            for (int i = 0; i < languages.Length; i++)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"insert into Languages (name) values (@name)";
                    command.Parameters.Add("@name", SqlDbType.VarChar).Value = names[i + 1];
                    command.ExecuteNonQuery();
                }
            }

            //table LanguageTranslations
            for (int i = 0; i < languages.Length; i++)
            {
                for (int j = 0; j < languages.Length; j++)
                {
                    if (i >= languages.Length) break;
                    using (SqlCommand command = new SqlCommand())
                    {
                        string[] properties = GetPropertyValues(languages[i]);
                        command.Connection = connection;
                        command.CommandText = "insert into LanguageTranslations (translation, lang_id, native_lang_id) " +
                            $"values (@translation, @lang_id, @native_lang_id)";
                        command.Parameters.Add("@translation", SqlDbType.NVarChar).Value = properties[j + 1];
                        command.Parameters.Add("@lang_id", SqlDbType.VarChar).Value = i + 1;
                        command.Parameters.Add("@native_lang_id", SqlDbType.VarChar).Value = j + 1;
                        command.ExecuteNonQuery();
                    }

                }
            }
        }

        private static void LoadCategories(SqlConnection connection, string path)
        {
            DirectoryInfo rootDir = new DirectoryInfo(path);
            DirectoryInfo[] subDirEntries = rootDir.GetDirectories();
            subDirEntries = subDirEntries.Where(dir => Path.GetFileName(dir.FullName) != "pictures" 
            && Path.GetFileName(dir.FullName) != "Test_Icons").ToArray();
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
            for (int i = 0; i < subDirEntries.Length; i++)
            {
                string DirName = Path.GetFileName(subDirEntries[i].FullName);
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"insert into Categories (name, picture) values (@name, @picture)";
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = DirName;
                    FileInfo pictureToInsert = Array.Find(pictures, picture => Path.GetFileName(picture.FullName).ToLower() == (DirName + ".jpg").ToLower());
                    command.Parameters.Add("@picture", SqlDbType.NVarChar, 200).Value = pictureToInsert.FullName;
                    command.ExecuteNonQuery();
                    parentCategoryId++;

                    // CategoryTranslation
                    ExcelQueryFactory factory = new ExcelQueryFactory(@"D:\LanguageSite\DictionaryForFullStack\CategoriesRoot.xlsx");
                    ExcelFileRow[] categories = (from row in factory.Worksheet<ExcelFileRow>(0)
                                                select row).ToList().DeleteNulls();

                    int categoryId = -1;
                    SqlCommand readerCommand = new SqlCommand();
                    readerCommand.Connection = connection;
                    readerCommand.CommandText = "select id from Categories where name = @name";
                    readerCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = DirName;
                    SqlDataReader reader = readerCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        categoryId = (int)reader.GetValue(0);
                    }
                    reader.Close();

                    string[] properties = GetPropertyValues(categories[i]);
                    for (int j = 1; j < properties.Length; j++)
                    {
                        if (j >= properties.Length) break;
                        using (SqlCommand commandForTranslation = new SqlCommand())
                        {
                            commandForTranslation.Connection = connection;
                            commandForTranslation.CommandText = "insert into CategoriesTranslations (translation, category_id, lang_id) " +
                                $"values (@translation, @category_id, @lang_id)";
                            commandForTranslation.Parameters.Add("@translation", SqlDbType.NVarChar).Value = properties[j];
                            commandForTranslation.Parameters.Add("@category_id", SqlDbType.VarChar).Value = categoryId;
                            commandForTranslation.Parameters.Add("@lang_id", SqlDbType.VarChar).Value = j;
                            commandForTranslation.ExecuteNonQuery();
                        }

                    }
                }
                LoadSubDir(subDirEntries[i].FullName, connection);
            }
        }

        private static void LoadSubDir(string path, SqlConnection connection)
        {
            DirectoryInfo rootDir = new DirectoryInfo(path);
            DirectoryInfo[] subDirEntries = rootDir.GetDirectories();
            subDirEntries = subDirEntries.Where(dir => Path.GetFileName(dir.FullName) != "pictures").ToArray();
            if (Array.Exists(subDirEntries, dir => Path.GetFileName(dir.FullName) == "pronounce"))
            {
                LoadWords(path, connection);
            }
            FileInfo[] pictures;
            try
            {
                string currentOath = path;
                DirectoryInfo picturesInfo = new DirectoryInfo(currentOath += @"\pictures");
                pictures = picturesInfo.GetFiles();
            }
            catch
            {
                Console.WriteLine("Directory has no pictures");
                pictures = Array.Empty<FileInfo>();
            }
            for (int i = 0; i < subDirEntries.Length; i++)
            {
                string DirName = Path.GetFileName(subDirEntries[i].FullName);
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    if (pictures.Length > 0)
                    {
                        command.CommandText = $"insert into Categories (name, parent_id, picture) values (@name, @parent_id, @picture)";
                        command.Parameters.Add("@picture", SqlDbType.NVarChar, 200).Value = pictures[i].FullName;
                    }
                    else
                    {
                        command.CommandText = $"insert into Categories (name, parent_id) values (@name, @parent_id)";
                    }

                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = DirName;
                    command.Parameters.Add("@parent_id", SqlDbType.Int).Value = parentCategoryId;
                    command.ExecuteNonQuery();
                }

                int categoryId = -1;
                SqlCommand readerCommand = new SqlCommand();
                readerCommand.Connection = connection;
                readerCommand.CommandText = "select id from Categories where name = @name";
                readerCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = DirName;
                SqlDataReader reader = readerCommand.ExecuteReader();
                if (reader.Read())
                {
                    categoryId = (int)reader.GetValue(0);
                }
                reader.Close();
                

                //CategoryTranslation
                ExcelQueryFactory factory = new ExcelQueryFactory(path + @"\" + Path.GetFileName(path) + ".xlsx");
                ExcelFileRow[] categories = (from row in factory.Worksheet<ExcelFileRow>(0)
                                             orderby row.Name
                                             select row).ToList().DeleteNulls();
                string[] properties = GetPropertyValues(categories[i]);
                for (int j = 1; j < properties.Length; j++)
                {
                    if (j >= properties.Length) break;
                    using (SqlCommand commandForTranslation = new SqlCommand())
                    {
                        commandForTranslation.Connection = connection;
                        commandForTranslation.CommandText = "insert into CategoriesTranslations (translation, category_id, lang_id) " +
                            $"values (@translation, @category_id, @lang_id)";
                        commandForTranslation.Parameters.Add("@translation", SqlDbType.NVarChar).Value = properties[j];
                        commandForTranslation.Parameters.Add("@category_id", SqlDbType.VarChar).Value = categoryId;
                        commandForTranslation.Parameters.Add("@lang_id", SqlDbType.VarChar).Value = j;
                        commandForTranslation.ExecuteNonQuery();
                    }

                }
                
                LoadSubDir(subDirEntries[i].FullName, connection);
            }
            parentCategoryId += subDirEntries.Length;
        }

        private static void LoadWords(string path, SqlConnection connection)
        {
            ExcelQueryFactory factory = new ExcelQueryFactory(path + @"\" + Path.GetFileName(path) + ".xlsx");
            ExcelFileRow[] words = (from row in factory.Worksheet<ExcelFileRow>(0)
                                    orderby row.Name
                                    select row).ToList().DeleteNulls();
            FileInfo[] pictures = new DirectoryInfo(path + @"\pictures").GetFiles();
            DirectoryInfo[] pronounce = new DirectoryInfo(path + @"\pronounce").GetDirectories();
            FileInfo[] sounds;
            try
            {
                sounds = new DirectoryInfo(path + @"\sounds").GetFiles();
            }
            catch
            {
                Console.WriteLine("Category has no sounds");
                sounds = Array.Empty<FileInfo>();
            }
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words.Length; j++)
                {
                    using (SqlCommand wordInsert = new SqlCommand())
                    {
                        wordInsert.Connection = connection;
                        wordInsert.CommandText = "insert into Words (name, category_id, sound, picture) values (@name, @category_id, @sound, @picture)";
                        wordInsert.Parameters.Add("@name", SqlDbType.VarChar).Value = words[j].Name;
                        wordInsert.Parameters.Add("@category_id", SqlDbType.Int).Value = 
                    }
                }
                
            }
     
        }
    }
}
