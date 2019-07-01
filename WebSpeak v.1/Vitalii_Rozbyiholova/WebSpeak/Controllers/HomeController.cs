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
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;

namespace WebSpeak.Controllers
{
    public class HomeController : Controller
    {
        private readonly SessionHelper helper;

        public HomeController(SessionHelper helper)
        {
            this.helper = helper;
        }

        [DefaultBreadcrumb("Home")]
        public IActionResult Index()
        {
            helper.SetLanguages();
            return View();
        }

        [Breadcrumb("Languages")]
        public IActionResult Languages()
        {
            IRepository<Languages> languagesRepository = new LanguagesRepository();
            List<Languages> languagesList = languagesRepository.GetAll().ToList();
            SelectList languages = new SelectList(languagesList, "Id", "Name");
            return View(languages);
        }

        [HttpPost]
        public IActionResult Languages(IFormCollection form)
        {
            helper.SetLanguages(form);
            return RedirectToAction(nameof(Categories));
        }

        [Breadcrumb("Categories")]
        public IActionResult Categories()
        {
            CategoriesRepository categoriesRepository = new CategoriesRepository();
            Tuple<int, int> ids = helper.GetLanguagesId();
            int nativeLang = ids.Item1;
            int learningLang = ids.Item2;
            List<DTO> DTOs = categoriesRepository.GetDTO(nativeLang, learningLang, null);

            return View(DTOs);
        }

        [Breadcrumb("Subcategories", FromAction = "Categories", FromController = typeof(HomeController))]
        [Route("Categories/Subcategories")]
        public IActionResult SubCategories(int id)
        {
            int idToUse = id;
            if(idToUse > 0)
            {
                helper.SetCategory(id);
            }
            else
            {
                idToUse = helper.GetLastСategoryId();
            }
            CategoriesRepository categoriesRepository = new CategoriesRepository();
            Tuple<int, int> ids = helper.GetLanguagesId();
            int nativeLang = ids.Item1;
            int learningLang = ids.Item2;
            List<DTO> DTOs = categoriesRepository.GetDTO(nativeLang, learningLang, idToUse);

            return View(DTOs);
        }

        [Breadcrumb("Manual View", FromController = typeof(HomeController), FromAction = "SubCategories")]
        [Route("Categories/Subcategories/Manual")]
        public async Task<IActionResult> Manual(int categoryId)
        {
            Tuple<int, int> ids = helper.GetLanguagesId();
            int nativeLang = ids.Item1;
            int learningLang = ids.Item2;
            WordsRepository wordsRepository = new WordsRepository();
            List<DTO> words = wordsRepository.GetDTO(nativeLang, learningLang, categoryId);

            return View(words);
        }

        [Breadcrumb("Slideshow", FromController = typeof(HomeController), FromAction = "SubCategories")]
        [Route("Categories/Subcategories/Slideshow")]
        public async Task<IActionResult> SlideShow(int categoryId)
        {
            Tuple<int, int> ids = helper.GetLanguagesId();
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
    }
}
