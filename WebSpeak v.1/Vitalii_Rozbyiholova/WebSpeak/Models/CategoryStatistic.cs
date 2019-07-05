using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSpeak.Models
{
    public class CategoryStatistic
    {
        public string CategoryName { get; set; }
        public List<SubcategoryStatistic> SubcategoryStatistics { get; set; }
        public int CategoryScore => SubcategoryStatistics.Sum(s => s.Score);
    }
}
