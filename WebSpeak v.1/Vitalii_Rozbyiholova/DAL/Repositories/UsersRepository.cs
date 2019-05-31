using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class UsersRepository : IRepository<Users>
    {
        ProductHouseContext db;

        public UsersRepository()
        {
            db = new ProductHouseContext(ConfigurateOptions.GetOptions());
        }

        public IEnumerable<Users> GetAll()
        {
            return db.Users;
        }

        public Users GetById(int id)
        {
            using (ProductHouseContext db = new ProductHouseContext())
            {
                try
                {
                    return db.Users.FirstOrDefault();
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
