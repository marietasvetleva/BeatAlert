using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Models;
using BeatAlert.Logic.Services;
using Microsoft.Data.SqlClient;

namespace BeatAlert.DAL.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly string _connectionString;

        public AlbumRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Album> GetAllAlbums()
        {
            var albums = new List<Album>();

            try
            {
                using var connection = new SqlConnection(_connectionString);

                const string sql = "SELECT album_id, title, release_date, cover_image FROM Album";

                using var command = new SqlCommand(sql, connection);

                connection.Open();

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    albums.Add(new Album
                    {
                        AlbumId = reader.GetInt32(reader.GetOrdinal("album_id")),
                        Title = reader.GetString(reader.GetOrdinal("title")),
                        ReleaseDate = reader.IsDBNull(reader.GetOrdinal("release_date"))
                            ? null
                            : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("release_date"))),
                        CoverImage = reader.IsDBNull(reader.GetOrdinal("cover_image"))
                            ? "/images/placeholders/album.png"
                            : reader.GetString(reader.GetOrdinal("cover_image")),
                        Artists = new List<Artist>(),
                        Songs = new List<Song>()
                    });
                }

                return albums;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database error while retrieving all albums.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessException("Database operation error while retrieving all albums.", ex);
            }
        }

        public Album? GetAlbumById(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                const string sql = @"
                    -- 1) Album
                    SELECT album_id, title, release_date, cover_image
                    FROM Album
                    WHERE album_id = @Id;

                    -- 2) Songs
                    SELECT song_id, title, duration, release_date, lyrics, album_id
                    FROM Song
                    WHERE album_id = @Id
                    ORDER BY song_id;

                    -- 3) Artist(s)
                    SELECT a.artist_id, a.stage_name, a.image_url, a.banner_url, a.biography
                    FROM Artist a
                    JOIN Album_Artist aa ON a.artist_id = aa.artist_id
                    WHERE aa.album_id = @Id;
                ";

                using var command = new SqlCommand(sql, connection);

                // better than AddWithValue (prevents wrong SQL type inference)
                command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

                connection.Open();

                using var reader = command.ExecuteReader();

                Album? album = null;

                // 1) Album
                if (reader.Read())
                {
                    album = new Album
                    {
                        AlbumId = reader.GetInt32(reader.GetOrdinal("album_id")),
                        Title = reader.GetString(reader.GetOrdinal("title")),
                        ReleaseDate = reader.IsDBNull(reader.GetOrdinal("release_date"))
                            ? null
                            : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("release_date"))),
                        CoverImage = reader.IsDBNull(reader.GetOrdinal("cover_image"))
                            ? "/images/placeholders/album.png"
                            : reader.GetString(reader.GetOrdinal("cover_image")),
                        Artists = new List<Artist>(),
                        Songs = new List<Song>()
                    };
                }
                else
                {
                    // normal case: not found
                    return null;
                }

                // 2) Songs
                reader.NextResult();
                while (reader.Read())
                {
                    album.Songs.Add(new Song
                    {
                        SongId = reader.GetInt32(reader.GetOrdinal("song_id")),
                        Title = reader.GetString(reader.GetOrdinal("title")),
                        DurationSeconds = reader.GetInt32(reader.GetOrdinal("duration")),
                        ReleaseDate = reader.IsDBNull(reader.GetOrdinal("release_date"))
                            ? null
                            : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("release_date"))),
                        Lyrics = reader.IsDBNull(reader.GetOrdinal("lyrics"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("lyrics")),
                        AlbumId = reader.GetInt32(reader.GetOrdinal("album_id")),
                        Artists = new List<Artist>()
                    });
                }

                // 3) Artist(s)
                reader.NextResult();
                while (reader.Read()) // use while if album can have multiple artists
                {
                    album.Artists.Add(new Artist
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

                return album;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Database error while retrieving album with id {id}.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessException($"Database operation error while retrieving album with id {id}.", ex);
            }
        }

        public List<Album> SearchAlbum(string queryText)
        {
            // normal behavior: empty query returns empty results (or you can return all, up to you)
            if (string.IsNullOrWhiteSpace(queryText))
            {
                return new List<Album>();
            }

            var albums = new List<Album>();

            try
            {
                using var connection = new SqlConnection(_connectionString);

                const string sql = @"
                    SELECT album_id, title, release_date, cover_image
                    FROM Album
                    WHERE LOWER(title) LIKE '%' + LOWER(@query) + '%';
                ";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@query", System.Data.SqlDbType.NVarChar, 200).Value = queryText.Trim();

                connection.Open();

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    albums.Add(new Album
                    {
                        AlbumId = reader.GetInt32(reader.GetOrdinal("album_id")),
                        Title = reader.GetString(reader.GetOrdinal("title")),
                        ReleaseDate = reader.IsDBNull(reader.GetOrdinal("release_date"))
                            ? null
                            : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("release_date"))),
                        CoverImage = reader.IsDBNull(reader.GetOrdinal("cover_image"))
                            ? "/images/placeholders/album.png"
                            : reader.GetString(reader.GetOrdinal("cover_image")),
                        Artists = new List<Artist>(),
                        Songs = new List<Song>()
                    });
                }

                return albums;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Database error while searching albums.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessException("Database operation error while searching albums.", ex);
            }
        }
    }
}
