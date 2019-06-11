using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LanguageTranslationsRepository : IRepository<LanguageTranslations>
    {
        private LearningLanguagesContext db;

        public LanguageTranslationsRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(LanguageTranslations item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<LanguageTranslations> GetItem(int id)
        {
            return await db.LanguageTranslations.FindAsync(id);
        }

        public async Task<IEnumerable<LanguageTranslations>> GetList()
        {
            return await db.LanguageTranslations.ToListAsync();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(LanguageTranslations item)
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

        Task<List<DTO>> IRepository<LanguageTranslations>.GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            throw new NotImplementedException();
        }

        public Task<LanguageTranslations> GetItem(string value)
        {
            throw new NotImplementedException();
        }
    }
}
