using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSpeak.Models;
using DAL.Repositories;
using DAL.Models;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace WebSpeak.Controllers
{
    public class HomeController : Controller
    {
        private const string NativeLanguage = "NativeLanguage";
        private const string LearningLanguage = "LearningLanguage";

        private const int DefaultNativeLanguageId = 1;
        private const int DefaultLearningLanguageId = 3;

        public IActionResult Index()
        {

            HttpContext.Session.SetInt32(NativeLanguage, DefaultNativeLanguageId);
            HttpContext.Session.SetInt32(LearningLanguage, DefaultLearningLanguageId);
            return View();
        }

        public IActionResult Languages()
        {
            LanguagesRepository languagesRepository = new LanguagesRepository();
            SelectList languages = new SelectList(languagesRepository.GetAll().ToList(), "Id", "Name");
            return View(languages);
        }

        [HttpPost]
        public IActionResult Languages(IFormCollection form)
        {
            int nativeLangId, learningLangId;
            try
            {
                nativeLangId = Convert.ToInt32(form["Language1"]);
                learningLangId = Convert.ToInt32(form["Language2"]);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Set default languages: Ukrainian(native) and English(learning)");
                nativeLangId = DefaultNativeLanguageId;
                learningLangId = DefaultLearningLanguageId;
            }
            HttpContext.Session.SetInt32(NativeLanguage, nativeLangId);
            HttpContext.Session.SetInt32(LearningLanguage, learningLangId);

            return RedirectToAction(nameof(Categories));
        }

        public IActionResult Categories()
        {
            CategoriesRepository categoriesRepository = new CategoriesRepository();
            Tuple<int, int> ids = GetLanguagesId();
            int nativeLang = ids.Item1;
            int learningLang = ids.Item2;
            List<DTO> DTOs = categoriesRepository.GetDTO(nativeLang, learningLang, null);

            return View(DTOs);
        }

        public IActionResult SubCategories(int id)
        {
            CategoriesRepository categoriesRepository = new CategoriesRepository();
            Tuple<int, int> ids = GetLanguagesId();
            int nativeLang = ids.Item1;
            int learningLang = ids.Item2;
            List<DTO> DTOs = categoriesRepository.GetDTO(nativeLang, learningLang, id);

            return View(DTOs);
        }

        public async Task<IActionResult> Manual(int categoryId)
        {
            Tuple<int, int> ids = GetLanguagesId();
            int nativeLang = ids.Item1;
            int learningLang = ids.Item2;
            WordsRepository wordsRepository = new WordsRepository();
            List<DTO> words = wordsRepository.GetDTO(nativeLang, learningLang, categoryId);

            return View(words);
        }

        public async Task<IActionResult> SlideShow(int categoryId)
        {
            Tuple<int, int> ids = GetLanguagesId();
            int nativeLang = ids.Item1;
            int learningLang = ids.Item2;
            WordsRepository wordsRepository = new WordsRepository();
            List<DTO> words = wordsRepository.GetDTO(nativeLang, learningLang, categoryId);

            return View(words);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Tuple<int, int> GetLanguagesId()
        {
            int nativeLang, learningLang;
            try
            {
                nativeLang = (int)HttpContext.Session.GetInt32(NativeLanguage);
                learningLang = (int)HttpContext.Session.GetInt32(LearningLanguage);
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
    }
}
