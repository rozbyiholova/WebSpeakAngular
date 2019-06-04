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
            string enableSound = Convert.ToString(form["enableSound"]);
            string enablePronounceNativeLang = Convert.ToString(form["enablePronounceNativeLang"]);
            string enablePronounceLearnLang = Convert.ToString(form["enablePronounceLearnLang"]);

            HttpContext.Session.SetInt32("idLangNative", idLangNative);
            HttpContext.Session.SetInt32("idLangLearn", idLangLearn);
            HttpContext.Session.SetString("enableNativeLang", enableNativeLang);
            HttpContext.Session.SetString("enableSound", enableSound);
            HttpContext.Session.SetString("enablePronounceNativeLang", enablePronounceNativeLang);
            HttpContext.Session.SetString("enablePronounceLearnLang", enablePronounceLearnLang);

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
            string enableNativeLang = HttpContext.Session.GetString("enableNativeLang");
            string enableSound = HttpContext.Session.GetString("enableSound");
            string enablePronounceNativeLang = HttpContext.Session.GetString("enablePronounceNativeLang");
            string enablePronounceLearnLang = HttpContext.Session.GetString("enablePronounceLearnLang");

            List<DTO> NativeLearnLangWords = words.GetTranslations(idLangLearn, idLangNative, id);

            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;
            ViewBag.enableNativeLang = enableNativeLang;
            ViewBag.enableSound = enableSound;
            ViewBag.enablePronounceNativeLang = enablePronounceNativeLang;
            ViewBag.enablePronounceLearnLang = enablePronounceLearnLang;

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
            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View();
        }

        [Route("Home/Categories/SubCategories/Tests/Test01or05/Test")]
        [HttpGet]
        public IActionResult Test01or05One(int id)
        {
            int countOptions = 2;

            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            Random rand = new Random();

            var LearnLangWords = words.GetTranslations(idLangLearn, idLangNative, id);

            int[] randomWordsId = new int[countOptions];

            List<DTO> twoWords = new List<DTO>();

            for (int i = 0; i < countOptions; ++i)
            {
            a: randomWordsId[i] = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);
                for (int j = 0; j < i; j++)
                {
                    if (randomWordsId[j] == randomWordsId[i]) goto a;
                }
                twoWords.Add(LearnLangWords.Find(w => w.Id == randomWordsId[i]));
            }

            return new JsonResult(twoWords);
        }

        [Route("Home/Categories/SubCategories/Tests/Test02")]
        public IActionResult Test02(int id)
        {
            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View();
        }

        [Route("Home/Categories/SubCategories/Tests/Test02or03or04or08or09/Test")]
        [HttpGet]
        public IActionResult Test02or03or04or08or09One(int id)
        {
            int countOptions = 4;

            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            Random rand = new Random();

            var LearnLangWords = words.GetTranslations(idLangLearn, idLangNative, id);

            int[] randomWordsId = new int[countOptions];

            List<DTO> fourWords = new List<DTO>();

            for (int i = 0; i < countOptions; ++i)
            {
                a:  randomWordsId[i] = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);
                for (int j = 0; j < i; j++)
                {
                    if (randomWordsId[j] == randomWordsId[i]) goto a;
                }
                fourWords.Add(LearnLangWords.Find(w => w.Id == randomWordsId[i]));
            }

            return new JsonResult(fourWords);
        }

        [Route("Home/Categories/SubCategories/Tests/Test03")]
        public IActionResult Test03(int id)
        {
            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View();
        }

        [Route("Home/Categories/SubCategories/Tests/Test04")]
        public IActionResult Test04(int id)
        {
            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View();
        }

        [Route("Home/Categories/SubCategories/Tests/Test05")]
        public IActionResult Test05(int id)
        {
            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View();
        }

        [Route("Home/Categories/SubCategories/Tests/Test06")]
        public IActionResult Test06(int id)
        {
            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View();
        }

        [Route("Home/Categories/SubCategories/Tests/Test06or07/Test")]
        [HttpGet]
        public IActionResult Test06or07One(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            Random rand = new Random();

            var LearnLangWords = words.GetTranslations(idLangLearn, idLangNative, id);

            int randomWordId = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);

            DTO Word = LearnLangWords.Find(w => w.Id == randomWordId);

            return new JsonResult(Word);
        }

        [Route("Home/Categories/SubCategories/Tests/Test07")]
        public IActionResult Test07(int id)
        {
            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View();
        }

        [Route("Home/Categories/SubCategories/Tests/Test08")]
        public IActionResult Test08(int id)
        {
            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View();
        }

        [Route("Home/Categories/SubCategories/Tests/Test09")]
        public IActionResult Test09(int id)
        {
            ViewBag.subCategoryId = id;
            ViewBag.categoryId = categories.GetItem(id).ParentId;

            return View();
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
