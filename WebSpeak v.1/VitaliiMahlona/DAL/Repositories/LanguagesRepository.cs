using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<DTO>> GetTranslations(int idLangLearn, int idLangNative, int? parentId)
        {
            List<DTO> NativeLearnLang = await db.Languages
                .Join(
                    db.LanguageTranslations.Where(s => s.LangId == idLangNative),
                    lang => lang.Id,
                    langTrans => langTrans.LangId,
                    (lang, langTrans) => new DTO
                    {
                        Id = langTrans.NativeLangId,
                        WordNativeLang = langTrans.Translation,
                    }
                )
                .Join(
                    db.TotalScores,
                    lang => lang.Id,
                    total => total.LangId,
                    (lang, total) => new DTO
                    {
                        Id = lang.Id,
                        WordNativeLang = lang.WordNativeLang,
                        Total = total.Total,
                        UserId = total.UserId
                    }
                )
               .Distinct().ToListAsync();

            return NativeLearnLang;
        }

        public Task<Languages> GetItem(string value)
        {
            throw new NotImplementedException();
        }
    }
}
