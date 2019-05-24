using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using LinqToExcel;

namespace DbFillingTool
{
    public class FilesHandler
    {
        private string[] names;

        public void LoadMainEntities()
        {
            const string categoriesPath = @"D:\LanguageSite\DictionaryForFullStack";
            const string languagesPath = @"D:\LanguageSite\DictionaryForFullStack\Languages.xlsx";
            const string testsPath = @"D:\LanguageSite\DictionaryForFullStack\TestsNames.xlsx";
            
            try
            { 
                using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
                {
                    db.Open();
                    LoadLanguages(db, languagesPath);
                    LoadTests(db, testsPath);
                    LoadCategories(db, categoriesPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        private void LoadTests(SqlConnection connection, string path)
        {
            ExcelQueryFactory factory = new ExcelQueryFactory(path);
            ExcelFileRow[] tests = (from row in factory.Worksheet<ExcelFileRow>(0)
                                    select row).ToList().DeleteNulls();
            FileInfo[] icons = new DirectoryInfo(Path.GetDirectoryName(path) + @"\Test_Icons\white").GetFiles();

            //table Tests
            for (int i = 0; i < tests.Length; i++)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"insert into Tests (name, icon) values (@name, @icon)";
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = tests[i].Name;
                    command.Parameters.Add("@icon", SqlDbType.VarChar, 200).Value = icons[i + 2].FullName;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    //table TestTranslations
                    string[] properties = Helper.GetPropertyValues(tests[i]);

                    int test_id = Helper.GetId("Tests", tests[i].Name, connection);
                    //starts with 1 because properties[0] contains not values but names only
                    //exapmle (Names, UA, RU etc)
                    for (int j = 1; j < properties.Length; j++)
                    {
                        int language_id = Helper.GetId("Languages", names[j], connection);
                        using (SqlCommand testTranslationCommand = new SqlCommand())
                        {
                            testTranslationCommand.Connection = connection;
                            testTranslationCommand.CommandText = "insert into TestTranslations (translation, test_id, lang_id) " +
                                $"values (@translation, @test_id, @lang_id)";
                            testTranslationCommand.Parameters.Add("@translation", SqlDbType.NVarChar).Value = properties[j];
                            testTranslationCommand.Parameters.Add("@test_id", SqlDbType.Int).Value = test_id;
                            testTranslationCommand.Parameters.Add("@lang_id", SqlDbType.Int).Value = language_id;
                            testTranslationCommand.ExecuteNonQuery();
                            testTranslationCommand.Parameters.Clear();
                        }

                    }
                }
            }
        }

        private void LoadLanguages(SqlConnection connection, string path)
        {
            ExcelQueryFactory factory = new ExcelQueryFactory(path);
            ExcelFileRow[] languages = (from row in factory.Worksheet<ExcelFileRow>(0)
                                        select row).ToList().DeleteNulls();
            names = factory.GetColumnNames("Categories").ToArray();

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
                int language_id = Helper.GetId("Languages", names[i + 1], connection);
                for (int j = 1; j < languages.Length + 1; j++)
                {
                    int native_lang_id = Helper.GetId("Languages", names[j], connection);
                    if (i >= languages.Length) break;
                    using (SqlCommand command = new SqlCommand())
                    {
                        string[] properties = Helper.GetPropertyValues(languages[i]);
                        command.Connection = connection;
                        command.CommandText = "insert into LanguageTranslations (translation, lang_id, native_lang_id) " +
                            $"values (@translation, @lang_id, @native_lang_id)";
                        command.Parameters.Add("@translation", SqlDbType.NVarChar).Value = properties[j];
                        command.Parameters.Add("@lang_id", SqlDbType.VarChar).Value = language_id;
                        command.Parameters.Add("@native_lang_id", SqlDbType.VarChar).Value = native_lang_id;
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                }
            }
        }

        private void LoadCategories(SqlConnection connection, string path)
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

                    // CategoryTranslation
                    ExcelQueryFactory factory = new ExcelQueryFactory(@"D:\LanguageSite\DictionaryForFullStack\CategoriesRoot.xlsx");
                    ExcelFileRow[] categories = (from row in factory.Worksheet<ExcelFileRow>(0)
                                                select row).ToList().DeleteNulls();

                    int categoryId = Helper.GetId("Categories", DirName, connection);

                    string[] properties = Helper.GetPropertyValues(categories[i]);
                    for (int j = 1; j < properties.Length; j++)
                    {
                        if (j >= properties.Length) break;
                        using (SqlCommand commandForTranslation = new SqlCommand())
                        {
                            int language_id = Helper.GetId("Languages", names[j], connection);
                            commandForTranslation.Connection = connection;
                            commandForTranslation.CommandText = "insert into CategoriesTranslations (translation, category_id, lang_id) " +
                                $"values (@translation, @category_id, @lang_id)";
                            commandForTranslation.Parameters.Add("@translation", SqlDbType.NVarChar).Value = properties[j];
                            commandForTranslation.Parameters.Add("@category_id", SqlDbType.VarChar).Value = categoryId;
                            commandForTranslation.Parameters.Add("@lang_id", SqlDbType.VarChar).Value = language_id;
                            commandForTranslation.ExecuteNonQuery();
                            commandForTranslation.Parameters.Clear();
                        }

                    }
                }
                LoadSubDir(subDirEntries[i].FullName, connection);
            }
        }

