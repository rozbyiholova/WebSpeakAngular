using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Tests
    {
        public Tests()
        {
            TestResults = new HashSet<TestResults>();
            TestTranslations = new HashSet<TestTranslations>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<TestResults> TestResults { get; set; }
        public virtual ICollection<TestTranslations> TestTranslations { get; set; }
    }
}
