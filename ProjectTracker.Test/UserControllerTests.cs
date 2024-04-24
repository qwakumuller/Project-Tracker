using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Controllers;
using ProjectTracker.Data;
using ProjectTracker.Models;
using ProjectTracker.Utils;
using Xunit.Abstractions;

namespace ProjectTracker.Test;
public class UserControllerTests : IDisposable
{
    private UserController _controller;
    private ProjectTrackerContext _dbContext;

    public UserControllerTests()
    {
        _dbContext = new(new DbContextOptionsBuilder<ProjectTrackerContext>()
            .UseInMemoryDatabase(databaseName: "UserControllerTestDb").Options);
        _controller = new UserController(_dbContext);
    }

    [Fact]
    public async void GetUsersTest()
    {
        User u1 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Joe",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e"
        };
        User u2 = new User()
        {
            Email = "janesmith@gmail.com",
            FirstName = "Jane",
            LastName = "Smith",
            IsAdmin = false,
            Password = "1q2w3e4r"
        };
        User u3 = new User()
        {
            Email = "Franksredhot@gmail.com",
            FirstName = "Frank",
            LastName = "Redhot",
            IsAdmin = true,
            Password = "1q2w3e4r5t"
        };
        await _controller.Post(u1);
        await _controller.Post(u2);
        await _controller.Post(u3);
        var result = await _controller.GetUsers();
        Assert.IsType<OkObjectResult>(result);
        var ok = result as OkObjectResult;
        Assert.IsType<List<User>>(ok?.Value);
        var users = ok?.Value as List<User>;
        Assert.Equal(3, users?.Count);
    }

    [Fact]
    public async void GetUserByIdTest()
    {
        User u1 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Joe",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e"
        };
        User u2 = new User()
        {
            Email = "janesmith@gmail.com",
            FirstName = "Jane",
            LastName = "Smith",
            IsAdmin = false,
            Password = "1q2w3e4r"
        };
        await _controller.Post(u1);
        await _controller.Post(u2);
        var result = await _controller.GetUserById(1);
        Assert.IsType<OkObjectResult>(result);
        var ok = result as OkObjectResult;
        Assert.IsType<User>(ok?.Value);
        var user = ok?.Value as User;
        Assert.Equal(u1, user);
    }

    [Fact]
    public async void DeleteUserById()
    {
        User u1 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Joe",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e"
        };
        User u2 = new User()
        {
            Email = "janesmith@gmail.com",
            FirstName = "Jane",
            LastName = "Smith",
            IsAdmin = false,
            Password = "1q2w3e4r"
        };
        await _controller.Post(u1);
        await _controller.Post(u2);

        var result = await _controller.DeleteUserById(1);
        Assert.IsType<OkObjectResult>(result);

        var getAllResult = await _controller.GetUsers();
        Assert.IsType<OkObjectResult>(getAllResult);
        var ok = getAllResult as OkObjectResult;
        Assert.IsType <List<User>>(ok?.Value);
        var users = ok?.Value as List<User>;
        Assert.Equal(1, users?.Count);
        
    }

    [Fact]
    public async void DeleteUserDoesNotExist_ShouldFail()
    {
        var result = await _controller.DeleteUserById(99999);
        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Fact]
    public async void UserNotFound_ShouldFail()
    {
        var result = await _controller.GetUserById(99999);
        Assert.IsType<NotFoundObjectResult>(result);

    }
    [Fact]
    public async void UserUpdateWithExistingEmailFails()
    {
        User u1 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Joe",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e"
        };
        User u2 = new User()
        {
            Email = "janesmith@gmail.com",
            FirstName = "Jane",
            LastName = "Smith",
            IsAdmin = false,
            Password = "1q2w3e4r"
        };
        await _controller.Post(u1);
        await _controller.Post(u2);

        User u3 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Jane",
            LastName = "Smith",
            IsAdmin = true,
            Password = "12334",
            UserId = 2
        };

        var result = await _controller.UpdateUser(u3);
        Assert.IsType<ConflictObjectResult>(result);
        
    }

    [Fact]
    public async void CreateUserTwice()
    {
        User u1 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Joe",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e"
        };

        await _controller.Post(u1);

        var result = await _controller.Post(u1);
        Assert.IsType<ConflictObjectResult>(result);
    }
    
    [Fact]
    public async void UpdateUserDoesNotExist()
    {
        User u1 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Joe",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e"
        };
        User u2 = new User()
        {
            Email = "janesmith@gmail.com",
            FirstName = "Jane",
            LastName = "Smith",
            IsAdmin = false,
            Password = "1q2w3e4r"
        };
        await _controller.Post(u1);

        var result = await _controller.UpdateUser(u2);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async void UpdateUser()
    {
        User u1 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Joe",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e"
        };
        User u2 = new User()
        {
            Email = "janesmith@gmail.com",
            FirstName = "Jane",
            LastName = "Smith",
            IsAdmin = false,
            Password = "1q2w3e4r"
        };
        await _controller.Post(u1);
        await _controller.Post(u2);

        User updatedU1 = new User()
        {
            Email = "xxyy@gmail.com",
            FirstName = "Joe",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e",
            UserId = 1
        };

        await _controller.UpdateUser(updatedU1);
        var getResult = await _controller.GetUserById(1);

        Assert.IsType<OkObjectResult>(getResult);
        var ok = getResult as OkObjectResult;
        Assert.IsType<User>(ok?.Value);
        var updatedUser = ok?.Value as User;
        
        Assert.Equal(updatedU1.Email, updatedUser?.Email);
    }

    [Fact]
    public async void UpdateUserNameOnly()
    {
        User u1 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Joe",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e"
        };
        await _controller.Post(u1);

        User updatedU1 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Joseph",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e",
            UserId = 1
        };

        await _controller.UpdateUser(updatedU1);
        var getResult = await _controller.GetUserById(1);

        Assert.IsType<OkObjectResult>(getResult);
        var ok = getResult as OkObjectResult;
        Assert.IsType<User>(ok?.Value);
        var updatedUser = ok?.Value as User;
        Assert.Equal("Joseph", updatedUser?.FirstName);
    }
    
    [Fact]
    public async void LoginTestWrongCredentials()
    {
        
        User u1 = new User()
        {
            Email = "joesmith@gmail.com",
            FirstName = "Joe",
            LastName = "Smith",
            IsAdmin = true,
            Password = "1q2w3e"
        };
        
        LoginRequest l1 = new LoginRequest()
        {
            Email = "joesmith@gmail.com",
            Password = "1q2w3e10"
        };
        
        await _controller.Post(u1);
        var response = _controller.Login(l1);
        var getResponse = response.Result.GetValue<ResponseHeaders>();
        Assert.IsType<UnauthorizedObjectResult>(response.Result);
        var UnAuthorizedResult = response.Result as UnauthorizedObjectResult;
        
        Assert.Equal("Wrong Email and Password Combination", UnAuthorizedResult.Value);
        
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
    }
}