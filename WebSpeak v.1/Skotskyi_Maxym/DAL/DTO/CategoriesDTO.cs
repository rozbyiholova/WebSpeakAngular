using System;
using System.Collections.Generic;
using System.Text;
using DAL.ModelConfiguration;

namespace DAL.DTO
{
    public class CategoriesDTO
    {
        public int CategoryId { get; set; }
        public string Native { get; set; }
        public string Translation { get; set; }
        public int LanguageId { get; set; }
        public string Picture { get; set; }
        public int? ParentId { get; set; }
        public string Sound { get; set; }
        public string PronounceNative { get; set; }
        public string PronounceLearn { get; set; }
        public string TestName { get; set; }
    }
}
