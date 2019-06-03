using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Users
    {
        public Users()
        {
            TestResults = new HashSet<TestResults>();
            TotalScores = new HashSet<TotalScores>();
        }

        public string Id { get; set; }

        public virtual ICollection<TestResults> TestResults { get; set; }
        public virtual ICollection<TotalScores> TotalScores { get; set; }
    }
}
