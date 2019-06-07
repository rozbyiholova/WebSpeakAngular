using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace WebSpeak.Controllers
{
    public class TestController : Controller
    {
        private readonly SessionHelper helper;

        private int SubcategoryId;

        private int NativeLanguageId { get; set; }
        private int LearningLanguageId { get; set; }

        private List<DTO> Categories { get; set; }

        public TestController(SessionHelper helper)
        {
            this.helper = helper;

            CategoriesRepository categoriesRepository = new CategoriesRepository();
            Tuple<int, int> ids = helper.GetLanguagesId();
            NativeLanguageId = ids.Item1;
            LearningLanguageId = ids.Item2;

            int lastCategoryId = helper.GetLastСategoryId();
            Categories = categoriesRepository.GetDTO(NativeLanguageId, LearningLanguageId, lastCategoryId);
        }

        [Breadcrumb("Tests", FromAction = "SubCategories", FromController =typeof(HomeController))]
        public async Task<IActionResult> Index(int subcategoryId)
        {
            int idToUse = subcategoryId;
            if(idToUse > 0)
            {
                helper.SetSubcategory(subcategoryId);
            }

            TestsRepository testsRepository = new TestsRepository();
            
            List<DTO> DTOs = testsRepository.GetDTO(NativeLanguageId, LearningLanguageId);
            return View(DTOs);
        }

        [Breadcrumb("Testing", FromAction = nameof(Index), FromController = typeof(TestController))]
        public async Task<IActionResult> Test1()
        {
            return View(Categories);
        }

        public async Task<IActionResult> Test2(int subCategoryId)
        {
            return View();
        }

        public async Task<IActionResult> Test3(int subCategoryId)
        {
            return View();
        }

        public async Task<IActionResult> Test4(int subCategoryId)
        {
            return View();
        }
        public async Task<IActionResult> Test5(int subCategoryId)
        {
            return View();
        }
        public async Task<IActionResult> Test6(int subCategoryId)
        {
            return View();
        }
        public async Task<IActionResult> Test7(int subCategoryId)
        {
            return View();
        }
        public async Task<IActionResult> Test8(int subCategoryId)
        {
            return View();
        }

        public async Task<IActionResult> Test9(int subCategoryId)
        {
            return View();
        }

        public async Task<IActionResult> Test10(int subCategoryId)
        {
            return View();
        }
    }
}