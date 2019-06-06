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
        private readonly Helper helper;

        private int SubcategoryId;

        public TestController(Helper helper)
        {
            this.helper = helper;
        }

        [Breadcrumb("Tests", FromAction = "SubCategories", FromController =typeof(HomeController))]
        public async Task<IActionResult> Index(int subcategoryId)
        {
            SubcategoryId = subcategoryId;

            TestsRepository testsRepository = new TestsRepository();
            Tuple<int, int> ids = helper.GetLanguagesId();
            int nativeLang = ids.Item1;
            int learningLang = ids.Item2;
            List<DTO> DTOs = testsRepository.GetDTO(nativeLang, learningLang);
            return View(DTOs);
        }

        public async Task<IActionResult> Test1(int subCategoryId)
        {
            return View();
        }
    }
}