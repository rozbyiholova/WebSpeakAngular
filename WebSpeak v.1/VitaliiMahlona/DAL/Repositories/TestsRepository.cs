using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;

namespace DAL.Repositories
{
    public class TestsRepository : IRepository<Tests>
    {
        private LearningLanguagesContext db;

        public TestsRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(Tests item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Tests GetItem(int id)
        {
            return db.Tests.Find(id);
        }

        public IEnumerable<Tests> GetList()
        {
            return db.Tests;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Tests item)
        {
            throw new NotImplementedException();
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

        public List<DTO> GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            var LearnLangTests = db.Tests
                .Join(
                    db.TestTranslations.Where(s => s.LangId == idLangLearn),
                    test => test.Id,
                    testTrans => testTrans.TestId,
                    (test, testTrans) => new
                    {
                        Id = test.Id,
                        Name = testTrans.Translation
                    }
            ).ToList();

            List<DTO> NativeLearnLangTests = db.Tests
                .Join(
                    db.TestTranslations.Where(s => s.LangId == idLangNative),
                    test => test.Id,
                    testTrans => testTrans.TestId,
                    (test, testTrans) => new DTO
                    {
                        Id = test.Id,
                        WordNativeLang = testTrans.Translation,
                        Picture = test.Icon,
                        WordLearnLang = LearnLangTests.Find(x => x.Id == test.Id).Name
                    }
            ).ToList();

            return NativeLearnLangTests;
        }
    }
}
