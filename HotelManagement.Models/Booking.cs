using System;

namespace HotelManagement.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int RoomID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int BookingStatus { get; set; } // 1: Active, 2: Cancelled, 3: Completed

        // Navigation properties
        public Customer Customer { get; set; }
        public RoomInformation Room { get; set; }
    }
}