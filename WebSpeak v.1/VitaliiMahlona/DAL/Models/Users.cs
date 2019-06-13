﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Users : IdentityUser
    {
        public Users()
        {
            TestResults = new HashSet<TestResults>();
            TotalScores = new HashSet<TotalScores>();
        }

        public virtual ICollection<TestResults> TestResults { get; set; }
        public virtual ICollection<TotalScores> TotalScores { get; set; }
    }
}
