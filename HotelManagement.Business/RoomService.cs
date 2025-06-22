using HotelManagement.DataAccess;
using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HotelManagement.Business
{
    public class RoomService
    {
        private RoomInformationRepository roomRepository;
        private RoomTypeRepository roomTypeRepository;
        private BookingRepository bookingRepository;
        private CustomerRepository customerRepository;

        public RoomService()
        {
            roomRepository = RoomInformationRepository.Instance;
            roomTypeRepository = RoomTypeRepository.Instance;
            bookingRepository = BookingRepository.Instance;
            customerRepository = CustomerRepository.Instance;
        }

        public List<RoomInformation> GetAllRooms()
        {
            var rooms = roomRepository.GetAll();
            foreach (var room in rooms)
            {
                room.RoomType = roomTypeRepository.GetById(room.RoomTypeID);
            }
            return rooms;
        }

        public RoomInformation GetRoomById(int id)
        {
            var room = roomRepository.GetById(id);
            if (room != null)
            {
                room.RoomType = roomTypeRepository.GetById(room.RoomTypeID);
            }
            return room;
        }

        public void AddRoom(RoomInformation room)
        {
            roomRepository.Add(room);
        }

        public void UpdateRoom(RoomInformation room)
        {
            roomRepository.Update(room);
        }

        public void DeleteRoom(int id)
        {
            roomRepository.Delete(id);
        }

        public List<RoomInformation> SearchRooms(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAllRooms();

            var rooms = roomRepository.Find(r =>
                r.RoomNumber.Contains(searchTerm) ||
                r.RoomDescription.Contains(searchTerm));

            foreach (var room in rooms)
            {
                room.RoomType = roomTypeRepository.GetById(room.RoomTypeID);
            }
            return rooms;
        }

        public List<RoomType> GetAllRoomTypes()
        {
            return roomTypeRepository.GetAll();
        }
        public (int TotalBookings, decimal TotalRevenue, List<Booking> Bookings) GetBookingStatistics(DateTime startDate, DateTime endDate)
        {
            var bookings = bookingRepository.Find(b =>
                b.CheckInDate >= startDate &&
                b.CheckInDate <= endDate &&
                b.BookingStatus != 2);

            foreach (var booking in bookings)
            {
                booking.Customer = customerRepository.GetById(booking.CustomerID);
                booking.Room = roomRepository.GetById(booking.RoomID);
            }

            return (
                TotalBookings: bookings.Count,
                TotalRevenue: bookings.Sum(b => b.TotalPrice),
                Bookings: bookings.OrderByDescending(b => b.CheckInDate).ToList()
            );
        }

    }
}