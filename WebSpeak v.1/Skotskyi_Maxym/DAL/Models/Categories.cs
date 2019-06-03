using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Categories
    {
        public Categories()
        {
            CategoriesTranslations = new HashSet<CategoriesTranslations>();
            InverseParent = new HashSet<Categories>();
            TestResults = new HashSet<TestResults>();
            Words = new HashSet<Words>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Picture { get; set; }

        public virtual Categories Parent { get; set; }
        public virtual ICollection<CategoriesTranslations> CategoriesTranslations { get; set; }
        public virtual ICollection<Categories> InverseParent { get; set; }
        public virtual ICollection<TestResults> TestResults { get; set; }
        public virtual ICollection<Words> Words { get; set; }
    }
}
