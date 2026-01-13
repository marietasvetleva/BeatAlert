using BeatAlert.Logic.Interfaces___Repositories;
using Microsoft.Data.SqlClient;

namespace BeatAlert.DAL.Repositories;

public class FollowRepository : IFollowRepository
{
    private readonly string _connectionString;

    public FollowRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public bool Exists(int music_lover_id, int artist_id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT 1 FROM Follow WHERE music_lover_id = @MusicLoverId AND artist_id = @ArtistId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MusicLoverId", music_lover_id);
                command.Parameters.AddWithValue("@ArtistId", artist_id);
                
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    return reader.Read();   
                }
            }
        }
    }

    public void Add(int music_lover_id, int artist_id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "INSERT INTO Follow (music_lover_id, artist_id) VALUES (@MusicLoverId, @ArtistId)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MusicLoverId", music_lover_id);
                command.Parameters.AddWithValue("@ArtistId", artist_id);
                
                connection.Open();

                command.ExecuteNonQuery();

            }
        }
    }

    public void Remove(int music_lover_id, int artist_id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "DELETE FROM Follow WHERE music_lover_id = @MusicLoverId AND artist_id = @ArtistId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MusicLoverId", music_lover_id);
                command.Parameters.AddWithValue("@ArtistId", artist_id); 
                
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}