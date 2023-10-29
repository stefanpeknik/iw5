using System.Net;
using System.Net.Http.Json;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Common.Models.User;
using Xunit;

namespace TaHooK.Api.App.EndToEndTests.EndToEndTests;

public class UserControllerTests : EndToEndTestsBase
{

    [Fact]
    public async Task GetAllUsers_Returns_At_Least_One_User()
    {
        // Arrange
        var userSeed = UserSeeds.DefaultUser;
        var userSeedModel = mapper.Map<UserListModel>(userSeed);
        
        // Act
        var response = await client.Value.GetAsync("/api/users");
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<ICollection<UserListModel>>();
        
        // Assert
        Assert.NotNull(users);
        Assert.NotEmpty(users);
        Assert.Contains(users, user => user.Id == userSeedModel.Id);
    }
    
    [Fact]
    public async Task GetUserById_Returns_User_With_The_Same_Id()
    {
        // Arrange
        var userSeed = UserSeeds.DefaultUser;
        var userSeedModel = mapper.Map<UserDetailModel>(userSeed);
        
        // Act
        var response = await client.Value.GetAsync($"/api/users/{userSeedModel.Id}");
        var user = await response.Content.ReadFromJsonAsync<UserDetailModel>();
        
        // Assert
        Assert.NotNull(user);
        Assert.Equal(userSeedModel.Id, user.Id);
    }

    [Fact]
    public async Task GetUserById_Returns_NotFound_When_User_Does_Not_Exist()
    {
        // Act
        var response = await client.Value.GetAsync($"/api/users/{Guid.NewGuid()}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateUser_Returns_Created_User_Id()
    {
        // Arrange
        var userSeed = UserSeeds.DefaultUser;
        var userSeedModel = mapper.Map<UserDetailModel>(userSeed);
        
        // Act
        var post = await client.Value.PostAsJsonAsync("/api/users", userSeedModel);
        var postId = await post.Content.ReadFromJsonAsync<Guid>();
        var get = await client.Value.GetAsync($"/api/users/{postId}");
        var getId = (await get.Content.ReadFromJsonAsync<UserDetailModel>())!.Id;
        
        // Assert
        Assert.Equal(HttpStatusCode.Accepted, post.StatusCode);
        Assert.Equal(postId, getId);
    }
    
    [Fact]
    public async Task CreateGarbage_Returns_BadRequest()
    {
        // Arrange
        var garbage = new { Garbage = "Garbage" };
        
        // Act
        var post = await client.Value.PostAsJsonAsync("/api/users", garbage);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);
    }
    
    [Fact]
    public async Task UpdateUserById_Returns_Updated_User_Id()
    {
        // Arrange
        var userSeed = UserSeeds.UserToUpdate;
        var userSeedModel = mapper.Map<UserDetailModel>(userSeed);
        var userSeedModelUpdated = mapper.Map<UserDetailModel>(userSeed);
        userSeedModelUpdated.Name = "Updated text";
        
        // Act
        var put = await client.Value.PutAsJsonAsync($"/api/users/{userSeedModel.Id}", userSeedModelUpdated);
        var putId = await put.Content.ReadFromJsonAsync<Guid>();
        var get = await client.Value.GetAsync($"/api/users/{putId}");
        var getId = (await get.Content.ReadFromJsonAsync<UserDetailModel>())!.Id;
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, put.StatusCode);
        Assert.Equal(putId, getId);
    }

    [Fact]
    public async Task UpdateUserById_Returns_NotFound_When_User_Does_Not_Exist()
    {
        // Arrange
        var userSeed = UserSeeds.UserToUpdate;
        var userSeedModelUpdated = mapper.Map<UserDetailModel>(userSeed);
        userSeedModelUpdated.Name = "Updated text";
        var nonExistentId = Guid.NewGuid();
        
        // Act
        var put = await client.Value.PutAsJsonAsync($"/api/users/{nonExistentId}", userSeedModelUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, put.StatusCode);
    }
    
    [Fact]
    public async Task UpdateUserByIdWithGarbage_Returns_BadRequest()
    {
        // Arrange
        var userSeed = UserSeeds.UserToUpdate;
        var userSeedModelUpdated = mapper.Map<UserDetailModel>(userSeed);
        var garbage = new { Garbage = "Garbage" };
        
        // Act
        var put = await client.Value.PutAsJsonAsync($"/api/users/{userSeedModelUpdated.Id}", garbage);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);
    }
    
    [Fact]
    public async Task DeleteUserById_Returns_Ok()
    {
        // Arrange
        var userSeed = UserSeeds.UserToDelete;
        var userSeedModel = mapper.Map<UserDetailModel>(userSeed);
        
        // Act
        var delete = await client.Value.DeleteAsync($"/api/users/{userSeedModel.Id}");
        var get = await client.Value.GetAsync($"/api/users/{userSeedModel.Id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, delete.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }
    
    [Fact]
    public async Task DeleteUserById_Returns_NotFound_When_User_Does_Not_Exist()
    {
        // Arrange
        var nonexistentId = Guid.NewGuid();
        
        // Act
        var delete = await client.Value.DeleteAsync($"/api/users/{nonexistentId}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);
    }
}
