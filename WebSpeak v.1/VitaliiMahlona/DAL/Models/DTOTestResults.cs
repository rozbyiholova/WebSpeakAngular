using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class DTOTestResults
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public int Result { get; set; }
        public string LangName { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }
        public DateTime TestDate { get; set; }
        public int Total { get; set; }
    }
}
