using DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Words
    {
        public Words()
        {
            WordTranslations = new HashSet<WordTranslations>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Sound { get; set; }
        public string Picture { get; set; }

        public virtual Categories Category { get; set; }
        public virtual ICollection<WordTranslations> WordTranslations { get; set; }
    }
}
