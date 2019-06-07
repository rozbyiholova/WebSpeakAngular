using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSpeak
{
    public class SessionHelper
    {
        private const string NativeLanguage = "NativeLanguage";
        private const string LearningLanguage = "LearningLanguage";
        private const string LastCategoryId = "LastCategoryId";
        private const string LastSubcategoryId = "LastSubcategoryId";

        private const int DefaultNativeLanguageId = 1;
        private const int DefaultLearningLanguageId = 3;

        IHttpContextAccessor _httpContextAccessor;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetLanguages(IFormCollection form = null)
        {
            int nativeLangId, learningLangId;
            try
            {
                nativeLangId = Convert.ToInt32(form["Language1"]);
                learningLangId = Convert.ToInt32(form["Language2"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Set default languages: Ukrainian(native) and English(learning)");
                nativeLangId = DefaultNativeLanguageId;
                learningLangId = DefaultLearningLanguageId;
            }
            _httpContextAccessor.HttpContext.Session.SetInt32(NativeLanguage, nativeLangId);
            _httpContextAccessor.HttpContext.Session.SetInt32(LearningLanguage, learningLangId);
        }
        public Tuple<int, int> GetLanguagesId()
        {
            int nativeLang, learningLang;
            try
            {
                nativeLang = (int)_httpContextAccessor.HttpContext.Session.GetInt32(NativeLanguage);
                learningLang = (int)_httpContextAccessor.HttpContext.Session.GetInt32(LearningLanguage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Set default languages: Ukrainian(native) and English(learning)");
                nativeLang = DefaultNativeLanguageId;
                learningLang = DefaultLearningLanguageId;
            }
            return new Tuple<int, int>(nativeLang, learningLang);
        }

        public void SetCategory(int id)
        {
            try
            {
                _httpContextAccessor.HttpContext.Session.SetInt32(LastCategoryId, id);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public int GetLastСategoryId()
        {
            int id = (int)_httpContextAccessor.HttpContext.Session.GetInt32(LastCategoryId);
            return id;
        }

        public void SetSubcategory(int id)
        {
            try
            {
                _httpContextAccessor.HttpContext.Session.SetInt32(LastSubcategoryId, id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public int GetLastSubcategoryId()
        {
            int id = (int)_httpContextAccessor.HttpContext.Session.GetInt32(LastSubcategoryId);
            return id;
        }
    }
}
