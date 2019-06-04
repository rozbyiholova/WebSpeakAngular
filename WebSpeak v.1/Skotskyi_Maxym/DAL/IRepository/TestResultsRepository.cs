using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;

namespace DAL.IRepository
{
    public class TestResultsRepository : IRepository<TestResults>
    {
        private Languages_bdContext db;

        public TestResultsRepository()
        {
            this.db = new Languages_bdContext();
        }
        public void Create(TestResults item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public TestResults GetItem(int id)
        {
            return db.TestResults.Find(id);
        }

        public IEnumerable<TestResults> GetList()
        {
            return db.TestResults;
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
    }
}
