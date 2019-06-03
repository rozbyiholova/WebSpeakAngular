using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;
using System.Linq;


namespace DAL.IRepository
{
    public class CategoriesTranslationsRepository : IRepository<CategoriesTranslations>
    {
        private Languages_bdContext db;

        public void Create(CategoriesTranslations item)
        {
            throw new NotImplementedException();
        }

        public CategoriesTranslationsRepository()
        {
            db = new Languages_bdContext(ConfigurateOptions.GetOptions());
        }

        public IEnumerable<CategoriesTranslations> GetAll()
        {
            return db.CategoriesTranslations;
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CategoriesTranslations GetItem(int id)
        {
            return db.CategoriesTranslations.Find(id);
        }

        public IEnumerable<CategoriesTranslations> GetList()
        {
            return db.CategoriesTranslations;
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
    }
}
