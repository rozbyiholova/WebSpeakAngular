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
            // cnn.Close();


            string[] allFoundFiles = Directory.GetFiles("D:/DictionaryForFullStack/", "*.xlsx ", SearchOption.AllDirectories);

            foreach (string file in allFoundFiles)
            {
                Console.WriteLine(file);

                string excelConnectionString =$"Provider=Microsoft.Jet.OLEDB.4.0; Data Source={file};" ;

                Console.WriteLine(excelConnectionString);

                Console.ReadKey();
               
                    string exelName = Path.GetFileName(file);
                    Console.WriteLine(exelName);                

                string excelCS = string.Format($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={file};Extended Properties=Excel 8.0", exelName);
                if (!exelName.StartsWith("~$"))
                {
                    using (OleDbConnection con = new OleDbConnection(excelCS))
                    {

                        con.Open();
                        Console.WriteLine("file opened");
                        
                        DataTable dtsheet = new DataTable();

                        dtsheet = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string ExcelSheetName = dtsheet.Rows[0]["Table_Name"].ToString();
                        Console.WriteLine($" - nazva lista - {ExcelSheetName}");
                                               
                        Console.WriteLine($" - bez ost simvola - {ExcelSheetName.Substring(0, ExcelSheetName.Length - 1)}");

                        OleDbCommand cmd = new OleDbCommand($"select * from [{ExcelSheetName}] WHERE [name] IS NOT NULL", con);
                        OleDbDataAdapter adap = new OleDbDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adap.Fill(ds);


                        foreach (DataTable dt in ds.Tables)
                        {



                            for (int i = 0; dt.Rows.Count > i; i++)
                            {
                                
                                string[] picture = Directory.GetFiles("D:/DictionaryForFullStack/", $"{dt.Rows[i][0].ToString()}.jpg ", SearchOption.AllDirectories);

                                SqlCommand command = new SqlCommand("INSERT INTO Categories (Name, picture)  VALUES( '"+dt.Rows[i][0].ToString()+"', '"+picture[0].ToString()+"')", cnn);  
                                command.ExecuteNonQuery();


                                Console.WriteLine(picture[0]);
                                Console.ReadKey();

                            }

                            Console.WriteLine(ExcelSheetName); // название таблицы
                                                             // перебор всех столбцов
                            foreach (DataColumn column in dt.Columns)
                                Console.Write("\t{0}", column.ColumnName);
                            Console.WriteLine();
                            // перебор всех строк таблицы
                            foreach (DataRow row in dt.Rows)
                            {
                                // получаем все ячейки строки
                                var cells = row.ItemArray;
                                foreach (object cell in cells)
                                    Console.Write("\t{0}", cell);
                                Console.WriteLine();
                            }
                        }



                 



                        Console.ReadKey();


                        

                        con.Close();



                    }

                }
                

            }

        }
    }
}
