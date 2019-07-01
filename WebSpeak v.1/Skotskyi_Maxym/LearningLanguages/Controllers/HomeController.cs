using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LearningLanguages.Models;
using DAL;
using DAL.IRepository;
using DAL.Models;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;
using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;
using System.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;



namespace LearningLanguages.Controllers
{

    public class HomeController : Controller
    {

        private Languages_bdContext db;
        public HomeController(Languages_bdContext context)
        {
            db = context;
        }
   
        public ActionResult GetTestResult (int score, int icon, int lang_id, int cat_id)
        {
            int catId = cat_id;
            int langId = lang_id;
            int TestIcon = icon;
            if (TestIcon == 0) { TestIcon = 10; };

            int Testscore = score;
            string strCurrentUserId = User.Identity.GetUserId();
            int PrevScore;
            int IdPrevScore;

            DateTime currentData = DateTime.Now;

            TestResults testResult = new TestResults();

            TestResultsRepository testResultRepos = new TestResultsRepository();

            var ListTestresults = db.TestResults.Where(x => x.UserId == strCurrentUserId && x.TestId == TestIcon && x.LangId == langId && x.CategoryId == catId).ToList();


            

            if (ListTestresults.Count != 0)
            { 
                PrevScore = ListTestresults[0].Result;
                IdPrevScore = ListTestresults[0].Id;
                if (PrevScore < Testscore)
                {
                    ListTestresults[0].Result = Testscore;
                    ListTestresults[0].TestDate = currentData;
                    db.SaveChanges();

                    var ListTotalScore = db.TotalScores.Where(x => x.UserId == strCurrentUserId).ToList();
                    ListTotalScore[0].Total = Testscore;
                    db.SaveChanges();
                }                      
            }

          

            if (ListTestresults.Count == 0)
            {
                db.TestResults.Add(new TestResults { TestId = TestIcon, TestDate = currentData, Result = Testscore, UserId = strCurrentUserId, CategoryId = catId, LangId = langId });
                db.SaveChanges();


            }

            

            var ListTotalTestresults = db.TestResults.Where(x => x.UserId == strCurrentUserId && x.LangId == langId).ToList();
            
            if ((ListTotalTestresults.Count == 1) && (ListTestresults.Count == 0))
            {
                db.TotalScores.Add(new TotalScores { UserId = strCurrentUserId, LangId = langId, Total = Testscore });
                db.SaveChanges();
            }

            if (ListTotalTestresults.Count > 1)
            {
                int TotalScore = 0;
                var ListTotalScore = db.TotalScores.Where(x => x.UserId == strCurrentUserId && x.LangId == langId).ToList();
                foreach (var item in ListTotalTestresults)
                {
                    TotalScore = TotalScore + item.Result;
                }

                ListTotalScore[0].Total = TotalScore;
                db.SaveChanges();
            }

            return View("_Close"); 
        }


        public  ActionResult LangSwichLearn(string lg)
        {
           HttpContext.Session.SetString("lang", lg);
           return RedirectToAction("Index");
        }

        public ActionResult LangSwichNatevi(string nt_lg)
        {
           HttpContext.Session.SetString("native_lang", nt_lg);
           return RedirectToAction("Index");
        }


        [DefaultBreadcrumb("Home")]
        public IActionResult Index()
        {
            return View();
        }

        [Breadcrumb("Categories")]
        public IActionResult CategoriesView()
        {

            string language = HttpContext.Session.GetString("lang");
            var lang_id = from s in db.Languages where s.Name.ToString() == language select s.Id;
            int[] lgid = lang_id.ToArray();

            string native_lang = HttpContext.Session.GetString("native_lang");
            var native_lang_id = from s in db.Languages where s.Name.ToString() == native_lang select s.Id;
            int[] ntv_lgid = native_lang_id.ToArray();

            CategoriesDTOsRepository categoryDTOs = new CategoriesDTOsRepository();

            List<CategoriesDTO> DTO = categoryDTOs.GetCategoriesDTOs(ntv_lgid[0], lgid[0]);

            return View(DTO.Where(x => x.ParentId == null));
        }




        [Breadcrumb("SubCategories" , FromAction = "CategoriesView", FromController = typeof(HomeController))]
        public IActionResult SubCategoriesView(int id)
        {
            var query = from item in db.Categories where item.ParentId == null select item;

            string language = HttpContext.Session.GetString("lang");
            var lang_id = from s in db.Languages where s.Name.ToString() == language select s.Id;
            int[] lgid = lang_id.ToArray();

            string native_lang = HttpContext.Session.GetString("native_lang");
            var native_lang_id = from s in db.Languages where s.Name.ToString() == native_lang select s.Id;
            int[] ntv_lgid = native_lang_id.ToArray();

            if (id != 0)
            {
                HttpContext.Session.SetInt32("idCat", id);
            }

            if (id == 0)
            {
                id = (int)HttpContext.Session.GetInt32("idCat");
                HttpContext.Session.SetInt32("idCat", id);
            }

            CategoriesDTOsRepository categoryDTOs = new CategoriesDTOsRepository();

            List<CategoriesDTO> DTO = categoryDTOs.GetCategoriesDTOs(ntv_lgid[0], lgid[0]);

            return View(DTO.Where(x => x.ParentId == id));
        }


