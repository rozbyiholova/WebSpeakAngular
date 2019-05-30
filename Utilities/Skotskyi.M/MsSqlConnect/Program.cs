using System;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace MsSqlConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connetionString = @"Data Source=VINW0231;Initial Catalog=Languages_bd;Integrated Security=True";

            string[] allFoundFiles = Directory.GetFiles("D:/DictionaryForFullStack/", "*.xlsx", SearchOption.AllDirectories);

            string buffer = allFoundFiles[0]; // swap Categories.xlsx with Languages.xlsx
            allFoundFiles[0] = allFoundFiles[1];
            allFoundFiles[1] = buffer; // First file must be Languages.xlsx

            DataTable dtsheet = new DataTable();

            try
            {
                using (SqlConnection cnn = new SqlConnection(connetionString))
                {
                    cnn.Open();

                    foreach (string file in allFoundFiles)
                    {
                        string exelName = Path.GetFileName(file);
                        Console.WriteLine($"- file opened - {exelName}");

                        string excelCS = string.Format($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={file};Extended Properties=Excel 8.0", exelName);

                        string[] icons = Directory.GetFiles(@"D:\DictionaryForFullStack\Test_Icons\white", "Test*.png", SearchOption.AllDirectories);

                        if (!exelName.StartsWith("~$"))
                        {
                            using (OleDbConnection con = new OleDbConnection(excelCS))
                            {
                                con.Open();

                                dtsheet = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string ExcelSheetName = dtsheet.Rows[0]["Table_Name"].ToString();

                                Console.WriteLine($" - nazva lista - {ExcelSheetName}");

                                ExcelSheetName = ExcelSheetName.Replace("\'\'", "\'");

                                OleDbCommand cmd = new OleDbCommand($"select * from [{ExcelSheetName}] WHERE [name] IS NOT NULL", con);

                                OleDbDataAdapter adap = new OleDbDataAdapter(cmd);
                                DataSet ds = new DataSet();

                                adap.Fill(ds);

                                DataTable dt = ds.Tables[0];

                                if (exelName == "Languages.xlsx") //lang filling
                                {
                                    FillLang(dt, cnn);
                                }

                                if (exelName == "TestsNames.xlsx") //test filling
                                {
                                    FillTest(dt, cnn, icons);
                                }

                                if (ExcelSheetName != "Categories$" && !exelName.StartsWith("~$"))
                                {
                                    string DirName = Path.GetFileName(Path.GetDirectoryName(file));
                                    FillWords(dt, cnn, DirName); // words filling

                                    FillWordsTranslation(dt, cnn, file); // words Translation filling
                                }

                                if (ExcelSheetName == "Categories$" && exelName != "Languages.xlsx" && exelName != "TestsNames.xlsx")
                                {
                                    if (exelName == "CategoriesRoot.xlsx")
                                    {
                                        FillCategoriesRoot(dt, cnn);
                                    }
                                    else //SubCategoriesFill
                                    {
                                        FillCategories(dt, cnn, exelName);

                                        FillCategoriesTranslation(dt, cnn);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
                Console.ReadKey();
            }

        }
        public static string CheckPrefixLang(string s) //check prefix
        {
            switch (s)
            {
                case "Ukrainian":
                    s = "UA";
                    break;
                case "Russian":
                    s = "RU";
                    break;
                case "English":
                    s = "ENU";
                    break;
                case "German":
                    s = "GER";
                    break;
                case "Chinese":
                    s = "CHI";
                    break;
                case "Portuguese":
                    s = "POR";
                    break;
                case "Spanish":
                    s = "SPA";
                    break;
                case "Polish":
                    s = "POL";
                    break;
            }
            return s;
        }
        public static void FillLang(DataTable dt, SqlConnection cnn) // Languages Filling
        {
            for (int i = 1; dt.Columns.Count > i; i++)
            {
                SqlCommand command = new SqlCommand(String.Format("INSERT INTO Languages (Name)  VALUES('{0}')", dt.Columns[i].ToString()), cnn);

                command.ExecuteNonQuery();
            }

            for (int i = 0; dt.Rows.Count > i; i++)
            {
                for (int j = 1; dt.Columns.Count > j; j++)
                {
                    SqlCommand native_lang_id = new SqlCommand(String.Format("SELECT Id FROM Languages WHERE Name = '{0}'", dt.Columns[j].ToString()), cnn);
                    var native_lang_ids = native_lang_id.ExecuteScalar();

                    string Native_lang_trans = dt.Rows[i][0].ToString();

                    Native_lang_trans = CheckPrefixLang(Native_lang_trans);

                    SqlCommand lang_id = new SqlCommand(String.Format("SELECT Id FROM Languages WHERE Name = '{0}'", Native_lang_trans.ToString()), cnn);
                    var lang_ids = lang_id.ExecuteScalar();

                    SqlCommand command = new SqlCommand(String.Format("INSERT INTO LanguageTranslations (translation,lang_id,native_lang_id)  VALUES( N'{0}', '{1}','{2}')", dt.Rows[i][j].ToString(), native_lang_ids, lang_ids), cnn);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void FillTest(DataTable dt, SqlConnection cnn, string[] icons) //Test filling
        {
            for (int i = 0; dt.Rows.Count > i; i++)
            {
                SqlCommand command = new SqlCommand(String.Format("INSERT INTO Tests (Name, icon)  VALUES( '{0}', '{1}')", dt.Rows[i][0].ToString(), icons[i]), cnn);
                command.ExecuteNonQuery();
            }

            for (int i = 0; dt.Rows.Count > i; i++)
            {
                for (int j = 1; dt.Columns.Count > j; j++)
                {
                    SqlCommand native_lang_id = new SqlCommand(String.Format("SELECT Id FROM Languages WHERE Name = '{0}'", dt.Columns[j].ToString()), cnn);
                    var native_lang_ids = native_lang_id.ExecuteScalar();

                    SqlCommand test_id = new SqlCommand(String.Format("SELECT Id FROM Tests WHERE Name = '{0}'", dt.Rows[i][0].ToString()), cnn);
                    var test_ids = test_id.ExecuteScalar();

                    SqlCommand command = new SqlCommand(String.Format("INSERT INTO TestTranslations (translation, lang_id, test_id)  VALUES( N'{0}', '{1}','{2}')", dt.Rows[i][j].ToString(), native_lang_ids, test_ids), cnn);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void FillWords(DataTable dt, SqlConnection cnn, string DirName) //Words Filling
        {
            for (int i = 0; dt.Rows.Count > i; i++) //Words Filling 
            {
                string[] picture = Directory.GetFiles("D:/DictionaryForFullStack/", $"{dt.Rows[i][0].ToString()}.jpg ", SearchOption.AllDirectories);

                SqlCommand cat_id = new SqlCommand(String.Format("SELECT Id FROM Categories WHERE name = '{0}'", DirName), cnn);
                var cat_ids = cat_id.ExecuteScalar();

                SqlCommand command = new SqlCommand(String.Format("INSERT INTO Words (Name, picture, category_id)  VALUES( @word , @picture ,'{0}')", cat_ids), cnn);
                command.Parameters.Add("@word", SqlDbType.NVarChar).Value = dt.Rows[i][0].ToString();
                command.Parameters.Add("@picture", SqlDbType.NVarChar).Value = picture[0].ToString();

                command.ExecuteNonQuery();
            }
        }
        public static void FillWordsTranslation(DataTable dt, SqlConnection cnn, string file) //Words Translation
        {
            for (int i = 0; dt.Rows.Count > i; i++)
            {
                for (int j = 1; dt.Columns.Count > j; j++) //words trans
                {

                    SqlCommand lang_id = new SqlCommand(String.Format("SELECT Id FROM Languages WHERE Name = '{0}'", dt.Columns[j].ToString()), cnn);
                    var lang_ids = lang_id.ExecuteScalar();

                    SqlCommand word_id = new SqlCommand("SELECT Id FROM Words WHERE Name = @name", cnn);
                    word_id.Parameters.Add("@name", SqlDbType.NVarChar).Value = dt.Rows[i][0].ToString();
                    var word_ids = word_id.ExecuteScalar();

                    string DirName = Path.GetDirectoryName(file);

                    string[] pronounce = Directory.GetFiles($@"{DirName}\pronounce\{dt.Columns[j].ToString()}", $"{dt.Rows[i][0].ToString()}.wav ", SearchOption.AllDirectories);

                    SqlCommand command = new SqlCommand(String.Format("INSERT INTO WordTranslations (translation, word_id, lang_id, pronounce )  VALUES( @words, '{0}' , '{1}' , @pronounce)", word_ids, lang_ids), cnn);
                    command.Parameters.Add("@words", SqlDbType.NVarChar).Value = dt.Rows[i][j].ToString();
                    command.Parameters.Add("@pronounce", SqlDbType.NVarChar).Value = pronounce[0];
                    command.ExecuteNonQuery();

                }
            }
        }
        public static void FillCategoriesRoot(DataTable dt, SqlConnection cnn) //CategoriesRoot Translation filling
        {
            for (int i = 0; dt.Rows.Count > i; i++)
            {
                string[] picture = Directory.GetFiles("D:/DictionaryForFullStack/", $"{dt.Rows[i][0].ToString()}.jpg ", SearchOption.AllDirectories);

                SqlCommand command = new SqlCommand(String.Format("INSERT INTO Categories (Name, picture)  VALUES( '{0}', '{1}')", dt.Rows[i][0].ToString(), picture[0].ToString()), cnn);

                command.ExecuteNonQuery();
            }

            for (int i = 0; dt.Rows.Count > i; i++) //cat trans fill
            {
                for (int j = 1; dt.Columns.Count > j; j++)

                {
                    SqlCommand lang_id = new SqlCommand(String.Format("SELECT Id FROM Languages WHERE Name = '{0}'", dt.Columns[j].ToString()), cnn);
                    var lang_ids = lang_id.ExecuteScalar();

                    SqlCommand cat_id = new SqlCommand(String.Format("SELECT Id FROM Categories WHERE Name = '{0}'", dt.Rows[i][0].ToString()), cnn);
                    var cat_ids = cat_id.ExecuteScalar();

                    SqlCommand command = new SqlCommand(String.Format("INSERT INTO CategoriesTranslations (translation, category_id, lang_id )  VALUES( N'{0}', '{1}', '{2}')", dt.Rows[i][j].ToString(), cat_ids, lang_ids), cnn);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void FillCategories(DataTable dt, SqlConnection cnn, string exelName) //Categories fill
        {
            for (int i = 0; dt.Rows.Count > i; i++)
            {
                string[] picture = Directory.GetFiles("D:/DictionaryForFullStack/", $"{dt.Rows[i][0].ToString()}.jpg ", SearchOption.AllDirectories);

                SqlCommand parent_id = new SqlCommand(String.Format("SELECT Id FROM Categories WHERE Name ='{0}'", exelName.Substring(0, exelName.Length - 5)), cnn);
                var parent_ids = parent_id.ExecuteScalar();

                string sqlExpression = String.Format("INSERT INTO Categories(Name, picture, parent_id)  VALUES( @words, @picture, '{0}')", parent_ids);

                SqlCommand command = new SqlCommand(sqlExpression, cnn);
                command.Parameters.Add("@words", SqlDbType.NVarChar).Value = dt.Rows[i][0].ToString();
                command.Parameters.Add("@picture", SqlDbType.NVarChar).Value = picture[0].ToString();
                command.ExecuteNonQuery();
            }
        }

        public static void FillCategoriesTranslation(DataTable dt, SqlConnection cnn) //Categories Translation filling
        {
            for (int i = 0; dt.Rows.Count > i; i++) //cat trans fill
            {
                for (int j = 1; dt.Columns.Count > j; j++)
                {
                    SqlCommand lang_id = new SqlCommand(String.Format("SELECT Id FROM Languages WHERE Name = '{0}'", dt.Columns[j].ToString()), cnn);
                    var lang_ids = lang_id.ExecuteScalar();

                    SqlCommand cat_id = new SqlCommand("SELECT Id FROM Categories WHERE Name = @name", cnn);
                    cat_id.Parameters.Add("@name", SqlDbType.NVarChar).Value = dt.Rows[i][0].ToString();
                    var cat_ids = cat_id.ExecuteScalar();

                    string word = dt.Rows[i][j].ToString();

                    string sqlExpression = String.Format(@"INSERT INTO CategoriesTranslations (translation, category_id, lang_id )  VALUES( @word, '{0}', '{1}')", cat_ids, lang_ids);

                    SqlCommand command = new SqlCommand(sqlExpression, cnn);
                    SqlParameter nameParam = new SqlParameter("@word", word);
                    command.Parameters.Add(nameParam);
                    command.ExecuteNonQuery();

                }
            }
        }

    }
}
