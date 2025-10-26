// Models/User.cs
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public List<Project> Projects { get; set; } = [];
}

// Models/Project.cs
public class Project
{
    public int Id { get; set; }
    
    [Required, StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public int UserId { get; set; }
    public User? User { get; set; }
    public List<TaskItem> Tasks { get; set; } = [];
}

// Models/TaskItem.cs
public class TaskItem
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    
    // For scheduling
    public int? EstimatedHours { get; set; }
    public List<string> Dependencies { get; set; } = [];
}