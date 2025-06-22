using HotelManagement.DataAccess;
using HotelManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagement.Business
{
    public class CustomerService
    {
        private CustomerRepository customerRepository;

        public CustomerService()
        {
            customerRepository = CustomerRepository.Instance;
        }

        public List<Customer> GetAllCustomers()
        {
            return customerRepository.GetAll();
        }

        public Customer GetCustomerById(int id)
        {
            return customerRepository.GetById(id);
        }

        public void AddCustomer(Customer customer)
        {
            customerRepository.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            customerRepository.Update(customer);
        }

        public void DeleteCustomer(int id)
        {
            customerRepository.Delete(id);
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAllCustomers();

            return customerRepository.Find(c =>
                c.CustomerFullName.Contains(searchTerm) ||
                c.EmailAddress.Contains(searchTerm) ||
                c.Telephone.Contains(searchTerm));
        }

        public Customer Authenticate(string email, string password)
        {
            return customerRepository.GetByEmailAndPassword(email, password);
        }
    }
}