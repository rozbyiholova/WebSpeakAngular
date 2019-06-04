using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CategoriesTranslationsRepository : IRepository<CategoriesTranslations>
    {
        private LearningLanguagesContext db;

        public CategoriesTranslationsRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(CategoriesTranslations item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoriesTranslations> GetItem(int id)
        {
            return await db.CategoriesTranslations.FindAsync(id);
        }

        public async Task<IEnumerable<CategoriesTranslations>> GetList()
        {
            return await db.CategoriesTranslations.ToListAsync();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(CategoriesTranslations item)
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

        Task<List<DTO>> IRepository<CategoriesTranslations>.GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            throw new NotImplementedException();
        }
    }
}
