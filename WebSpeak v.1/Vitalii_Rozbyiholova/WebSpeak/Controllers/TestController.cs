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

        private int NativeLanguageId { get; }
        private int LearningLanguageId { get; }

        private int SubcategoryId {
            get
            {
                return helper.GetLastSubcategoryId();
            }
        }

        private List<DTO> Words {
            get
            {
                WordsRepository wordsRepository = new WordsRepository();
                return wordsRepository.GetDTO(NativeLanguageId, LearningLanguageId, SubcategoryId);
            }
        }

        public TestController(SessionHelper helper)
        {
            this.helper = helper;

            Tuple<int, int> ids = helper.GetLanguagesId();
            NativeLanguageId = ids.Item1;
            LearningLanguageId = ids.Item2;;
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
            
            List<DTO> DTOs = await Task.Run(() => testsRepository.GetDTO(NativeLanguageId, LearningLanguageId));
            return View(DTOs);
        }

        [Breadcrumb("Testing", FromAction = nameof(Index), FromController = typeof(TestController))]
        public async Task<IActionResult> Test1()
        {
            return View(Words);
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