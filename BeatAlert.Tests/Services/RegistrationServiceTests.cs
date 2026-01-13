using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
using BeatAlert.Logic.Services;
using Moq;

namespace BeatAlert.Tests.Services;

public class RegistrationServiceTests
{
    [Fact]
    public void Register_ValidInput_ReturnsSuccess()
    {
        // ARRANGE
        var mockRepo = new Mock<IUserRepository>();
        
        mockRepo.Setup(r => r.GetByEmail("test@example.com"))
            .Returns((MusicLover)null);
        
        mockRepo.Setup(r => r.GetByUsername("ana"))
            .Returns((MusicLover)null);
        
        mockRepo.Setup(r => r.CreateUser(It.IsAny<MusicLover>()))
            .Returns(1);

        var service = new RegistrationService(mockRepo.Object);

        var request = new MusicLoverDTO
        {
            FirstName = "Ana",
            LastName = "Ivanova",
            Email = "test@example.com",
            Username = "ana",
            Password = "password123",
            HomeCity = "Eindhoven"
        };

        // ACT
        var result = service.Register(request);

        // ASSERT
        Assert.True(result.Success);
        Assert.Null(result.ErrorMessage);
        Assert.Equal(1, result.NewUserId);
        
    }
}
