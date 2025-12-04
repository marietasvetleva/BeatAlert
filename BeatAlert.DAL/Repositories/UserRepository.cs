using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
using Microsoft.Data.SqlClient;

namespace BeatAlert.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int CreateUser(MusicLover musicLover)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"
                INSERT INTO MusicLover (first_name, last_name, email, password_hash, username, home_city, avatar_url)
                OUTPUT INSERTED.music_lover_id
                VALUES (@FirstName, @LastName, @Email, @PasswordHash, @Username, @HomeCity, @AvatarUrl);
            ";


            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@FirstName", musicLover.FirstName);
                command.Parameters.AddWithValue("@LastName", musicLover.LastName);
                command.Parameters.AddWithValue("@Email", musicLover.Email);
                command.Parameters.AddWithValue("@Username", musicLover.Username);
                command.Parameters.AddWithValue("@HomeCity", (object?)musicLover.HomeCity ?? DBNull.Value);
                command.Parameters.AddWithValue("@AvatarUrl", (object?)musicLover.AvatarUrl ?? DBNull.Value);
                command.Parameters.AddWithValue("@PasswordHash", musicLover.PasswordHash);

                var newId = command.ExecuteScalar(); 
                return (int)newId!; 

            }
        }
    }

    public MusicLover GetByEmail(string email)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query =
                "SELECT music_lover_id, first_name, last_name, email, password_hash, username, home_city, avatar_urlFROM MusicLover WHERE email = @Email";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MusicLover
                        {
                            MusicLoverId = reader.GetInt32(reader.GetOrdinal("music_lover_id")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            Username = reader.GetString(reader.GetOrdinal("username")),
                            HomeCity = reader.IsDBNull(reader.GetOrdinal("home_city"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("home_city")),
                            AvatarUrl = reader.IsDBNull(reader.GetOrdinal("avatar_url"))
                                ? "/images/default-avatar.jpg"
                                : reader.GetString(reader.GetOrdinal("avatar_url")),
                           PasswordHash = reader.GetString(reader.GetOrdinal("password_hash"))
                        };
                    }
                }

            }
        }

        return null;
    }

    public MusicLover GetByUsername(string username)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query =
                "SELECT music_lover_id, first_name, last_name, email, password_hash, username, home_city, avatar_url FROM MusicLover WHERE username = @Username";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MusicLover
                        {
                            MusicLoverId = reader.GetInt32(reader.GetOrdinal("music_lover_id")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            Username = reader.GetString(reader.GetOrdinal("username")),
                            HomeCity = reader.IsDBNull(reader.GetOrdinal("home_city"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("home_city")),
                            AvatarUrl = reader.IsDBNull(reader.GetOrdinal("avatar_url"))
                                ? "/images/default-avatar.jpg"
                                : reader.GetString(reader.GetOrdinal("avatar_url")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("password_hash"))
                        };
                    }
                }

            }
        }

        return null;
    }
}
