using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class CategoriesRepository : IRepository<Categories>
    {
        ProductHouseContext db;

        public CategoriesRepository()
        {
            db = new ProductHouseContext(ConfigurateOptions.GetOptions());
        }

        public IEnumerable<Categories> GetAll()
        {
            return db.Categories;
        }

        public Categories GetById(int id)
        {
            using (ProductHouseContext db = new ProductHouseContext())
            {
                try
                {
                    return db.Categories.Where(c => c.Id == id).FirstOrDefault();
                }
                catch(Exception e)
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
