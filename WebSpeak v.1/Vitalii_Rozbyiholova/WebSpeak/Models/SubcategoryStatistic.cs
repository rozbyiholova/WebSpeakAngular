using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSpeak.Models
{
    [Serializable]
    public class SubcategoryStatistic
    {
        public string SubcategoryName { get; set; }
        public List<string> TestNames { get; set; }
        public List<int> TestsScore { get; set; }
        public int CategoryId { get; set; }

        public int Score => TestsScore?.Sum() ?? 0;
    }
}

