using Microsoft.EntityFrameworkCore;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests.IntegrationTests;

public class UserRepositoryTests : DALTestsBase
{
    public UserRepositoryTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetAll_Users()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<UserEntity>();

        // Act
        var result = repository.Get();

        // Assert
        Assert.True(result.Contains(UserSeeds.DefaultUser));
        Assert.True(result.Contains(UserSeeds.UserToDelete));
    }

    [Fact]
    public async Task Exists_User_True()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<UserEntity>();

        // Act
        var result = await repository.ExistsAsync(UserSeeds.DefaultUser.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Exists_User_False()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<UserEntity>();
        var user = UserSeeds.DefaultUser with { Id = Guid.NewGuid() };


        // Act
        var result = await repository.ExistsAsync(user.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task InsertNew_User()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<UserEntity>();
        var newUser = UserSeeds.DefaultUser with { Id = Guid.NewGuid() };

        // Act
        await repository.InsertAsync(newUser);
        await UnitOfWork.CommitAsync();

        // Assert
        var retrieved = await DbContextInstance.Users.FindAsync(newUser.Id);
        Assert.Equal(newUser, retrieved);
    }

    [Fact]
    public async Task Update_User()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<UserEntity>();
        var userToUpdate = UserSeeds.UserToUpdate with { Name = "Ferda2" };

        // Act
        await repository.UpdateAsync(userToUpdate);
        await UnitOfWork.CommitAsync();

        // Assert
        var contains = await DbContextInstance.Users.ContainsAsync(userToUpdate);
        Assert.True(contains);
    }

    [Fact]
    public async Task Delete_User()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<UserEntity>();

        // Act
        await repository.DeleteAsync(UserSeeds.UserToDelete.Id);
        await UnitOfWork.CommitAsync();

        // Assert
        var retrieved = await DbContextInstance.Users.FindAsync(UserSeeds.UserToDelete.Id);
        Assert.Null(retrieved);
    }

    [Fact]
    public async Task Delete_User_Scores_Cascade()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<UserEntity>();

        // Act
        await repository.DeleteAsync(UserSeeds.UserToDelete.Id);
        await UnitOfWork.CommitAsync();

        // Assert
        var retrieved = await DbContextInstance.Scores.FindAsync(ScoreSeeds.ScoreWithUserToDelete.Id);
        Assert.Null(retrieved);
    }
}