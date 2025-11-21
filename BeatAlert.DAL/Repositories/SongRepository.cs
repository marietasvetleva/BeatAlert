using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
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

    // Get song by Id
    public Song GetSongById(int id)
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

    // Search song by title
    public List<Song> SearchSong(string queryText)
    {
        var songs = new List<Song>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT song_id, title, duration, release_date, lyrics, album_id FROM Song WHERE LOWER(title) LIKE '%' + LOWER(@query) + '%'";

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
}
