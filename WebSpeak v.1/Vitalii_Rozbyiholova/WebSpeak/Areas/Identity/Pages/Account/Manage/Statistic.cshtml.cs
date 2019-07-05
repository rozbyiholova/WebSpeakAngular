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

        //private List<StatisticViewModel> GetStatistic()
        //{
        //    List<StatisticViewModel> result = new List<StatisticViewModel>();

        //    LanguagesRepository languagesRepository = new LanguagesRepository();
        //    CategoriesRepository categoriesRepository = new CategoriesRepository();
        //    LanguagesTranslationsRepository languagesTranslationsRepository = new LanguagesTranslationsRepository();

        //    IEnumerable<Languages> languages = languagesRepository.GetAll();
        //    IEnumerable<Categories> categories = categoriesRepository.GetAll().Where(c => c.ParentId == null);
        //    List<int> languagesId = languages.Select(l => l.Id).ToList();
        //    List<int> categoriesId = categories.Select(c => c.Id).ToList();

        //    foreach (int langId in languagesId)
        //    {
        //        IEnumerable<LanguageTranslations> languageTranslationsList = languagesTranslationsRepository.GetAll();
        //        string langName = languageTranslationsList.First(l => l.LangId == langId).Translation;
        //        List<CategoryStatistic> categoryStatistics = new List<CategoryStatistic>();
        //        foreach (int categoryId in categoriesId)
        //        {
        //            categoryStatistics.Add(GetCategoryStatistics(categoryId, langId));
        //        }
        //        result.Add(new StatisticViewModel
        //        {
        //            LanguageName = langName,
        //            CategoryStatistics = categoryStatistics
        //        });
        //    }

        //    return result;
        //}

        //private CategoryStatistic GetCategoryStatistics(int categoryId, int langId)
        //{
        //    CategoryStatistic result = new CategoryStatistic();

        //    CategoriesRepository categoriesRepository = new CategoriesRepository();
        //    CategoriesTranslationsRepository categoriesTranslationsRepository = new CategoriesTranslationsRepository();

        //    IEnumerable<Categories> categories = categoriesRepository.GetAll().Where(c => c.ParentId == categoryId);
        //    IEnumerable<int> subcategoriesId = categories.Select(s => s.Id);

        //    List<SubcategoryStatistic> subcategoryStatistics = new List<SubcategoryStatistic>();
        //    CategoriesTranslations categoriesTranslation = categoriesTranslationsRepository.GetAll()
        //        .First(c => c.CategoryId == categoryId && c.LangId == langId);
        //    string categoryName = categoriesTranslation.Translation;

        //    foreach (int subcategoryId in subcategoriesId)
        //    {
        //        subcategoryStatistics.Add(GetSubcategoryStatistics(subcategoryId, langId));
        //    }

        //    result.CategoryName = categoryName;
        //    result.SubcategoryStatistics = subcategoryStatistics;
        //    return result;
        //}

        //private SubcategoryStatistic GetSubcategoryStatistics(int subcategoryId, int langId)
        //{
        //    SubcategoryStatistic result = new SubcategoryStatistic();

        //    TestsRepository testsRepository = new TestsRepository();
        //    CategoriesTranslationsRepository categoriesTranslationsRepository = new CategoriesTranslationsRepository();
        //    TestsTranslationsRepository testsTranslationsRepository = new TestsTranslationsRepository();
        //    TestsResultsRepository testsResultsRepository = new TestsResultsRepository();

        //    IEnumerable<Tests> tests = testsRepository.GetAll();
        //    IEnumerable<int> testsId = tests.Select(t => t.Id);


        //    IEnumerable<TestResults> testResults = new List<TestResults>();
        //    foreach (int testId in testsId)
        //    {
        //        if (this.UserId != null)
        //        {
        //            testResults = testsResultsRepository.GetAll().Where(tr =>
        //                tr.CategoryId == subcategoryId && tr.LangId == langId &&
        //                tr.UserId == this.UserId);
        //        }
        //        else
        //        {
        //            testResults = null;
        //        }

        //        IEnumerable<TestTranslations> testTranslations = testsTranslationsRepository.GetAll()
        //            .Where(tt => tt.LangId == langId && tt.TestId == testId);
        //        CategoriesTranslations categoriesTranslation = categoriesTranslationsRepository.GetAll()
        //            .First(c => c.LangId == langId && c.CategoryId == subcategoryId);


        //        string subcategoryName = categoriesTranslation.Translation;
        //        List<string> testNames = testTranslations.Select(tt => tt.Translation).ToList();
        //        List<int> testsScore = testResults?.Select(tr => tr.Result).ToList();
        //        result.SubcategoryName = subcategoryName;
        //        result.TestNames = testNames;
        //        result.TestsScore = testsScore;
        //    }

        //    return result;
        //}
        private List<CategoryStatistic> CategoryStatistics(int langId)
                {
                    List<CategoryStatistic> categoryStatistic = new List<CategoryStatistic>();

                    using (ProductHouseContext db = new ProductHouseContext())
                    {
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
                            join testResults in db.TestResults on test.Id equals testResults.TestId
                            where testResults.UserId == this.UserId && testResults.LangId == langId
                            select new
                            {
                                TestTrans = testTrans.Translation,
                                TestId = test.Id,
                                CategoryId = testResults.CategoryId,
                                Result = testResults.Result
                            }).ToList();


                        foreach (var category in CategoryTranslations.Where(c => c.ParentId == null))
                        {
                            int i = 0;
                            categoryStatistic.Add(new CategoryStatistic
                            {
                                CategoryName = category.CategoryTrans
                            });

                            List<SubcategoryStatistic> subcategoryStatistics = new List<SubcategoryStatistic>();

                            var subcategories = CategoryTranslations.Where(c => c.ParentId == category.CategoryId); 
                            foreach (var subcategory in subcategories)
                            {
                                var tests = testsTrans.Where(t => t.CategoryId == subcategory.CategoryId);
                                List<string> testNames = tests.Select(t => t.TestTrans).ToList();
                                List<int> testResults = tests.Select(t => t.Result).ToList();
                                subcategoryStatistics.Add(new SubcategoryStatistic
                                {
                                    SubcategoryName = subcategory.CategoryTrans,
                                    TestNames = testNames,
                                    TestsScore = testResults
                                });
                            }

                            categoryStatistic[i].SubcategoryStatistics = subcategoryStatistics;
                            i++;
                        }
                    }

                    return categoryStatistic;
                }



        private List<StatisticViewModel> GetStatisticViewModels()
        {
            List<StatisticViewModel> result = new List<StatisticViewModel>();

            LanguagesRepository languagesRepository = new LanguagesRepository();
            LanguagesTranslationsRepository languagesTranslationsRepository = new LanguagesTranslationsRepository();
            IEnumerable<Languages> languages = languagesRepository.GetAll();
            List<int> languagesId = languages.Select(l => l.Id).ToList();
            List<CategoryStatistic> categoryStatistics = FillCategory();

            foreach (int languageId in languagesId)
            {
                IEnumerable<LanguageTranslations> languageTranslationsList = languagesTranslationsRepository.GetAll();
                string langName = languageTranslationsList
                    .First(l => l.LangId == languageId && l.NativeLangId == _userLanguageId).Translation;
                result.Add(new StatisticViewModel
                {
                    LanguageName = langName,
                    CategoryStatistics = categoryStatistics
                });

                StatisticViewModel statisticViewModel = result.ElementAt(languageId - 1);
                FillResults(ref statisticViewModel, languageId);
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

        private void FillResults(ref StatisticViewModel model, int langId)
        {
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
            int categoriesCount = model.CategoryStatistics.Count;
            int testsCount = tests.Count();

            for (int i = 0; i < categoriesCount; i++)
            {
                int subcategoriesCount = model.CategoryStatistics.ElementAt(i).SubcategoryStatistics.Count();
                for (int j = 0; j < subcategoriesCount; j++)
                {
                    SubcategoryStatistic subcategoryStatistic =
                        model.CategoryStatistics.ElementAt(i).SubcategoryStatistics.ElementAt(j);
                    for (int k = 0; k < testsCount; k++)
                    {
                        subcategoryStatistic.TestsScore = new List<int>();
                        int score;
                        if (userTests != null)
                        {
                            try
                            {
                                score = userTests.First(t =>
                                    t.CategoryId == subcategoryStatistic.CategoryId &&
                                    t.LangId == langId &&
                                    t.TestId == k + 1).Result;
                            }
                            catch (InvalidOperationException)
                            {
                                score = 0;
                            }
                            catch (Exception e)
                            {
                                score = 0;
                                Console.WriteLine(e.Message);
                            }
                        }
                        else
                        {
                            score = 0;
                        }
                                
                        subcategoryStatistic.TestsScore.Add(score);
                    }
                }
            }
        }
        

        //Add LanguageId to SubcategoryStatistic
        //Does not go out of the FillResult method
    }
}