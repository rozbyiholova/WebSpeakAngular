using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DbFillingTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            FilesHandler.LoadMainEntities();
            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadKey();
        }
    }
}
