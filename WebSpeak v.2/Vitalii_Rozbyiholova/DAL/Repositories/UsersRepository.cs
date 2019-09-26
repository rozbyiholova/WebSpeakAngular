using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
            return db.Users.Include(u => u.UserSettings);
        }

        public Users GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Users GetById(string id)
        {
            using (db)
            {
                try
                {
                    return db.Users.First(u => u.Id == id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
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
