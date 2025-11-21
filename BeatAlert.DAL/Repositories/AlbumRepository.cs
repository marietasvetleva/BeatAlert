using System.Data.SqlClient;
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
                                ReleaseDate = reader.GetDateTime(reader.GetOrdinal("release_date")),
                                CoverImage = reader.IsDBNull(reader.GetOrdinal("cover_image"))
                                    ? "/images/default-cover.jpg"
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
                var query = "SELECT album_id, title, release_date, cover_image FROM Album WHERE album_id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Album
                            {
                                AlbumId = reader.GetInt32(reader.GetOrdinal("album_id")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                ReleaseDate = reader.GetDateTime(reader.GetOrdinal("release_date")),
                                CoverImage = reader.IsDBNull(reader.GetOrdinal("cover_image"))
                                    ? "/images/default-cover.jpg"
                                    : reader.GetString(reader.GetOrdinal("cover_image")),
                                
                                Artists = new List<Artist>(),
                                Songs = new List<Song>()
                            };
                        }
                    }
                }
            }

            return null;
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
                                ReleaseDate = reader.GetDateTime(reader.GetOrdinal("release_date")),
                                CoverImage = reader.IsDBNull(reader.GetOrdinal("cover_image"))
                                    ? "/images/default-cover.jpg"
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
