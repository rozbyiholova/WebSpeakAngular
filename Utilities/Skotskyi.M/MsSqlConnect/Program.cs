using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace MsSqlConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=VINW0231;Initial Catalog=Languages_bd;Integrated Security=True";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            Console.WriteLine("Connection Open  !");
           


            string[] allFoundFiles = Directory.GetFiles("D:/DictionaryForFullStack/", "*.xlsx", SearchOption.AllDirectories);
            string buffer = allFoundFiles[0];
            allFoundFiles[0] = allFoundFiles[1];
            allFoundFiles[1] = buffer;

            DataTable dtsheet = new DataTable();

            foreach (string file in allFoundFiles)
            {
                
                string exelName = Path.GetFileName(file);
                    Console.WriteLine(exelName);                   

                string excelCS = string.Format($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={file};Extended Properties=Excel 8.0", exelName);

                string[] icons = Directory.GetFiles(@"D:\DictionaryForFullStack\Test_Icons\white", "Test*.png", SearchOption.AllDirectories);

                Console.WriteLine(icons[0]);
                Console.ReadKey();

                if (!exelName.StartsWith("~$"))
                {
                    using (OleDbConnection con = new OleDbConnection(excelCS))
                     {

                        con.Open();
                        Console.WriteLine("file opened");
                        
                        

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
                                for (int i = 1; dt.Columns.Count > i; i++)
                                {
                                    SqlCommand command = new SqlCommand("INSERT INTO Languages (Name)  VALUES( '" + dt.Columns[i].ToString() + "')", cnn);
                                    command.ExecuteNonQuery();
                                }

                                for (int i = 0; dt.Rows.Count > i; i++)
                                for (int j = 1; dt.Columns.Count > j; j++)
                                {                                    
                                    {
                                       SqlCommand native_lang_id = new SqlCommand($"SELECT Id FROM Languages WHERE Name = '{dt.Columns[j].ToString()}'", cnn);
                                       var native_lang_ids = native_lang_id.ExecuteScalar();

                                            string Native_lang_trans = dt.Rows[i][0].ToString();

                                            switch (Native_lang_trans)
                                            {
                                                case "Ukrainian":
                                                    Native_lang_trans = "UA";
                                                    break;
                                                case "Russian":
                                                    Native_lang_trans = "RU";
                                                    break;
                                                case "English":
                                                    Native_lang_trans = "ENU";
                                                    break;
                                                case "German":
                                                    Native_lang_trans = "GER";
                                                    break;
                                                case "Chinese":
                                                    Native_lang_trans = "CHI";
                                                    break;
                                                case "Portuguese":
                                                    Native_lang_trans = "POR";
                                                    break;
                                                case "Spanish":
                                                    Native_lang_trans = "SPA";
                                                    break;
                                                case "Polish":
                                                    Native_lang_trans = "POL";
                                                    break;
                                            }

                                            SqlCommand lang_id = new SqlCommand($"SELECT Id FROM Languages WHERE Name = '{Native_lang_trans}'", cnn);
                                            var lang_ids = lang_id.ExecuteScalar();

                                            SqlCommand command = new SqlCommand("INSERT INTO LanguageTranslations (translation,lang_id,native_lang_id)  VALUES( N'" + dt.Rows[i][j].ToString() + "', '" + native_lang_ids + "','" + lang_ids + "')", cnn);
                                            command.ExecuteNonQuery();
                                    }
                                }

                            }

                        if (exelName == "TestsNames.xlsx") //test filling
                        {
                            for (int i = 0; dt.Rows.Count > i; i++)
                            {
                                SqlCommand command = new SqlCommand($"INSERT INTO Tests (Name, icon)  VALUES( '" + dt.Rows[i][0].ToString() + "', '"+ icons[i] +"')", cnn);
                                command.ExecuteNonQuery();
                            }

                            for (int i = 0; dt.Rows.Count > i; i++)
                                for (int j = 1; dt.Columns.Count > j; j++)
                                {
                                    {
                                        SqlCommand native_lang_id = new SqlCommand($"SELECT Id FROM Languages WHERE Name = '{dt.Columns[j].ToString()}'", cnn);
                                        var native_lang_ids = native_lang_id.ExecuteScalar();



                                        SqlCommand test_id = new SqlCommand($"SELECT Id FROM Tests WHERE Name = '{dt.Rows[i][0].ToString()}'", cnn);
                                        var test_ids = test_id.ExecuteScalar();


                                        Console.WriteLine(native_lang_ids);
                                        Console.WriteLine(test_ids);


                                        



                                        SqlCommand command = new SqlCommand("INSERT INTO TestTranslations (translation, lang_id, test_id)  VALUES( N'" + dt.Rows[i][j].ToString() + "', '" + native_lang_ids + "','" + test_ids + "')", cnn);
                                        command.ExecuteNonQuery();
                                    }
                                }

                        }

                        //Words Filling

                        if (ExcelSheetName != "Categories$" && !exelName.StartsWith("~$"))
                            {

                                for (int i = 0; dt.Rows.Count > i; i++)
                                {
                                    string[] picture = Directory.GetFiles("D:/DictionaryForFullStack/", $"{dt.Rows[i][0].ToString()}.jpg ", SearchOption.AllDirectories);

                                
                                    string DirName = Path.GetFileName(Path.GetDirectoryName(file));

                                  


                                    SqlCommand cat_id = new SqlCommand($"SELECT Id FROM Categories WHERE name = @DirName", cnn);
                                    cat_id.Parameters.Add("@DirName", SqlDbType.NVarChar).Value = DirName;
                                    var cat_ids = cat_id.ExecuteScalar();
                                                                   

                                    SqlCommand command = new SqlCommand("INSERT INTO Words (Name, picture, category_id)  VALUES( @words, @picture, '" + cat_ids + "')", cnn);
                                    command.Parameters.Add("@words", SqlDbType.NVarChar).Value = dt.Rows[i][0].ToString();
                                    command.Parameters.Add("@picture", SqlDbType.NVarChar).Value = picture[0].ToString();

                                    command.ExecuteNonQuery();
                               

                                 }

                                for (int i = 0; dt.Rows.Count > i; i++) //wods trans
                                for (int j = 1; 9 > j; j++) //wods trans
                                {
                                    {
                                        SqlCommand lang_id = new SqlCommand($"SELECT Id FROM Languages WHERE Name = '{dt.Columns[j].ToString()}'", cnn);
                                        var lang_ids = lang_id.ExecuteScalar();

                                        SqlCommand word_id = new SqlCommand($"SELECT Id FROM Words WHERE Name = @words", cnn);
                                        word_id.Parameters.Add("@words", SqlDbType.NVarChar).Value = dt.Rows[i][0].ToString();
                                        var word_ids = word_id.ExecuteScalar();

                                        string DirName = Path.GetDirectoryName(file);
                                        Console.WriteLine(dt.Columns[j]);

                                        string[] pronounce = Directory.GetFiles($@"{DirName}\pronounce\{dt.Columns[j].ToString()}", $"{dt.Rows[i][0].ToString()}.wav ", SearchOption.AllDirectories);

                                        SqlCommand command = new SqlCommand("INSERT INTO WordTranslations (translation, word_id, lang_id, pronounce )  VALUES( @words, '" + word_ids + "', '" + lang_ids + "' , @pronounce)", cnn);
                                        command.Parameters.Add("@words", SqlDbType.NVarChar).Value = dt.Rows[i][j].ToString();
                                        command.Parameters.Add("@pronounce", SqlDbType.NVarChar).Value = pronounce[0];
                                        command.ExecuteNonQuery();
                                        
                                    }

                                }
                            }

                            //ctegoriesRoot Filling
                            if (ExcelSheetName == "Categories$" && exelName!= "Languages.xlsx" && exelName != "TestsNames.xlsx")
                            {
                                if (exelName == "CategoriesRoot.xlsx")
                                {
                                    for (int i = 0; dt.Rows.Count > i; i++)
                                    {
                                        string[] picture = Directory.GetFiles("D:/DictionaryForFullStack/", $"{dt.Rows[i][0].ToString()}.jpg ", SearchOption.AllDirectories);


                                        SqlCommand command = new SqlCommand("INSERT INTO Categories (Name, picture)  VALUES( '" + dt.Rows[i][0].ToString() + "', '" + picture[0].ToString() + "')", cnn);

                                        command.ExecuteNonQuery();


                                    }

                                 for (int i = 0; dt.Rows.Count > i; i++) //cat trans fill
                                    for (int j = 1; dt.Columns.Count > j; j++)
                                    {
                                        {
                                            SqlCommand lang_id = new SqlCommand($"SELECT Id FROM Languages WHERE Name = '{dt.Columns[j].ToString()}'", cnn);
                                            var lang_ids = lang_id.ExecuteScalar();

                                            SqlCommand cat_id = new SqlCommand($"SELECT Id FROM Categories WHERE Name = '{dt.Rows[i][0].ToString()}'", cnn);
                                            var cat_ids = cat_id.ExecuteScalar();

                                        
                                            SqlCommand command = new SqlCommand("INSERT INTO CategoriesTranslations (translation, category_id, lang_id )  VALUES( N'" + dt.Rows[i][j].ToString() + "', '" + cat_ids + "', '" + lang_ids + "')", cnn);
                                            command.ExecuteNonQuery();

                                        }
                                    }
                                }


                                else //SubCategoriesFill
                                {
                                    for (int i = 0; dt.Rows.Count > i; i++)
                                    {
                                        string[] picture = Directory.GetFiles("D:/DictionaryForFullStack/", $"{dt.Rows[i][0].ToString()}.jpg ", SearchOption.AllDirectories);

                                        SqlCommand parent_id = new SqlCommand($"SELECT Id FROM Categories WHERE Name ='{exelName.Substring(0, exelName.Length - 5)}'", cnn);
                                        var parent_ids = parent_id.ExecuteScalar();


                                                                              

                                        string sqlExpression = "INSERT INTO Categories(Name, picture, parent_id)  VALUES( @words, @picture, '" + parent_ids + "')";

                                        SqlCommand command = new SqlCommand(sqlExpression, cnn);
                                        command.Parameters.Add("@words", SqlDbType.NVarChar).Value = dt.Rows[i][0].ToString();
                                        command.Parameters.Add("@picture", SqlDbType.NVarChar).Value = picture[0].ToString();
                                        command.ExecuteNonQuery();
                                    }

                                    for (int i = 0; dt.Rows.Count > i; i++) //cat trans fill
                                        for (int j = 1; dt.Columns.Count > j; j++)
                                        {
                                            {
                                                SqlCommand lang_id = new SqlCommand($"SELECT Id FROM Languages WHERE Name = '{dt.Columns[j].ToString()}'", cnn);
                                                var lang_ids = lang_id.ExecuteScalar();

                                                SqlCommand cat_id = new SqlCommand($"SELECT Id FROM Categories WHERE Name = @name", cnn);
                                                cat_id.Parameters.Add("@name", SqlDbType.NVarChar).Value = dt.Rows[i][0].ToString();
                                                var cat_ids = cat_id.ExecuteScalar();

                                                string word = dt.Rows[i][j].ToString();
                                                SqlParameter nameParam = new SqlParameter("@word", word);
                                                

                                                string sqlExpression = @"INSERT INTO CategoriesTranslations (translation, category_id, lang_id )  VALUES( @word, '" + cat_ids + "', '" + lang_ids + "')";
                                                

                                                SqlCommand command = new SqlCommand(sqlExpression, cnn);
                                                command.Parameters.Add(nameParam);
                                                command.ExecuteNonQuery();

                                            }
                                        }
                                }
                            }
                    }
                }
            }
        }
    }
}
