using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
using Microsoft.Data.SqlClient;

namespace BeatAlert.DAL.Repositories;

public class ShowRepository : IShowRepository
{
    private readonly string _connectionString;

    public ShowRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Search shows by tour name 
    public List<Show> SearchShowsByName(string name)
    {
        var shows = new List<Show>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"
                SELECT s.show_id, s.start_datetime, s.ticket_url, s.tour_id, s.venue_id
                FROM Show s
                INNER JOIN Tour t ON s.tour_id = t.tour_id
                WHERE LOWER(t.tour_name) LIKE '%' + LOWER(@name) + '%';";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var show = new Show
                        {
                            ShowId = reader.GetInt32(reader.GetOrdinal("show_id")),
                            StartDateTime = reader.GetDateTime(reader.GetOrdinal("start_datetime")),
                            TicketUrl = reader.GetString(reader.GetOrdinal("ticket_url")),
                            TourId = reader.GetInt32(reader.GetOrdinal("tour_id")),
                            VenueId = reader.GetInt32(reader.GetOrdinal("venue_id")),

                            
                            Venue = null,
                            Tour = null
                        };

                        shows.Add(show);
                    }
                }
            }
        }

        return shows;
    }

    public List<Show> GetShowsByTourId(int tourId)
    {
        var shows = new List<Show>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"
                SELECT show_id, start_datetime, ticket_url, tour_id, venue_id
                FROM Show
                WHERE tour_id = @tourId;";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tourId", tourId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var show = new Show
                        {
                            ShowId = reader.GetInt32(reader.GetOrdinal("show_id")),
                            StartDateTime = reader.GetDateTime(reader.GetOrdinal("start_datetime")),
                            TicketUrl = reader.GetString(reader.GetOrdinal("ticket_url")),
                            TourId = reader.GetInt32(reader.GetOrdinal("tour_id")),
                            VenueId = reader.GetInt32(reader.GetOrdinal("venue_id")),

                            Venue = null,
                            Tour = null
                        };

                        shows.Add(show);
                    }
                }
            }
        }

        return shows;
    }

    public Show? GetShowById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"
                SELECT show_id, start_datetime, ticket_url, tour_id, venue_id
                FROM Show
                WHERE show_id = @id;";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Show
                        {
                            ShowId = reader.GetInt32(reader.GetOrdinal("show_id")),
                            StartDateTime = reader.GetDateTime(reader.GetOrdinal("start_datetime")),
                            TicketUrl = reader.GetString(reader.GetOrdinal("ticket_url")),
                            TourId = reader.GetInt32(reader.GetOrdinal("tour_id")),
                            VenueId = reader.GetInt32(reader.GetOrdinal("venue_id")),

                            Venue = null,
                            Tour = null
                        };
                    }
                }
            }
        }

        return null;
    }
}
