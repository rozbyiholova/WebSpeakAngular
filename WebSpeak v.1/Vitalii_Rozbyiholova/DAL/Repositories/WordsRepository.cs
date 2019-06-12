using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class WordsRepository : IRepository<Words>
    {
        ProductHouseContext db;

        public WordsRepository()
        {
            db = new ProductHouseContext(ConfigurateOptions.GetOptions());
        }

        public IEnumerable<Words> GetAll()
        {
            return db.Words;
        }

        public Words GetById(int id)
        {
            using (ProductHouseContext db = new ProductHouseContext())
            {
                try
                {
                    return db.Words.Where(c => c.Id == id).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<DTO> GetDTO(int nativeLanguage, int learningLanguage, int categoryId)
        {
            WordsRepository wordsRepository = new WordsRepository();
            WordsTranslationsRepository wordsTranslationsRepository = new WordsTranslationsRepository();
            List<Words> words = wordsRepository.GetAll().ToList();
            List<WordTranslations> wordsTranslations = wordsTranslationsRepository.GetAll().ToList();


            var wordsNative = words.Where(w => w.CategoryId == categoryId)
                .Join(wordsTranslations,
                word => word.Id,
                wordTrans => wordTrans.WordId,
                (word, wordTrans) => new
                {
                    Id = word.Id,
                    Translation = wordTrans.Translation,
                    NativePronounce = wordTrans.Pronounce,
                    NativeLanguageId = wordTrans.LangId
                });

            var wordsTranslation = words.Where(w => w.CategoryId == categoryId)
                .Join(wordsTranslations,
                word => word.Id,
                wordTrans => wordTrans.WordId,
                (word, wordTrans) => new
                {
                    Id = word.Id,
                    Translation = wordTrans.Translation,
                    TransPronounce = wordTrans.Pronounce,
                    Picture = word.Picture,
                    Sound = word.Sound,
                    TransLanguageId = wordTrans.LangId
                });

            List<DTO> DTOs = wordsNative.Where(wordNative => wordNative.NativeLanguageId == nativeLanguage)
                .Join(wordsTranslation.Where(wordTrans => wordTrans.TransLanguageId == learningLanguage),
                wordsN => wordsN.Id,
                wordsT => wordsT.Id,
                (wordsN, wordsT) => new DTO()
                {
                    Id = wordsN.Id,
                    Native = wordsN.Translation,
                    NativePronounce = wordsN.NativePronounce,
                    Picture = wordsT.Picture,
                    Sound = wordsT.Sound,
                    Translation = wordsT.Translation,
                    TranslationPronounce = wordsT.TransPronounce
                }).ToList();


            //var wordsNative = wordsTranslations.Where(c => c.LangId == nativeLanguage)
            //   .Join(words,
            //   wt => wt.WordId,
            //   w => w.Id,
            //   (wt, w) => new
            //   {
            //       Id = w.Id,
            //       Translation = wt.Translation,
            //       NPronounce = wt.Pronounce
            //   });

            //var wordsTrans = wordsTranslations.Where(c => c.LangId == learningLanguage)
            //    .Join(words,
            //    wt => wt.WordId,
            //    w => w.Id,
            //    (wt, w) => new
            //    {
            //        Id = w.Id,
            //        Translation = wt.Translation,
            //        Picture = w.Picture,
            //        Sound = w.Sound,
            //        LPronounce = wt.Pronounce,
            //        CategoryId = w.CategoryId
            //    });

            //List<DTO> DTOs = (from wn in wordsNative
            //                  join wt in wordsTrans
            //                  on wn.Id equals wt.Id
            //                  where wt.CategoryId == categoryId
            //                  select new DTO()
            //                  {
            //                      Id = wn.Id,
            //                      Native = wn.Translation,
            //                      Translation = wt.Translation,
            //                      Picture = wt.Picture,
            //                      Sound = wt.Sound,
            //                      NativePronounce = wn.NPronounce,
            //                      TranslationPronounce = wt.LPronounce
            //                  }).ToList();
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
