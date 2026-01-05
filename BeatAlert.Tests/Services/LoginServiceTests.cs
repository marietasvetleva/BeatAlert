using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
using BeatAlert.Logic.Services;
using Moq;

namespace BeatAlert.Tests.Services;

public class LoginServiceTests
{
    [Fact]
    public void Login_WithValidEmailAndPassword_ReturnsSuccess()
    {
        // ARRANGE
        var mockRepo = new Mock<IUserRepository>();

        string PlainPassword = "password123";
        string HashedPassword = BCrypt.Net.BCrypt.HashPassword(PlainPassword);

        var FakeUser = new MusicLover
        {
            MusicLoverId = 1,
            Email = "test@example.com",
            PasswordHash = HashedPassword
        };

        mockRepo
            .Setup(r => r.GetByEmail("test@example.com"))
            .Returns(FakeUser);

        var service = new LoginService(mockRepo.Object);

        var request = new LoginRequestDTO
        {
            Identifier = "test@example.com",
            Password = PlainPassword
        };
        
        // ACT
        var result = service.Login(request);

        // ASSERT
        Assert.True(result.Success);
        Assert.Null(result.ErrorMessage);
        Assert.Equal(1, result.UserId);
    }
}