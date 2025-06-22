using System;

namespace HotelManagement.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerFullName { get; set; } // max 50 chars
        public string Telephone { get; set; } // max 12 chars
        public string EmailAddress { get; set; } // max 50 chars
        public DateTime CustomerBirthday { get; set; }
        public int CustomerStatus { get; set; } // 1 Active, 2 Deleted
        public string Password { get; set; } // max 50 chars
    }
}