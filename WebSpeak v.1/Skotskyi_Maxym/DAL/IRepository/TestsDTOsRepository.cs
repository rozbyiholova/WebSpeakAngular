using DAL.DTO;
using DAL.Models;
using DAL.ModelConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.IRepository
{
    public class TestsDTOsRepository
    {
        TestsRepository testRepository = new TestsRepository();
        TestTranslationsRepository testTranslationsRepository = new TestTranslationsRepository();

        public List<CategoriesDTO> GetTestDTOs(int nativeLang, int LearningLang)
        {
            List<Tests> test = testRepository.GetAll().ToList();
            List<TestTranslations> testTranslations = testTranslationsRepository.GetAll().ToList();


            var testNative = testTranslations.Where(c => c.LangId == nativeLang)
                .Join(test,
                ct => ct.TestId,
                c => c.Id,
                (ct, c) => new
                {
                    Id = c.Id,
                    Translation = ct.Translation
                });

            var testTrans = testTranslations.Where(c => c.LangId == LearningLang)
                .Join(test,
                ct => ct.TestId,
                c => c.Id,
                (ct, c) => new
                {
                    Id = c.Id,
                    Translation = ct.Translation,
                    Picture = c.Icon
                });

            List<CategoriesDTO> DTOs = (from tn in testNative
                                        join tt in testTrans
                                        on tn.Id equals tt.Id
                                        select new CategoriesDTO()
                                        {
                                            TestId = tn.Id,
                                            CategoryId = tn.Id,
                                            Native = tn.Translation,
                                            Translation = tt.Translation,
                                            Picture = tt.Picture
                                        }).ToList();
            return DTOs;
        }
    }
}
