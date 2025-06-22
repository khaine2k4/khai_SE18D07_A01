using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Models
{
    public class RoomInformation
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; } // max 50 chars
        public string RoomDescription { get; set; } // max 220 chars
        public int RoomMaxCapacity { get; set; }
        public int RoomStatus { get; set; } // 1 Active, 2 Deleted
        public decimal RoomPricePerDate { get; set; }
        public int RoomTypeID { get; set; }

        // Navigation property
        public RoomType RoomType { get; set; }
    }
}
