using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
using Microsoft.Data.SqlClient;

namespace BeatAlert.DAL.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly string _connectionString;

    public ArtistRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Get all artists
    public List<Artist> GetAllArtists()
    {
        var artists = new List<Artist>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT artist_id, stage_name, image_url, banner_url, biography FROM Artist";

            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var artist = new Artist
                        {
                            ArtistId = reader.GetInt32(reader.GetOrdinal("artist_id")),
                            StageName = reader.GetString(reader.GetOrdinal("stage_name")),
                            AvatarUrl = reader.IsDBNull(reader.GetOrdinal("image_url"))
                                ? "/images/placeholders/artist.png"
                                : reader.GetString(reader.GetOrdinal("image_url")),
                            BannerUrl = reader.IsDBNull(reader.GetOrdinal("banner_url"))
                                ? "/images/default-banner.jpg"
                                : reader.GetString(reader.GetOrdinal("banner_url")),
                            Biography = reader.IsDBNull(reader.GetOrdinal("biography"))
                                ? "No biography available."
                                : reader.GetString(reader.GetOrdinal("biography")),
                                
                            Albums = new List<Album>(),
                            Songs = new List<Song>(),
                            Tours = new List<Tour>()
                        };

                        artists.Add(artist);
                    }
                }
            }
        }

        return artists;
    }

    // Get artist by id
    public Artist? GetArtistById(int id)
{
    using var connection = new SqlConnection(_connectionString);

    var sql = @"
        -- 1) Artist
        SELECT artist_id, stage_name, image_url, banner_url, biography
        FROM Artist
        WHERE artist_id = @Id;

        -- 2) Albums via junction
        SELECT a.album_id, a.title, a.release_date, a.cover_image
        FROM Album a
        JOIN Album_Artist aa ON a.album_id = aa.album_id
        WHERE aa.artist_id = @Id;

        -- 3) Tours (direct FK)
        SELECT tour_id, tour_name, promo_image, description
        FROM Tour
        WHERE artist_id = @Id;
    ";

    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddWithValue("@Id", id);

    connection.Open();

    using var reader = command.ExecuteReader();

    Artist? artist = null;

    if (reader.Read())
    {
        artist = new Artist
        {
            ArtistId = reader.GetInt32(reader.GetOrdinal("artist_id")),
            StageName = reader.GetString(reader.GetOrdinal("stage_name")),
            AvatarUrl = reader.IsDBNull(reader.GetOrdinal("image_url"))
                ? "/images/placeholders/artist.png"
                : reader.GetString(reader.GetOrdinal("image_url")),
            BannerUrl = reader.IsDBNull(reader.GetOrdinal("banner_url"))
                ? "/images/default-banner.jpg"
                : reader.GetString(reader.GetOrdinal("banner_url")),
            Biography = reader.IsDBNull(reader.GetOrdinal("biography"))
                ? "No biography available."
                : reader.GetString(reader.GetOrdinal("biography")),

            Albums = new List<Album>(),
            Songs  = new List<Song>(),
            Tours  = new List<Tour>()
        };
    }
    else
    {
        return null;
    }

    reader.NextResult();

    while (reader.Read())
    {
        artist.Albums.Add(new Album
        {
            AlbumId = reader.GetInt32(reader.GetOrdinal("album_id")),
            Title = reader.GetString(reader.GetOrdinal("title")),
            ReleaseDate = reader.GetDateTime(reader.GetOrdinal("release_date")),
            CoverImage = reader.IsDBNull(reader.GetOrdinal("cover_image"))
                ? "/images/placeholders/album.png"
                : reader.GetString(reader.GetOrdinal("cover_image"))
        });
    }

    reader.NextResult();

    while (reader.Read())
    {
        artist.Tours.Add(new Tour
        {
            TourId = reader.GetInt32(reader.GetOrdinal("tour_id")),
            Name = reader.GetString(reader.GetOrdinal("tour_name")),
            Description = reader.GetString(reader.GetOrdinal("description")),
            PromoPic = reader.IsDBNull(reader.GetOrdinal("promo_image"))
                ? "/images/placeholders/tour.png"
                : reader.GetString(reader.GetOrdinal("promo_image"))
        });
    }

    return artist;
}


    public List<Artist> SearchArtist(string queryText)
    {
        var artists = new List<Artist>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var query =
                "SELECT artist_id, stage_name, image_url, banner_url, biography FROM Artist WHERE LOWER(stage_name) LIKE '%' + LOWER(@query) + '%'";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@query", queryText);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var artist = new Artist
                        {
                            ArtistId = reader.GetInt32(reader.GetOrdinal("artist_id")),
                            StageName = reader.GetString(reader.GetOrdinal("stage_name")),
                            AvatarUrl = reader.IsDBNull(reader.GetOrdinal("image_url"))
                                ? "/images/placeholders/artist.png"
                                : reader.GetString(reader.GetOrdinal("image_url")),
                            BannerUrl = reader.IsDBNull(reader.GetOrdinal("banner_url"))
                                ? "/images/default-banner.jpg"
                                : reader.GetString(reader.GetOrdinal("banner_url")),
                            Biography = reader.IsDBNull(reader.GetOrdinal("biography"))
                                ? "No biography available."
                                : reader.GetString(reader.GetOrdinal("biography")),
                                
                            Albums = new List<Album>(),
                            Songs = new List<Song>(),
                            Tours = new List<Tour>()
                        };

                        artists.Add(artist);
                    }
                }
            }
        }
        return artists;
    }
}
