using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class WordsTranslationsRepository : IRepository<WordTranslations>
    {
        ProductHouseContext db;

        public WordsTranslationsRepository()
        {
            db = new ProductHouseContext(ConfigurateOptions.GetOptions());
        }

        public IEnumerable<WordTranslations> GetAll()
        {
            return db.WordTranslations;
        }

        public WordTranslations GetById(int id)
        {
            using (ProductHouseContext db = new ProductHouseContext())
            {
                try
                {
                    return db.WordTranslations.Where(c => c.Id == id).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
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
