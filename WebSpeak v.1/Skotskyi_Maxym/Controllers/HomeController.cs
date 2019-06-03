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

namespace LearningLanguages.Controllers
{

    public class HomeController : Controller
    {

        private Languages_bdContext db;
        public HomeController(Languages_bdContext context)
        {
            db = context;
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

        public async Task<IActionResult> Index()
        {
            return View();
        }

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

        public IActionResult SubCategoriesView(int id)
        {

            var query = from item in db.Categories where item.ParentId == null select item;

            string language = HttpContext.Session.GetString("lang");
            var lang_id = from s in db.Languages where s.Name.ToString() == language select s.Id;
            int[] lgid = lang_id.ToArray();


            string native_lang = HttpContext.Session.GetString("native_lang");
            var native_lang_id = from s in db.Languages where s.Name.ToString() == native_lang select s.Id;
            int[] ntv_lgid = native_lang_id.ToArray();


            CategoriesDTOsRepository categoryDTOs = new CategoriesDTOsRepository();

            List<CategoriesDTO> DTO = categoryDTOs.GetCategoriesDTOs(ntv_lgid[0], lgid[0]);

            return View(DTO.Where(x => x.ParentId == id));
        }

        public IActionResult  WordsView(int id)
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

        public async Task<IActionResult> WordsTranslationsView(int id)
        {


            string language = HttpContext.Session.GetString("lang");
            var lang_id = from s in db.Languages where s.Name.ToString() == language select s.Id;
            int[] lgid = lang_id.ToArray();

            string native_lang = HttpContext.Session.GetString("native_lang");
            var native_lang_id = from s in db.Languages where s.Name.ToString() == native_lang select s.Id;
            int[] ntv_lgid = native_lang_id.ToArray();



            var query = from item in db.WordTranslations where item.LangId == lgid[0] select item;

            return View(await query.ToListAsync());
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
