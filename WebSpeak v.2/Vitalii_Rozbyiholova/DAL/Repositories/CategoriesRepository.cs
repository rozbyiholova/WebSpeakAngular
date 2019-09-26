using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class CategoriesRepository : IRepository<Categories>
    {
        ProductHouseContext db;

        public CategoriesRepository()
        {
            db = new ProductHouseContext(ConfigurateOptions.GetOptions());
        }

        public IEnumerable<Categories> GetAll()
        {
            return db.Categories;
        }

        public Categories GetById(int id)
        {
            using (ProductHouseContext db = new ProductHouseContext())
            {
                try
                {
                    return db.Categories.FirstOrDefault(c => c.Id == id);
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }

        public List<DTO> GetDTO(int nativeLanguage, int learningLanguage, int? parrentId)
        {
            CategoriesRepository categoriesRepository = new CategoriesRepository();
            CategoriesTranslationsRepository categoriesTranslationsRepository = new CategoriesTranslationsRepository();
            List<Categories> categories = categoriesRepository.GetAll().ToList();
            List<CategoriesTranslations> categoriesTranslations = categoriesTranslationsRepository.GetAll().ToList();

            var categoriesNative = categoriesTranslations.Where(c => c.LangId == nativeLanguage)
               .Join(categories,
               ct => ct.CategoryId,
               c => c.Id,
               (ct, c) => new
               {
                   Id = c.Id,
                   Translation = ct.Translation
               });

            var categoriesTrans = categoriesTranslations.Where(c => c.LangId == learningLanguage)
                .Join(categories,
                ct => ct.CategoryId,
                c => c.Id,
                (ct, c) => new
                {
                    Id = c.Id,
                    Translation = ct.Translation,
                    ParentId = c.ParentId,
                    Picture = c.Picture
                });

            List<DTO> DTOs = (from cn in categoriesNative
                              join ct in categoriesTrans
                              on cn.Id equals ct.Id
                              where ct.ParentId == parrentId
                              select new DTO()
                              {
                                  Id = cn.Id,
                                  Native = cn.Translation,
                                  Translation = ct.Translation,
                                  Picture = ct.Picture,
                                  Type = DTOType.Category
                              }).ToList();
            return DTOs;
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
