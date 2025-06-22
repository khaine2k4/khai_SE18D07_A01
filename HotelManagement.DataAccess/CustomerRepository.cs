using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HotelManagement.Models;

using System.Linq.Expressions;


namespace HotelManagement.DataAccess
{
    public class CustomerRepository : IRepository<Customer>
    {
        private static CustomerRepository _instance;
        private static readonly object _lock = new object();
        private List<Customer> customers;

        private CustomerRepository()
        {
            InitializeData();
        }

        public static CustomerRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new CustomerRepository();
                    }
                }
                return _instance;
            }
        }

        private void InitializeData()
        {
            customers = new List<Customer>
            {
                new Customer
                {
                    CustomerID = 1,
                    CustomerFullName = "Nguyen Van A",
                    Telephone = "0123456789",
                    EmailAddress = "nguyenvana@gmail.com",
                    CustomerBirthday = new DateTime(1990, 1, 1),
                    CustomerStatus = 1,
                    Password = "123456"
                },
                new Customer
                {
                    CustomerID = 2,
                    CustomerFullName = "Tran Thi B",
                    Telephone = "0987654321",
                    EmailAddress = "tranthib@gmail.com",
                    CustomerBirthday = new DateTime(1995, 5, 15),
                    CustomerStatus = 1,
                    Password = "abcdef"
                }
            };
        }

        public List<Customer> GetAll()
        {
            return customers.Where(c => c.CustomerStatus == 1).ToList();
        }

        public Customer GetById(int id)
        {
            return customers.FirstOrDefault(c => c.CustomerID == id && c.CustomerStatus == 1);
        }

        public void Add(Customer entity)
        {
            entity.CustomerID = customers.Max(c => c.CustomerID) + 1;
            entity.CustomerStatus = 1;
            customers.Add(entity);
        }

        public void Update(Customer entity)
        {
            var existingCustomer = customers.FirstOrDefault(c => c.CustomerID == entity.CustomerID);
            if (existingCustomer != null)
            {
                existingCustomer.CustomerFullName = entity.CustomerFullName;
                existingCustomer.Telephone = entity.Telephone;
                existingCustomer.EmailAddress = entity.EmailAddress;
                existingCustomer.CustomerBirthday = entity.CustomerBirthday;
                existingCustomer.Password = entity.Password;
            }
        }

        public void Delete(int id)
        {
            var customer = customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer != null)
            {
                customer.CustomerStatus = 2; // Soft delete
            }
        }

        public List<Customer> Find(Expression<Func<Customer, bool>> predicate)
        {
            return customers.AsQueryable().Where(predicate).Where(c => c.CustomerStatus == 1).ToList();
        }

        public Customer GetByEmailAndPassword(string email, string password)
        {
            return customers.FirstOrDefault(c => c.EmailAddress == email &&
                                                c.Password == password &&
                                                c.CustomerStatus == 1);
        }
    }
}
