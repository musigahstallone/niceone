using Microsoft.EntityFrameworkCore;
namespace niceone;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; } // Add the Users DbSet

    protected override void OnConfiguring(DbContextOptionsBuilder op)
    {
        op.UseSqlite("Data Source=blogging.db");
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Seed initial data
        mb.Entity<Blog>().HasData(
            new Blog { BlogId = 1, Url = "https://example.com/blog1", Rating = 5 },
            new Blog { BlogId = 2, Url = "https://example.com/blog2", Rating = 3 }
        );
        mb.Entity<Post>().HasData(
            new Post { PostId = 1, Title = "First Post", Content = "Content of the first post", BlogId = 1 },
            new Post { PostId = 2, Title = "Second Post", Content = "Content of the second post", BlogId = 1 },
            new Post { PostId = 3, Title = "Third Post", Content = "Content of the third post", BlogId = 2 }
        );
        // Seed initial user
        mb.Entity<User>().HasData(
            new User { UserId = 1, Username = "admin", Password = "password" } // Note: Hash passwords in real applications
        );
    }
}