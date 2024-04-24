using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using ProjectTracker.Controllers;
using ProjectTracker.Data;
using ProjectTracker.DTO;
using ProjectTracker.Models;
using System.Threading.Tasks;
using Task = ProjectTracker.Models.Task;

namespace ProjectTracker.Test;

public class TaskControllerTests : IDisposable
{
    private TaskController _controller;
    private ProjectTrackerContext _dbContext;

    public TaskControllerTests()
    {
        _dbContext = new(new DbContextOptionsBuilder<ProjectTrackerContext>()
            .UseInMemoryDatabase(databaseName: "TaskControllerTestDb").Options);
        _controller = new TaskController(_dbContext);
    }

    [Fact]
    public async void AddTaskTest()
    {
        //check for existing task
        var result = await _dbContext.Tasks
            .FirstOrDefaultAsync(x => x.TaskId == 1);
        Assert.True(result is null);

        var task = new Task()
        {
            TaskId = 1,
            ProjectId = 10,
            Name = "Test",
            Description = "Description Test",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2024, 1, 1),
            IsComplete = false,
            PrereqTaskId = 10,
            UserId = 20,
        };

        await _controller.AddTask(task);

        result = await _dbContext.Tasks
            .FirstOrDefaultAsync(x => x.TaskId == task.TaskId);

        Assert.True(result is not null);
        Assert.Equal(result, task);
     }

    [Fact]
    public async void GetTaskByIdTest()
    {
        var result = await _controller.GetTask(1);
        Assert.IsType<NotFoundObjectResult>(result);

        Task task1 = new Task()
        {
            TaskId = 1,
            ProjectId = 10,
            Name = "Test1",
            Description = "Description Test1",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2024, 1, 1),
            IsComplete = false,
            PrereqTaskId = 10,
            UserId = 20,
        };

        Task task2 = new Task()
        {
            TaskId = 2,
            ProjectId = 10,
            Name = "Test2",
            Description = "Description Test1",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2024, 1, 1),
            IsComplete = false,
            PrereqTaskId = 11,
            UserId = 21,
        };
        await _controller.AddTask(task1);
        await _controller.AddTask(task2);

        var getResult = await _controller.GetTask(task1.TaskId);
        var okGetResult = getResult as OkObjectResult;
        Assert.IsType<OkObjectResult>(okGetResult);

        if (okGetResult?.Value is Task taskResult)
        {
             Assert.Equal(task1, taskResult);
        }
    }

    [Fact]
    public async void GetProjectTasks()
    {
        var result = await _controller.GetTask(1);
        Assert.IsType<NotFoundObjectResult>(result);

        Task task1 = new Task()
        {
            TaskId = 1,
            ProjectId = 10,
            Name = "Test1",
            Description = "Description Test1",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2024, 1, 1),
            PrereqTaskId = 10,
            UserId = 20,
        };

        Task task2 = new Task()
        {
            TaskId = 2,
            ProjectId = 10,
            Name = "Test2",
            Description = "Description Test1",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2024, 1, 1),
            PrereqTaskId = 11,
            UserId = 21,
        };
        await _controller.AddTask(task1);
        await _controller.AddTask(task2);

        var getResult = await _controller.GetProjectTasks(10);
        var okGetResult = getResult as OkObjectResult;
        Assert.IsType<OkObjectResult>(okGetResult);
        var taskResults = okGetResult?.Value as List<TaskDTO>;
        
        Assert.Equal(2, taskResults?.Count);
        Assert.Equal(task1.Name, taskResults?[0].Name);
        Assert.Equal(task2.Name, taskResults?[1].Name);

    }

    [Fact]
    public async void DeleteTaskTest()
    {
        Task task1 = new Task()
        {
            TaskId = 1,
            ProjectId = 10,
            Name = "Test1",
            Description = "Description Test1",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2024, 1, 1),
            IsComplete = false,
            PrereqTaskId = 10,
            UserId = 20,
        };

        var result = await _controller.GetTask(task1.TaskId);
        Assert.IsType<NotFoundObjectResult>(result);

        await _controller.AddTask(task1);


        var taskResult = await _dbContext.Tasks
           .FirstOrDefaultAsync(x => x.TaskId == task1.TaskId);
        Assert.True(result is not null);
        Assert.Equal(taskResult, task1);

        result = await _controller.DeleteTask(task1.TaskId);
        taskResult = await _dbContext.Tasks
            .FirstOrDefaultAsync(x => x.TaskId == task1.TaskId);
        Assert.True(taskResult is null);
    }
    
     [Fact]
    public async void DeleteTaskNoTaskExistsTest()
    {
        var result = await _controller.DeleteTask(1);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsType<NotFoundObjectResult>(notFoundResult);
        if (notFoundResult?.Value is String deleteResult)
        {
            Assert.Equal("Task with ID 1 was not found.", deleteResult);
        }
    }

    [Fact]
    public async void UpdateTaskTest()
    {
        Task task1 = new Task()
        {
            TaskId = 1,
            ProjectId = 10,
            Name = "Test1",
            Description = "Description Test1",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2024, 1, 1),
            IsComplete = false,
            PrereqTaskId = 10,
            UserId = 20,
        };
        
        //check for updates without an existing object
        var result = await _controller.UpdateTask(task1);
        Assert.IsType<NotFoundObjectResult>(result);

        await _controller.AddTask(task1);

        Task task1Update = new Task()
        {
            TaskId = 1,
            ProjectId = 20,
            Name = "Test2",
            Description = "Description Test2",
            StartDate = new DateTime(2022, 1, 1),
            EndDate = new DateTime(2025, 1, 1),
            IsComplete = true,
            PrereqTaskId = 11,
            UserId = 21,
        };

        await _controller.UpdateTask(task1Update);
        var taskResult = await _dbContext.Tasks
            .FirstOrDefaultAsync(x => x.TaskId == task1.TaskId);
        Assert.Equal(taskResult?.TaskId, task1Update.TaskId);
        Assert.Equal(taskResult?.ProjectId, task1Update.ProjectId);
        Assert.Equal(taskResult?.Name, task1Update.Name);
        Assert.Equal(taskResult?.Description, task1Update.Description);
        Assert.Equal(taskResult?.StartDate, task1Update.StartDate);
        Assert.Equal(taskResult?.EndDate, task1Update.EndDate);
        Assert.Equal(taskResult?.PrereqTaskId, task1Update.PrereqTaskId);
        Assert.Equal(taskResult?.IsComplete, task1Update.IsComplete);
        Assert.Equal(taskResult?.UserId, task1Update.UserId);
        Assert.Equal(taskResult?.ProjectId, task1Update.ProjectId);

    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
    }
}
