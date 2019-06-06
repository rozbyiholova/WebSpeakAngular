using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TestResultsRepository : IRepository<TestResults>
    {
        private LearningLanguagesContext db;

        public TestResultsRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(TestResults item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<TestResults> GetItem(int id)
        {
            return await db.TestResults.FindAsync(id);
        }

        public async Task<IEnumerable<TestResults>> GetList()
        {
            return await db.TestResults.ToListAsync();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(TestResults item)
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

        Task<List<DTO>> IRepository<TestResults>.GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            throw new NotImplementedException();
        }
    }
}
