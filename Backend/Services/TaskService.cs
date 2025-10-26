// Services/TaskService.cs (partial)
public async Task<ScheduleResponseDto> GenerateSchedule(int projectId, ScheduleRequestDto request, int userId)
{
    // Verify project belongs to user
    var project = await _context.Projects
        .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
    
    if (project == null)
        throw new Exception("Project not found");

    // Topological sort for task dependencies
    var sortedTasks = TopologicalSort(request.Tasks);
    
    // Calculate start dates considering dependencies
    var startDates = CalculateStartDates(sortedTasks, request.Tasks);
    
    return new ScheduleResponseDto
    {
        RecommendedOrder = sortedTasks,
        StartDates = startDates
    };
}

private List<string> TopologicalSort(List<ScheduledTask> tasks)
{
    var graph = new Dictionary<string, List<string>>();
    var inDegree = new Dictionary<string, int>();
    
    // Initialize graph and in-degree
    foreach (var task in tasks)
    {
        graph[task.Title] = [];
        inDegree[task.Title] = 0;
    }
    
    // Build graph
    foreach (var task in tasks)
    {
        foreach (var dependency in task.Dependencies)
        {
            if (graph.ContainsKey(dependency))
            {
                graph[dependency].Add(task.Title);
                inDegree[task.Title]++;
            }
        }
    }
    
    // Topological sort using Kahn's algorithm
    var queue = new Queue<string>();
    foreach (var task in inDegree.Where(x => x.Value == 0))
    {
        queue.Enqueue(task.Key);
    }
    
    var result = new List<string>();
    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        result.Add(current);
        
        foreach (var neighbor in graph[current])
        {
            inDegree[neighbor]--;
            if (inDegree[neighbor] == 0)
            {
                queue.Enqueue(neighbor);
            }
        }
    }
    
    return result.Count == tasks.Count ? result : throw new Exception("Circular dependency detected");
}

private Dictionary<string, DateTime> CalculateStartDates(List<string> order, List<ScheduledTask> tasks)
{
    var taskDict = tasks.ToDictionary(t => t.Title);
    var startDates = new Dictionary<string, DateTime>();
    var currentDate = DateTime.Today;
    
    foreach (var taskTitle in order)
    {
        var task = taskDict[taskTitle];
        DateTime startDate = currentDate;
        
        // Consider dependencies
        if (task.Dependencies.Any())
        {
            startDate = task.Dependencies
                .Select(dep => startDates.ContainsKey(dep) ? startDates[dep].AddHours(taskDict[dep].EstimatedHours) : currentDate)
                .Max();
        }
        
        // Don't start after due date
        if (task.DueDate.HasValue && startDate > task.DueDate.Value)
        {
            startDate = task.DueDate.Value.AddHours(-task.EstimatedHours);
        }
        
        startDates[taskTitle] = startDate;
        currentDate = startDate.AddHours(task.EstimatedHours);
    }
    
    return startDates;
}