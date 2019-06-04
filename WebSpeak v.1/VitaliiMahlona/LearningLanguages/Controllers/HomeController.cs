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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LearningLanguages.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Categories> _categories = new CategoriesRepository();

        IRepository<Languages> _languages = new LanguagesRepository();

        IRepository<Words> _words = new WordsRepository();

        IRepository<Tests> _tests = new TestsRepository();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SelectList languagesList = new SelectList(await _languages.GetList(), "Id", "Name");

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
        public async Task<IActionResult> Categories()
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLangCat = await _categories.GetTranslations(idLangLearn, idLangNative, null);

            return View(NativeLearnLangCat);
        }

        [Route("Home/Categories/SubCategories")]
        [HttpGet]
        public async Task<IActionResult> SubCategories(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLangSubCat =await _categories.GetTranslations(idLangLearn, idLangNative, id);

            return View(NativeLearnLangSubCat);
        }

        [Route("Home/Categories/SubCategories/Tests")]
        public async Task<IActionResult> Tests(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLangTests = await _tests.GetTranslations(idLangLearn, idLangNative, id);

            Categories category = await _categories.GetItem(id);

            NativeLearnLangTests.First().CategoryId = category.ParentId;
            NativeLearnLangTests.First().SubCategoryId = category.Id;

            return View(NativeLearnLangTests);
        }

        [Route("Home/Categories/SubCategories/Tests/Manual")]
        public async Task<IActionResult> Manual(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");
            string enableNativeLang = HttpContext.Session.GetString("enableNativeLang");
            string enableSound = HttpContext.Session.GetString("enableSound");
            string enablePronounceNativeLang = HttpContext.Session.GetString("enablePronounceNativeLang");
            string enablePronounceLearnLang = HttpContext.Session.GetString("enablePronounceLearnLang");

            List<DTO> NativeLearnLangWords = await _words.GetTranslations(idLangLearn, idLangNative, id);

            Categories category = await _categories.GetItem(id);

            NativeLearnLangWords.First().CategoryId = category.ParentId;
            NativeLearnLangWords.First().SubCategoryId = category.Id;

            ViewBag.enableNativeLang = enableNativeLang;
            ViewBag.enableSound = enableSound;
            ViewBag.enablePronounceNativeLang = enablePronounceNativeLang;
            ViewBag.enablePronounceLearnLang = enablePronounceLearnLang;

            return View(NativeLearnLangWords);
        }

        [Route("Home/Categories/SubCategories/Tests/Slideshow")]
        public async Task<IActionResult> Slideshow(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLangWords = await _words.GetTranslations(idLangLearn, idLangNative, id);

            Categories category = await _categories.GetItem(id);

            NativeLearnLangWords.First().CategoryId = category.ParentId;
            NativeLearnLangWords.First().SubCategoryId = category.Id;

            return View(NativeLearnLangWords);
        }

        [Route("Home/Categories/SubCategories/Tests/Test01")]
        public async Task<IActionResult> Test01(int id)
        {
            Categories category = await _categories.GetItem(id);

            return View(category);
        }

        [Route("Home/Categories/SubCategories/Tests/Test01or05/Test")]
        [HttpGet]
        public async Task<IActionResult> Test01or05One(int id)
        {
            int countOptions = 2;

            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            Random rand = new Random();

            var LearnLangWords = await _words.GetTranslations(idLangLearn, idLangNative, id);

            int[] randomWordsId = new int[countOptions];

            List<DTO> twoWords = new List<DTO>();

            for (int i = 0; i < countOptions; ++i)
            {
                randomWordsId[i] = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);

                for (int j = 0; j < i; j++)
                {
                    while (randomWordsId[j] == randomWordsId[i])
                    {
                        randomWordsId[i] = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);
                    }
                }
                twoWords.Add(LearnLangWords.Find(w => w.Id == randomWordsId[i]));
            }

            return new JsonResult(twoWords);
        }

        [Route("Home/Categories/SubCategories/Tests/Test02")]
        public async Task<IActionResult> Test02(int id)
        {
            Categories category = await _categories.GetItem(id);

            return View(category);
        }

        [Route("Home/Categories/SubCategories/Tests/Test02or03or04or08or09/Test")]
        [HttpGet]
        public async Task<IActionResult> Test02or03or04or08or09One(int id)
        {
            int countOptions = 4;

            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            Random rand = new Random();

            var LearnLangWords = await _words.GetTranslations(idLangLearn, idLangNative, id);

            int[] randomWordsId = new int[countOptions];

            List<DTO> fourWords = new List<DTO>();

            for (int i = 0; i < countOptions; ++i)
            {
                randomWordsId[i] = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);

                for (int j = 0; j < i; j++)
                {
                    while (randomWordsId[j] == randomWordsId[i])
                    {
                        randomWordsId[i] = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);
                    }
                }
                fourWords.Add(LearnLangWords.Find(w => w.Id == randomWordsId[i]));
            }

            return new JsonResult(fourWords);
        }

        [Route("Home/Categories/SubCategories/Tests/Test03")]
        public async Task<IActionResult> Test03(int id)
        {
            Categories category = await _categories.GetItem(id);

            return View(category);
        }

        [Route("Home/Categories/SubCategories/Tests/Test04")]
        public async Task<IActionResult> Test04(int id)
        {
            Categories category = await _categories.GetItem(id);

            return View(category);
        }

        [Route("Home/Categories/SubCategories/Tests/Test05")]
        public async Task<IActionResult> Test05(int id)
        {
            Categories category = await _categories.GetItem(id);

            return View(category);
        }

        [Route("Home/Categories/SubCategories/Tests/Test06")]
        public async Task<IActionResult> Test06(int id)
        {
            Categories category = await _categories.GetItem(id);

            return View(category);
        }

        [Route("Home/Categories/SubCategories/Tests/Test06or07/Test")]
        [HttpGet]
        public async Task<IActionResult> Test06or07One(int id)
        {
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            Random rand = new Random();

            var LearnLangWords = await _words.GetTranslations(idLangLearn, idLangNative, id);

            int randomWordId = rand.Next(LearnLangWords.First().Id, LearnLangWords.Last().Id + 1);

            DTO Word = LearnLangWords.Find(w => w.Id == randomWordId);

            return new JsonResult(Word);
        }

        [Route("Home/Categories/SubCategories/Tests/Test07")]
        public async Task<IActionResult> Test07(int id)
        {
            Categories category = await _categories.GetItem(id);

            return View(category);
        }

        [Route("Home/Categories/SubCategories/Tests/Test08")]
        public async Task<IActionResult> Test08(int id)
        {
            Categories category = await _categories.GetItem(id);

            return View(category);
        }

        [Route("Home/Categories/SubCategories/Tests/Test09")]
        public async Task<IActionResult> Test09(int id)
        {
            Categories category = await _categories.GetItem(id);

            return View(category);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
