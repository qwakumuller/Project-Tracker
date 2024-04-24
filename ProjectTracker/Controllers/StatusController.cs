using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data;
using ProjectTracker.Models;

namespace ProjectTracker.Controllers;

[Route("api/status")]
[ApiController]
public class StatusController : ControllerBase
{
    private ProjectTrackerContext _dbContext;

    public StatusController(ProjectTrackerContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    /// <summary>
    /// Create a status
    /// </summary>
    /// <param name="value">Status object</param>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Status value)
    {
        var result = await _dbContext.Statuses.FirstOrDefaultAsync(x => x.Name == value.Name);
        if (result != null)
            return BadRequest("Status Exists");

        _dbContext.Add(value);
        await _dbContext.SaveChangesAsync();

        return Ok("Status Added");
    }

    /// <summary>
    /// Update a status
    /// </summary>
    /// <param name="value">Status object</param>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Status value)
    {
        var result = await _dbContext.Statuses.SingleOrDefaultAsync(x => x.StatusId == value.StatusId);
        if (result == null)
            return NotFound("Status does not exist");

        var exist = await _dbContext.Statuses.FirstOrDefaultAsync(x => x.Name == value.Name && x.ProjectId == value.ProjectId);
        if (exist?.Name == value.Name)
            return BadRequest("Status Exists");

        result.Name = value.Name;

        await _dbContext.SaveChangesAsync();
        return Ok("Status Updated");
    }

    /// <summary>
    /// Delete a status
    /// </summary>
    /// <param name="id">Status ID</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _dbContext.Statuses.SingleOrDefaultAsync(x => x.StatusId == id);
        if (result == null)
            return NotFound("Status does not exist");

        _dbContext.Statuses.Remove(result);
        await _dbContext.SaveChangesAsync();
        return Ok("Status Deleted");
    }

    /// <summary>
    /// Get all the statuses associated with a given project
    /// </summary>
    /// <param name="id">Project ID</param>
    [HttpGet("{id}/all")]
    public async Task<IActionResult> GetAllStatuses(int id)
    {
        var result = await _dbContext.Statuses.Where(x => x.ProjectId == id).ToListAsync();
        if (result.Count == 0)
            return BadRequest("No Statuses");

        return Ok(result);
    }

    /// <summary>
    /// Get only active the statuses associated with a given project
    /// </summary>
    /// <param name="id">Project ID</param>
    [HttpGet("{id}/only-active")]
    public async Task<IActionResult> GetOnlyActiveStatuses(int id)
    {
        var result = await _dbContext.Statuses.Where(x => x.ProjectId == id && x.IsActive).ToListAsync();
        if (result.Count == 0)
            return BadRequest("No Statuses");

        return Ok(result);
    }

    /// <summary>
    /// If the status is active, change it to inactive
    /// </summary>
    /// <param name="id">Status ID</param>
    [HttpPut("{id}/toggle")]
    public async Task<IActionResult> Put(int id)
    {
        var result = await _dbContext.Statuses.SingleOrDefaultAsync(x => x.StatusId == id);
        if (result == null)
            return NotFound("Status does not exist");

        if (result.IsActive) result.IsActive = false;
        else result.IsActive = true;

        await _dbContext.SaveChangesAsync();
        return Ok($"Status {(result.IsActive ? "Enabled" : "Disabled")}");
    }
}
