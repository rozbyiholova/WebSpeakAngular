using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class WordTranslationsRepository : IRepository<WordTranslations>
    {
        private LearningLanguagesContext db;

        public WordTranslationsRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(WordTranslations item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<WordTranslations> GetItem(int id)
        {
            return await db.WordTranslations.FindAsync(id);
        }

        public async Task<IEnumerable<WordTranslations>> GetList()
        {
            return await db.WordTranslations.ToListAsync();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(WordTranslations item)
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

        Task<List<DTO>> IRepository<WordTranslations>.GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            throw new NotImplementedException();
        }

        public Task<WordTranslations> GetItem(string value)
        {
            throw new NotImplementedException();
        }
    }
}
