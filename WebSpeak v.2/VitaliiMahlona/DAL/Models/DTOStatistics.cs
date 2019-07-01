using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class DTOStatistics
    {
        public List<DTOTestResults> testResults { get; set; }
        public List<DTO> LangList { get; set; }
        public List<DTO> CatList { get; set; }
        public List<DTO> SubCatList { get; set; }
        public List<DTOTotalRating> TotalRatings { get; set; }
        public List<DTOTotalRating> LangRatings { get; set; }
    }
}
