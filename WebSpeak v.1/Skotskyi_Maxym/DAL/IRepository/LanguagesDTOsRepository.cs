using DAL.DTO;
using DAL.Models;
using DAL.ModelConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.IRepository
{
    public class LanguagesDTOsRepository
    {
        LanguagesRepository languagesRepository = new LanguagesRepository();
        LanguageTranslationsRepository languageTranslationsRepository = new LanguageTranslationsRepository();

        public List<CategoriesDTO> GetLanguagesDTOs(int nativeLang, int LearningLang)
        {
        
            List<Languages> languages = languagesRepository.GetAll().ToList();
            List<LanguageTranslations> languageTranslations = languageTranslationsRepository.GetAll().ToList();

            var categoriesNative = languageTranslations.Where(c => c.LangId == nativeLang)
                .Join(languages,
                ct => ct.LangId,
                c => c.Id,
                (ct, c) => new
                {
                    Id = ct.NativeLangId,
                    Translation = ct.Translation
                });

            var categoriesTrans = languageTranslations.Where(c => c.LangId == LearningLang)
                .Join(languages,
                ct => ct.LangId,
                c => c.Id,
                (ct, c) => new
                {
                    Id = ct.NativeLangId,
                    Translation = ct.Translation
                });

            List<CategoriesDTO> DTOs = (from cn in categoriesNative
                                        join ct in categoriesTrans
                                        on cn.Id equals ct.Id
                                        select new CategoriesDTO()
                                        {
                                            LanguageId = ct.Id,

                                            Native = cn.Translation,

                                            Translation = ct.Translation,
                            
                                        }).ToList();
            return DTOs;
        }
    }
}
