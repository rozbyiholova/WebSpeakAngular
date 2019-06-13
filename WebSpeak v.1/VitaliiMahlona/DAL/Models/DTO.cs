using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class DTO
    {
        public int Id { get; set; }
        public string WordNativeLang { get; set; }
        public string WordLearnLang { get; set; }
        public string Picture { get; set; }
        public string Sound { get; set; }
        public string PronounceLearn { get; set; }
        public string PronounceNative { get; set; }
        public int? SubCategoryId { get; set; }
        public int? CategoryId { get; set; }
        public string EnableNativeLang { get; set; }
        public string EnableSound { get; set; }
        public string EnablePronounceNativeLang { get; set; }
        public string EnablePronounceLearnLang { get; set; }
        public int TestId { get; set; }
        public int Total { get; set; }
        public string UserId { get; set; }
    }
}
