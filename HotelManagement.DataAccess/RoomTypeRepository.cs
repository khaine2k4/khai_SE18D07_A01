using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HotelManagement.DataAccess
{
    public class RoomTypeRepository : IRepository<RoomType>
    {
        private static RoomTypeRepository _instance;
        private static readonly object _lock = new object();
        private List<RoomType> roomTypes;

        private RoomTypeRepository()
        {
            InitializeData();
        }

        public static RoomTypeRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new RoomTypeRepository();
                    }
                }
                return _instance;
            }
        }

        private void InitializeData()
        {
            roomTypes = new List<RoomType>
            {
                new RoomType { RoomTypeID = 1, RoomTypeName = "Standard", TypeDescription = "Standard room with basic amenities", TypeNote = "Good for budget travelers" },
                new RoomType { RoomTypeID = 2, RoomTypeName = "Deluxe", TypeDescription = "Deluxe room with premium amenities", TypeNote = "Popular choice" },
                new RoomType { RoomTypeID = 3, RoomTypeName = "Suite", TypeDescription = "Luxury suite with premium services", TypeNote = "Best experience" }
            };
        }

        public List<RoomType> GetAll() => roomTypes;
        public RoomType GetById(int id) => roomTypes.FirstOrDefault(rt => rt.RoomTypeID == id);
        public void Add(RoomType entity)
        {
            entity.RoomTypeID = roomTypes.Max(rt => rt.RoomTypeID) + 1;
            roomTypes.Add(entity);
        }
        public void Update(RoomType entity)
        {
            var existing = roomTypes.FirstOrDefault(rt => rt.RoomTypeID == entity.RoomTypeID);
            if (existing != null)
            {
                existing.RoomTypeName = entity.RoomTypeName;
                existing.TypeDescription = entity.TypeDescription;
                existing.TypeNote = entity.TypeNote;
            }
        }
        public void Delete(int id)
        {
            var roomType = roomTypes.FirstOrDefault(rt => rt.RoomTypeID == id);
            if (roomType != null)
                roomTypes.Remove(roomType);
        }
        public List<RoomType> Find(Expression<Func<RoomType, bool>> predicate)
        {
            return roomTypes.AsQueryable().Where(predicate).ToList();
        }
    }
}
