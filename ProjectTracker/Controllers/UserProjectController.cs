using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data;
using ProjectTracker.Models;

namespace ProjectTracker.Controllers;

[ApiController]
[Route("api/userproject")]
public class UserProjectController : Controller
{
    private ProjectTrackerContext _projectTrackerContext;
 
    public UserProjectController(ProjectTrackerContext projectTrackerContext)
    {
        _projectTrackerContext = projectTrackerContext;
    }
  
    [HttpPost]
    public async Task<IActionResult> AddUserToProjects([FromBody] UserProject request)
    {
        var existObject = await GetUserProject(request.UserId, request.ProjectId);
        if (existObject != null)
            return BadRequest("The user was already added to this project");

        var user = await GetUserById(request.UserId);
        if (user == null)
            return NotFound("User Does Not Exist");

        var userProject = await _projectTrackerContext.Projects.SingleOrDefaultAsync(x => x.ProjectId == request.ProjectId);
        if (userProject == null)
            return NotFound("Project Does Not Exist");

        _projectTrackerContext.UserProject.Add(new() { ProjectId=request.ProjectId, UserId=request.UserId});
        await _projectTrackerContext.SaveChangesAsync();

        return Ok("true");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUserFromProject([FromBody] UserProject userProjectRequest)
    {
        var userProject = await GetUserProject(userProjectRequest.UserId, userProjectRequest.ProjectId);

        if (userProject is null)
            return BadRequest("Project or User Does Not Exist");

        _projectTrackerContext.Entry(userProject).State = EntityState.Deleted;
        await _projectTrackerContext.SaveChangesAsync();
        
        return Ok("true");
    }
    
    private async Task<User?> GetUserById(int userId)
    {
       return await _projectTrackerContext.Users.FindAsync(userId);
    }

    private async Task<List<UserProject>> GetAllUserProjects()
    {
        return await _projectTrackerContext.UserProject.ToListAsync();
    }
    
    private async Task<UserProject?> GetUserProject(int userId, int userProjectId)
    {
        var userproject = await _projectTrackerContext.UserProject.FirstOrDefaultAsync(userproject =>
            userproject.UserId == userId && userproject.ProjectId == userProjectId);
        return userproject;

    }
}