using System;
using System.ComponentModel;
using System.Globalization;

namespace _036_MoviesMvcBilgeAdam.Models
{
    public class MovieReportLeftOuterJoinModel
    {
        [DisplayName("Movie Name")]
        public string MovieName { get; set; }

        [DisplayName("Movie Production Year")]
        public string MovieProductionYear { get; set; }

        public double? MovieBoxOfficeReturnValue { get; set; }

        [DisplayName("Movie Box Office Return")]
        public string MovieBoxOfficeReturn => MovieBoxOfficeReturnValue.HasValue ? MovieBoxOfficeReturnValue.Value.ToString(new CultureInfo("en")) : "";

        [DisplayName("Director Name")]
        public string DirectorFullName { get; set; }

        public bool? DirectorRetiredValue { get; set; }

        [DisplayName("Is Director Retired?")]
        public string DirectorRetired => DirectorRetiredValue.HasValue ? (DirectorRetiredValue.Value ? "Yes" : "No") : "";

        [DisplayName("Review Content")]
        public string ReviewContent { get; set; }

        public int? ReviewRatingValue { get; set; }

        [DisplayName("Review Rating")]
        public string ReviewRating
        {
            get
            {
                if (ReviewRatingValue == null)
                    return "";
                if (ReviewRatingValue.Value >= 1 && ReviewRatingValue.Value <= 3)
                    return "Bad";
                if (ReviewRatingValue.Value >= 4 && ReviewRatingValue.Value <= 6)
                    return "Medium";
                if (ReviewRatingValue.Value >= 7 && ReviewRatingValue.Value <= 9)
                    return "Good";
                return "Very Good";
            }
        }

        [DisplayName("Reviewer")]
        public string ReviewReviewer { get; set; }

        public DateTime? ReviewDateValue { get; set; }

        [DisplayName("Review Date")]
        public string ReviewDate => ReviewDateValue != null ? ReviewDateValue.Value.ToString("MM/dd/yyyy", new CultureInfo("en")) : "";
    }
}