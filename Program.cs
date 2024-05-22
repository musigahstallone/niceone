using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace niceone;

class Program
{
    static void Main()
    {
        using var context = new BloggingContext();
        // Ensure database is created
        context.Database.EnsureCreated();

        // Prompt user for credentials
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        // Authenticate user
        if (AuthenticateUser(username, password, context))
        {
            Console.WriteLine("Authentication successful.");

            // Add new data
            // AddNewData(context);

            // Fetch and display data
            var blogs = context.Blogs.Include(b => b.Posts).ToList();
            foreach (var blog in blogs)
            {
                Console.WriteLine($"Blog: {blog.Url ?? "No URL"} (Rating: {blog.Rating})");
                if (blog.Posts != null)
                {
                    foreach (var post in blog.Posts)
                    {
                        Console.WriteLine($"  Post: {post.Title ?? "No Title"} - {post.Content ?? "No Content"}");
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Authentication failed. Access denied.");
        }
    }

    public static bool AuthenticateUser(string username, string password, BloggingContext context)
    {
        var user = context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
        return user != null;
    }

    public static void AddNewData(BloggingContext context)
    {
        Console.WriteLine("Do you want to add a new blog? (yes/no)");
        string response = Console.ReadLine()?.ToLower();

        if (response == "yes")
        {
            Console.Write("Enter Blog URL: ");
            string url = Console.ReadLine();
            Console.Write("Enter Blog Rating: ");
            int rating = int.Parse(Console.ReadLine());

            var newBlog = new Blog { Url = url, Rating = rating };
            context.Blogs.Add(newBlog);
            context.SaveChanges();

            Console.WriteLine("Blog added successfully.");

            Console.WriteLine("Do you want to add a post to this blog? (yes/no)");
            response = Console.ReadLine()?.ToLower();

            if (response == "yes")
            {
                Console.Write("Enter Post Title: ");
                string title = Console.ReadLine();
                Console.Write("Enter Post Content: ");
                string content = Console.ReadLine();

                var newPost = new Post { Title = title, Content = content, BlogId = newBlog.BlogId };
                context.Posts.Add(newPost);
                context.SaveChanges();

                Console.WriteLine("Post added successfully.");
            }
        }
    }
}
