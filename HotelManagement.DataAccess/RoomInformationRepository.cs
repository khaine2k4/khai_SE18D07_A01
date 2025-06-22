using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HotelManagement.DataAccess
{
    public class RoomInformationRepository : IRepository<RoomInformation>
    {
        private static RoomInformationRepository _instance;
        private static readonly object _lock = new object();
        private List<RoomInformation> rooms;

        private RoomInformationRepository()
        {
            InitializeData();
        }

        public static RoomInformationRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new RoomInformationRepository();
                    }
                }
                return _instance;
            }
        }

        private void InitializeData()
        {
            rooms = new List<RoomInformation>
            {
                new RoomInformation
                {
                    RoomID = 1,
                    RoomNumber = "101",
                    RoomDescription = "Standard room on first floor",
                    RoomMaxCapacity = 2,
                    RoomStatus = 1,
                    RoomPricePerDate = 500000,
                    RoomTypeID = 1
                },
                new RoomInformation
                {
                    RoomID = 2,
                    RoomNumber = "102",
                    RoomDescription = "Standard room with city view",
                    RoomMaxCapacity = 2,
                    RoomStatus = 1,
                    RoomPricePerDate = 600000,
                    RoomTypeID = 1
                },
                new RoomInformation
                {
                    RoomID = 3,
                    RoomNumber = "201",
                    RoomDescription = "Deluxe room with balcony",
                    RoomMaxCapacity = 4,
                    RoomStatus = 1,
                    RoomPricePerDate = 1000000,
                    RoomTypeID = 2
                }
            };
        }

        public List<RoomInformation> GetAll()
        {
            return rooms.Where(r => r.RoomStatus == 1).ToList();
        }

        public RoomInformation GetById(int id)
        {
            return rooms.FirstOrDefault(r => r.RoomID == id && r.RoomStatus == 1);
        }

        public void Add(RoomInformation entity)
        {
            entity.RoomID = rooms.Max(r => r.RoomID) + 1;
            entity.RoomStatus = 1;
            rooms.Add(entity);
        }

        public void Update(RoomInformation entity)
        {
            var existing = rooms.FirstOrDefault(r => r.RoomID == entity.RoomID);
            if (existing != null)
            {
                existing.RoomNumber = entity.RoomNumber;
                existing.RoomDescription = entity.RoomDescription;
                existing.RoomMaxCapacity = entity.RoomMaxCapacity;
                existing.RoomPricePerDate = entity.RoomPricePerDate;
                existing.RoomTypeID = entity.RoomTypeID;
            }
        }

        public void Delete(int id)
        {
            var room = rooms.FirstOrDefault(r => r.RoomID == id);
            if (room != null)
            {
                room.RoomStatus = 2; // Soft delete
            }
        }

        public List<RoomInformation> Find(Expression<Func<RoomInformation, bool>> predicate)
        {
            return rooms.AsQueryable().Where(predicate).Where(r => r.RoomStatus == 1).ToList();
        }
    }
}