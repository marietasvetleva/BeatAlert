using System.Data.SqlClient;
using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
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

        // Get all albums
        public List<Album> GetAllAlbums()
        {
            var albums = new List<Album>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT album_id, title, release_date, cover_image FROM Album";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var album = new Album
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

                            albums.Add(album);
                        }
                    }
                }
            }

            return albums;
        }

        // Get album by ID
        public Album? GetAlbumById(int id)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        var query = @"
            -- 1) Album
            SELECT album_id, title, release_date, cover_image
            FROM Album
            WHERE album_id = @Id;

            -- 2) Songs
            SELECT song_id, title, duration, release_date, lyrics, album_id
            FROM Song
            WHERE album_id = @Id
            ORDER BY song_id;

            -- 2) Artist
            SELECT a.artist_id, a.stage_name, a.image_url, a.banner_url, a.biography
            FROM Artist a
            JOIN Album_Artist aa ON a.artist_id = aa.artist_id
            WHERE aa.album_id = @Id;
        ";

        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                Album? album = null;

                // 1) Read album
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
                    return null;
                }

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

                reader.NextResult();

                if (reader.Read())
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
        }
    }
}


        // Search albums by title 
        public List<Album> SearchAlbum(string queryText)
        {
            var albums = new List<Album>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT album_id, title, release_date, cover_image 
                    FROM Album 
                    WHERE LOWER(title) LIKE '%' + LOWER(@query) + '%'
                ";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@query", queryText);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var album = new Album
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

                            albums.Add(album);
                        }
                    }
                }
            }

            return albums;
        }
    }
}
