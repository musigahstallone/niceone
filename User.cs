namespace niceone;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // In a real-world application, store hashed passwords
}

public class Blog
{
    public int BlogId { get; set; }
    public string? Url { get; set; }
    public int Rating { get; set; }
    public List<Post> Posts { get; set; } = []; // Initialize the list
}

public class Post
{
    public int PostId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!; // Use null-forgiving operator
}
