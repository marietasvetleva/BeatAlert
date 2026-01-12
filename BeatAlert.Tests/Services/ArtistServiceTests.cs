using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
using BeatAlert.Logic.Services;
using Moq;

namespace BeatAlert.Tests.Services;

public class ArtistServiceTests
{
    [Fact]
    public void GetById_ValidId_ReturnsMappedArtistDetails()
    {
        // ARRANGE
        var mockRepo = new Mock<IArtistRepository>();

        var fakeArtist = new Artist
        {
            ArtistId = 1,
            StageName = "Lana Del Rey",
            AvatarUrl = "avatar.jpg",
            BannerUrl = "banner.jpg",
            Biography = "Artist bio",
            Albums = new List<Album>
            {
                new Album
                {
                    AlbumId = 10,
                    Title = "Born to Die",
                    ReleaseDate = new DateOnly(2012, 1, 1),
                    Duration = 3600
                }
            },
            Tours = new List<Tour>
            {
                new Tour
                {
                    TourId = 20,
                    Name = "LA to the Moon Tour",
                    Description = "World tour"
                }
            }
        };

        mockRepo
            .Setup(r => r.GetArtistById(1))
            .Returns(fakeArtist);

        var service = new ArtistService(mockRepo.Object);

        // ACT
        var result = service.GetById(1);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal(1, result.ArtistId);
        Assert.Equal("Lana Del Rey", result.StageName);
        Assert.Single(result.Albums);
        Assert.Single(result.Tours);

        var album = result.Albums[0];
        Assert.Equal(10, album.AlbumId);
        Assert.Equal("Born to Die", album.Title);

        var tour = result.Tours[0];
        Assert.Equal(20, tour.TourId);
        Assert.Equal("LA to the Moon Tour", tour.Name);

        mockRepo.Verify(r => r.GetArtistById(1), Times.Once);
    }
}
