using Microsoft.EntityFrameworkCore;
using WebApplicationBlogPost.Models;

namespace WebApplicationBlogPost.Data
{
    public class AppDbContext:DbContext
    {
        //This is Databas Connection section 1.Step
        //
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Technology" },
                new Category { Id = 2, Name = "Health" },
                new Category { Id = 3, Name = "LifStyle" },
                new Category { Id = 4, Name = "Education"}
                );

            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = 1,
                    Title = "Technology Post Content 1",
                    Content = "Technology Blog Post Content 1",
                    Author = "Sapi",
                    PublishedDate =new DateTime(2026,2,10), // static date instead  DateTime.Now,
                    CategoryId=1,
                    FeathureImagePath="Web_image.png", // simple image path
                },
                 new Post
                 {
                     Id = 2,
                     Title = "Health Post Content 1",
                     Content = "Health Blog Post Content 1",
                     Author = "Sapi",
                     PublishedDate = new DateTime(2026, 2, 10), // static date instead  DateTime.Now,
                     CategoryId = 2,
                     FeathureImagePath = "Web_image.png", // simple image path
                 },
                 new Post
                 {
                     Id = 3,
                     Title = "LifeStyle Post Content 1",
                     Content = "LifeStyle Blog Post Content 1",
                     Author = "Sapi",
                     PublishedDate = new DateTime(2026, 2, 10), // static date instead  DateTime.Now,
                     CategoryId = 3,
                     FeathureImagePath = "Web_image.png", // simple image path
                 },
                  new Post
                  {
                      Id = 4,
                      Title = "Education Post Content 1",
                      Content = "Education Blog Post Content 1",
                      Author = "Sapi",
                      PublishedDate = new DateTime(2026, 2, 10), // static date instead  DateTime.Now,
                      CategoryId = 4,
                      FeathureImagePath = "Web_image.png", // simple image path
                  }
                  );

        }

    }
}
