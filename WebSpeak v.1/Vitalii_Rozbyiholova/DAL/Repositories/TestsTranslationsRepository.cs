using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class TestsTranslationsRepository : IRepository<TestTranslations>
    {
        ProductHouseContext db;

        public TestsTranslationsRepository()
        {
            db = new ProductHouseContext(ConfigurateOptions.GetOptions());
        }

        public IEnumerable<TestTranslations> GetAll()
        {
            return db.TestTranslations;
        }

        public TestTranslations GetById(int id)
        {
            using (ProductHouseContext db = new ProductHouseContext())
            {
                try
                {
                    return db.TestTranslations.Where(c => c.Id == id).FirstOrDefault();
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
