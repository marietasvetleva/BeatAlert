using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Classes;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

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

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "SELECT album_id, title, release_date, cover_image FROM Album",
                    connection
                );

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Album album = new Album
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            ReleaseDate = reader.GetDateTime(2),
                            CoverImage = reader.IsDBNull(3) ? "/images/default-cover.jpg" : reader.GetString(3)
                        };
                    }
                }
            }

            return albums;
        }

        public Album GetAlbumById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "SELECT album_id, title, release_date, cover_image FROM Album WHERE Id = @Id",
                    connection
                );
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Album
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            ReleaseDate = reader.GetDateTime(2),
                            CoverImage = reader.IsDBNull(3) ? "/images/default-cover.jpg" : reader.GetString(3)
                        };
                    }
                }
            }

            return null;
        }
    }
}
