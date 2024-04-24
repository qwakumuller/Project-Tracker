using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ProjectTracker.Data;
using ProjectTracker.Models;
using ProjectTracker.Utils;

namespace ProjectTracker.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private ProjectTrackerContext _databaseContext;

    public UserController(ProjectTrackerContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _databaseContext.Users.ToListAsync();
        return Ok(result);

    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var user = await _databaseContext.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound($"User with ID {id} was not found.");
        }
        return Ok(user);

    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User user)
    {
        var existingUser = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
       
        if (existingUser is not null) 
        {
            return Conflict($"User Email {user.Email} already exists.");
        }
        _databaseContext.Entry(user).State = EntityState.Added;
        await _databaseContext.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        var dbUser = await _databaseContext.Users.FindAsync(user.UserId);
        if (dbUser is null)
        {
           return NotFound($"User with id: {user.UserId} was not found to update.");
        }

        // If email update is requested
        if (user.Email != dbUser.Email)
        {
            // Then search to find if existing email is in use
            var existingUserWithEmail = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUserWithEmail is not null)
            {
                return Conflict($"Cannot update User id: {user.UserId} with email: {user.Email} as that email is already in use.");
            }
        }
        

        dbUser.Email = user.Email;
        dbUser.FirstName = user.FirstName;
        dbUser.LastName = user.LastName;
        dbUser.IsAdmin = user.IsAdmin;
        dbUser.Password = user.Password;
        
        await _databaseContext.SaveChangesAsync();
        
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserById([FromRoute] int id)
    {
        var user = await _databaseContext.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound($"User with ID {id} was not found.");
        }

        _databaseContext.Entry(user).State = EntityState.Deleted;
        await _databaseContext.SaveChangesAsync();
        return Ok(user);

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {

        var findUser =
            await _databaseContext.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);
        if (findUser is null)
        {
            return Unauthorized("Wrong Email and Password Combination");
        }

        var cookie = new CookieOptions();
        Response.Cookies.Append("userId", findUser.UserId.ToString(), cookie);
        return Ok("Login Successfully ");
        

    }

    [HttpDelete("logout")]
    public IActionResult Logout()
    {
        if (Request.Cookies["userId"] is not null)
        {
            var cookie = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            Response.Cookies.Append("userId", "null", cookie);
        }

        return Ok("Logout Successfully");
    }


}
