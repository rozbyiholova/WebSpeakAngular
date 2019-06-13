using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBreadcrumbs.Attributes;
using WebSpeak.Models;

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
       public IActionResult Test(int testID)
        {
            string viewName = String.Format("Test{0}", testID);
            return View(viewName, Words);
        }

        [HttpPost]
        public IActionResult Result(object testResult)
        {
            TestResultViewModel model = JsonConvert.DeserializeObject<TestResultViewModel>(testResult.ToString());
            return View(testResult);
        }

       //public IActionResult Result(int score)
       // {
       //     CategoriesTranslationsRepository categoriesTranslations = new CategoriesTranslationsRepository();
       //     string categoryName = categoriesTranslations.GetById(SubcategoryId).Translation;
       //     Tuple<string, int> result = new Tuple<string, int>(categoryName, score);
       //     return View(result);
       // }
    }
}