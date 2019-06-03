using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;
using DAL.ModelConfiguration;
using DAL.IRepository;
using DAL.DTO;
using System.Linq;

namespace DAL.IRepository
{
    public class CategoriesRepository : IRepository<Categories>
    {
        Languages_bdContext db;

        public CategoriesRepository()
        {
            db = new Languages_bdContext(ConfigurateOptions.GetOptions());
        }

        public IEnumerable<Categories> GetAll()
        {
            return db.Categories;
        }

        public Categories GetById(int id)
        {
            using (Languages_bdContext db = new Languages_bdContext())
            {
                try
                {
                    return db.Categories.Where(c => c.Id == id).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void Create(Categories item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Categories GetItem(int id)
        {
            return db.Categories.Find(id);
        }

        public IEnumerable<Categories> GetList()
        {
            return db.Categories;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Categories item)
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
