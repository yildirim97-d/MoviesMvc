using System.Collections.Generic;
using _036_MoviesMvcBilgeAdam.Entities;

namespace _036_MoviesMvcBilgeAdam.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<_036_MoviesMvcBilgeAdam.Contexts.MoviesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(_036_MoviesMvcBilgeAdam.Contexts.MoviesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            #region Entity Framework DbSet'leri üzerinden ilk verileri oluşturma.
            // verileri liste olarak oluşturma:
            List<Movie> movieList = new List<Movie>()
            {
                new Movie() {Id = 1, Name = "Avatar", ProductionYear = "2009", BoxOfficeReturn = 1000000},
                new Movie() {Id = 2, Name = "Sherlock Holmes", ProductionYear = "2009"},
                new Movie() {Id = 3, Name = "Law Abiding Citizen", ProductionYear = "2009", BoxOfficeReturn = 300000},
                new Movie() {Id = 4, Name = "Aliens", ProductionYear = "1986", BoxOfficeReturn = 10000000}
            };

            List<Director> directorList = new List<Director>()
            {
                new Director() {Id = 1, Name = "James", Surname = "Cameron", Retired = false},
                new Director() {Id = 2, Name = "Guy", Surname = "Ritchie", Retired = false},
                new Director() {Id = 3, Name = "F. Gary", Surname = "Gray", Retired = false}
            };

            List<Review> reviewList = new List<Review>()
            {
                new Review() {Id = 1, Content = "Very good movie.", Rating = 9, Reviewer = "Çağıl Alsaç", MovieId = 1, Movie = movieList[0], Date = DateTime.Parse("21.02.2021")},
                new Review() {Id = 2, Content = "Nice movie.", Rating = 7, Reviewer = "Leo Alsaç", MovieId = 1, Movie = movieList[0], Date = DateTime.Parse("31.12.2018")},
                new Review() {Id = 3, Content = "Not a bad movie.", Rating = 6, Reviewer = "Angel Alsaç", MovieId = 1, Movie = movieList[0], Date = DateTime.Parse("23.06.2017")},
                new Review() {Id = 4, Content = "Pretty successful!", Rating = 8, Reviewer = "Ali Veli", MovieId = 2, Movie = movieList[1], Date = DateTime.Parse("15.05.2020")},
                new Review() {Id = 5, Content = "Didn't like it.", Rating = 3, Reviewer = "Zeynep Can", MovieId = 3, Movie = movieList[2], Date = DateTime.Parse("21.10.2018")},
                new Review() {Id = 6, Content = "Superb!", Rating = 10, Reviewer = "Cem Tan", MovieId = 4, Movie = movieList[3], Date = DateTime.Parse("04.05.2016")},
                new Review() {Id = 7, Content = "A bad movie.", Rating = 2, Reviewer = "Cem Tan", MovieId = 2, Movie = movieList[1], Date = DateTime.Parse("19.08.2018")},
                new Review() {Id = 8, Content = "Bad movie.", Rating = 3, Reviewer = "Çağıl Alsaç", MovieId = 2, Movie = movieList[1], Date = DateTime.Parse("21.02.2020")},
                new Review() {Id = 9, Content = "A very bad movie.", Rating = 1, Reviewer = "Leo Alsaç", MovieId = 2, Movie = movieList[1], Date = DateTime.Parse("18.05.2018")},
                new Review() {Id = 10, Content = "A masterpiece.", Rating = 10, Reviewer = "Angel Alsaç", MovieId = 2, Movie = movieList[1], Date = DateTime.Parse("11.03.2016")},
                new Review() {Id = 11, Content = "Pretty good!", Rating = 6, Reviewer = "Ali Veli", MovieId = 3, Movie = movieList[2], Date = DateTime.Parse("19.05.2021")},
                new Review() {Id = 12, Content = "Liked it.", Rating = 6, Reviewer = "Zeynep Can", MovieId = 4, Movie = movieList[3], Date = DateTime.Parse("09.08.2017")},
                new Review() {Id = 13, Content = "Super!", Rating = 9, Reviewer = "Cem Tan", MovieId = 1, Movie = movieList[0], Date = DateTime.Parse("01.02.2021")},
                new Review() {Id = 14, Content = "A mediocre movie.", Rating = 5, Reviewer = "Cem Tan", MovieId = 1, Movie = movieList[0], Date = DateTime.Parse("30.05.2019")}
            };

            List<MovieDirector> movieDirectorList = new List<MovieDirector>()
            {
                new MovieDirector() {Id = 1, MovieId = 1, DirectorId = 1, Movie = movieList[0], Director = directorList[0]}, // directorList index üzerinden Director objesini set etme
                new MovieDirector() {Id = 2, MovieId = 2, DirectorId = 2, Movie = movieList[1], Director = directorList.SingleOrDefault(d => d.Id == 2)}, // directorList LINQ üzerinden Director objesini set etme
                new MovieDirector() {Id = 3, MovieId = 3, DirectorId = 3, Movie = movieList[2], Director = directorList[2]},
                new MovieDirector() {Id = 4, MovieId = 4, DirectorId = 1, Movie = movieList[3], Director = directorList[0]}
            };

            // veritabanındaki tüm verileri silme:
            var movieDirectors = context.MovieDirectors.ToList();
            context.MovieDirectors.RemoveRange(movieDirectors);
            var reviews = context.Reviews.ToList();
            context.Reviews.RemoveRange(reviews);
            var movies = context.Movies.ToList();
            context.Movies.RemoveRange(movies);
            var directors = context.Directors.ToList();
            context.Directors.RemoveRange(directors);
            context.SaveChanges();

            // veritabanına yukarıda liste olarak oluşturulan verileri ekleme:
            context.MovieDirectors.AddRange(movieDirectorList);
            context.Reviews.AddRange(reviewList);
            #endregion

            #region SQL üzerinden ilk verileri oluşturma. Herhangi bir SQL script dosyası (~/SQLs/MoviesMvcBilgeAdamDB.sql) üzerinden de yapılabilir.
            //context.Database.ExecuteSqlCommand("delete from MovieDirectors");
            //context.Database.ExecuteSqlCommand("delete from Reviews");
            //context.Database.ExecuteSqlCommand("delete from Movies");
            //context.Database.ExecuteSqlCommand("delete from Directors");

            //context.Database.ExecuteSqlCommand("dbcc CHECKIDENT('MovieDirectors', RESEED, 0)"); // tablodaki Id sütununun 1'den başlaması için
            //context.Database.ExecuteSqlCommand("dbcc CHECKIDENT('Reviews', RESEED, 0)");
            //context.Database.ExecuteSqlCommand("dbcc CHECKIDENT('Movies', RESEED, 0)");
            //context.Database.ExecuteSqlCommand("dbcc CHECKIDENT('Directors', RESEED, 0)");

            //context.Database.ExecuteSqlCommand("insert into Directors values(N'James', N'Cameron', 0)");
            //context.Database.ExecuteSqlCommand("insert into Directors values(N'Guy', N'Ritchie', 0)");
            //context.Database.ExecuteSqlCommand("insert into Directors values(N'F. Gary', N'Gray', 0)");
            //context.Database.ExecuteSqlCommand("insert into Movies values(N'Avatar', N'2009', 1000000)");
            //context.Database.ExecuteSqlCommand("insert into Movies values(N'Sherlock Holmes', N'2009', NULL)");
            //context.Database.ExecuteSqlCommand("insert into Movies values(N'Law Abiding Citizen', N'2009', 300000)");
            //context.Database.ExecuteSqlCommand("insert into Movies values(N'Aliens', N'1986', 10000000)");
            //context.Database.ExecuteSqlCommand("insert into Reviews values(N'Very good movie.', 9, N'Çağıl Alsaç', '2021-02-21', 1)");
            //context.Database.ExecuteSqlCommand("insert into MovieDirectors values(1, 1)");
            //context.Database.ExecuteSqlCommand("insert into MovieDirectors values(2, 2)");
            //context.Database.ExecuteSqlCommand("insert into MovieDirectors values(3, 3)");
            //context.Database.ExecuteSqlCommand("insert into MovieDirectors values(4, 1)");
            #endregion
        }
    }
}
