using BeatAlert.Logic.Services;

namespace BeatAlert.Tests.Services;

public class AlbumServiceTest
{
    [Fact]
    public void Test1()
    {
        // Arrange 
        var repo = new FakeAlbumRepository();
        var service = new AlbumService(repo); 
        var releaseDate = DateTime.Now.AddDays(-15);
        
        // Act
        var result = service.IsNewRelease(releaseDate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Test2()
    {
        // Arrange 
        var repo = new FakeAlbumRepository();
        var service = new AlbumService(repo); 
        var releaseDate = DateTime.Now.AddDays(-50);
        
        //Act 
        var result = service.IsNewRelease(releaseDate);
        
        // Assert
        Assert.False(result);
    }
    
}