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

        private const string Tests = "Tests";
        private const string Languages = "Languages";
        private const string Categories = "Categories";
        private const string Words = "Words";

        private string[] names;

        public void LoadMainEntities()
        {
            const string CategoriesPath = @"D:\LanguageSite\DictionaryForFullStack";
            const string LanguagesPath = @"D:\LanguageSite\DictionaryForFullStack\Languages.xlsx";
            const string TestsPath = @"D:\LanguageSite\DictionaryForFullStack\TestsNames.xlsx";
            
            try
            { 
                using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
                {
                    db.Open();
                    LoadLanguages(db, LanguagesPath);
                    LoadTests(db, TestsPath);
                    LoadCategories(db, CategoriesPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        private void LoadTests(SqlConnection connection, string path)
        {
            ExcelFileRow[] tests = Helper.GetExcel(path);
            FileInfo[] icons;
            try
            {
                icons = new DirectoryInfo(Path.GetDirectoryName(path) + @"\Test_Icons\grey").GetFiles();
            }
            catch
            {
                icons = Array.Empty<FileInfo>();
                Console.WriteLine("Tests have no icons");
            }

            //table Tests
            if (Helper.Check(icons))
            {
                for (int i = 0; i < tests.Length; i++)
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = $"insert into Tests (name, icon) values (@name, @icon)";
                        command.Parameters.Add("@name", SqlDbType.NVarChar).Value = tests[i].Name;
                        command.Parameters.Add("@icon", SqlDbType.VarChar, 200).Value = icons[i + 2].FullName.Substring(3);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();

                        //table TestTranslations
                        string[] properties = Helper.GetPropertyValues(tests[i]);

                        int test_id = Helper.GetId(Tests, tests[i].Name, connection);
                        //starts with 1 because properties[0] contains not values but names only
                        //exapmle (Names, UA, RU etc)
                        for (int j = 1; j < properties.Length; j++)
                        {
                            int language_id = Helper.GetId(Languages, names[j], connection);
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
            
        }

        private void LoadLanguages(SqlConnection connection, string path)
        {
            ExcelQueryFactory factory = new ExcelQueryFactory(path);
            ExcelFileRow[] languages = (from row in factory.Worksheet<ExcelFileRow>(0)
                                   select row).ToList().DeleteNulls();
            names = factory.GetColumnNames(Categories).ToArray();

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
                int language_id = Helper.GetId(Languages, names[i + 1], connection);
                for (int j = 1; j < languages.Length + 1; j++)
                {
                    int native_lang_id = Helper.GetId(Languages, names[j], connection);
                    if (i >= languages.Length)
                    {
                        break;
                    }
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
            DirectoryInfo[] subDirEntries = Helper.GetSubDirs(path);
            subDirEntries = subDirEntries.Where(dir => Path.GetFileName(dir.FullName) != "Test_Icons").ToArray();
            FileInfo[] pictures = Helper.GetPictures(path);
            for (int i = 0; i < subDirEntries.Length; i++)
            {
                string DirName = Path.GetFileName(subDirEntries[i].FullName);
                FileInfo pictureToInsert = null;
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"insert into Categories (name, picture) values (@name, @picture)";
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = DirName;
                    try
                    {
                        pictureToInsert = Array.Find(pictures, picture => Path.GetFileName(picture.FullName).ToLower() == (DirName + ".jpg").ToLower());
                    }
                    catch
                    {
                        throw new Exception("Can not find file");
                    }
                    if (Helper.Check(pictureToInsert))
                    {
                        command.Parameters.Add("@picture", SqlDbType.NVarChar, 200).Value = pictureToInsert.FullName.Substring(3);
                    }
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    // CategoryTranslation
                    ExcelFileRow[] categories = Helper.GetExcel(@"D:\LanguageSite\DictionaryForFullStack\CategoriesRoot.xlsx");
                    int categoryId = Helper.GetId(Categories, DirName, connection);

                    string[] properties = Helper.GetPropertyValues(categories[i]);
                    for (int j = 1; j < properties.Length; j++)
                    {
                        if (j >= properties.Length)
                        {
                            break;
                        }
                        using (SqlCommand commandForTranslation = new SqlCommand())
                        {
                            int language_id = Helper.GetId(Languages, names[j], connection);
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
            DirectoryInfo[] subDirEntries = Helper.GetSubDirs(path);
            if (Array.Exists(subDirEntries, dir => Path.GetFileName(dir.FullName) == "pronounce"))
            {
                string pathForWords = path;
                LoadWords(pathForWords, connection);
                return;
            }

            FileInfo[] pictures = Helper.GetPictures(path);
            for (int i = 0; i < subDirEntries.Length; i++)
            {
                string parentName = Path.GetFileName(path);
                int parent_id = Helper.GetId(Categories, parentName, connection);
                string DirName = Path.GetFileName(subDirEntries[i].FullName);
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    if (pictures.Length > 0)
                    {
                        command.CommandText = $"insert into Categories (name, parent_id, picture) values (@name, @parent_id, @picture)";
                        command.Parameters.Add("@picture", SqlDbType.NVarChar, 200).Value = pictures[i].FullName.Substring(3);
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


                int categoryId = Helper.GetId(Categories, DirName, connection);
                //CategoryTranslation
                ExcelFileRow[] categories = Helper.GetExcel(path);
                string[] properties = Helper.GetPropertyValues(categories[i]);
                for (int j = 1; j < properties.Length; j++)
                {
                    int language_id = Helper.GetId(Languages, names[j], connection);
                    if (j >= properties.Length)
                    {
                        break;
                    }
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
            ExcelFileRow[] words = Helper.GetExcel(path);
            DirectoryInfo[] pronounce = new DirectoryInfo(path + @"\pronounce").GetDirectories();
            FileInfo[] pictures = Helper.GetPictures(path);
            FileInfo[] sounds = Array.Empty<FileInfo>();
            try
            {
                sounds = new DirectoryInfo(path + @"\sounds").GetFiles();
            }
            catch
            {
                Console.WriteLine($"Category {dirName} has no sound");
            }

            int categoryId = Helper.GetId(Categories, dirName, connection);
            ExcelQueryFactory factory = new ExcelQueryFactory(path + @"\" + Path.GetFileName(path) + ".xlsx");
            string[] sheetNames = factory.GetWorksheetNames().ToArray();
            string[] headers = factory.GetColumnNames(sheetNames[0]).ToArray();
            for (int i = 0; i < words.Length; i++)
            {
                using (SqlCommand wordInsert = new SqlCommand())
                {
                    FileInfo picture = null;
                    FileInfo soundFileInfo = Array.Find(sounds,
                               s => Path.GetFileName(s.FullName).ToLower() == words[i].Name.ToLower() + ".wav");
                    wordInsert.Connection = connection;
                    if (sounds.Length > 0 && soundFileInfo != null)
                    {
                        wordInsert.CommandText =
                            "insert into Words (name, category_id, sound, picture) values (@name, @category_id, @sound, @picture)";
                            wordInsert.Parameters.Add("@sound", SqlDbType.VarChar, 200).Value = soundFileInfo.FullName.Substring(3);
                    }
                    else
                    {
                        wordInsert.CommandText =
                            "insert into Words (name, category_id, picture) values (@name, @category_id,  @picture)";
                    }
                    wordInsert.Parameters.Add("@name", SqlDbType.VarChar).Value = words[i].Name;
                    wordInsert.Parameters.Add("@category_id", SqlDbType.Int).Value = categoryId;
                    try
                    {
                        picture = Array.Find(pictures,
                       p => Path.GetFileName(p.FullName).ToLower().Replace(" ", String.Empty) == words[i].Name.ToLower().Replace(" ", String.Empty) + ".jpg");
                    }
                    catch
                    {
                        throw new Exception("Can not find picture");
                    }
                    wordInsert.Parameters.Add("@picture", SqlDbType.VarChar, 200).Value = picture.FullName.Substring(3);
                    wordInsert.ExecuteNonQuery();
                    wordInsert.Parameters.Clear();
                }

                using (SqlCommand wordTranslationCommand = new SqlCommand())
                {
                    int word_id = Helper.GetId(Words, words[i].Name, connection);
                    string[] properties = Helper.GetPropertyValues(words[i]);
                    for (int j = 1; j < properties.Length; j++)
                    {
                        int lang_id = Helper.GetId(Languages, names[j], connection);
                        FileInfo[] pronounses = new DirectoryInfo(path + @"\pronounce\" + headers[j]).GetFiles();
                        FileInfo pronounceToInsert = Array.Find(pronounses,
                            p => Path.GetFileName(p.FullName).ToLower().Replace(" ", String.Empty) == words[i].Name.ToLower().Replace(" ", String.Empty) + ".wav");

                        if (Helper.Check(pronounceToInsert))
                        {
                            wordTranslationCommand.Connection = connection;
                            wordTranslationCommand.CommandText = "insert into WordTranslations (lang_id, translation, word_id, pronounce) " +
                                                     "values (@lang_id, @translation, @word_id, @pronounce)";
                            wordTranslationCommand.Parameters.Add("@lang_id", SqlDbType.Int).Value = lang_id;
                            wordTranslationCommand.Parameters.Add("@translation", SqlDbType.NVarChar).Value = properties[j];
                            wordTranslationCommand.Parameters.Add("@word_id", SqlDbType.Int).Value = word_id;
                            wordTranslationCommand.Parameters.Add("@pronounce", SqlDbType.VarChar, 200).Value = pronounceToInsert.FullName.Substring(3);
                            wordTranslationCommand.ExecuteNonQuery();
                            wordTranslationCommand.Parameters.Clear();
                        }
                        else
                        {
                            Console.WriteLine($"Pronounce can not be null");
                        }
                    }
                }

            }

        }
    }
}
