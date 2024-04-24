using ProjectTracker.Models;
using Task = ProjectTracker.Models.Task;

namespace ProjectTracker.Data;

public class DbInitializer
{
    public static void Initialize(ProjectTrackerContext context)
    {
        if (context.Users.Any()) return; //don't reseed database if data already exists

        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.smith@gmail.com",
            Password = "agdDhd44%sj_B!",
            IsAdmin = false
        };

        var user2 = new User
        {
            FirstName = "Manny",
            LastName = "Jah",
            Email = "manny.jah@gmail.com",
            Password = "agdDhd44%sj_B!",
            IsAdmin = true,
        };

        var user3 = new User
        {
            FirstName = "Mark",
            LastName = "Ateer",
            Email = "mark.ateer@gmail.com",
            Password = "agdDhd44%sj_B!",
            IsAdmin = true,
        };

        var user4 = new User
        {
            FirstName = "Reeve",
            LastName = "Kade",
            Email = "reeve.kade@gmail.com",
            Password = "agdDhd44%sj_B!",
            IsAdmin = true,
        };

        var user5 = new User
        {
            FirstName = "Emma",
            LastName = "Grate",
            Email = "emma.grate@gmail.com",
            Password = "agdDhd44%sj_B!",
            IsAdmin = true,
        };

        context.Users.Add(user);
        context.Users.Add(user2);
        context.Users.Add(user3);
        context.Users.Add(user4);
        context.Users.Add(user5);
        context.SaveChanges();

        var project = new Project
        {
            Name = "Project Tracker",
            Description = "A very complex project",
            StartDate = DateTime.Now,
            EndDate = new DateTime(2024, 6, 1),
            Statuses = new List<Status> { new() { Name = "Active", IsActive = true }, new() { Name = "Inactive", IsActive = true }, new() { Name = "Waiting", IsActive = true } }
        };

        var project2 = new Project
        {
            Name = "Shopping Cart Software",
            Description = "Online store",
            StartDate = DateTime.Now,
            EndDate = new DateTime(2024,1,1),
            Statuses = new List<Status> { new() {Name = "Active",IsActive = true}, new() {Name = "Inactive",IsActive = true} }
        };

        var project3 = new Project
        {
            Name = "Employee Manager",
            Description = "Allocate resources (people and materials) between jobs and organize them to meet objectives.",
            StartDate = DateTime.Now,
            EndDate = new DateTime(2024, 2, 1),
            Statuses = new List<Status> { new() { Name = "Active", IsActive = true }, new() { Name = "Waiting", IsActive = true } }
        };

        var project4 = new Project
        {
            Name = "Scheduling Software",
            Description = "Like Calendly",
            StartDate = DateTime.Now,
            EndDate = new DateTime(2024, 3, 1)
        };

        context.Projects.Add(project);
        context.Projects.Add(project2);
        context.Projects.Add(project3);
        context.Projects.Add(project4);
        context.SaveChanges();

        var userProject = new UserProject()
        {
            UserId = user.UserId,
            ProjectId = project.ProjectId
        };

        var userProject2 = new UserProject()
        {
            UserId = user2.UserId,
            ProjectId = project.ProjectId
        };

        var userProject3 = new UserProject()
        {
            UserId = user3.UserId,
            ProjectId = project.ProjectId
        };

        var userProject4 = new UserProject()
        {
            UserId = user4.UserId,
            ProjectId = project.ProjectId
        };

        var userProject5 = new UserProject()
        {
            UserId = user2.UserId,
            ProjectId = project2.ProjectId
        };

        var userProject6 = new UserProject()
        {
            UserId = user3.UserId,
            ProjectId = project2.ProjectId
        };

        var userProject7 = new UserProject()
        {
            UserId = user.UserId,
            ProjectId = project3.ProjectId
        };

        var userProject8 = new UserProject()
        {
            UserId = user2.UserId,
            ProjectId = project3.ProjectId
        };

        var userProject9 = new UserProject()
        {
            UserId = user3.UserId,
            ProjectId = project4.ProjectId
        };

        var userProject10 = new UserProject()
        {
            UserId = user4.UserId,
            ProjectId = project4.ProjectId
        };

        var userProject11 = new UserProject()
        {
            UserId = user.UserId,
            ProjectId = project2.ProjectId
        };

        var userProject12 = new UserProject()
        {
            UserId = user.UserId,
            ProjectId = project4.ProjectId
        };

