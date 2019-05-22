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
        private static int parentCategoryId { get; set; } = 0;
        private static int wordId { get; set; } = 0;

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

        private static void LoadTests(SqlConnection connection, string path)
        {

        }

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
                    command.Parameters.Clear();
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
                        command.Parameters.Clear();
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
                    command.Parameters.Clear();
                    parentCategoryId++;

                    // CategoryTranslation
                    ExcelQueryFactory factory = new ExcelQueryFactory(@"D:\LanguageSite\DictionaryForFullStack\CategoriesRoot.xlsx");
                    ExcelFileRow[] categories = (from row in factory.Worksheet<ExcelFileRow>(0)
                                                select row).ToList().DeleteNulls();

                    int categoryId = GetCategoryId(DirName, connection);

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
                            commandForTranslation.Parameters.Clear();
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
                string pathForWords = path;
                LoadWords(pathForWords, connection);
                return;
            }
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
                    command.Parameters.Clear();
                }


                int categoryId = GetCategoryId(DirName, connection);
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
                        commandForTranslation.Parameters.Clear();
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
            string dirName = Path.GetFileName(path);
            try
            {
                sounds = new DirectoryInfo(path + @"\sounds").GetFiles();
            }
            catch
            {
                Console.WriteLine($"Category {dirName} has no sounds");
                sounds = Array.Empty<FileInfo>();
            }

            int categoryId = GetCategoryId(dirName, connection);
            string[] sheetNames = factory.GetWorksheetNames().ToArray();
            string[] headers = factory.GetColumnNames(sheetNames[0]).ToArray();
            for (int i = 0; i < words.Length; i++)
            {
                using (SqlCommand wordInsert = new SqlCommand())
                {
                    FileInfo soundFileInfo = Array.Find(sounds,
                               s => Path.GetFileName(s.FullName).ToLower() == words[i].Name.ToLower() + ".wav");
                    wordInsert.Connection = connection;
                    if (sounds.Length > 0 && soundFileInfo != null)
                    {
                        wordInsert.CommandText =
                            "insert into Words (name, category_id, sound, picture) values (@name, @category_id, @sound, @picture)";
                            wordInsert.Parameters.Add("@sound", SqlDbType.VarChar, 200).Value = soundFileInfo.FullName;
                    }
                    else
                    {
                        wordInsert.CommandText =
                            "insert into Words (name, category_id, picture) values (@name, @category_id,  @picture)";
                    }
                    wordInsert.Parameters.Add("@name", SqlDbType.VarChar).Value = words[i].Name;
                    wordInsert.Parameters.Add("@category_id", SqlDbType.Int).Value = categoryId;
                    FileInfo picture = Array.Find(pictures,
                        p => Path.GetFileName(p.FullName).ToLower().Replace(" ", String.Empty) == words[i].Name.ToLower().Replace(" ", String.Empty) + ".jpg");
                    wordInsert.Parameters.Add("@picture", SqlDbType.VarChar, 200).Value = picture.FullName;
                    wordInsert.ExecuteNonQuery();
                    wordInsert.Parameters.Clear();
                    wordId++;
                }

                using (SqlCommand wordTranslationCommand = new SqlCommand())
                {
                    string[] properties = GetPropertyValues(words[i]);
                    for (int j = 1; j < properties.Length; j++)
                    {
                        wordTranslationCommand.Connection = connection;
                        wordTranslationCommand.CommandText = "insert into WordTranslations (lang_id, translation, word_id, pronounce) " +
                                                 "values (@lang_id, @translation, @word_id, @pronounce)";
                        wordTranslationCommand.Parameters.Add("@lang_id", SqlDbType.Int).Value = j;
                        wordTranslationCommand.Parameters.Add("@translation", SqlDbType.NVarChar).Value = properties[j];
                        wordTranslationCommand.Parameters.Add("@word_id", SqlDbType.Int).Value = wordId;
                        FileInfo[] pronounses = new DirectoryInfo(path + @"\pronounce\" + headers[j]).GetFiles();
                        FileInfo pronounceToInsert = Array.Find(pronounses,
                            p => Path.GetFileName(p.FullName).ToLower().Replace(" ", String.Empty) == words[i].Name.ToLower().Replace(" ", String.Empty) + ".wav");
                        wordTranslationCommand.Parameters.Add("@pronounce", SqlDbType.VarChar, 200).Value =
                            pronounceToInsert.FullName;
                        wordTranslationCommand.ExecuteNonQuery();
                        wordTranslationCommand.Parameters.Clear();
                    }
                }

            }

        }

        private static int GetCategoryId(string categoryName, SqlConnection connection)
        {
                int categoryId = -1;
                using (SqlCommand readerCommand = new SqlCommand())
                {
                    readerCommand.Connection = connection;
                    readerCommand.CommandText = "select id from Categories where name = @name";
                    readerCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = categoryName;
                    SqlDataReader reader = readerCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        categoryId = (int)reader.GetValue(0);
                    }
                    reader.Close();
                readerCommand.Parameters.Clear();
                }
                
                return categoryId;
        }
    }
}
