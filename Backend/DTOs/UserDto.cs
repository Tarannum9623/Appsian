// DTOs/UserDto.cs
public class UserRegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required, StringLength(100, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;
    
    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}

public class UserLoginDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

// DTOs/ProjectDto.cs
public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public int TaskCount { get; set; }
}

public class ProjectCreateDto
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
}

// DTOs/TaskDto.cs
public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public int ProjectId { get; set; }
    public int? EstimatedHours { get; set; }
    public List<string> Dependencies { get; set; } = [];
}

public class TaskCreateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int? EstimatedHours { get; set; }
    public List<string> Dependencies { get; set; } = [];
}

// DTOs/ScheduleDto.cs
public class ScheduleRequestDto
{
    public List<ScheduledTask> Tasks { get; set; } = [];
}

public class ScheduledTask
{
    public string Title { get; set; } = string.Empty;
    public int EstimatedHours { get; set; }
    public DateTime DueDate { get; set; }
    public List<string> Dependencies { get; set; } = [];
}

public class ScheduleResponseDto
{
    public List<string> RecommendedOrder { get; set; } = [];
    public Dictionary<string, DateTime> StartDates { get; set; } = [];
}