using DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class TestResults
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int Result { get; set; }
        public string UserId { get; set; }
        public int LangId { get; set; }
        public int CategoryId { get; set; }
        public DateTime TestDate { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Languages Lang { get; set; }
        public virtual Tests Test { get; set; }
        public virtual Users User { get; set; }
    }
}
