using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Models/Booking.cs
namespace WebApplication4.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int SeatId { get; set; }
        public int UserId { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
