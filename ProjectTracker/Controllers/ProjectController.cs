using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data;
using ProjectTracker.Models;

namespace ProjectTracker.Controllers;

[ApiController]
[Route("api/project")]
public class ProjectController : ControllerBase
{
    private ProjectTrackerContext _dbContext;
      
    public ProjectController(ProjectTrackerContext dbContext)
    {
      _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
      var result = await _dbContext.Projects.ToListAsync();
      return Ok(result);     
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectById([FromRoute] int id)
    {
        var project = await _dbContext.Projects.FindAsync(id);
        if (project is null)
        {
          return NotFound($"Project with ID {id} was not found.");
        }
        return Ok(project);
    }

  [HttpGet("user/{id}")]
  public async Task<IActionResult> GetProjectsByUserId([FromRoute] int id)
  {
    var projects = await (
        from p in _dbContext.Projects
        join up in _dbContext.UserProject on p.ProjectId equals up.ProjectId
        join u in _dbContext.Users on up.UserId equals u.UserId
        where u.UserId == id
        select p
      ).ToListAsync();
        if (projects.Count <= 0)
        {
          return NotFound($"Projects with User ID {id} were not found.");
        }
        return Ok(projects);
  }

  [HttpPost]
    public async Task<IActionResult> AddProject([FromBody] Project project)
    {
      _dbContext.Entry(project).State = EntityState.Added;
      await _dbContext.SaveChangesAsync();
      return Ok(project);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Project project)
    {
      var result = await _dbContext.Projects.FindAsync(project.ProjectId);
      if (result is null)
      {
        result = _dbContext.Projects.Add(project).Entity;
      }
      else
      {
        result.Name = project.Name ?? result.Name;
        result.Description = project.Description ?? result.Description;
        result.StartDate = project.StartDate != DateTime.MinValue ? project.StartDate : result.StartDate;
        result.EndDate = project.EndDate != DateTime.MinValue ? project.EndDate : result.EndDate;
      }
      await _dbContext.SaveChangesAsync();
      return Created("", result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject([FromRoute] int id)
    {
      var project = await _dbContext.Projects.FindAsync(id);
      if (project is null)
      {
        return NotFound($"Project with ID {id} was not found.");
      }
        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Get all the users that can be added to a project.
    /// </summary>
    /// <param name="id">Project ID</param>
    /// <returns>All available new users</returns>
    [HttpGet("{id}/new-users")]
    public async Task<IActionResult> AllNewUsers([FromRoute] int id)
    {
        var userProject = await _dbContext.UserProject
            .Where(x => x.ProjectId == id)
            .Select(x => x.UserId)
            .ToListAsync();

        var newUsers = await _dbContext.Users.Where(x => !userProject.Contains(x.UserId)).ToListAsync();

        return Ok(newUsers);
    }

    /// <summary>
    /// Get all the users that are associated with a given project.
    /// </summary>
    /// <param name="id">Project ID</param>
    /// <returns>All Users</returns>
    [HttpGet("{id}/all-users")]
    public async Task<IActionResult> AllUsers([FromRoute] int id)
    {
        var users = await _dbContext.UserProject
            .Where(x => x.ProjectId == id)
            .SelectMany(x => _dbContext.Users.Where(y => y.UserId == x.UserId))
            .Select(x => new
            {
                x.UserId,
                x.FirstName,
                x.LastName,
                x.Email
            })
            .ToListAsync();

        return Ok(users);
    }
}

