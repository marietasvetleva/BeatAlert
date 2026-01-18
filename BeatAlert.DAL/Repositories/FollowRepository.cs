using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using BeatAlert.Logic.Services;

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
        try
        {
            using var connection = new SqlConnection(_connectionString);

            const string sql =
                "SELECT 1 FROM Follow WHERE music_lover_id = @MusicLoverId AND artist_id = @ArtistId;";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@MusicLoverId", SqlDbType.Int).Value = music_lover_id;
            command.Parameters.Add("@ArtistId", SqlDbType.Int).Value = artist_id;

            connection.Open();

            // faster than ExecuteReader for exists checks
            var result = command.ExecuteScalar();
            return result != null;
        }
        catch (SqlException ex)
        {
            throw new DataAccessException(
                $"Database error while checking follow existence (user {music_lover_id}, artist {artist_id}).", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException(
                $"Database operation error while checking follow existence (user {music_lover_id}, artist {artist_id}).",
                ex);
        }
    }

    public void Add(int music_lover_id, int artist_id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            // If you have a UNIQUE constraint on (music_lover_id, artist_id), this prevents an exception
            // even if someone clicks Follow twice.
            const string sql = @"
                IF NOT EXISTS (
                    SELECT 1 FROM Follow
                    WHERE music_lover_id = @MusicLoverId AND artist_id = @ArtistId
                )
                BEGIN
                    INSERT INTO Follow (music_lover_id, artist_id)
                    VALUES (@MusicLoverId, @ArtistId);
                END
            ";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@MusicLoverId", SqlDbType.Int).Value = music_lover_id;
            command.Parameters.Add("@ArtistId", SqlDbType.Int).Value = artist_id;

            connection.Open();
            command.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            // could be FK violation (user/artist doesn't exist), connection issues, etc.
            throw new DataAccessException(
                $"Database error while adding follow (user {music_lover_id}, artist {artist_id}).", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException(
                $"Database operation error while adding follow (user {music_lover_id}, artist {artist_id}).", ex);
        }
    }

    public void Remove(int music_lover_id, int artist_id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            const string sql =
                "DELETE FROM Follow WHERE music_lover_id = @MusicLoverId AND artist_id = @ArtistId;";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@MusicLoverId", SqlDbType.Int).Value = music_lover_id;
            command.Parameters.Add("@ArtistId", SqlDbType.Int).Value = artist_id;

            connection.Open();
            command.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            throw new DataAccessException(
                $"Database error while removing follow (user {music_lover_id}, artist {artist_id}).", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException(
                $"Database operation error while removing follow (user {music_lover_id}, artist {artist_id}).", ex);
        }
    }

}