        private void LoadSubDir(string path, SqlConnection connection)
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
                string parentName = Path.GetFileName(path);
                int parent_id = Helper.GetId("Categories", parentName, connection);
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
                    command.Parameters.Add("@parent_id", SqlDbType.Int).Value = parent_id;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }


                int categoryId = Helper.GetId("Categories", DirName, connection);
                //CategoryTranslation
                ExcelQueryFactory factory = new ExcelQueryFactory(path + @"\" + Path.GetFileName(path) + ".xlsx");
                ExcelFileRow[] categories = (from row in factory.Worksheet<ExcelFileRow>(0)
                                             orderby row.Name
                                             select row).ToList().DeleteNulls();
                string[] properties = Helper.GetPropertyValues(categories[i]);
                for (int j = 1; j < properties.Length; j++)
                {
                    int language_id = Helper.GetId("Languages", names[j], connection);
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
        }

        private void LoadWords(string path, SqlConnection connection)
        {
            string dirName = Path.GetFileName(path);
            ExcelQueryFactory factory = new ExcelQueryFactory(path + @"\" + Path.GetFileName(path) + ".xlsx");
            ExcelFileRow[] words = (from row in factory.Worksheet<ExcelFileRow>(0)
                                    orderby row.Name
                                    select row).ToList().DeleteNulls();
            DirectoryInfo[] pronounce = new DirectoryInfo(path + @"\pronounce").GetDirectories();
            FileInfo[] pictures = new DirectoryInfo(path + @"\pictures").GetFiles();
            FileInfo[] sounds;
            try
            {
                sounds = new DirectoryInfo(path + @"\sounds").GetFiles();
            }
            catch
            {
                sounds = Array.Empty<FileInfo>();
            }

            int categoryId = Helper.GetId("Categories", dirName, connection);
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
                }

                using (SqlCommand wordTranslationCommand = new SqlCommand())
                {
                    int word_id = Helper.GetId("Words", words[i].Name, connection);
                    string[] properties = Helper.GetPropertyValues(words[i]);
                    for (int j = 1; j < properties.Length; j++)
                    {
                        int lang_id = Helper.GetId("Languages", names[j], connection);
                        wordTranslationCommand.Connection = connection;
                        wordTranslationCommand.CommandText = "insert into WordTranslations (lang_id, translation, word_id, pronounce) " +
                                                 "values (@lang_id, @translation, @word_id, @pronounce)";
                        wordTranslationCommand.Parameters.Add("@lang_id", SqlDbType.Int).Value = lang_id;
                        wordTranslationCommand.Parameters.Add("@translation", SqlDbType.NVarChar).Value = properties[j];
                        wordTranslationCommand.Parameters.Add("@word_id", SqlDbType.Int).Value = word_id;
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

    }
}
