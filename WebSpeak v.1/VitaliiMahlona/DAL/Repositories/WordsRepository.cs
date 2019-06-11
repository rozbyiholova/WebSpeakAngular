using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Words> GetItem(int id)
        {
            return await db.Words.FindAsync(id);
        }

        public async Task<IEnumerable<Words>> GetList()
        {
            return await db.Words.ToListAsync();
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

        public async Task<List<DTO>> GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            var LearnLangWords = await db.Words.Where(s => s.CategoryId == parentId)
                .Join(
                    db.WordTranslations.Where(s => s.LangId == idLangLearn),
                    word => word.Id,
                    wordTrans => wordTrans.WordId,
                    (word, wordTrans) => new
                    {
                        word.Id,
                        Name = wordTrans.Translation,
                        PronounceLearn = wordTrans.Pronounce
                    }
            ).ToListAsync();

            List<DTO> NativeLearnLangWords = await db.Words.Where(s => s.CategoryId == parentId)
                .Join(
                    db.WordTranslations.Where(s => s.LangId == idLangNative),
                    word => word.Id,
                    wordTrans => wordTrans.WordId,
                    (word, wordTrans) => new DTO
                    {
                        Id = word.Id,
                        WordNativeLang = wordTrans.Translation,
                        Picture = word.Picture,
                        WordLearnLang = LearnLangWords.Find(x => x.Id == word.Id).Name,
                        Sound = word.Sound,
                        PronounceNative = wordTrans.Pronounce,
                        PronounceLearn = LearnLangWords.Find(x => x.Id == word.Id).PronounceLearn,
                        SubCategoryId = word.CategoryId
                    }
            ).ToListAsync();

            return NativeLearnLangWords;
        }

        public Task<Words> GetItem(string value)
        {
            throw new NotImplementedException();
        }
    }
}
