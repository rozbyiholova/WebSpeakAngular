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

        IRepository<LanguageTranslations> _languageTranslations = new LanguageTranslationsRepository();

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

            var testResultQuery = _testResults.GetAll().Where(x => x.UserId == currentUserId);
            var totalScoresQuery = _totalScores.GetAll().Where(x => x.UserId == currentUserId);

            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLang = await _languages.GetAll()
                .Join(
                    _languageTranslations.GetAll().Where(s => s.LangId == idLangNative),
                    lang => lang.Id,
                    langTrans => langTrans.LangId,
                    (lang, langTrans) => new DTO
                    {
                        Id = langTrans.NativeLangId,
                        WordNativeLang = langTrans.Translation,
                    }
                )
                .Join(
                    _totalScores.GetAll(),
                    lang => lang.Id,
                    total => total.LangId,
                    (lang, total) => new DTO
                    {
                        Id = lang.Id,
                        WordNativeLang = lang.WordNativeLang,
                        Total = total.Total,
                        UserId = total.UserId
                    }
                )
               .Where(x => x.UserId == currentUserId).Distinct().ToListAsync();

            List<DTO> NativeLearnLangTests;
            List<DTO> NativeLearnLangSubCat;
            List<DTO> NativeLearnLangCat;

            NativeLearnLangTests = await _tests.GetTranslations(idLangLearn, idLangNative, null);
            NativeLearnLangSubCat = await _categories.GetTranslations(idLangLearn, idLangNative, -1);
            NativeLearnLangCat = await _categories.GetTranslations(idLangLearn, idLangNative, null);

            List<DTOTestResults> LearnLangCat = await testResultQuery
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
                       CategoryName = NativeLearnLangCat.Find(item => item.Id == testResult.Category.ParentId).WordNativeLang,
                       TestDate = testResult.TestDate,
                       Result = testResult.Result
                   }
            ).ToListAsync();

            DTOStatistics statistics = new DTOStatistics()
            {
                testResults = LearnLangCat,
                LangList = NativeLearnLang,
                CatList = NativeLearnLangCat,
                SubCatList = NativeLearnLangSubCat
            };

            return View("./Manage/Statistics", statistics);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View("./Manage/ChangePassword");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("./Manage/ChangePassword");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            string oldPassword = Convert.ToString(model.OldPassword);
            string newPassword = Convert.ToString(model.NewPassword);

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View("./Manage/ChangePassword");
            }

            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("ChangePassword", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Rating()
        {
            var totalScoresQuery = _totalScores.GetAll();

            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");

            List<DTO> NativeLearnLang = await _languages.GetAll()
                .Join(
                    _languageTranslations.GetAll().Where(s => s.LangId == idLangNative),
                    lang => lang.Id,
                    langTrans => langTrans.LangId,
                    (lang, langTrans) => new DTO
                    {
                        Id = langTrans.NativeLangId,
                        WordNativeLang = langTrans.Translation,
                    }
                ).Distinct()
                .Join(
                    _totalScores.GetAll(),
                    lang => lang.Id,
                    total => total.LangId,
                    (lang, total) => new DTO
                    {
                        Id = lang.Id,
                        WordNativeLang = lang.WordNativeLang,
                        Total = total.Total,
                        UserId = total.UserId
                    }
                )
               .Distinct().ToListAsync();

            DTOStatistics statistics = new DTOStatistics()
            {
                LangList = NativeLearnLang
            };

            return View("./Manage/Rating", statistics);
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