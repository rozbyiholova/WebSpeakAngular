using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebSpeak.Models
{
    public class StatisticViewModel
    {
        public string LanguageName { get; set; }
        public List<CategoryStatistic> CategoryStatistics { get; set; }
        public int TotalScore => CategoryStatistics.Sum(c => c.CategoryScore);
    }
}