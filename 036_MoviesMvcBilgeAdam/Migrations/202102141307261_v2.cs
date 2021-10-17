namespace _036_MoviesMvcBilgeAdam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MovieDirectors", "DirectorId", "dbo.Directors");
            DropForeignKey("dbo.MovieDirectors", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.Reviews", "MovieId", "dbo.Movies");
            AddForeignKey("dbo.MovieDirectors", "DirectorId", "dbo.Directors", "Id");
            AddForeignKey("dbo.MovieDirectors", "MovieId", "dbo.Movies", "Id");
            AddForeignKey("dbo.Reviews", "MovieId", "dbo.Movies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.MovieDirectors", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.MovieDirectors", "DirectorId", "dbo.Directors");
            AddForeignKey("dbo.Reviews", "MovieId", "dbo.Movies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MovieDirectors", "MovieId", "dbo.Movies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MovieDirectors", "DirectorId", "dbo.Directors", "Id", cascadeDelete: true);
        }
    }
}
