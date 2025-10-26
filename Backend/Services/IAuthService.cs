// Services/IAuthService.cs
public interface IAuthService
{
    Task<UserResponseDto> Register(UserRegisterDto registerDto);
    Task<UserResponseDto> Login(UserLoginDto loginDto);
    string GenerateJwtToken(User user);
}

// Services/IProjectService.cs
public interface IProjectService
{
    Task<List<ProjectDto>> GetUserProjects(int userId);
    Task<Project?> GetProject(int id, int userId);
    Task<Project> CreateProject(ProjectCreateDto projectDto, int userId);
    Task<bool> DeleteProject(int id, int userId);
}

// Services/ITaskService.cs
public interface ITaskService
{
    Task<TaskItem> CreateTask(int projectId, TaskCreateDto taskDto, int userId);
    Task<TaskItem?> UpdateTask(int id, TaskDto taskDto, int userId);
    Task<bool> DeleteTask(int id, int userId);
    Task<ScheduleResponseDto> GenerateSchedule(int projectId, ScheduleRequestDto request, int userId);
}