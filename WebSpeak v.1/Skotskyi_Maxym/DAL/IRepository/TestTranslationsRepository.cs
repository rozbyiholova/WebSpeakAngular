using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;

namespace DAL.IRepository
{
    public class TestTranslationsRepository : IRepository<TestTranslations>
    {
        private Languages_bdContext db;

        public TestTranslationsRepository()
        {
            this.db = new Languages_bdContext();
        }
        public void Create(TestTranslations item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public TestTranslations GetItem(int id)
        {
            return db.TestTranslations.Find(id);
        }

        public IEnumerable<TestTranslations> GetList()
        {
            return db.TestTranslations;
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
    }
}
