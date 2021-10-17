using _036_MoviesMvcBilgeAdam.Contexts;
using _036_MoviesMvcBilgeAdam.Entities;
using _036_MoviesMvcBilgeAdam.Models;
using System;
using System.Linq;

namespace _036_MoviesMvcBilgeAdam.Services
{
    public class MovieReportService
    {
        private readonly MoviesContext _db;

        public MovieReportService(MoviesContext db)
        {
            _db = db;
        }

        public IQueryable<MovieReportInnerJoinModel> GetInnerJoinQuery()
        {
            try
            {
                //IQueryable<Movie> movieQuery = _db.Movies;
                IQueryable<Movie> movieQuery = _db.Movies.AsQueryable();

                IQueryable<MovieDirector> movieDirectorQuery = _db.MovieDirectors.AsQueryable();
                IQueryable<Director> directorQuery = _db.Directors.AsQueryable();
                IQueryable<Review> reviewQuery = _db.Reviews.AsQueryable();

                IQueryable<MovieReportInnerJoinModel> query = from m in movieQuery
                                                              join md in movieDirectorQuery
                                                                  on m.Id equals md.MovieId
                                                              join d in directorQuery
                                                                  on md.DirectorId equals d.Id
                                                              join r in reviewQuery
                                                                  on m.Id equals r.MovieId
                                                              select new MovieReportInnerJoinModel()
                                                              {
                                                                  MovieName = m.Name,
                                                                  MovieProductionYear = m.ProductionYear,
                                                                  MovieBoxOfficeReturnValue = m.BoxOfficeReturn,

                                                                  // aşağıdaki şekilde veri dönüşüm işlemi yapıldığında Entity Framework hata verir!
                                                                  //MovieBoxOfficeReturn = m.BoxOfficeReturn.HasValue ? m.BoxOfficeReturn.Value.ToString(new CultureInfo("en")) : "",

                                                                  DirectorFullName = d.Name + " " + d.Surname,
                                                                  DirectorRetiredValue = d.Retired,
                                                                  ReviewContent = r.Content,
                                                                  ReviewRatingValue = r.Rating,
                                                                  ReviewReviewer = r.Reviewer,
                                                                  ReviewDateValue = r.Date
                                                              };
                return query;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public IQueryable<MovieReportLeftOuterJoinModel> GetLeftOuterJoinQuery()
        {
            try
            {
                IQueryable<Movie> movieQuery = _db.Movies.AsQueryable();
                IQueryable<MovieDirector> movieDirectorQuery = _db.MovieDirectors.AsQueryable();
                IQueryable<Director> directorQuery = _db.Directors.AsQueryable();
                IQueryable<Review> reviewQuery = _db.Reviews.AsQueryable();
                IQueryable<MovieReportLeftOuterJoinModel> query = from m in movieQuery
                                                                  join md in movieDirectorQuery
                                                                      on m.Id equals md.MovieId into movieDirectorJoin
                                                                  from subMovieDirectorJoin in movieDirectorJoin.DefaultIfEmpty()
                                                                  join d in directorQuery
                                                                      on subMovieDirectorJoin.DirectorId equals d.Id into directorJoin
                                                                  from subDirectorJoin in directorJoin.DefaultIfEmpty()
                                                                  join r in reviewQuery
                                                                      on m.Id equals r.MovieId into reviewJoin
                                                                  from subReviewJoin in reviewJoin.DefaultIfEmpty()
                                                                  select new MovieReportLeftOuterJoinModel()
                                                                  {
                                                                      MovieName = m.Name,
                                                                      MovieProductionYear = m.ProductionYear,
                                                                      MovieBoxOfficeReturnValue = m.BoxOfficeReturn,
                                                                      DirectorFullName = (subDirectorJoin.Name + " " + subDirectorJoin.Surname) ?? "",
                                                                      DirectorRetiredValue = subDirectorJoin.Retired,
                                                                      ReviewContent = subReviewJoin.Content ?? "",
                                                                      ReviewRatingValue = subReviewJoin.Rating,
                                                                      ReviewReviewer = subReviewJoin.Reviewer ?? "",
                                                                      ReviewDateValue = subReviewJoin.Date
                                                                  };
                return query;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}