using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class TotalScores
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int LangId { get; set; }
        public int Total { get; set; }

        public virtual Languages Lang { get; set; }
    }
}
