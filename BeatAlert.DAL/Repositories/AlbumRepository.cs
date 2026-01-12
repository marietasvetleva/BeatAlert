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
                    return null; // album not found
                }

                // 2) Move to songs result set
                reader.NextResult();

                // Read ALL songs
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
