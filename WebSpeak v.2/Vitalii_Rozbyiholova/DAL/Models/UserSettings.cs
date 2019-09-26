using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class UserSettings
    {
        public int Id { get; set; }
        public bool? SlidePause { get; set; }
        public bool? TranslationText { get; set; }
        public bool? PronounceLearning { get; set; }
        public bool? PronounceNative { get; set; }
        public string UserId { get; set; }
        public int NativeLanguageId { get; set; }
        public int LearningLanguageId { get; set; }

        public virtual Users User { get; set; }
    }
}
