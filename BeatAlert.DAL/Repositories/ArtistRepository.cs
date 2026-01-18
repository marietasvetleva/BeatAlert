using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
using BeatAlert.Logic.Services;
using Microsoft.Data.SqlClient;

namespace BeatAlert.DAL.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly string _connectionString;

    public ArtistRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Artist> GetAllArtists()
    {
        var artists = new List<Artist>();

        try
        {
            using var connection = new SqlConnection(_connectionString);

            const string sql = "SELECT artist_id, stage_name, image_url, banner_url, biography FROM Artist";

            using var command = new SqlCommand(sql, connection);

            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                artists.Add(new Artist
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
                });
            }

            return artists;
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database error while retrieving all artists.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException("Database operation error while retrieving all artists.", ex);
        }
    }

    public Artist? GetArtistById(int id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            const string sql = @"
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
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

            connection.Open();

            using var reader = command.ExecuteReader();

            Artist? artist = null;

            // 1) Artist
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
                    Songs = new List<Song>(),
                    Tours = new List<Tour>()
                };
            }
            else
            {
                // normal case: not found
                return null;
            }

            // 2) Albums
            reader.NextResult();
            while (reader.Read())
            {
                artist.Albums.Add(new Album
                {
                    AlbumId = reader.GetInt32(reader.GetOrdinal("album_id")),
                    Title = reader.GetString(reader.GetOrdinal("title")),
                    ReleaseDate = reader.IsDBNull(reader.GetOrdinal("release_date"))
                        ? null
                        : DateOnly.FromDateTime(
                            reader.GetFieldValue<DateTime>(reader.GetOrdinal("release_date"))
                        ),
                    CoverImage = reader.IsDBNull(reader.GetOrdinal("cover_image"))
                        ? "/images/placeholders/album.png"
                        : reader.GetString(reader.GetOrdinal("cover_image")),
                    Artists = new List<Artist>(),
                    Songs = new List<Song>()
                });
            }

            // 3) Tours
            reader.NextResult();
            while (reader.Read())
            {
                artist.Tours.Add(new Tour
                {
                    TourId = reader.GetInt32(reader.GetOrdinal("tour_id")),
                    Name = reader.GetString(reader.GetOrdinal("tour_name")),
                    Description = reader.IsDBNull(reader.GetOrdinal("description"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("description")),
                    PromoPic = reader.IsDBNull(reader.GetOrdinal("promo_image"))
                        ? "/images/placeholders/tour.png"
                        : reader.GetString(reader.GetOrdinal("promo_image"))
                });
            }

            return artist;
        }
        catch (SqlException ex)
        {
            throw new DataAccessException($"Database error while retrieving artist with id {id}.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException($"Database operation error while retrieving artist with id {id}.", ex);
        }
    }

    public List<Artist> SearchArtist(string queryText)
    {
        if (string.IsNullOrWhiteSpace(queryText))
        {
            return new List<Artist>();
        }

        var artists = new List<Artist>();

        try
        {
            using var connection = new SqlConnection(_connectionString);

            const string sql = @"
                SELECT artist_id, stage_name, image_url, banner_url, biography
                FROM Artist
                WHERE LOWER(stage_name) LIKE '%' + LOWER(@query) + '%';
            ";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@query", System.Data.SqlDbType.NVarChar, 200).Value = queryText.Trim();

            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                artists.Add(new Artist
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
                });
            }

            return artists;
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database error while searching artists.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException("Database operation error while searching artists.", ex);
        }
    }
}
