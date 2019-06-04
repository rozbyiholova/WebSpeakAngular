using DAL.DTO;
using DAL.Models;
using DAL.ModelConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DAL.IRepository
{
    public class CategoriesDTOsRepository
    {
        CategoriesRepository categoriesRepository = new CategoriesRepository();
        CategoriesTranslationsRepository categoriesTranslationsRepository = new CategoriesTranslationsRepository();

        public List<CategoriesDTO> GetCategoriesDTOs(int nativeLang, int LearningLang)
        {
            List<Categories> categories = categoriesRepository.GetAll().ToList();
            List<CategoriesTranslations> categoriesTranslations = categoriesTranslationsRepository.GetAll().ToList();


            var categoriesNative = categoriesTranslations.Where(c => c.LangId == nativeLang)
                .Join(categories,
                ct => ct.CategoryId,
                c => c.Id,
                (ct, c) => new
                {
                    Id = c.Id,
                    Translation = ct.Translation
                });

            var categoriesTrans = categoriesTranslations.Where(c => c.LangId == LearningLang)
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

            List<CategoriesDTO> DTOs = (from cn in categoriesNative
                                        join ct in categoriesTrans
                                        on cn.Id equals ct.Id
                                        select new CategoriesDTO()
                                        {
                                            CategoryId = cn.Id,
                                            ParentId = ct.ParentId,
                                            Native = cn.Translation,
                                            Translation = ct.Translation,
                                            Picture = ct.Picture
                                        }).ToList();
            return DTOs;
        }
    }
}
