// Controllers/AuthController.cs
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDto>> Register(UserRegisterDto registerDto)
    {
        try
        {
            var user = await _authService.Register(registerDto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponseDto>> Login(UserLoginDto loginDto)
    {
        try
        {
            var user = await _authService.Login(loginDto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}

// Controllers/ProjectsController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var projects = await _projectService.GetUserProjects(userId);
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var project = await _projectService.GetProject(id, userId);
        return project != null ? Ok(project) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(ProjectCreateDto projectDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var project = await _projectService.CreateProject(projectDto, userId);
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var success = await _projectService.DeleteProject(id, userId);
        return success ? NoContent() : NotFound();
    }
}

// Controllers/TasksController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> UpdateTask(int id, TaskDto taskDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var task = await _taskService.UpdateTask(id, taskDto, userId);
        return task != null ? Ok(task) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var success = await _taskService.DeleteTask(id, userId);
        return success ? NoContent() : NotFound();
    }
}

// Controllers/ProjectTasksController.cs
[ApiController]
[Route("api/projects/{projectId}/[controller]")]
[Authorize]
public class ProjectTasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public ProjectTasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask(int projectId, TaskCreateDto taskDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var task = await _taskService.CreateTask(projectId, taskDto, userId);
        return CreatedAtAction(nameof(CreateTask), new { id = task.Id }, task);
    }

    [HttpPost("schedule")]
    public async Task<ActionResult<ScheduleResponseDto>> ScheduleTasks(int projectId, ScheduleRequestDto request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var schedule = await _taskService.GenerateSchedule(projectId, request, userId);
        return Ok(schedule);
    }
}