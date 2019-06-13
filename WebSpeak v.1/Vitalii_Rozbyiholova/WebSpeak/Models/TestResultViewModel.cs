using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSpeak.Models
{
    public class TestResultViewModel
    {
        public string[] QuestionNames { get; set; }

        public bool[] QuestionResults { get; set; }

        public int Total
        {
            get
            {
                int res = 0;
                foreach (bool i in QuestionResults)
                {
                    if (i) { res++; }
                }
                return res;
            }
        }
    }
}
