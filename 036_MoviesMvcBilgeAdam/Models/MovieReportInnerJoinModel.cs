using System;
using System.ComponentModel;
using System.Globalization;

namespace _036_MoviesMvcBilgeAdam.Models
{
    public class MovieReportInnerJoinModel
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

        public bool DirectorRetiredValue { get; set; }

        [DisplayName("Is Director Retired?")] 
        public string DirectorRetired => DirectorRetiredValue ? "Yes" : "No";

        [DisplayName("Review Content")] 
        public string ReviewContent { get; set; }

        public int ReviewRatingValue { get; set; }

        [DisplayName("Review Rating")]
        public string ReviewRating
        {
            get
            {
                if (ReviewRatingValue >= 1 && ReviewRatingValue <= 3)
                    return "Bad";
                if (ReviewRatingValue >= 4 && ReviewRatingValue <= 6)
                    return "Medium";
                if (ReviewRatingValue >= 7 && ReviewRatingValue <= 9)
                    return "Good";
                return "Very Good";
            }
        }

        [DisplayName("Reviewer")] 
        public string ReviewReviewer { get; set; }

        public DateTime ReviewDateValue { get; set; }

        [DisplayName("Review Date")]
        public string ReviewDate => ReviewDateValue.ToString("MM/dd/yyyy", new CultureInfo("en"));
    }
}