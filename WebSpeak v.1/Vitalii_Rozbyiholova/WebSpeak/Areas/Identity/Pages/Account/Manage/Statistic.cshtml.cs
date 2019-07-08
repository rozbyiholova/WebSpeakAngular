using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebSpeak.Models;

namespace WebSpeak.Areas.Identity.Pages.Account.Manage
{
    public class StatisticModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<StatisticModel> _logger;
        private readonly int _userLanguageId;

        private string UserId
        {
            get
            {
                string userId;
                try
                {
                    userId = User.Claims.First().Value;
                }
                catch (InvalidOperationException)
                {
                    userId = null;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                return userId;
            }
        }

        public StatisticModel(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            ILogger<StatisticModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userLanguageId = Helper.GetLanguagesId().ToValueTuple().Item1;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public List<StatisticViewModel> Statistic => GetStatisticViewModels();

        private List<StatisticViewModel> GetStatisticViewModels()
        {
            List<StatisticViewModel> result = new List<StatisticViewModel>();

            LanguagesRepository languagesRepository = new LanguagesRepository();
            LanguagesTranslationsRepository languagesTranslationsRepository = new LanguagesTranslationsRepository();
            IEnumerable<Languages> languages = languagesRepository.GetAll();
            List<int> languagesId = languages.Select(l => l.Id).ToList();
            List<CategoryStatistic> categoryStatistics = FillCategory();

            foreach (var languageId in languagesId)
            {
                IEnumerable<LanguageTranslations> languageTranslationsList = languagesTranslationsRepository.GetAll();
                var id = languageId;
                string langName = languageTranslationsList
                    .First(l => l.LangId == id && l.NativeLangId == _userLanguageId).Translation;
                List<CategoryStatistic> listToCopy = Helper.DeepClone(categoryStatistics);
                result.Add(new StatisticViewModel
                {
                    LanguageName = langName,
                    CategoryStatistics = listToCopy,
                    LanguageId = languageId
                });
            }
            StatisticViewModel[] statisticArray = new StatisticViewModel[result.Count];
            for (int j = 0; j < statisticArray.Length; j++)
            {
                statisticArray[j] = FillResults(result[j]);
            }

            return result;
        }

        private List<CategoryStatistic> FillCategory()
        {
            List<CategoryStatistic> categoryStatistic = new List<CategoryStatistic>();
            ProductHouseContext db = new ProductHouseContext();

            var CategoryTranslations = (from categoryTrans in db.CategoriesTranslations
                join category in db.Categories on categoryTrans.CategoryId equals category.Id
                where categoryTrans.LangId == _userLanguageId
                select new
                {
                    CategoryTrans = categoryTrans.Translation,
                    CategoryId = category.Id,
                    ParentId = category.ParentId
                }).ToList();

            var testsTrans = (from testTrans in db.TestTranslations
                where testTrans.LangId == _userLanguageId
                join test in db.Tests on testTrans.TestId equals test.Id
                select new
                {
                    TestTrans = testTrans.Translation,
                    TestId = test.Id
                }).ToList();

            var parentCategories = CategoryTranslations.Where(c => c.ParentId == null).ToArray();
            for (int i = 0; i < parentCategories.Count(); i++)
            {
                var parentCategory = parentCategories[i];
                categoryStatistic.Add(new CategoryStatistic
                {
                    CategoryName = parentCategory.CategoryTrans
                });

                List<SubcategoryStatistic> subcategoryStatistics = new List<SubcategoryStatistic>();
                foreach (var subcategory in CategoryTranslations.Where(s => s.ParentId == parentCategory.CategoryId))
                {
                    subcategoryStatistics.Add(new SubcategoryStatistic
                    {
                        SubcategoryName = subcategory.CategoryTrans,
                        TestNames = testsTrans.Select(t => t.TestTrans).ToList(),
                        CategoryId = subcategory.CategoryId
                    });
                }

                categoryStatistic[i].SubcategoryStatistics = subcategoryStatistics;
            }

            return categoryStatistic;
        }

        private StatisticViewModel FillResults(StatisticViewModel model)
        {
            StatisticViewModel result = new StatisticViewModel
            {
                LanguageName = model.LanguageName,
                LanguageId = model.LanguageId,
                CategoryStatistics = model.CategoryStatistics
            };
            int langId = result.LanguageId;
            TestsResultsRepository testsResultsRepository = new TestsResultsRepository();
            TestsRepository testsRepository = new TestsRepository();

            IEnumerable<TestResults> testResults = testsResultsRepository.GetAll();
            IEnumerable<Tests> tests = testsRepository.GetAll();

            IEnumerable<TestResults> userTests;
            try
            {
                userTests = testResults.Where(t => t.UserId == this.UserId).ToArray();
            }
            catch (InvalidOperationException)
            {
                userTests = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            int categoriesCount = result.CategoryStatistics.Count;
            int testsCount = tests.Count();

            for (int i = 0; i < categoriesCount; i++)
            {
                int subcategoriesCount = result.CategoryStatistics.ElementAt(i).SubcategoryStatistics.Count();
                for (int j = 0; j < subcategoriesCount; j++)
                {
                    SubcategoryStatistic subcategoryStatistic =
                        result.CategoryStatistics[i].SubcategoryStatistics[j];
                    List<int> subcategoryScore = new List<int>();
                    for (int k = 0; k < testsCount; k++)
                    {
                        int score;
                        if (userTests != null)
                        {
                            bool isScore = userTests.ToList().Exists(t =>
                                t.CategoryId == subcategoryStatistic.CategoryId &&
                                t.LangId == langId &&
                                t.TestId == k + 1);
                            if (isScore)
                            {
                                score = userTests.First(t =>
                                    t.CategoryId == subcategoryStatistic.CategoryId &&
                                    t.LangId == langId &&
                                    t.TestId == k + 1).Result;
                            }
                            else
                            {
                                score = 0;
                            }
                        }
                        else
                        {
                            score = 0;
                        }
                                
                        subcategoryScore.Add(score);
                    }

                    subcategoryStatistic.TestsScore = subcategoryScore;
                }
            }

            return result;
        }
    }
}