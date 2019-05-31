using DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class LanguageTranslations
    {
        public int Id { get; set; }
        public string Translation { get; set; }
        public int LangId { get; set; }
        public int NativeLangId { get; set; }

        public virtual Languages Lang { get; set; }
        public virtual Languages NativeLang { get; set; }
    }
}
