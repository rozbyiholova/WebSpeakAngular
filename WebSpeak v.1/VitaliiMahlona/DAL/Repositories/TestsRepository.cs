using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Tests> GetItem(int id)
        {
            return await db.Tests.FindAsync(id);
        }

        public async Task<IEnumerable<Tests>> GetList()
        {
            return await db.Tests.ToListAsync();
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

        public async Task<List<DTO>> GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            var LearnLangTests = await db.Tests
                .Join(
                    db.TestTranslations.Where(s => s.LangId == idLangLearn),
                    test => test.Id,
                    testTrans => testTrans.TestId,
                    (test, testTrans) => new
                    {
                        Id = test.Id,
                        Name = testTrans.Translation
                    }
            ).ToListAsync();

            List<DTO> NativeLearnLangTests = await db.Tests
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
            ).ToListAsync();

            return NativeLearnLangTests;
        }
    }
}
