using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Faculties;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Users;

public class UsersControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Faculty _mainFaculty = FacultiesData.MainFaculty;
    private readonly User _mainUser;

    public UsersControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainUser = UsersData.MainUser(_mainFaculty.Id);
    }

   [Fact]
    public async Task ShouldCreateUser()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var request = new UserDto(
            Id: null,
            FirstName: firstName,
            LastName: lastName,
            FullName: null,
            UpdatedAt: null,
            FacultyId: _mainFaculty.Id.Value,
            Faculty: null);

        // Act
        var response = await Client.PostAsJsonAsync("users", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var responseUser = await response.ToResponseModel<UserDto>();
        var userId = new UserId(responseUser.Id!.Value);

        var dbUser = await Context.Users.FirstAsync(x => x.Id == userId);
        dbUser.FirstName.Should().Be(firstName);
        dbUser.LastName.Should().Be(lastName);
        dbUser.FullName.Should().NotBeEmpty();
        dbUser.FacultyId.Value.Should().Be(_mainFaculty.Id.Value);
    }
    
    [Fact]
    public async Task ShouldNotCreateUserBecauseNameDuplicated()
    {
        // Arrange
        var request = new UserDto(
            Id: null,
            FirstName: _mainUser.FirstName,
            LastName: _mainUser.LastName,
            FullName: _mainUser.FullName,
            UpdatedAt: null,
            FacultyId: _mainFaculty.Id.Value,
            Faculty: null);

        // Act
        var response = await Client.PostAsJsonAsync("users", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
    [Fact]
    public async Task ShouldUpdateUser()
    {
        // Arrange

        var updatedFirstName = "Jane";
        var request = new UserDto(
            Id: _mainUser.Id.Value,
            FirstName: updatedFirstName,
            LastName: _mainUser.LastName,
            FullName: null,
            UpdatedAt: null,
            FacultyId: _mainFaculty.Id.Value,
            Faculty: null);

        // Act
        var response = await Client.PutAsJsonAsync("users", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var dbUser = await Context.Users.FirstAsync(x => x.Id == _mainUser.Id);
        dbUser.FirstName.Should().Be(updatedFirstName);
    }
    
    public async Task InitializeAsync()
    {
        await Context.Faculties.AddAsync(_mainFaculty);
        await Context.Users.AddAsync(_mainUser);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Faculties.RemoveRange(Context.Faculties);
        Context.Users.RemoveRange(Context.Users);

        await SaveChangesAsync();
    }
}