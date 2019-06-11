using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TestTranslationsRepository : IRepository<TestTranslations>
    {
        private LearningLanguagesContext db;

        public TestTranslationsRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(TestTranslations item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<TestTranslations> GetItem(int id)
        {
            return await db.TestTranslations.FindAsync(id);
        }

        public async Task<IEnumerable<TestTranslations>> GetList()
        {
            return await db.TestTranslations.ToListAsync();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(TestTranslations item)
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

        Task<List<DTO>> IRepository<TestTranslations>.GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            throw new NotImplementedException();
        }

        public Task<TestTranslations> GetItem(string value)
        {
            throw new NotImplementedException();
        }
    }
}
