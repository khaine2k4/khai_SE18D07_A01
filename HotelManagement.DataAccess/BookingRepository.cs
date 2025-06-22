using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HotelManagement.DataAccess
{
    public class BookingRepository : IRepository<Booking>
    {
        private static BookingRepository _instance;
        private static readonly object _lock = new object();
        private List<Booking> bookings;

        private BookingRepository()
        {
            InitializeData();
        }

        public static BookingRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new BookingRepository();
                    }
                }
                return _instance;
            }
        }

        private void InitializeData()
        {
            bookings = new List<Booking>
            {
                new Booking
                {
                    BookingID = 1,
                    CustomerID = 1,
                    RoomID = 1,
                    CheckInDate = DateTime.Now,
                    CheckOutDate = DateTime.Now.AddDays(2),
                    TotalPrice = 1000000,
                    BookingStatus = 1
                },
                new Booking
                {
                    BookingID = 2,
                    CustomerID = 2,
                    RoomID = 2,
                    CheckInDate = DateTime.Now.AddDays(-5),
                    CheckOutDate = DateTime.Now.AddDays(-3),
                    TotalPrice = 1200000,
                    BookingStatus = 3
                }
            };
        }

        public List<Booking> GetAll()
        {
            return bookings.Where(b => b.BookingStatus != 2).ToList();
        }

        public Booking GetById(int id)
        {
            return bookings.FirstOrDefault(b => b.BookingID == id && b.BookingStatus != 2);
        }

        public void Add(Booking entity)
        {
            entity.BookingID = bookings.Max(b => b.BookingID) + 1;
            entity.BookingStatus = 1;
            bookings.Add(entity);
        }

        public void Update(Booking entity)
        {
            var existing = bookings.FirstOrDefault(b => b.BookingID == entity.BookingID);
            if (existing != null)
            {
                existing.CustomerID = entity.CustomerID;
                existing.RoomID = entity.RoomID;
                existing.CheckInDate = entity.CheckInDate;
                existing.CheckOutDate = entity.CheckOutDate;
                existing.TotalPrice = entity.TotalPrice;
                existing.BookingStatus = entity.BookingStatus;
            }
        }

        public void Delete(int id)
        {
            var booking = bookings.FirstOrDefault(b => b.BookingID == id);
            if (booking != null)
            {
                booking.BookingStatus = 2; // Soft delete
            }
        }

        public List<Booking> Find(Expression<Func<Booking, bool>> predicate)
        {
            return bookings.AsQueryable().Where(predicate).Where(b => b.BookingStatus != 2).ToList();
        }
    }
}