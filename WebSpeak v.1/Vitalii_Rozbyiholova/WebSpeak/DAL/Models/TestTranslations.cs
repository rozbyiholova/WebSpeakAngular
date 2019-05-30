using DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class TestTranslations
    {
        public int Id { get; set; }
        public string Translation { get; set; }
        public int TestId { get; set; }
        public int LangId { get; set; }

        public virtual Languages Lang { get; set; }
        public virtual Tests Test { get; set; }
    }
}
