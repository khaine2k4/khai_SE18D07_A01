using HotelManagement.DataAccess;
using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagement.Business
{
    public class BookingService
    {
        private BookingRepository bookingRepository;
        private CustomerRepository customerRepository;
        private RoomInformationRepository roomRepository;

        public BookingService()
        {
            bookingRepository = BookingRepository.Instance;
            customerRepository = CustomerRepository.Instance;
            roomRepository = RoomInformationRepository.Instance;
        }

        public List<Booking> GetAllBookings()
        {
            var bookings = bookingRepository.GetAll();
            foreach (var booking in bookings)
            {
                booking.Customer = customerRepository.GetById(booking.CustomerID);
                booking.Room = roomRepository.GetById(booking.RoomID);
            }
            return bookings;
        }

        public Booking GetBookingById(int id)
        {
            var booking = bookingRepository.GetById(id);
            if (booking != null)
            {
                booking.Customer = customerRepository.GetById(booking.CustomerID);
                booking.Room = roomRepository.GetById(booking.RoomID);
            }
            return booking;
        }

        public List<Booking> GetBookingsByCustomer(int customerId)
        {
            var bookings = bookingRepository.Find(b => b.CustomerID == customerId);
            foreach (var booking in bookings)
            {
                booking.Customer = customerRepository.GetById(booking.CustomerID);
                booking.Room = roomRepository.GetById(booking.RoomID);
            }
            return bookings;
        }

        public void AddBooking(Booking booking)
        {
            var conflictingBookings = bookingRepository.Find(b =>
                b.RoomID == booking.RoomID &&
                b.BookingStatus == 1 &&
                !(booking.CheckOutDate <= b.CheckInDate || booking.CheckInDate >= b.CheckOutDate));

            if (conflictingBookings.Any())
            {
                throw new Exception("Phòng đã được đặt trong khoảng thời gian này.");
            }
            bookingRepository.Add(booking);
        }

        public void UpdateBooking(Booking booking)
        {
            bookingRepository.Update(booking);
        }

        public void DeleteBooking(int id)
        {
            bookingRepository.Delete(id);
        }

        public List<Booking> SearchBookings(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAllBookings();

            var allBookings = GetAllBookings();
            return allBookings.Where(b =>
                (b.Customer?.CustomerFullName?.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (b.Room?.RoomNumber?.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) ||
                b.BookingID.ToString().Contains(searchTerm)).ToList();
        }

        public List<Booking> GetBookingsByDateRange(DateTime startDate, DateTime endDate)
        {
            var bookings = bookingRepository.Find(b =>
                b.CheckInDate >= startDate && b.CheckInDate <= endDate);

            foreach (var booking in bookings)
            {
                booking.Customer = customerRepository.GetById(booking.CustomerID);
                booking.Room = roomRepository.GetById(booking.RoomID);
            }
            return bookings;
        }

        //public BookingStatistics GetBookingStatistics()
        //{
        //    var allBookings = GetAllBookings();
        //    return new BookingStatistics
        //    {
        //        TotalBookings = allBookings.Count,
        //        PendingBookings = allBookings.Count(b => b.BookingStatus == 0),
        //        ConfirmedBookings = allBookings.Count(b => b.BookingStatus == 1),
        //        CancelledBookings = allBookings.Count(b => b.BookingStatus == 2),
        //        CompletedBookings = allBookings.Count(b => b.BookingStatus == 3),
        //        TotalRevenue = allBookings.Where(b => b.BookingStatus == 3 || b.BookingStatus == 4)
        //                                 .Sum(b => b.TotalPrice ?? 0)
        //    };
        //}

        public BookingStatistics GetBookingStatistics()
        {
            var allBookings = GetAllBookings();
            return new BookingStatistics
            {
                TotalBookings = allBookings.Count,
                PendingBookings = allBookings.Count(b => b.BookingStatus == 0),
                ConfirmedBookings = allBookings.Count(b => b.BookingStatus == 1),
                CancelledBookings = allBookings.Count(b => b.BookingStatus == 2),
                CompletedBookings = allBookings.Count(b => b.BookingStatus == 3),
                TotalRevenue = allBookings.Where(b => b.BookingStatus == 3 || b.BookingStatus == 4)
                                           .Sum(b => (decimal)b.TotalPrice)
            };
        }

        public BookingStatistics GetBookingStatistics(DateTime startDate, DateTime endDate)
        {
            var bookings = GetBookingsByDateRange(startDate, endDate);
            return new BookingStatistics
            {
                TotalBookings = bookings.Count,
                PendingBookings = bookings.Count(b => b.BookingStatus == 0),
                ConfirmedBookings = bookings.Count(b => b.BookingStatus == 1),
                CancelledBookings = bookings.Count(b => b.BookingStatus == 2),
                CompletedBookings = bookings.Count(b => b.BookingStatus == 3),
                TotalRevenue = bookings.Where(b => b.BookingStatus == 3 || b.BookingStatus == 4)
                                       .Sum(b => (decimal)b.TotalPrice),
                Bookings = bookings


            };
        }
        //public BookingStatistics GetBookingStatistics(DateTime startDate, DateTime endDate)
        //{
        //    var bookings = GetBookingsByDateRange(startDate, endDate);
        //    return new BookingStatistics
        //    {
        //        TotalBookings = bookings.Count,
        //        PendingBookings = bookings.Count(b => b.BookingStatus == 0),
        //        ConfirmedBookings = bookings.Count(b => b.BookingStatus == 1),
        //        CancelledBookings = bookings.Count(b => b.BookingStatus == 2),
        //        CompletedBookings = bookings.Count(b => b.BookingStatus == 3),
        //        TotalRevenue = bookings.Where(b => b.BookingStatus == 3 || b.BookingStatus == 4)
        //                             .Sum(b => b.TotalPrice ?? 0)
        //    };
        //}

        public bool IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null)
        {
            var conflictingBookings = bookingRepository.Find(b =>
                b.RoomID == roomId &&
                b.BookingStatus == 1 &&
                (excludeBookingId == null || b.BookingID != excludeBookingId) &&
                !(checkOut <= b.CheckInDate || checkIn >= b.CheckOutDate));

            return !conflictingBookings.Any();
        }

        public List<RoomInformation> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            var allRooms = roomRepository.GetAll();
            var availableRooms = new List<RoomInformation>();

            foreach (var room in allRooms)
            {
                if (IsRoomAvailable(room.RoomID, checkIn, checkOut))
                {
                    availableRooms.Add(room);
                }
            }

            return availableRooms;
        }
        public class BookingStatistics
        {
            public int TotalBookings { get; set; }
            public int PendingBookings { get; set; }
            public int ConfirmedBookings { get; set; }
            public int CancelledBookings { get; set; }
            public int CompletedBookings { get; set; }
            public decimal TotalRevenue { get; set; }
            public List<Booking> Bookings { get; set; }
        }
    }
}
