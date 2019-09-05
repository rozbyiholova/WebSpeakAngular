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
    [Route("[controller]/")]
    public class HomeController : Controller
    {
        private CategoriesRepository _categoriesRepository;
        private WordsRepository _wordsRepository;
        private TestsRepository _testsRepository;

        
        public HomeController()
        {
            _categoriesRepository = new CategoriesRepository();
            _wordsRepository = new WordsRepository();
            _testsRepository = new TestsRepository();
        }

        [HttpGet("Categories")]
        public List<DTO> Index()
        {
            List<DTO> DTOs = _categoriesRepository.GetDTO(1, 3, null);
            return DTOs;
        }

        [HttpGet("Categories/Subcategories/{parentId}")]
        public List<DTO> Subcategories(int parentId)
        {
            List<DTO> DTOs = _categoriesRepository.GetDTO(1, 3, parentId);
            return DTOs;
        }

        [HttpGet("Categories/Subcategories/Words/{subcategoryId}")]
        public List<DTO> Words(int subcategoryId)
        {
            List<DTO> words = _wordsRepository.GetDTO(1, 3, subcategoryId);
            return words;
        }

        [HttpGet("Categories/Subcategories/Tests/{subcategoryId}")]
        public List<DTO> TestsIndex()
        {
            List<DTO> tests = _testsRepository.GetDTO(1, 3);
            return tests;
        }

        [HttpGet("Categories/Subcategories/Tests/Test/{subcategoryId}")]
        public List<DTO> Test(int subcategoryId)
        {
            List<DTO> words = _wordsRepository.GetDTO(1, 3, subcategoryId);
            return words;
        }
    }
}