using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;

namespace DAL.Repositories
{
    public class CategoriesRepository : IRepository<Categories>
    {
        private LearningLanguagesContext db;

        public CategoriesRepository()
        {
            this.db = new LearningLanguagesContext(ConfigurateOptions.GetOptions());
        }

        public void Create(Categories item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Categories GetItem(int id)
        {
            return db.Categories.Find(id);
        }

        public IEnumerable<Categories> GetList()
        {
            return db.Categories;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Categories item)
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
            var LearnLangCat = db.Categories.Where(s => s.ParentId == parentId)
                .Join(
                    db.CategoriesTranslations.Where(s => s.LangId == idLangLearn),
                    category => category.Id,
                    categoryTrans => categoryTrans.CategoryId,
                    (category, categoryTrans) => new
                    {
                        Id = category.Id,
                        Name = categoryTrans.Translation
                    }
            ).ToList();

            List<DTO> NativeLearnLangCat = db.Categories.Where(s => s.ParentId == parentId)
                .Join(
                    db.CategoriesTranslations.Where(s => s.LangId == idLangNative),
                    category => category.Id,
                    categoryTrans => categoryTrans.CategoryId,
                    (category, categoryTrans) => new DTO
                    {
                        Id = category.Id,
                        WordNativeLang = categoryTrans.Translation,
                        Picture = category.Picture,
                        WordLearnLang = LearnLangCat.Find(x => x.Id == category.Id).Name
                    }
            ).ToList();

            return NativeLearnLangCat;
        }
    }
}
