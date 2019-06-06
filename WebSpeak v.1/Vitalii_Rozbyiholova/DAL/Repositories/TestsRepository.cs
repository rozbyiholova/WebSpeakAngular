using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class TestsRepository : IRepository<Tests>
    {
        ProductHouseContext db;

        public TestsRepository()
        {
            db = new ProductHouseContext(ConfigurateOptions.GetOptions());
        }

        public IEnumerable<Tests> GetAll()
        {
            return db.Tests;
        }

        public Tests GetById(int id)
        {
            using (ProductHouseContext db = new ProductHouseContext())
            {
                try
                {
                    return db.Tests.Where(c => c.Id == id).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<DTO> GetDTO(int nativeLanguage, int learningLanguage)
        {
            TestsRepository testsRepository = new TestsRepository();
            TestsTranslationsRepository testsTranslationsRepository = new TestsTranslationsRepository();
            List<Tests> tests = testsRepository.GetAll().ToList();
            List<TestTranslations> testTranslations = testsTranslationsRepository.GetAll().ToList();

            var testsNative = testTranslations.Where(c => c.LangId == nativeLanguage)
               .Join(tests,
               tt => tt.TestId,
               t => t.Id,
               (tt, t) => new
               {
                   Id = t.Id,
                   Translation = tt.Translation,
               });

            var testsTrans = testTranslations.Where(c => c.LangId == learningLanguage)
                .Join(tests,
                tt => tt.TestId,
                t => t.Id,
                (tt, t) => new
                {
                    Id = t.Id,
                    Translation = tt.Translation,
                    Picture = t.Icon
                });

            List<DTO> DTOs = (from tn in testsNative
                              join tt in testsTrans
                              on tn.Id equals tt.Id
                              select new DTO()
                              {
                                  Id = tn.Id,
                                  Native = tn.Translation,
                                  Translation = tt.Translation,
                                  Picture = tt.Picture
                              }).ToList();
            return DTOs;
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
