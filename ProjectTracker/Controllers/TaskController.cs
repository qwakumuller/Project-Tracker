using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data;
using ProjectTracker.DTO;
using ProjectTracker.Models;

namespace ProjectTracker.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private ProjectTrackerContext _dbContext;

    public TaskController(ProjectTrackerContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    //create a new task
    [HttpPost]
    public async Task<IActionResult> AddTask([FromBody] ProjectTracker.Models.Task task)
    {
        _dbContext.Add(task).State = EntityState.Added;
        await _dbContext.SaveChangesAsync();
        return Created("", task);
    }

    //return all tasks from a project by Project ID
    [HttpGet("project/{id}")] 
    public async Task<IActionResult> GetProjectTasks([FromRoute] int id)
    {
        var result = await _dbContext.Tasks
            .Include(x => x.Status)
            .Where(p => p.ProjectId == id)
            .ToListAsync();

        List<TaskDTO> tasks = new();

        foreach (var task in result)
        {
            var userResult = await _dbContext.Users.Where(x => x.UserId == task.UserId).FirstOrDefaultAsync();
            var preTask = await _dbContext.Tasks.Where(x => x.TaskId == task.PrereqTaskId).FirstOrDefaultAsync();
            tasks.Add(new()
            {
                TaskId = task.TaskId,
                ProjectId = task.ProjectId,
                Name = task.Name,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                IsComplete = task.IsComplete,
                StatusName = task?.Status?.Name ?? "-",
                PrereqTaskId = task?.PrereqTaskId,
                UserId = task?.UserId,
                User = userResult != null ? new()
                {
                    Id = task?.UserId,
                    LastName = userResult.LastName,
                    FirstName = userResult.FirstName,
                    Email = userResult.Email                    
                } : null,
                PreTask = preTask != null ? new()
                {
                    Id = preTask.TaskId,
                    TaskName = preTask.Name,
                    IsComplete = preTask.IsComplete,
                } : null
            });
        }

        return Ok(tasks);
    }
    
    //Return a task by ID
    [HttpGet("{id}")]
    public async Task<IActionResult>  GetTask([FromRoute] int id)
    {
        var task = await _dbContext.Tasks.FindAsync(id);
        if (task is null)
        {
            return NotFound($"Task with ID {id} was not found.");
        }
        
        return Ok(task);        
    }
    
    //delete a task by ID
    [HttpDelete("{id}")] 
    public async Task<IActionResult> DeleteTask([FromRoute] int id)
    {
        var task = await _dbContext.Tasks.FindAsync(id);
        if (task is null)
        {
            return NotFound($"Task with ID {id} was not found.");
        }
        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
    //update a task    
    [HttpPut]
    public async Task<IActionResult> UpdateTask([FromBody] ProjectTracker.Models.Task task)
    {
        var result = await _dbContext.Tasks.FindAsync(task.TaskId);
        if (result is null)
        {
            return NotFound($"Task with ID {task.TaskId} was not found.");
        }
        else
        {           
            result.ProjectId = task.ProjectId ?? result.ProjectId;
            result.Name = task.Name ?? result.Name;
            result.Description = task.Description ?? result.Description;
            result.StartDate = task.StartDate != DateTime.MinValue ? task.StartDate : result.StartDate;
            result.EndDate = task.EndDate != DateTime.MinValue ? task.EndDate : result.EndDate;
            result.IsComplete = task.IsComplete;
            result.PrereqTaskId = task.PrereqTaskId == 0 ? null : task.PrereqTaskId ?? result.PrereqTaskId;
            result.UserId = task.UserId == 0 ? null : task.UserId ?? result.UserId;
            result.StatusId = task.StatusId == 0 ? null : task.StatusId ?? result.StatusId;
            result.Status = task.Status ?? result.Status;
            // StatusName = task?.Status?.Name ?? "-",
        }
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    // Return all tasks for the Gantt Chart by a project ID
    [HttpGet("project/{id}/chart-tasks")]
    public async Task<IActionResult> GetProjectTasksForChart([FromRoute] int id)
    {
        var parentTasks = await _dbContext.Tasks
            .Include(x => x.Status)
            .Where(p => p.ProjectId == id)
            .ToListAsync();

        List<ChartTaskDTO> tasks = new();

        foreach (var task in parentTasks)
        {
            var childTasks = await _dbContext.Tasks.Where(x => x.TaskId == task.PrereqTaskId).ToListAsync();
            tasks.Add(new()
            {
                Id = task.TaskId,
                Name = task.Name,
                Start = task.StartDate,
                End = task.EndDate,
                IsDisabled = !task.IsComplete,
                Dependencies = childTasks.Select(x => x.TaskId).ToList(),
                Progress = CalculateProgress(task.StartDate, task.EndDate)
            });
        }

        return Ok(tasks);
    }

    private double CalculateProgress(DateTime start, DateTime end)
    {
        var dif = (DateTime.Now - start).TotalDays / (end - start).TotalDays * 100;
        if (dif < 0)
            return 0;
        if (dif > 100)
            return 100;
        return dif;
    }
}
