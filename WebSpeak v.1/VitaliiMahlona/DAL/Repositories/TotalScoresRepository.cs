using System;
using System.Collections.Generic;
using DAL.Models;

namespace DAL.Repositories
{
    public class TotalScoresRepository : IRepository<TotalScores>
    {
        private LearningLanguagesContext db;

        public TotalScoresRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(TotalScores item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public TotalScores GetItem(int id)
        {
            return db.TotalScores.Find(id);
        }

        public IEnumerable<TotalScores> GetList()
        {
            return db.TotalScores;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(TotalScores item)
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

        public List<DTO> GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            throw new NotImplementedException();
        }
    }
}
