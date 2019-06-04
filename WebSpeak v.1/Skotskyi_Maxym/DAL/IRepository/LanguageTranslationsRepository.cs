using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;

namespace DAL.IRepository
{
    public class LanguageTranslationsRepository : IRepository<LanguageTranslations>
    {
        private Languages_bdContext db;

        public LanguageTranslationsRepository()
        {
            this.db = new Languages_bdContext();
        }
        public void Create(LanguageTranslations item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public LanguageTranslations GetItem(int id)
        {
            return db.LanguageTranslations.Find(id);
        }

        public IEnumerable<LanguageTranslations> GetList()
        {
            return db.LanguageTranslations;
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
    }
}
