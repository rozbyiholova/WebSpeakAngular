using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTOs
{
    public class CategoriesDTO
    {
        public int CategoryId { get; set; }
        public string Native { get; set; }
        public string Translation { get; set; }
        public int LanguageId { get; set; }
        public string Picture { get; set; }
        public int? ParentId { get; set; }
    }
}