        context.UserProject.Add(userProject);
        context.UserProject.Add(userProject2);
        context.UserProject.Add(userProject3);
        context.UserProject.Add(userProject4);
        context.UserProject.Add(userProject5);
        context.UserProject.Add(userProject6);
        context.UserProject.Add(userProject7);
        context.UserProject.Add(userProject8);
        context.UserProject.Add(userProject9);
        context.UserProject.Add(userProject10);
        context.UserProject.Add(userProject11);
        context.UserProject.Add(userProject12);
        context.SaveChanges();

        var task = new Task
        {
            Name = "Task 1",
            Description = "Description Test 1",
            ProjectId = project.ProjectId,
            UserId = user.UserId,
            IsComplete= true,
            StartDate = new(2023, 1, 17),
            EndDate = new(2023, 1, 30),
            StatusId = project.Statuses[0].StatusId
        };

        var task2 = new Task
        {
            Name = "Task 2",
            Description = "Description Test 2",
            ProjectId = project.ProjectId,
            UserId = user2.UserId,
            IsComplete = true,
            StartDate = new(2022, 6, 6),
            EndDate = new(2022, 7, 30),
            StatusId = project.Statuses[1].StatusId
        };

        var task3 = new Task
        {
            Name = "Task 3",
            Description = "Description Test 3",
            ProjectId = project.ProjectId,
            UserId = user3.UserId,
            IsComplete = false,
            StartDate = new(2023, 3, 3),
            EndDate = new(2023, 4, 18),
            StatusId = project.Statuses[2].StatusId
        };

        var task4 = new Task
        {
            Name = "Task 4",
            Description = "Description Test 4",
            ProjectId = project.ProjectId,
            UserId = user4.UserId,
            IsComplete = false,
            StartDate = new(2023, 1, 1),
            EndDate = new(2023, 6, 6),
            StatusId = project.Statuses[0].StatusId
        };

        var task5 = new Task
        {
            Name = "Task 5",
            Description = "Description Test 5",
            ProjectId = project.ProjectId,
            UserId = user5.UserId,
            IsComplete = false,
            StartDate = new(2023, 2, 17),
            EndDate = new(2023, 6, 19)
        };

        var task6 = new Task
        {
            Name = "Task 7",
            Description = "Description Test 7",
            ProjectId = project2.ProjectId,
            UserId = user.UserId,
            IsComplete = false,
            StartDate = new(2023, 1, 17),
            EndDate = new(2023, 5, 30)
        };

        var task7 = new Task
        {
            Name = "Task 8",
            Description = "Description Test 8",
            ProjectId = project2.ProjectId,
            UserId = user2.UserId,
            IsComplete = false,
            StartDate = new(2022, 10, 10),
            EndDate = new(2023, 3, 27)
        };

        var task8 = new Task
        {
            Name = "Task 9",
            Description = "Description Test 9",
            ProjectId = project2.ProjectId,
            UserId = user3.UserId,
            IsComplete = false,
            StartDate = new(2023, 2, 12),
            EndDate = new(2023, 4, 30)
        };

        var task9 = new Task
        {
            Name = "Task 10",
            Description = "Description Test 10",
            ProjectId = project.ProjectId,
            UserId = user4.UserId,
            IsComplete = false,
            StartDate = new(2023, 1, 14),
            EndDate = new(2023, 8, 19)
        };

        var task10 = new Task
        {
            Name = "Task 11",
            Description = "Description Test 11",
            ProjectId = project2.ProjectId,
            UserId = user5.UserId,
            IsComplete = false,
            StartDate = new(2023, 1, 17),
            EndDate = new(2023, 4, 17)
        };

        var task11 = new Task
        {
            Name = "Task 11",
            Description = "Description Test 11",
            ProjectId = project3.ProjectId,
            UserId = user2.UserId,
            IsComplete = false,
            StartDate = new(2023, 4, 14),
            EndDate = new(2023, 4, 30),
            StatusId = project3.Statuses[0].StatusId
        };

        var task12 = new Task
        {
            Name = "Task 12",
            Description = "Description Test 12",
            ProjectId = project3.ProjectId,
            UserId = user3.UserId,
            IsComplete = false,
            StartDate = new(2023, 6, 30),
            EndDate = new(2023, 8, 30),
            StatusId = project3.Statuses[1].StatusId
        };

        var task13 = new Task
        {
            Name = "Task 13",
            Description = "Description Test 13",
            ProjectId = project3.ProjectId,
            UserId = user4.UserId,
            IsComplete = true,
            StartDate = new(2023, 1, 17),
            EndDate = new(2023, 3, 8)
        };

