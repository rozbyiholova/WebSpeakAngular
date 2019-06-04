using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class DTO
    {
        public int Id { get; set; }
        public string Native { get; set; }
        public string Translation { get; set; }
        public string Picture { get; set; }
        public string Sound { get; set; }
        public string NativePronounce { get; set; }
        public string TranslationPronounce { get; set; }
    }
}
