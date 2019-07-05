using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class DTOStatistics
    {
        public List<DTOTestResults> testResults { get; set; }
        public List<DTO> LangList { get; set; }
        public List<DTOTotalRating> TotalRatings { get; set; }
        public List<DTOTotalRating> LangRatings { get; set; }
        public Users CurrentUser { get; set; }
        public List<HashSet<string>> Categories { get; set; }
        public List<HashSet<string>> SubCategories { get; set; }
        public List<HashSet<string>> Tests { get; set; }
        public List<List<DTOTestResults>> TestScores { get; set; }
        public bool IsSignedIn { get; set; }
    }
}
