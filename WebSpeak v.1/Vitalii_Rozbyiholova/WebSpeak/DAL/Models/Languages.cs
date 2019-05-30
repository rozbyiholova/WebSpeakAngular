using DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Languages
    {
        public Languages()
        {
            CategoriesTranslations = new HashSet<CategoriesTranslations>();
            LanguageTranslationsLang = new HashSet<LanguageTranslations>();
            LanguageTranslationsNativeLang = new HashSet<LanguageTranslations>();
            TestResults = new HashSet<TestResults>();
            TestTranslations = new HashSet<TestTranslations>();
            TotalScores = new HashSet<TotalScores>();
            WordTranslations = new HashSet<WordTranslations>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CategoriesTranslations> CategoriesTranslations { get; set; }
        public virtual ICollection<LanguageTranslations> LanguageTranslationsLang { get; set; }
        public virtual ICollection<LanguageTranslations> LanguageTranslationsNativeLang { get; set; }
        public virtual ICollection<TestResults> TestResults { get; set; }
        public virtual ICollection<TestTranslations> TestTranslations { get; set; }
        public virtual ICollection<TotalScores> TotalScores { get; set; }
        public virtual ICollection<WordTranslations> WordTranslations { get; set; }
    }
}
