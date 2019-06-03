using DAL.DTO;
using DAL.Models;
using DAL.ModelConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.IRepository
{
    public class WordsDTOsRepository
    {
        WordsRepository wordsRepository = new WordsRepository();
        WordTranslationsRepository wordTranslationsRepository = new WordTranslationsRepository();

        public List<CategoriesDTO> GetWordsDTOs(int nativeLang, int LearningLang)
        {
            List<Words> words = wordsRepository.GetAll().ToList();
            List<WordTranslations> WordTranslations = wordTranslationsRepository.GetAll().ToList();


            var WordsNative = WordTranslations.Where(c => c.LangId == nativeLang)
                .Join(words,
                wt => wt.WordId,
                w => w.Id,
                (wt, w) => new
                {
                    Id = w.Id,
                    Translation = wt.Translation,
                    PronounceNative = wt.Pronounce,
                    CategoryId = w.CategoryId
                });

            var WordsTrans = WordTranslations.Where(c => c.LangId == LearningLang)
                .Join(words,
                wt => wt.WordId,
                w => w.Id,
                (wt, w) => new
                {
                    Id = w.Id,
                    Translation = wt.Translation,
                    Picture = w.Picture,
                    PronounceLearn = wt.Pronounce,                
                });

            List<CategoriesDTO> DTOs = (from wn in WordsNative
                                        join wt in WordsTrans
                                        on wn.Id equals wt.Id
                                        select new CategoriesDTO()
                                        {
                                            CategoryId = wn.CategoryId,
                                            PronounceNative = wn.PronounceNative,
                                            PronounceLearn = wt.PronounceLearn,
                                            Native = wn.Translation,
                                            Translation = wt.Translation,
                                            Picture = wt.Picture
                                        }).ToList();
            return DTOs;
        }
    }
}

