using System;

namespace WebApplication4.Models
{
    public class Seat
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; }
        public bool IsBooked { get; set; }
        public DateTime? LastBookedTime { get; set; }

        // Method to check if the seat can be unbooked after 3 minutes
        public bool CanBeUnbooked()
        {
            return IsBooked && LastBookedTime.HasValue && (DateTime.Now - LastBookedTime.Value).TotalMinutes >= 3;
        }

        // Method to check if the seat can be rebooked
        public bool CanBeRebooked()
        {
            return !IsBooked || (LastBookedTime.HasValue && (DateTime.Now - LastBookedTime.Value).TotalMinutes >= 3);
        }
    }
}