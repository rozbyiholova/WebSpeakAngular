using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
/*                                             ---------------------------------------------------------------------
                                               |                             Angular                               |
                                               ---------------------------------------------------------------------
    */
namespace WebSpeakAngular.Controllers
{
    public class HomeController : Controller
    {
        private CategoriesRepository _categoriesRepository;
        private WordsRepository _wordsRepository;

        public HomeController()
        {
            _categoriesRepository = new CategoriesRepository();
            _wordsRepository = new WordsRepository();
        }

        [HttpGet]
        [Route("Categories")]
        public List<DTO> Index()
        {
            List<DTO> DTOs = _categoriesRepository.GetDTO(1, 3, null);
            return DTOs;
        }

        [HttpGet]
        [Route("Categories/Subcategories/{parentId}")]
        public List<DTO> Subcategories(int parentId)
        {
            List<DTO> DTOs = _categoriesRepository.GetDTO(1, 3, parentId);
            return DTOs;
        }

        [HttpGet]
        [Route("Categories/Subcategories/Words/{subcategoryId}")]
        public List<DTO> Words(int subcategoryId)
        {
            List<DTO> words = _wordsRepository.GetDTO(1, 3, subcategoryId);
            return words;
        }
    }
}