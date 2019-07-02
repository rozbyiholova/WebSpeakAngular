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

namespace LearningLanguagesAngular.Controllers
{
    public class AccountController : Controller
    {
        private ExternalLoginViewModel externalLoginViewModel = new ExternalLoginViewModel();

        private IList<AuthenticationScheme> ExternalLogins { get; set; }

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

        [HttpGet("Account/Manage/Index")]
        public async Task<DTO> Index()
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

            return index;
        }

        [HttpPost("Account/Manage/Index")]
        public IActionResult Index([FromBody]DTO form)
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

            return Ok();
        }

        [HttpPost("Account/Register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
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
                }
                else
                {
                    foreach (var error in addedUser.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Ok();
        }

        [HttpGet("Account/Login")]
        public async Task<LoginViewModel> Login(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            LoginViewModel login = new LoginViewModel
            {
                ExternalLogins = ExternalLogins,
                ReturnUrl = returnUrl
            };

            return login;
        }

        [HttpPost("Account/Login")]
        public async Task<LoginViewModel> Login([FromBody]LoginViewModel model)
        {
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (!result.Succeeded)
                {
                    model.ErrorMessage = "Incorrect username and/or password";
                }
                else
                {
                    model.ErrorMessage = "";
                }
            }

            return model;
        }

        [HttpGet("Account/Login/ExternalLogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            //var redirectUrl = Url.Action("Account", "Callback", new { returnUrl = externalLogin.ReturnUrl });
            var redirectUrl = $"/#/Account/Callback?returnUrl={returnUrl}";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet("Account/Callback")]
        public async Task<ExternalLoginViewModel> Callback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            externalLoginViewModel.ReturnUrl = returnUrl;

            if (remoteError != null)
            {
                externalLoginViewModel.ErrorMessage = $"Error from external provider: {remoteError}";
                return externalLoginViewModel;
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                externalLoginViewModel.ErrorMessage = "Error loading external login information.";
                return externalLoginViewModel;
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return externalLoginViewModel;
            }

            if (result.IsLockedOut)
            {
                externalLoginViewModel.ErrorMessage = "This account has been locked out, please try again later.";
                return externalLoginViewModel;
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                externalLoginViewModel.LoginProvider = info.LoginProvider;

                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    ExternalLoginViewModel login = new ExternalLoginViewModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return externalLoginViewModel;
            }
        }

        [HttpPost("Account/Callback")]
        public async Task<ExternalLoginViewModel> Callback([FromBody]ExternalLoginViewModel externalLoginViewModel)
        {
            externalLoginViewModel.ReturnUrl = externalLoginViewModel.ReturnUrl ?? Url.Content("~/");

            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                externalLoginViewModel.ErrorMessage = "Error loading external login information during confirmation.";

                return externalLoginViewModel;
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

                        return externalLoginViewModel;
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            externalLoginViewModel.LoginProvider = info.LoginProvider;

            return externalLoginViewModel;
        }

        [HttpGet("Account/Logout")]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet("Account/UsersInfo")]
        public async Task<DTOUsersInfo> UsersInfo()
        {
            DTOUsersInfo usersInfo = new DTOUsersInfo();

            if (_signInManager.IsSignedIn(User))
            {
                Users currrentUser = await _userManager.GetUserAsync(User);
                usersInfo.CurrentUser = currrentUser;
                usersInfo.IsSignedIn = true;
                usersInfo.Avatar = currrentUser.Avatar;
            }
            else
            {
                usersInfo.IsSignedIn = false;
            }

            return usersInfo;
        }

        [HttpGet("Account/Manage/PersonalInfo")]
        public async Task<PersonalInfoViewModel> PersonalInfo()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                PersonalInfoViewModel error = new PersonalInfoViewModel
                {
                    ErrorMessage = "Failed to get user"
                };
                return error;
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

            return personalInfo;
        }

        [HttpPost("Account/Manage/PersonalInfo")]
        public async Task<PersonalInfoViewModel> PersonalInfo([FromForm]PersonalInfoViewModel model)
        {
            PersonalInfoViewModel error = new PersonalInfoViewModel
            {
                ErrorMessage = "Data entered incorrectly"
            };
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    error.ErrorMessage = "Failed to get user";
                    return error;
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

                error.ErrorMessage = null;

                return error;
            }

            return error;
        }

        [HttpPost("Account/Manage/ChangePassword")]
        public async Task<ChangePasswordViewModel> ChangePassword([FromBody]ChangePasswordViewModel model)
        {
            ChangePasswordViewModel error = new ChangePasswordViewModel
            {
                ErrorMessage = "Data entered incorrectly"
            };

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    error.ErrorMessage = "Failed to get user";
                    return error;
                }

                string oldPassword = Convert.ToString(model.OldPassword);
                string newPassword = Convert.ToString(model.NewPassword);

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

                if (!changePasswordResult.Succeeded)
                {
                    error.ErrorMessage = "";
                    foreach (var err in changePasswordResult.Errors)
                    {
                        error.ErrorMessage += err.Description + '\n';
                    }

                    return error;
                }

                await _signInManager.RefreshSignInAsync(user);

                error.ErrorMessage = null;

                return error;
            }

            return error;
        }

        [HttpGet("Account/Manage/Statistics")]
        public async Task<DTOStatistics> Statistics()
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

            return statistics;
        }

        [HttpGet("Account/Manage/Rating")]
        public async Task<DTOStatistics> Rating()
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

            foreach(var item in NativeLearnLang.GroupBy(x => x.UserId).Select(g => new { UserId = g.Key, Total = g.Sum(x => x.Total) }).OrderByDescending(o => o.Total).ToList())
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

            foreach(var item in NativeLearnLang)
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

            DTOStatistics statistics = new DTOStatistics()
            {
                LangList = NativeLearnLang,
                TotalRatings = totalRatings,
                LangRatings = langRatings
            };

            return statistics;
        }

    }
}