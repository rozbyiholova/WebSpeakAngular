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
                    return db.Users.Include(u => u.UserSettings).First(u => u.Id == id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public Users GetByEmailOrName(string loginString)
        {
            List<Users> users = GetAll().ToList();
            Users userByEmail = users.FirstOrDefault(u => u.Email == loginString);
            Users userByName = users.FirstOrDefault(u => u.UserName == loginString);

            if (userByName != null || userByEmail != null)
            {
                return userByName ?? userByEmail;
            }

            return null;
        }

        public bool ChangeLanguages(string login, int nativeLanguageId, int learningLanguageId)
        {
            Users user = GetByEmailOrName(login);

            if (user == null) { return false; }

            user.UserSettings.First().NativeLanguageId = nativeLanguageId;
            user.UserSettings.First().LearningLanguageId = learningLanguageId;

            db.SaveChanges();

            return true;
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
