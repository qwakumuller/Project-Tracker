using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Controllers;
using ProjectTracker.Data;
using ProjectTracker.Models;

namespace ProjectTracker.Test;

[TestCaseOrderer("ProjectTracker.Test.TestInOrder", "ProjectTracker.Test")]
public class ProjectControllerTests : IDisposable
{
    private ProjectController _controller;
    private ProjectTrackerContext _dbContext;

    public ProjectControllerTests()
    {
        _dbContext = new(new DbContextOptionsBuilder<ProjectTrackerContext>()
            .UseInMemoryDatabase(databaseName: "ProjectControllerDb").Options);
        _controller = new ProjectController(_dbContext);
    }

    [Fact]
    public async void AGetNullProjectsTest()
    {
        var projectEntities = await _controller.GetProjects();

        var result = projectEntities.GetValue<List<Project>>();

        Assert.IsType<OkObjectResult>(projectEntities);
        Assert.True(result?.Count == 0);
    }

    [Fact]
    public async void BAddProjectTest()
    {
        var projectEntity = await _controller.AddProject(new Project
        {
            Name = "Project Tracker B",
            Description = "A very complex project",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(3),
        });

        var result = projectEntity.GetValue<Project>();

        Assert.IsType<OkObjectResult>(projectEntity);
        Assert.True(result is not null && result.Name.Equals("Project Tracker B"));
    }

    [Fact]
    public async void CGetValidProjectByIdTest()
    {
        await _controller.AddProject(new Project
        {
            Name = "Project Tracker C",
            Description = "A very complex project",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(3),
        });

        var projectEntity = await _controller.GetProjectById(1);

        var result = projectEntity.GetValue<Project>();

        Assert.IsType<OkObjectResult>(projectEntity);
        Assert.True(result is not null && result.Name.Equals("Project Tracker C"));
    }

    [Fact]
    public async void DGetInvalidProjectByIdTest()
    {
        var projectEntity = await _controller.GetProjectById(99);

        var result = projectEntity.GetValue<Project>();

        Assert.IsType<NotFoundObjectResult>(projectEntity);
        Assert.True(result is null);
    }

    [Fact]
    public async void EGetProjectsTest()
    {
        await _controller.AddProject(new Project
        {
            Name = "Project Tracker E1",
            Description = "A very complex project",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(3),
        });

        await _controller.AddProject(new Project
        {
            Name = "Project Tracker E2",
            Description = "A very complex project",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(4),
        });

        var projectEntities = await _controller.GetProjects();

        var result = projectEntities.GetValue<List<Project>>();

        Assert.IsType<OkObjectResult>(projectEntities);
        Assert.True(result?.Count == 2);
    }

    [Fact]
    public async void FUpdateExistingProjectTest()
    {
        var projectEntity = new Project
        {
            Name = "Project Tracker F1",
            Description = "A very complex project",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(3),
        };

        await _controller.AddProject(projectEntity);

        projectEntity.Name = "Project Tracker F2";

        var projectResult = await _controller.Put(projectEntity);

        var result = projectResult.GetValue<Project>();

        Assert.IsType<CreatedResult>(projectResult);
        Assert.True(result is not null && result.Name.Equals("Project Tracker F2"));
    }

    [Fact]
    public async void GUpdateNotExistingProjectTest()
    {
        var projectEntity = new Project
        {
            ProjectId = 99,
            Name = "Project Tracker 1",
            Description = "A very complex project",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(3),
        };

        // The put controller will try to insert the object if it does not exist.
        var projectResult = await _controller.Put(projectEntity);

        var result = projectResult.GetValue<Project>();

        Assert.IsType<CreatedResult>(projectResult);
        Assert.True(result is not null && result.Name.Equals("Project Tracker 1"));
    }

    [Fact]
    public async void HDeleteExistingProjectTest()
    {
        var projectEntity = new Project
        {
            ProjectId = 85,
            Name = "Project Tracker 85",
            Description = "A very complex project",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(3),
        };
        await _controller.AddProject(projectEntity);

        var projectResult = await _controller.DeleteProject(projectEntity.ProjectId);

        var result = projectResult.GetValue<Project>();

        Assert.IsType<OkResult>(projectResult);
        Assert.True(result is not null);
    }

    [Fact]
    public async void IDeleteNotExistingProjectTest()
    {
        var projectResult = await _controller.DeleteProject(61);

        var result = projectResult.GetValue<Project>();

        Assert.IsType<NotFoundObjectResult>(projectResult);
        Assert.True(result is null);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
    }
}
