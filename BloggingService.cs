using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace niceone
{
    public class BloggingService
    {
        private readonly BloggingContext _context;

        public BloggingService(BloggingContext context)
        {
            _context = context;
        }

        public void EnsureDatabaseCreated()
        {
            _context.Database.EnsureCreated();
        }

        public bool UsersExist()
        {
            return _context.Users.Any();
        }

        public void CreateUser()
        {
            Console.Write("Enter new username: ");
            string? newUsername = Console.ReadLine();
            Console.Write("Enter new password: ");
            string? newPassword = Console.ReadLine();

            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newPassword))
            {
                Console.WriteLine("Username and password cannot be empty.");
                return;
            }

            var newUser = new User { Username = newUsername, Password = newPassword };
            _context.Users.Add(newUser);
            _context.SaveChanges();

            Console.WriteLine("User created successfully.");
        }

        public bool AuthenticateUser(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            return user != null;
        }

        public void AddNewData()
        {
            Console.WriteLine("Do you want to add a new blog? (yes/no)");
            string? response = Console.ReadLine()?.ToLower();

            if (response == "yes" || response == "y")
            {
                Console.Write("Enter Blog URL: ");
                string? url = Console.ReadLine();
                Console.Write("Enter Blog Rating: ");
                string? ratingInput = Console.ReadLine();
                int rating;

                if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(ratingInput) || !int.TryParse(ratingInput, out rating))
                {
                    Console.WriteLine("Invalid input for URL or Rating.");
                    return;
                }

                var newBlog = new Blog { Url = url, Rating = rating };
                _context.Blogs.Add(newBlog);
                _context.SaveChanges();

                Console.WriteLine("Blog added successfully.");

                Console.WriteLine("Do you want to add a post to this blog? (yes/no)");
                response = Console.ReadLine()?.ToLower();

                if (response == "yes" || response == "y")
                {
                    Console.Write("Enter Post Title: ");
                    string? title = Console.ReadLine();
                    Console.Write("Enter Post Content: ");
                    string? content = Console.ReadLine();

                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                    {
                        Console.WriteLine("Title and Content cannot be empty.");
                        return;
                    }

                    var newPost = new Post { Title = title, Content = content, BlogId = newBlog.BlogId };
                    _context.Posts.Add(newPost);
                    _context.SaveChanges();

                    Console.WriteLine("Post added successfully.");

                    Console.WriteLine("Do you want to add tags to this post? (yes/no)");
                    response = Console.ReadLine()?.ToLower();

                    if (response == "yes" || response == "y")
                    {
                        AddTagsToPost(newPost.PostId);
                    }
                }
            }
        }

        public void DeleteBlog()
        {
            Console.Write("Enter new Blogname: ");
            string? blogname = Console.ReadLine();

            if (string.IsNullOrEmpty(blogname))
            {
                Console.WriteLine("Blogname cannot be empty.");
                return;
            }

            var blog = _context.Blogs.SingleOrDefault(b => b.Url == blogname);
            if (blog == null)
            {
                Console.WriteLine("Blog not found.");
                return;
            }

            _context.Blogs.Remove(blog);
            _context.SaveChanges();

            Console.WriteLine("Blog deleted successfully.");
        }

        public void AddTagsToPost(int postId)
        {
            Console.WriteLine("Enter tags separated by commas:");
            string? tagsInput = Console.ReadLine();
            if (string.IsNullOrEmpty(tagsInput))
            {
                Console.WriteLine("No tags entered.");
                return;
            }

            var tags = tagsInput.Split(',')
                                .Select(t => t.Trim())
                                .Where(t => !string.IsNullOrEmpty(t))
                                .ToList();

            foreach (var tagName in tags)
            {
                var tag = _context.Tags.SingleOrDefault(t => t.Name == tagName) ?? new Tag { Name = tagName };
                if (tag.TagId == 0) // Tag doesn't exist in the database
                {
                    _context.Tags.Add(tag);
                    _context.SaveChanges(); // Save to get the TagId
                }
                _context.PostTags.Add(new PostTag { PostId = postId, TagId = tag.TagId });
            }
            _context.SaveChanges();

            Console.WriteLine("Tags added successfully.");
        }

        public void DisplayData()
        {
            var blogs = _context.Blogs
                .Include(b => b.Posts)
                .ThenInclude(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .ToList();

            foreach (var blog in blogs)
            {
                Console.WriteLine($"Blog: {blog.Url ?? "No URL"} (Rating: {blog.Rating})");
                
                foreach (var post in blog.Posts)
                {
                    Console.WriteLine($"  Post: {post.Title ?? "No Title"} - {post.Content ?? "No Content"}");
                    var tags = post.PostTags.Select(pt => pt.Tag.Name);
                    Console.WriteLine("    Tags: [" + string.Join(", ", tags) + "]");
                }
            }
        }
    }
}
