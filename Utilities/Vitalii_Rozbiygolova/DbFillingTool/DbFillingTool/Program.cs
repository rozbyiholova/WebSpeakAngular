using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbFillingTool
{
    class Program
    {
        static void Main(string[] args)
        {
         FilesHandler.LoadMainEntities();
            Console.WriteLine("All done");
          Console.ReadKey();
        }
    }
}
