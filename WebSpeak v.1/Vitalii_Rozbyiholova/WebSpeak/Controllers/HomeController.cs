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
using DAL.DTOs;
using Microsoft.AspNetCore.Http;

namespace WebSpeak.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            LanguagesRepository languagesRepository = new LanguagesRepository();
           SelectList languages = new SelectList(languagesRepository.GetAll().ToList(), "Id", "Name");
            return View(languages);
        }

        public IActionResult Privacy()
        {                       
            return View();
        }

        public IActionResult Categories()
        {
            CategoryDTOsRepository categoryDTOs = new CategoryDTOsRepository();
            int nativeLanguageId = Convert.ToInt32(Request.Form["Language1"]);
            int learnLanguageId = Convert.ToInt32(Request.Form["Language2"]);
            HttpContext.Session.SetInt32("NativeLanguage", nativeLanguageId);
            HttpContext.Session.SetInt32("LearningLanguage", learnLanguageId);

            List<CategoriesDTO> DTOs = categoryDTOs.GetCategoriesDTOs(nativeLanguageId, learnLanguageId);
            
            return View(DTOs.Where(x => x.ParentId == null));
        }

        public IActionResult SubCategories(int id)
        {
            CategoryDTOsRepository categoryDTOs = new CategoryDTOsRepository();
            int nativeLanguageId = (int)HttpContext.Session.GetInt32("NativeLanguage");
            int learnLanguageId = (int)HttpContext.Session.GetInt32("LearningLanguage");

            List<CategoriesDTO> DTOs = categoryDTOs.GetCategoriesDTOs(nativeLanguageId, learnLanguageId);

            return View(DTOs.Where(x => x.ParentId == id));
        }

        public IActionResult Words(int id)
        {
            WordsRepository wordsRepository = new WordsRepository();
            IEnumerable<Words> words = wordsRepository.GetAll();
            words = words.Where(c => c.CategoryId == id);

            return View(words);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
