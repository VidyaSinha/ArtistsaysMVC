using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication4.Models;

namespace WebApplication4.Repositories
{
    public class SeatRepository : IDisposable
    {
        private readonly string connectionString = "Data Source=LAPTOP-SE92TMUF\\SQLEXPRESS;Initial Catalog=Demo;Integrated Security=True;";

        // Method to get all available seats
        public List<Seat> GetAllSeats()
        {
            List<Seat> seats = new List<Seat>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Seats";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Seat seat = new Seat
                            {
                                SeatId = reader.GetInt32(0),
                                SeatNumber = reader.GetString(1),
                                IsBooked = reader.GetBoolean(2),
                                LastBookedTime = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3)
                            };
                            seats.Add(seat);
                        }
                    }
                }
            }

            return seats;
        }

        // Method to update seat booking
        public void UpdateSeat(Seat seat)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Seats SET IsBooked = @IsBooked, LastBookedTime = @LastBookedTime WHERE SeatId = @SeatId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsBooked", seat.IsBooked);
                    command.Parameters.AddWithValue("@LastBookedTime", seat.LastBookedTime ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SeatId", seat.SeatId);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to get a seat by ID
        public Seat GetSeatById(int seatId)
        {
            Seat seat = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Seats WHERE SeatId = @SeatId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SeatId", seatId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            seat = new Seat
                            {
                                SeatId = reader.GetInt32(0),
                                SeatNumber = reader.GetString(1),
                                IsBooked = reader.GetBoolean(2),
                                LastBookedTime = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3)
                            };
                        }
                    }
                }
            }

            return seat;
        }

        // Method to add a booking
        public void AddBooking(Booking booking)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the seat can be booked (i.e., last booked time should be at least 3 minutes ago)
                var seat = GetSeatById(booking.SeatId); // Fetch seat details
                if (seat == null || !seat.CanBeRebooked()) // If seat can't be booked or not available
                {
                    throw new InvalidOperationException("This Artpiece cannot be booked yet. Please try again later.");
                }

                string query = "INSERT INTO Bookings (SeatId, UserId, BookingTime) VALUES (@SeatId, @UserId, @BookingTime)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SeatId", booking.SeatId);
                    command.Parameters.AddWithValue("@UserId", booking.UserId);
                    command.Parameters.AddWithValue("@BookingTime", booking.BookingTime);

                    command.ExecuteNonQuery();
                }

                // Update seat booking status
                seat.IsBooked = true;
                seat.LastBookedTime = booking.BookingTime;
                UpdateSeat(seat);
            }
        }
        public void UnbookSeat(int seatId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Seats SET IsBooked = @IsBooked, LastBookedTime = NULL WHERE SeatId = @SeatId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsBooked", false);
                    command.Parameters.AddWithValue("@SeatId", seatId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Dispose()
        {
            // Dispose resources if needed
        }
    }
}