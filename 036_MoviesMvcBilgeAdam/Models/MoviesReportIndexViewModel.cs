using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace _036_MoviesMvcBilgeAdam.Models
{
    public class MoviesReportIndexViewModel
    {
        public List<MovieReportInnerJoinModel> InnerJoinList { get; set; }
        public List<MovieReportLeftOuterJoinModel> LeftOuterJoinList { get; set; }

        public SelectList OnlyMatchedSelectList { get; set; }

        [DisplayName("Only Matched")]
        //public int OnlyMatchedValue { get; set; }
        public int OnlyMatchedValue { get; set; } = 0;

        [DisplayName("Movie Name")]
        public string MovieName { get; set; }

        public MultiSelectList ProductionYearMultiSelectList { get; set; }

        [DisplayName("Production Years")]
        public List<string> ProductionYears { get; set; }

        [DisplayName("Box Office Return")]
        //public double? BoxOfficeReturn1 { get; set; }
        public string BoxOfficeReturn1 { get; set; }

        //public double? BoxOfficeReturn2 { get; set; }
        public string BoxOfficeReturn2 { get; set; }

        [DisplayName("Review Date")]
        public string ReviewDate1 { get; set; }

        public string ReviewDate2 { get; set; }
    }
}