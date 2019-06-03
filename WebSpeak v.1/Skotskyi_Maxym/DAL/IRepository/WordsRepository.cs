using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;

namespace DAL.IRepository
{
    public class WordsRepository : IRepository<Words>
    {
        private Languages_bdContext db;


        public WordsRepository()
        {
            db = new Languages_bdContext(ConfigurateOptions.GetOptions());
        }

        public IEnumerable<Words> GetAll()
        {
            return db.Words;
        }



        public void Create(Words item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Words GetItem(int id)
        {
            return db.Words.Find(id);
        }

        public IEnumerable<Words> GetList()
        {
            return db.Words;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Words item)
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
