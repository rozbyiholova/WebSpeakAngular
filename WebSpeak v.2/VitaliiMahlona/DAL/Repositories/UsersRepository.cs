using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UsersRepository : IRepository<Users>
    {
        private LearningLanguagesContext db;

        public UsersRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(Users item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Users> GetItem(int id)
        {
            return await db.Users.FindAsync(id);
        }

        public async Task<Users> GetItem(string id)
        {
            return await db.Users.FindAsync(id);
        }

        public async Task<IEnumerable<Users>> GetList()
        {
            return await db.Users.ToListAsync();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Users item)
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

        public Task<List<DTO>> GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Users> GetAll()
        {
            return db.Users;
        }
    }
}
