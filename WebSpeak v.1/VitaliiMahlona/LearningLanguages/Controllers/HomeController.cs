using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LearningLanguages.Models;
using DAL.Repositories;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace LearningLanguages.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Categories> categories = new CategoriesRepository();

        IRepository<Languages> languages = new LanguagesRepository();

        IRepository<Words> words = new WordsRepository();

        IRepository<Tests> tests = new TestsRepository();

        [HttpGet]
        public IActionResult Index()
        {
            SelectList languagesList = new SelectList(languages.GetList(), "Id", "Name");

            return View(languagesList);
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            int idLangNative = Convert.ToInt32(form["idLangNative"]);
            int idLangLearn = Convert.ToInt32(form["idLangLearn"]);
            string enableNativeLang = Convert.ToString(form["enableNativeLang"]);

            HttpContext.Session.SetString("enableNativeLang", enableNativeLang);
            HttpContext.Session.SetInt32("idLangNative", idLangNative);
            HttpContext.Session.SetInt32("idLangLearn", idLangLearn);

            return RedirectToAction("Categories");
        }
        [Route("Home/Categories")]
        public IActionResult Categories()
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLangCat = categories.GetTranslations(idLangLearn, idLangNative, null);

            return View(NativeLearnLangCat);
        }

        [Route("Home/Categories/SubCategories")]
        [HttpGet]
        public IActionResult SubCategories(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLangSubCat = categories.GetTranslations(idLangLearn, idLangNative, id);

            ViewBag.categoryId = id;

            return View(NativeLearnLangSubCat);
        }

        [Route("Home/Categories/SubCategories/Tests")]
        public IActionResult Tests(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLangTests = tests.GetTranslations(idLangLearn, idLangNative, id);

            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View(NativeLearnLangTests);
        }

        [Route("Home/Categories/SubCategories/Tests/Manual")]
        public IActionResult Manual(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLangWords = words.GetTranslations(idLangLearn, idLangNative, id);

            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View(NativeLearnLangWords);
        }

        [Route("Home/Categories/SubCategories/Tests/Slideshow")]
        public IActionResult Slideshow(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLangWords = words.GetTranslations(idLangLearn, idLangNative, id);

            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View(NativeLearnLangWords);
        }

        [Route("Home/Categories/SubCategories/Tests/Test01")]
        public IActionResult Test01(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");
            string enableNativeLang = HttpContext.Session.GetString("enableNativeLang");

            Random rand = new Random();

            var LearnLangWords = words.GetTranslations(idLangLearn, idLangNative, id);

            int randomWordId1 = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);
            int randomWordId2 = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);
            while (randomWordId1 == randomWordId2) {
                randomWordId2 = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);
            }

            DTO word1 = LearnLangWords.Find(w => w.Id == randomWordId1);
            DTO word2 = LearnLangWords.Find(w => w.Id == randomWordId2);

            List<DTO> twoWords = new List<DTO>() { word1, word2 };

            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;
            ViewBag.enableNativeLang = enableNativeLang;

            return View(twoWords);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
