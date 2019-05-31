using DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class WordTranslations
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public string Translation { get; set; }
        public string Pronounce { get; set; }
        public int LangId { get; set; }

        public virtual Languages Lang { get; set; }
        public virtual Words Word { get; set; }
    }
}
