using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Courses;
using Domain.Faculties;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.CourseUsers;

public class CourseUsersControllerTests : BaseIntegrationTest, IAsyncLifetime
{
        private readonly Course _mainCourse = CoursesData.MainCourse;
        private readonly Faculty _mainFaculty = FacultiesData.MainFaculty;
        private readonly CourseUser _mainCourseUser;
        private readonly User _mainUser;
        private readonly User _secondUser;


    public CourseUsersControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _secondUser = UsersData.MainUser(_mainFaculty.Id);
        _mainUser = UsersData.MainUser(_mainFaculty.Id);
        _mainCourseUser = CourseUsersData.MainCourseUser(_mainCourse.Id, _mainUser.Id);
    }
    [Fact]
    public async Task ShouldCreateCourseUser()
    {
        // Arrange
        var newRaiting = "10";
        var request = new CourseUserDto(
            Id: null,
            CourseId: _mainCourse.Id.Value,
            UserId: _secondUser.Id.Value,
            Rating: newRaiting,
            InviteToCourse: DateTime.UtcNow,
            FinishCourse: DateTime.UtcNow.AddMonths(1),
            IsApproved: true
            );

        // Act
        var response = await Client.PostAsJsonAsync("courseUsers", request);
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var fromResponse = await response.ToResponseModel<CourseUserDto>();
        var courseUserId = new CourseUserId(fromResponse.Id!.Value);

        var fromDataBase = await Context.CourseUsers.FirstOrDefaultAsync(x => x.Id == courseUserId);
        fromDataBase.Should().NotBeNull();

        fromDataBase!.CourseId.Should().Be(_mainCourse.Id);
        fromDataBase!.UserId.Should().Be(_secondUser.Id);
        fromDataBase!.Rating.Should().Be("10");
        fromDataBase!.IsApproved.Should().Be(true);

    }

    [Fact]
    public async Task ShouldUpdateCourseUser()
    {
        // Arrange
        var newRating = "12";
        var request = new CourseUserDto(
            Id: null,
            CourseId: _mainCourse.Id.Value,
            UserId: _secondUser.Id.Value,
            Rating: newRating,
            InviteToCourse: DateTime.UtcNow,
            FinishCourse: DateTime.UtcNow.AddMonths(1),
            IsApproved: true
        );

        // Act
        var response = await Client.PutAsJsonAsync("courseUsers", request);
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var fromResponse = await response.ToResponseModel<CourseUserDto>();
        var fromDataBase = await Context.CourseUsers
            .FirstOrDefaultAsync(x => x.Id == new CourseUserId(fromResponse.Id!.Value));

        fromDataBase.Should().NotBeNull();
        fromDataBase!.Rating.Should().Be(newRating);
    }
    [Fact]
    public async Task ShouldNotCreateCourseUserBecauseUserIdAndCourseIdAlreadyExists()
    {
        // Arrange
        var newRaiting = "12";
        var initialRequest = new CourseUserDto(
            Id: null,
            CourseId: _mainCourse.Id.Value,
            UserId: _secondUser.Id.Value,
            Rating: newRaiting,
            InviteToCourse: DateTime.UtcNow,
            FinishCourse: DateTime.UtcNow.AddMonths(1),
            IsApproved: true
        );
        
        var initialResponse = await Client.PostAsJsonAsync("courseUsers", initialRequest);
        initialResponse.IsSuccessStatusCode.Should().BeTrue();

      
        var duplicateRequest = new CourseUserDto(
            Id: null,
            CourseId: _mainCourse.Id.Value,  
            UserId: _secondUser.Id.Value,    
            Rating: newRaiting,
            InviteToCourse: DateTime.UtcNow,
            FinishCourse: DateTime.UtcNow.AddMonths(1),
            IsApproved: true
        );

        // Act
        var response = await Client.PostAsJsonAsync("courseUsers", duplicateRequest);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();  
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);  
    }

    public async Task InitializeAsync()
    {
        await Context.Courses.AddAsync(_mainCourse);
        await Context.Faculties.AddAsync(_mainFaculty);
        await Context.Users.AddAsync(_secondUser);
        await Context.Users.AddAsync(_mainUser);
        await Context.CourseUsers.AddAsync(_mainCourseUser);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Courses.RemoveRange(Context.Courses);
        Context.Users.RemoveRange(Context.Users);
        Context.Faculties.RemoveRange(Context.Faculties);
        Context.CourseUsers.RemoveRange(Context.CourseUsers);

        await SaveChangesAsync();
    }
}