using System.Data.Entity;
using _036_MoviesMvcBilgeAdam.Entities;

namespace _036_MoviesMvcBilgeAdam.Contexts
{
    public class MoviesContext : DbContext
    {
        public MoviesContext() : base("MoviesContext") // web.config'teki connection string name parametre olarak gönderilir
        {
            
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MovieDirector> MovieDirectors { get; set; }

        // Package Manager Console migration komutları:
        // Enable-Migrations -ContextTypeName _036_MoviesMvcBilgeAdam.Contexts.MoviesContext (Eğer tek bir DbContext class'ımız olsaydı enable-migrations yazmak yeterli olacaktı)
        // add-migration v1
        // update-database (update-database -verbose komutu ile çalıştırılan SQL sorguları görülebilir)
        /* Eğer Identity Framework kullanıyorsak kullanıcılarla ilgili tabloların veritabanımızda oluşturulması için:
        1) ~/Models/IdentityModels.cs içinde public ApplicationDbContext() constructor'ında base için parametre olarak projemizin web.config dosyasında tanımladığımız connection string name'ini yazıyoruz,
        2) Web uygulamamızı çalıştırıp Register link'i üzerinden yeni bir kullanıcı oluşturuyoruz. Girilen kullanıcı ve ilgili kullanıcı tabloları Entity Framework'ün yeteneğinden dolayı otomatik oluşacaktır.
        */
        // Örnek olarak olşturduğum kullanıcı adı: cagil@alsac.com, şifre: Cagil123!

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Fluent API:
            modelBuilder.Entity<Review>()
                .HasRequired(review => review.Movie) // required (HasRequired()): mutlaka olmalı, optional (HasOptional()): olmasa da olur
                .WithMany(movie => movie.Reviews)
                .HasForeignKey(review => review.MovieId)
                .WillCascadeOnDelete(false); // veritabanındaki Review ile Movie arasındaki ilişkide delete rule no action olur
            modelBuilder.Entity<MovieDirector>()
                .HasRequired(movieDirector => movieDirector.Movie)
                .WithMany(movie => movie.MovieDirectors)
                .HasForeignKey(movieDirector => movieDirector.MovieId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<MovieDirector>()
                .HasRequired(movieDirector => movieDirector.Director)
                .WithMany(director => director.MovieDirectors)
                .HasForeignKey(movieDirector => movieDirector.DirectorId)
                .WillCascadeOnDelete(false);
        }
    }
}