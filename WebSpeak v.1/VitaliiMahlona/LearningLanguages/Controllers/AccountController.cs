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
using Microsoft.AspNetCore.Authentication;
using System.IO;

namespace LearningLanguages.Controllers
{
    public class AccountController : Controller
    {
        private ExternalLoginViewModel externalLoginViewModel = new ExternalLoginViewModel();

        private IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        private string ErrorMessage { get; set; }

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
                Users user = new Users
                {
                    Email = model.Email,
                    UserName = model.Email
                };

                if (model.Avatar != null)
                {
                    var fileName = Path.GetFileName(model.Avatar.FileName);

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UserImages", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Avatar.CopyToAsync(fileStream);
                    }

                    user.Avatar = "../" + filePath.Substring(filePath.IndexOf("UserImages"));
                }

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
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            LoginViewModel login = new LoginViewModel
            {
                ExternalLogins = ExternalLogins,
                ReturnUrl = returnUrl
            };

            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");

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
        public IActionResult ExternalLogin()
        {
            return Redirect("./Login");
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("Callback", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> Callback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                 return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return View("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                externalLoginViewModel.ReturnUrl = returnUrl;
                externalLoginViewModel.LoginProvider = info.LoginProvider;

                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    ExternalLoginViewModel login = new ExternalLoginViewModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return View("./ExternalLogin",externalLoginViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Callback(ExternalLoginViewModel externalLoginViewModel)
        {
            externalLoginViewModel.ReturnUrl = externalLoginViewModel.ReturnUrl ?? Url.Content("~/");

            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";

                return RedirectToAction("Login", new { externalLoginViewModel.ReturnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new Users { UserName = externalLoginViewModel.Email, Email = externalLoginViewModel.Email };
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return LocalRedirect(externalLoginViewModel.ReturnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            externalLoginViewModel.LoginProvider = info.LoginProvider;

            return View("./ExternalLogin", externalLoginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var languagesList = await _languages.GetList();
            int idLangLearn = (int)HttpContext.Session.GetInt32("idLangLearn");
            int idLangNative = (int)HttpContext.Session.GetInt32("idLangNative");
            string enableNativeLang = HttpContext.Session.GetString("enableNativeLang");
            string enableSound = HttpContext.Session.GetString("enableSound");
            string enablePronounceNativeLang = HttpContext.Session.GetString("enablePronounceNativeLang");
            string enablePronounceLearnLang = HttpContext.Session.GetString("enablePronounceLearnLang");

            DTO index = new DTO()
            {
                LanguagesList = languagesList.ToList(),
                IdLangLearn = idLangLearn,
                IdLangNative = idLangNative,
                EnableNativeLang = enableNativeLang,
                EnableSound = enableSound,
                EnablePronounceNativeLang = enablePronounceNativeLang,
                EnablePronounceLearnLang = enablePronounceLearnLang
            };

            return View("./Manage/Index", index);
        }

        [HttpPost]
        public IActionResult Index(DTO form)
        {
            int idLangNative = form.IdLangNative;
            int idLangLearn = form.IdLangLearn;
            string enableNativeLang = form.EnableNativeLang;
            string enableSound = form.EnableSound;
            string enablePronounceNativeLang = form.EnablePronounceNativeLang;
            string enablePronounceLearnLang = form.EnablePronounceLearnLang;

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

            IQueryable<DTOTestResults> LearnLangCat = testResultQuery
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
            );

            List<HashSet<string>> Categories = new List<HashSet<string>>();
            List<HashSet<string>> SubCategories = new List<HashSet<string>>();
            List<HashSet<string>> Tests = new List<HashSet<string>>();
            List<List<DTOTestResults>> TestScores = new List<List<DTOTestResults>>();
   
            foreach(var lang in NativeLearnLang)
            {
                HashSet<string> categories = new HashSet<string>();

                foreach(var item in LearnLangCat.Where(item => item.LangName == lang.WordNativeLang))
                {
                    categories.Add(item.CategoryName);
                }

                Categories.Add(categories);

                foreach (var item in categories)
                {
                    HashSet<string> subCategories = new HashSet<string>();

                    foreach(var subCat in LearnLangCat.Where(subCat => subCat.LangName == lang.WordNativeLang && subCat.CategoryName == item))
                    {
                        subCategories.Add(subCat.SubCategoryName);
                    }

                    SubCategories.Add(subCategories);

                    foreach (var subCat in subCategories)
                    {
                        HashSet<string> tests = new HashSet<string>();

                        foreach(var test in LearnLangCat.Where(test => test.LangName == lang.WordNativeLang && test.CategoryName == item))
                        {
                            tests.Add(test.TestName.Replace("\\", "/"));
                        }

                        Tests.Add(tests);

                        foreach (var test in tests)
                        {
                            List<DTOTestResults> testScores = new List<DTOTestResults>();

                            foreach (var testScore in LearnLangCat.Where(testScore => testScore.LangName == lang.WordNativeLang &&
                                                                                      testScore.CategoryName == item && testScore.SubCategoryName == subCat &&
                                                                                      testScore.TestName.Replace("\\", "/") == test))
                            {
                                testScores.Add(testScore);
                            }

                            TestScores.Add(testScores);
                        }
                    }
                }
            }

            DTOStatistics statistics = new DTOStatistics()
            {
                LangList = NativeLearnLang,
                Categories = Categories,
                SubCategories = SubCategories,
                Tests = Tests,
                TestScores = TestScores
            };

            if (_signInManager.IsSignedIn(User))
            {
                statistics.IsSignedIn = true;
            }
            else
            {
                statistics.IsSignedIn = false;
            }

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
        public async Task<IActionResult> PersonalInfo()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var username = user.UserName;

            PersonalInfoViewModel personalInfo = new PersonalInfoViewModel
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Username = username
            };

            return View("./Manage/PersonalInfo", personalInfo);
        }

        [HttpPost]
        public async Task<IActionResult> PersonalInfo(PersonalInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("./Manage/PersonalInfo");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);

            if (model.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);

                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            string firstName = Convert.ToString(model.FirstName);
            string lastName = Convert.ToString(model.LastName);
            string username = Convert.ToString(model.Username);

            if (firstName != user.FirstName)
            {
                user.FirstName = firstName;
            }

            if (lastName != user.LastName)
            {
                user.LastName = lastName;
            }

            if (username != user.UserName)
            {
                user.UserName = username;
            }

            if (model.Avatar != null)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser.Avatar != null)
                {
                    var fileOldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UserImages", currentUser.Avatar);

                    System.IO.File.Delete(fileOldPath);
                }

                var fileName = Path.GetFileName(model.Avatar.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UserImages", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Avatar.CopyToAsync(fileStream);
                }

                user.Avatar = "../" + filePath.Substring(filePath.IndexOf("UserImages"));
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("PersonalInfo", "Account");
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

            List<DTOTotalRating> totalRatings = new List<DTOTotalRating>();

            foreach (var item in NativeLearnLang.GroupBy(x => x.UserId).Select(g => new { UserId = g.Key, Total = g.Sum(x => x.Total) }).OrderByDescending(o => o.Total).ToList())
            {
                var user = await _userManager.FindByIdAsync(item.UserId);
                string username = user.UserName;

                DTOTotalRating totalRating = new DTOTotalRating()
                {
                    Username = username,
                    Total = item.Total
                };

                totalRatings.Add(totalRating);
            }

            List<string> langs = new List<string>();

            foreach (var item in NativeLearnLang)
            {
                langs.Add(item.WordNativeLang);
            }

            HashSet<string> uniqueLangs = new HashSet<string>(langs);

            List<DTOTotalRating> langRatings = new List<DTOTotalRating>();

            List<DTO> nativeLearnLangCopy = new List<DTO>();

            foreach (var lang in NativeLearnLang)
            {
                if (!uniqueLangs.Contains(lang.WordNativeLang))
                {
                    nativeLearnLangCopy.Add(lang);
                    continue;
                }
                else
                {
                    uniqueLangs.Remove(lang.WordNativeLang);
                }

                int numberUser = 1;

                foreach (var item in NativeLearnLang.Where(item => item.WordNativeLang == lang.WordNativeLang).OrderByDescending(o => o.Total).ToList())
                {
                    var user = await _userManager.FindByIdAsync(item.UserId);
                    string username = user.UserName;

                    DTOTotalRating totalRating = new DTOTotalRating()
                    {
                        Username = username,
                        Total = item.Total,
                        Lang = lang.WordNativeLang,
                        Id = numberUser
                    };

                    langRatings.Add(totalRating);

                    numberUser++;
                }
            }

            foreach (var lang in nativeLearnLangCopy)
            {
                NativeLearnLang.Remove(lang);
            }

            Users currentUser = await _userManager.GetUserAsync(User);

            DTOStatistics statistics = new DTOStatistics()
            {
                LangList = NativeLearnLang,
                TotalRatings = totalRatings,
                LangRatings = langRatings,
                CurrentUser = currentUser
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