using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DAL;
using DAL.Repositories;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace LearningLanguages.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        IRepository<Categories> _categories = new CategoriesRepository();

        IRepository<Languages> _languages = new LanguagesRepository();

        IRepository<Words> _words = new WordsRepository();

        IRepository<Tests> _tests = new TestsRepository();

        IRepository<Users> _users = new UsersRepository();

        IRepository<TestResults> _testResults = new TestResultsRepository();

        IRepository<TotalScores> _totalScores = new TotalScoresRepository();

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users { Email = model.Email, UserName = model.Email };

                var addedUser = await _userManager.CreateAsync(user, model.Password);

                if (addedUser.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in addedUser.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username and/or password");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SelectList languagesList = new SelectList(await _languages.GetList(), "Id", "Name");

            return View("./Manage/Index", languagesList);
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

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            string currentUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var testResultsList = await _testResults.GetList();
            var totalScoresList = await _totalScores.GetList();

            var testResultQuery = testResultsList.Where(x => x.UserId == currentUserId);
            var totalScoresQuery = totalScoresList.Where(x => x.UserId == currentUserId);

            int idLangLearn = -1;
            int idLangNative = -1;
            int defaultLangId = 3;

            if (HttpContext?.Session?.GetInt32("idLangLearn") != null) idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            if (HttpContext?.Session?.GetInt32("idLangLearn") != null) idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLangTests;
            List<DTO> NativeLearnLang;
            List<DTO> NativeLearnLangSubCat;
            List<DTO> NativeLearnLangCat;

            if (idLangLearn != -1 && idLangNative != -1)
            {
                NativeLearnLangTests = await _tests.GetTranslations(idLangLearn, idLangNative, null);
                NativeLearnLang = await _languages.GetTranslations(idLangLearn, idLangNative, null);
                NativeLearnLangSubCat = await _categories.GetTranslations(idLangLearn, idLangNative, -1);
                NativeLearnLangCat = await _categories.GetTranslations(idLangLearn, idLangNative, null);
            }
            else
            {
                NativeLearnLangTests = await _tests.GetTranslations(defaultLangId, defaultLangId, null);
                NativeLearnLang = await _languages.GetTranslations(defaultLangId, defaultLangId, null);
                NativeLearnLangSubCat = await _categories.GetTranslations(defaultLangId, defaultLangId, -1);
                NativeLearnLangCat = await _categories.GetTranslations(defaultLangId, defaultLangId, null);
            }

            NativeLearnLang = NativeLearnLang.Where(x => x.UserId == currentUserId).ToList();

            List<DTOTestResults> LearnLangCat = testResultQuery
               .Join(
                   totalScoresQuery,
                   testResult => testResult.LangId,
                   totalScore => totalScore.LangId,
                   (testResult, totalScore) => new DTOTestResults
                   {
                       Id = testResult.Id,
                       TestName = NativeLearnLangTests.Find(item => item.Id == testResult.TestId).WordNativeLang,
                       LangName = NativeLearnLang.Find(item => item.Id == testResult.LangId).WordNativeLang,
                       SubCategoryName = NativeLearnLangSubCat.Find(item => item.Id == testResult.CategoryId).WordNativeLang,
                       CategoryName = NativeLearnLangCat.Find(item => testResult?.Category?.ParentId != null && item.Id == testResult?.Category?.ParentId).WordNativeLang,
                       TestDate = testResult.TestDate,
                       Result = testResult.Result
                   }
            ).ToList();

            DTOStatistics statistics = new DTOStatistics()
            {
                testResults = LearnLangCat,
                LangList = NativeLearnLang,
                CatList = NativeLearnLangCat,
                SubCatList = NativeLearnLangSubCat
            };

            return View("./Manage/Statistics", statistics);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}