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

namespace WebSpeak.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
                       
            return View();
        }

        public IActionResult Categories()
        {
            CategoriesRepository categoriesRepository = new CategoriesRepository();
            IEnumerable<Categories> categories = categoriesRepository.GetAll();
            categories = categories.Where(c => c.ParentId == null);
            
            return View(categories);
        }

        public IActionResult SubCategories(int id)
        {
            CategoriesRepository categoriesRepository = new CategoriesRepository();
            IEnumerable<Categories> subcategories = categoriesRepository.GetAll();
            subcategories = subcategories.Where(c => c.ParentId == id);
            
            return View(subcategories);
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
