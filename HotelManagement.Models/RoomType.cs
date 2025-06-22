using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace HotelManagement.Models
{
    public class RoomType
    {
        public int RoomTypeID { get; set; }
        public string RoomTypeName { get; set; } // max 50 chars
        public string TypeDescription { get; set; } // max 250 chars
        public string TypeNote { get; set; } // max 250 chars
    }
}
