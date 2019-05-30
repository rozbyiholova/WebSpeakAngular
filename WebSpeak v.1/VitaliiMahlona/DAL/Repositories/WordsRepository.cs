using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;

namespace DAL.Repositories
{
    public class WordsRepository : IRepository<Words>
    {
        private LearningLanguagesContext db;

        public WordsRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
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

        public List<DTO> GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            var LearnLangWords = db.Words.Where(s => s.CategoryId == parentId)
                .Join(
                    db.WordTranslations.Where(s => s.LangId == idLangLearn),
                    word => word.Id,
                    wordTrans => wordTrans.WordId,
                    (word, wordTrans) => new
                    {
                        Id = word.Id,
                        Name = wordTrans.Translation
                    }
            ).ToList();

            List<DTO> NativeLearnLangWords = db.Words.Where(s => s.CategoryId == parentId)
                .Join(
                    db.WordTranslations.Where(s => s.LangId == idLangNative),
                    word => word.Id,
                    wordTrans => wordTrans.WordId,
                    (word, wordTrans) => new DTO
                    {
                        Id = word.Id,
                        WordNativeLang = wordTrans.Translation,
                        Picture = word.Picture,
                        WordLearnLang = LearnLangWords.Find(x => x.Id == word.Id).Name
                    }
            ).ToList();

            return NativeLearnLangWords;
        }
    }
}
