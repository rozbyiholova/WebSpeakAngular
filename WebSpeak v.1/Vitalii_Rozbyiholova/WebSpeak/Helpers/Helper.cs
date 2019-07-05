using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using WebSpeak.Areas.Identity.Pages.Account.Manage;
using WebSpeak.Models;

namespace WebSpeak
{
    public class Helper
    {
        private const string NativeLanguage = "NativeLanguage";
        private const string LearningLanguage = "LearningLanguage";
        private const string LastCategoryId = "LastCategoryId";
        private const string LastSubcategoryId = "LastSubcategoryId";
        private const string LastTestId = "LastTestId";

        private const int DefaultNativeLanguageId = 1;
        private const int DefaultLearningLanguageId = 3;

        private readonly ProductHouseContext _db;

        static IHttpContextAccessor _httpContextAccessor;

        public Helper(IHttpContextAccessor httpContextAccessor)
        {
            _db = new ProductHouseContext();
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

        public static Tuple<int, int> GetLanguagesId()
        {
            int nativeLang, learningLang;
            try
            {
                nativeLang = (int) _httpContextAccessor.HttpContext.Session.GetInt32(NativeLanguage);
                learningLang = (int) _httpContextAccessor.HttpContext.Session.GetInt32(LearningLanguage);
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public int GetLastСategoryId()
        {
            int id = (int) _httpContextAccessor.HttpContext.Session.GetInt32(LastCategoryId);
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
            int id = (int) _httpContextAccessor.HttpContext.Session.GetInt32(LastSubcategoryId);
            return id;
        }

        public void SetTest(int id)
        {
            try
            {
                _httpContextAccessor.HttpContext.Session.SetInt32(LastTestId, id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public int GetLastTestId()
        {
            int id = (int) _httpContextAccessor.HttpContext.Session.GetInt32(LastTestId);
            return id;
        }


        public bool IsTestDoneOnce(string userId, int testId, int langId, int categoryId, out TestResults oldResults)
        {
            oldResults = null;
            TestResults results = null;
            try
            {
                results = _db.TestResults.First(r => r.UserId == userId &&
                                                     r.TestId == testId &&
                                                     r.CategoryId == categoryId &&
                                                     r.LangId == langId);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("New test result");
            }
            catch
            {
                throw;
            }

            if (results != null)
            {
                oldResults = results;
                return true;
            }
            else { return false; }
        }

        public void UpdateTotalScore(string userId, int langId)
        {
            int total = 0;
            List<TestResults> results = _db.TestResults.Where(r => r.UserId == userId && r.LangId == langId).ToList();
            foreach (TestResults testResult in results)
            {
                total += testResult.Result;
            }

            TotalScores totalScores = null;
            try
            {
                totalScores = _db.TotalScores.First(s => s.UserId == userId && s.LangId == langId);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("New test result");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            if (totalScores != null)
            {
                totalScores.Total = total;
                _db.Entry(totalScores).State = EntityState.Modified;
            }
            else
            {
                totalScores = new TotalScores();
                totalScores.LangId = langId;
                totalScores.UserId = userId;
                totalScores.Total = total;
                _db.TotalScores.Add(totalScores);
            }

            _db.SaveChanges();
        }
    }
}
