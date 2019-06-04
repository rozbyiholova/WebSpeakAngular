using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TotalScoresRepository : IRepository<TotalScores>
    {
        private LearningLanguagesContext db;

        public TotalScoresRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(TotalScores item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<TotalScores> GetItem(int id)
        {
            return await db.TotalScores.FindAsync(id);
        }

        public async Task<IEnumerable<TotalScores>> GetList()
        {
            return await db.TotalScores.ToListAsync();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(TotalScores item)
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

        Task<List<DTO>> IRepository<TotalScores>.GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            throw new NotImplementedException();
        }
    }
}
