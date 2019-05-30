using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class CategoriesTranslations
    {
        public int Id { get; set; }
        public string Translation { get; set; }
        public int CategoryId { get; set; }
        public int LangId { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Languages Lang { get; set; }
    }
}