        [Breadcrumb("Words", FromAction = "SubCategoriesView", FromController = typeof(HomeController))]
        public IActionResult  WordsView(int id)
        {
            string language = HttpContext.Session.GetString("lang");
            var lang_id = from s in db.Languages where s.Name.ToString() == language select s.Id;
            int[] lgid = lang_id.ToArray();

            string native_lang = HttpContext.Session.GetString("native_lang");
            var native_lang_id = from s in db.Languages where s.Name.ToString() == native_lang select s.Id;
            int[] ntv_lgid = native_lang_id.ToArray();

            if (id != 0)
            {
                HttpContext.Session.SetInt32("idWord", id);
            }

            if (id == 0)
            {
                id = (int)HttpContext.Session.GetInt32("idWord");
                HttpContext.Session.SetInt32("idWord", id);
            }

            CategoriesDTOsRepository catreps = new CategoriesDTOsRepository();
            List<CategoriesDTO> catrep = catreps.GetCategoriesDTOs(ntv_lgid[0], lgid[0]);



            var catNameNative = from n in catrep where n.CategoryId == id select n.Native;
            string[] nativ = catNameNative.ToArray();

            var catNameLearn = from n in catrep where n.CategoryId == id select n.Translation;
            string[] lern = catNameLearn.ToArray();



            WordsDTOsRepository WordsDTOs = new WordsDTOsRepository();

            List<CategoriesDTO> DTO = WordsDTOs.GetWordsDTOs(ntv_lgid[0], lgid[0]);

            foreach (var item in DTO)
            {
                item.TestName = $"{lern[0]}({nativ[0]})";
            }

            return View(DTO.Where(x => x.CategoryId == id));

        }

        [Breadcrumb("Test", FromAction = "WordsView", FromController = typeof(HomeController))]
        public IActionResult Test()
        {
            string language = HttpContext.Session.GetString("lang");
            var lang_id = from s in db.Languages where s.Name.ToString() == language select s.Id;
            int[] lgid = lang_id.ToArray();

            string native_lang = HttpContext.Session.GetString("native_lang");
            var native_lang_id = from s in db.Languages where s.Name.ToString() == native_lang select s.Id;
            int[] ntv_lgid = native_lang_id.ToArray();

            TestsDTOsRepository TestDTOs = new TestsDTOsRepository();

            List<CategoriesDTO> DTO = TestDTOs.GetTestDTOs(ntv_lgid[0], lgid[0]);

            return View(DTO);
        }

        [Breadcrumb("Testing", FromAction = "Test", FromController = typeof(HomeController))]
        public IActionResult Testing(int id)
        {
            string language = HttpContext.Session.GetString("lang");
            var lang_id = from s in db.Languages where s.Name.ToString() == language select s.Id;
            int[] lgid = lang_id.ToArray();

            string native_lang = HttpContext.Session.GetString("native_lang");
            var native_lang_id = from s in db.Languages where s.Name.ToString() == native_lang select s.Id;
            int[] ntv_lgid = native_lang_id.ToArray();

            int subCats = (int)HttpContext.Session.GetInt32("idWord");

            WordsDTOsRepository WordsDTOs = new WordsDTOsRepository();
            List<CategoriesDTO> WordDTO = WordsDTOs.GetWordsDTOs(ntv_lgid[0], lgid[0]);

            var TestIcon = from p in db.Tests where p.Id == id select p.Icon;          
            string[] Ticon = TestIcon.ToArray();

            foreach (var item in WordDTO)
            {
                item.TestName =$"{Ticon[0]}";
            }


            return View(WordDTO.Where(x => x.CategoryId == subCats));
        }


        [Breadcrumb("Manual", FromAction = "WordsView", FromController = typeof(HomeController))]
        public IActionResult Manual(int id)
        {

            string language = HttpContext.Session.GetString("lang");
            var lang_id = from s in db.Languages where s.Name.ToString() == language select s.Id;
            int[] lgid = lang_id.ToArray();

            string native_lang = HttpContext.Session.GetString("native_lang");
            var native_lang_id = from s in db.Languages where s.Name.ToString() == native_lang select s.Id;
            int[] ntv_lgid = native_lang_id.ToArray();

            WordsDTOsRepository WordsDTOs = new WordsDTOsRepository();

            List<CategoriesDTO> DTO = WordsDTOs.GetWordsDTOs(ntv_lgid[0], lgid[0]);

            return View(DTO.Where(x => x.CategoryId == id));

        }

        [HttpPost]
        public async Task<IActionResult> Create(Categories cat)
        {
            db.Categories.Add(cat);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
