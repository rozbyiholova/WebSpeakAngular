using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        Task<IEnumerable<T>> GetList(); 
        Task<T> GetItem(int id);
        void Create(T item); 
        void Update(T item); 
        void Delete(int id); 
        void Save();
        Task<List<DTO>> GetTranslations(int idLangLearn, int idLangNative, int? parentId);
        Task<T> GetItem(string value);
    }
}
