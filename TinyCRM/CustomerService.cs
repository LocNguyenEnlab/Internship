using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyCRM
{
    public class CustomerService
    {
        private List<Customer> _customers;


        public CustomerService()
        {
            _customers = new List<Customer>();
            var customer = new Customer
            {
                Id = 1,
                Name = "LOC",
                EmailHome = "loc@gmail.com",
                PhoneHome = 0393384646,
                EmailOffice = "loc.nguyen@enlabsoftware.com",
                PhoneOffice = 456
            };
            _customers.Add(customer);
        }

        public Result Save(Customer customer)
        {
            if (IsDuplicateCustomer(customer))
                return new Result() { Error = Errors.DuplicateCustomer, Message = "Customer already exists. Please input another one." };

            if (_customers.Count == 0)
            {
                customer.Id = 1;
            }
            else
            {
                customer.Id = _customers.Last().Id + 1;
            }
            _customers.Add(customer);

            return new Result() { Error = Errors.None, Message = "Add customer successfully!" };
        }

        internal Customer GetCustomer(int customerId)
        {
            return _customers.FirstOrDefault(customer => customer.Id == customerId);
        }

        internal bool IsDuplicateCustomer(Customer newCustomer)
        {
            foreach(var item in _customers)
            {
                if (item.Compare(newCustomer))
                    return false;
            }
            return true;
        }

        internal void Delete(Customer customer)
        {
            _customers.Remove(customer);
        }

        public List<Customer> GetCustomers()
        {
            return _customers;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}