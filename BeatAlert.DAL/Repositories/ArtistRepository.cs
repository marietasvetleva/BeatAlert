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
                                ? "/images/default-avatar.jpg"
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
    public Artist GetArtistById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query =
                "SELECT artist_id, stage_name, image_url, banner_url, biography FROM Artist WHERE artist_id = @Id";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Artist
                        {
                            ArtistId = reader.GetInt32(reader.GetOrdinal("artist_id")),
                            StageName = reader.GetString(reader.GetOrdinal("stage_name")),
                            AvatarUrl = reader.IsDBNull(reader.GetOrdinal("image_url"))
                                ? "/images/default-avatar.jpg"
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
                    }
                }

                return null;
            }
        }
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
                                ? "/images/default-avatar.jpg"
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
