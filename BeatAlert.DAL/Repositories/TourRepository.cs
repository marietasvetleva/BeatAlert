using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
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

    public Tour GetTourById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT tour_id, tour_name, promo_image, description, artist_id FROM Tour WHERE tour_id = @id";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Tour
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

                    return null;
                }
            }
        }
    }

    public List<Tour> SearchToursByName(string queryText)
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
}