        var task14 = new Task
        {
            Name = "Task 14",
            Description = "Description Test 14",
            ProjectId = project4.ProjectId,
            UserId = user5.UserId,
            IsComplete = true,
            StartDate = new(2022, 11, 1),
            EndDate = new(2023, 1, 30)
        };

        var task15 = new Task
        {
            Name = "Task 15",
            Description = "Description Test 15",
            ProjectId = project4.ProjectId,
            UserId = user5.UserId,
            IsComplete = true,
            StartDate = new(2023, 1, 30),
            EndDate = new(2023, 2, 28)
        };
        context.Tasks.Add(task);
        context.Tasks.Add(task2);
        context.Tasks.Add(task3);
        context.Tasks.Add(task4);
        context.Tasks.Add(task5);
        context.Tasks.Add(task6);
        context.Tasks.Add(task7);
        context.Tasks.Add(task8);
        context.Tasks.Add(task9);
        context.Tasks.Add(task10);
        context.Tasks.Add(task11);
        context.Tasks.Add(task12);
        context.Tasks.Add(task13);
        context.Tasks.Add(task14);
        context.Tasks.Add(task15);
        context.SaveChanges();

        var prereqTask = new Task
        {
            Name = "Test 122",
            Description = "Description Test 122",
            PrereqTaskId = task.TaskId,
            UserId = user2.UserId,
            ProjectId = project.ProjectId,
            IsComplete = true,
            StartDate= new(2023, 1, 15),
            EndDate= new(2023, 1, 29),
            StatusId = project.Statuses[0].StatusId
        };

        var prereqTask2 = new Task
        {
            Name = "Test 123",
            Description = "Description Test 123",
            PrereqTaskId = task.TaskId,
            UserId = user2.UserId,
            ProjectId = project.ProjectId,
            IsComplete = true,
            StartDate = new(2023, 1, 17),
            EndDate = new(2023, 1, 30),
            StatusId = project.Statuses[1].StatusId
        };

        var prereqTask3 = new Task
        {
            Name = "Test 124",
            Description = "Description Test 124",
            PrereqTaskId = task2.TaskId,
            UserId = user3.UserId,
            ProjectId = project.ProjectId,
            IsComplete = true,
            StartDate = new(2022, 5, 11),
            EndDate = new(2023, 1, 20)
        };

        var prereqTask4 = new Task
        {
            Name = "Test 125",
            Description = "Description Test 125",
            PrereqTaskId = task3.TaskId,
            UserId = user3.UserId,
            ProjectId = project.ProjectId,
            IsComplete = false,
            StartDate = new(2023, 2, 1),
            EndDate = new(2023, 8, 27)
        };

        var prereqTask5 = new Task
        {
            Name = "Test 126",
            Description = "Description Test 126",
            PrereqTaskId = task3.TaskId,
            UserId = user3.UserId,
            ProjectId = project.ProjectId,
            IsComplete = false,
            StartDate = new(2023, 1, 2),
            EndDate = new(2023, 6, 12),
            StatusId = project.Statuses[0].StatusId
        };

        var prereqTask6 = new Task
        {
            Name = "Test 127",
            Description = "Description Test 127",
            PrereqTaskId = task4.TaskId,
            UserId = user.UserId,
            ProjectId = project.ProjectId,
            IsComplete = false,
            StartDate = new(2023, 3, 6),
            EndDate = new(2023, 4, 30)
        };

        var prereqTask7 = new Task
        {
            Name = "Test 128",
            Description = "Description Test 128",
            PrereqTaskId = task5.TaskId,
            UserId = user.UserId,
            ProjectId = project.ProjectId,
            IsComplete = false,
            StartDate = new(2022, 12, 16),
            EndDate = new(2023, 4, 29)
        };

        var prereqTask8 = new Task
        {
            Name = "Test 129",
            Description = "Description Test 129",
            PrereqTaskId = task6.TaskId,
            UserId = user2.UserId,
            ProjectId = project2.ProjectId,
            IsComplete = true,
            StartDate = new(2023, 2, 10),
            EndDate = new(2023, 2, 20),
            StatusId = project2.Statuses[0].StatusId
        };

        context.Tasks.Add(prereqTask);
        context.Tasks.Add(prereqTask2);
        context.Tasks.Add(prereqTask3);
        context.Tasks.Add(prereqTask4);
        context.Tasks.Add(prereqTask5);
        context.Tasks.Add(prereqTask6);
        context.Tasks.Add(prereqTask7);
        context.Tasks.Add(prereqTask8);
        context.SaveChanges();
    }
}