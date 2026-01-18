using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
using BeatAlert.Logic.Services;
using Microsoft.Data.SqlClient;

namespace BeatAlert.DAL.Repositories;

public class SongRepository : ISongRepository
{
    private readonly string _connectionString;

    public SongRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Get all songs
    public List<Song> GetAllSongs()
    {
        try
        {
            var songs = new List<Song>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT song_id, title, duration, release_date, lyrics, album_id FROM Song";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var song = new Song()
                            {
                                SongId = reader.GetInt32(reader.GetOrdinal("song_id")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                DurationSeconds = reader.GetInt32(reader.GetOrdinal("duration")),
                                ReleaseDate = reader.IsDBNull(reader.GetOrdinal("release_date"))
                                    ? null
                                    : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("release_date"))),
                                Lyrics = reader.IsDBNull(reader.GetOrdinal("lyrics"))
                                    ? ""
                                    : reader.GetString(reader.GetOrdinal("lyrics")),
                                AlbumId = reader.GetInt32(reader.GetOrdinal("album_id")),
                                Artists = new List<Artist>()
                            };

                            songs.Add(song);
                        }
                    }
                }
            }

            return songs;
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database error while retrieving all songs.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException("Database operation error while retrieving all songs.", ex);
        }
    }

    // Get song by Id
    public Song GetSongById(int id)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT song_id, title, duration, release_date, lyrics, album_id FROM Song WHERE song_id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Song
                            {
                                SongId = reader.GetInt32(reader.GetOrdinal("song_id")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                DurationSeconds = reader.GetInt32(reader.GetOrdinal("duration")),
                                ReleaseDate = reader.IsDBNull(reader.GetOrdinal("release_date"))
                                    ? null
                                    : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("release_date"))),
                                Lyrics = reader.IsDBNull(reader.GetOrdinal("lyrics"))
                                    ? ""
                                    : reader.GetString(reader.GetOrdinal("lyrics")),
                                AlbumId = reader.GetInt32(reader.GetOrdinal("album_id")),
                                Artists = new List<Artist>()
                            };
                        }
                    }
                }
            }

            return null;
        }
        catch (SqlException ex)
        {
            throw new DataAccessException($"Database error while retrieving song with id={id}.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException($"Database operation error while retrieving song with id={id}.", ex);
        }
    }

    // Search song by title
    public List<Song> SearchSong(string queryText)
    {
        try
        {
            var songs = new List<Song>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query =
                    "SELECT song_id, title, duration, release_date, lyrics, album_id FROM Song WHERE LOWER(title) LIKE '%' + LOWER(@query) + '%'";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@query", queryText);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var song = new Song
                            {
                                SongId = reader.GetInt32(reader.GetOrdinal("song_id")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                DurationSeconds = reader.GetInt32(reader.GetOrdinal("duration")),
                                ReleaseDate = reader.IsDBNull(reader.GetOrdinal("release_date"))
                                    ? null
                                    : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("release_date"))),
                                Lyrics = reader.IsDBNull(reader.GetOrdinal("lyrics"))
                                    ? ""
                                    : reader.GetString(reader.GetOrdinal("lyrics")),
                                AlbumId = reader.GetInt32(reader.GetOrdinal("album_id")),
                                Artists = new List<Artist>()
                            };

                            songs.Add(song);
                        }
                    }
                }
            }

            return songs;
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database error while searching songs.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException("Database operation error while searching songs.", ex);
        }
    }
}
