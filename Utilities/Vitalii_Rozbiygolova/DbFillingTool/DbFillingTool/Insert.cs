using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbFillingTool
{
    public class Insert
    {
        public SqlConnection connection { get; set; }

        public Insert(SqlConnection connection)
        {
            this.connection = connection;
        }

        public void Words()
        {

        }

        public void WordTranslations() { }

        public void Categories() { }

        public void CategoryTranslations() { }

        public void Tests() { }

        public void TestTranslations() { }

        private FileInfo[] GetImages(string path)
        {
            FileInfo[] pictures;
            try
            {
                pictures = new DirectoryInfo(path + @"\pictures").GetFiles();
                return pictures;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private FileInfo[] GetAudio() { }
    }
}
