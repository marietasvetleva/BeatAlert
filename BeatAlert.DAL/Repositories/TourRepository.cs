using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
using BeatAlert.Logic.Services;
using Microsoft.Data.SqlClient;

namespace BeatAlert.DAL.Repositories;

public class TourRepository : ITourRepository
{
    private readonly string _connectionString;

    public TourRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Tour> GetToursByArtistId(int artistId)
    {
        try
        {
            var tours = new List<Tour>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query =
                    "SELECT tour_id, tour_name, promo_image, description, artist_id FROM Tour WHERE artist_id = @artistId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@artistId", artistId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tour = new Tour
                            {
                                TourId = reader.GetInt32(reader.GetOrdinal("tour_id")),
                                Name = reader.GetString(reader.GetOrdinal("tour_name")),
                                PromoPic = reader.IsDBNull(reader.GetOrdinal("promo_image"))
                                    ? "/images/default-tour.jpg"
                                    : reader.GetString(reader.GetOrdinal("promo_image")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                    ? "No description available."
                                    : reader.GetString(reader.GetOrdinal("description")),
                                ArtistId = reader.GetInt32(reader.GetOrdinal("artist_id")),

                                Shows = new List<Show>(),
                                Artist = null
                            };

                            tours.Add(tour);
                        }
                    }
                }
            }

            return tours;
        }
        catch (SqlException ex)
        {
            throw new DataAccessException($"Database error while retrieving tours for artistId={artistId}.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException($"Database operation error while retrieving tours for artistId={artistId}.", ex);
        }
    }

    public Tour? GetTourById(int id)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    -- 1) Tour
                    SELECT tour_id, tour_name, promo_image, description, artist_id
                    FROM Tour
                    WHERE tour_id = @id;

                    -- 2) Shows
                    SELECT show_id, start_datetime, venue_name, city, country
                    FROM Show
                    WHERE tour_id = @id;
                ";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        Tour? tour = null;

                        if (reader.Read())
                        {
                            tour = new Tour
                            {
                                TourId = reader.GetInt32(reader.GetOrdinal("tour_id")),
                                Name = reader.GetString(reader.GetOrdinal("tour_name")),
                                PromoPic = reader.IsDBNull(reader.GetOrdinal("promo_image"))
                                    ? "/images/default-tour.jpg"
                                    : reader.GetString(reader.GetOrdinal("promo_image")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                    ? "No description available."
                                    : reader.GetString(reader.GetOrdinal("description")),
                                ArtistId = reader.GetInt32(reader.GetOrdinal("artist_id")),
                                Shows = new List<Show>(),
                                Artist = null
                            };
                        }
                        else
                        {
                            return null;
                        }

                        reader.NextResult();

                        while (reader.Read())
                        {
                            tour.Shows.Add(new Show
                            {
                                ShowId = reader.GetInt32(reader.GetOrdinal("show_id")),
                                StartDateTime = reader.GetDateTime(reader.GetOrdinal("start_datetime")),
                                VenueName = reader.GetString(reader.GetOrdinal("venue_name")),
                                City = reader.GetString(reader.GetOrdinal("city")),
                                Country = reader.GetString(reader.GetOrdinal("country")),
                                Tour = null,
                                Venue = null
                            });
                        }

                        return tour;
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            throw new DataAccessException($"Database error while retrieving tour with id={id}.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException($"Database operation error while retrieving tour with id={id}.", ex);
        }
    }

    public List<Tour> SearchToursByName(string queryText)
    {
        try
        {
            var tours = new List<Tour>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                         SELECT tour_id, tour_name, promo_image, description, artist_id 
                         FROM Tour
                         WHERE LOWER(tour_name) LIKE '%' + LOWER(@query) + '%'";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@query", queryText);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tour = new Tour
                            {
                                TourId = reader.GetInt32(reader.GetOrdinal("tour_id")),
                                Name = reader.GetString(reader.GetOrdinal("tour_name")),
                                PromoPic = reader.IsDBNull(reader.GetOrdinal("promo_image"))
                                    ? "/images/default-tour.jpg"
                                    : reader.GetString(reader.GetOrdinal("promo_image")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                    ? "No description available."
                                    : reader.GetString(reader.GetOrdinal("description")),
                                ArtistId = reader.GetInt32(reader.GetOrdinal("artist_id")),
                                Shows = new List<Show>(),
                                Artist = null
                            };

                            tours.Add(tour);
                        }

                        return tours;
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            throw new DataAccessException("Database error while searching tours by name.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataAccessException("Database operation error while searching tours by name.", ex);
        }
    }
}
