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
    public string Url { get; set; } = string.Empty;
    public int Rating { get; set; }
    public List<Post> Posts { get; set; } = new List<Post>();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
    public List<PostTag> PostTags { get; set; } = new List<PostTag>();
}

public class Tag
{
    public int TagId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PostTag> PostTags { get; set; } = new List<PostTag>();
}


public class PostTag
{
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
