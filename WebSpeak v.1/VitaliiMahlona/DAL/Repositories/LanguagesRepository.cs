using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LanguagesRepository : IRepository<Languages>
    {
        private LearningLanguagesContext db;

        public LanguagesRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(Languages item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Languages> GetItem(int id)
        {
            return await db.Languages.FindAsync(id);
        }

        public async Task<IEnumerable<Languages>> GetList()
        {
            return await db.Languages.ToListAsync();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Languages item)
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

        Task<List<DTO>> IRepository<Languages>.GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            throw new NotImplementedException();
        }

        public Task<Languages> GetItem(string value)
        {
            throw new NotImplementedException();
        }
    }
}
