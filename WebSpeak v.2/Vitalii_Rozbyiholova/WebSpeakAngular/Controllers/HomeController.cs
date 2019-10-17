using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
/*                                             ---------------------------------------------------------------------
                                               |                             Angular                               |
                                               ---------------------------------------------------------------------
    */
namespace WebSpeakAngular.Controllers
{
    [Route("[controller]/")]
    public class HomeController : Controller
    {
        private readonly CategoriesRepository _categoriesRepository;
        private readonly WordsRepository _wordsRepository;
        private readonly TestsRepository _testsRepository;
        private readonly int defaultNativeLanguageId = 1;
        private readonly int defaultLearningLanguageId = 3;
        private readonly Users _currentUser;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _categoriesRepository = new CategoriesRepository();
            _wordsRepository = new WordsRepository();
            _testsRepository = new TestsRepository();

            string userLogin = httpContextAccessor?.HttpContext?.User?.FindFirst("userLogin")?.Value;

            if (userLogin != null)
            {
                _currentUser = new UsersRepository().GetByEmailOrName(userLogin);
            }
            
        }

        [HttpGet("Categories")]
        public List<DTO> Index()
        {
            HandleLanguages(out int native, out int learning);
            List<DTO> DTOs = _categoriesRepository.GetDTO(native, learning, null);
            return DTOs;
        }

        [HttpGet("Categories/Subcategories/{parentId}")]
        public List<DTO> Subcategories(int parentId)
        {
            HandleLanguages(out int native, out int learning);
            List<DTO> DTOs = _categoriesRepository.GetDTO(native, learning, parentId);
            return DTOs;
        }

        [HttpGet("Categories/Subcategories/Words/{subcategoryId}")]
        public List<DTO> Words(int subcategoryId)
        {
            HandleLanguages(out int native, out int learning);
            List<DTO> words = _wordsRepository.GetDTO(native, learning, subcategoryId);
            return words;
        }

        [HttpGet("Categories/Subcategories/Tests/{subcategoryId}")]
        public List<DTO> TestsIndex()
        {
            HandleLanguages(out int native, out int learning);
            List<DTO> tests = _testsRepository.GetDTO(native, learning);
            return tests;
        }

        [HttpGet("Categories/Subcategories/Tests/Test/{subcategoryId}")]
        public List<DTO> Test(int subcategoryId)
        {
            HandleLanguages(out int native, out int learning);
            List<DTO> words = _wordsRepository.GetDTO(native, learning, subcategoryId);
            return words;
        }

        [HttpPost, Route("SaveResult")]
        public void SaveTestResult([FromBody] TestResults testResult)
        {
            if (testResult != null)
            {
                testResult.TestDate = DateTime.Now;
                using (ProductHouseContext db = new ProductHouseContext())
                {
                    db.TestResults.Add(testResult);
                    db.SaveChanges();
                }
            }
        }

        public int NativeLanguageId
        {
            get
            {
                if (_currentUser != null)
                {
                    return _currentUser.UserSettings.First().NativeLanguageId;
                }
                else
                {
                    return 0;
                }
            }
        } 

        public int LearningLanguageId
        {
            get
            {
                if (_currentUser != null)
                {
                    return _currentUser.UserSettings.First().LearningLanguageId;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void HandleLanguages(out int native, out int learning)
        {
            native = NativeLanguageId;
            learning = LearningLanguageId;

            if (native == 0 || learning == 0)
            {
                native = defaultNativeLanguageId;
                learning = defaultLearningLanguageId;
            }
        }
    }
}