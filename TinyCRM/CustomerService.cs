using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
                PhoneHome = "0393384646",
                EmailOffice = "loc.nguyen@enlabsoftware.com",
                PhoneOffice = "456"
            };
            _customers.Add(customer);
        }

        public Result Save(Customer customer)
        {
            if (IsDuplicatedCustomer(customer))
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

        internal bool IsDuplicatedCustomer(Customer newCustomer)
        {
            foreach(var item in _customers)
            {
                if (item.Compare(newCustomer))
                    return true;
            }
            return false;
        }

        internal Result Update(Customer editCustomer)
        {
            try
            {
                var id = _customers.FindIndex(s=>s.Id == editCustomer.Id);
                _customers[id] = editCustomer;
                return new Result() { Error = Errors.None, Message = "Updated!" };
            }
            catch (Exception)
            {
                return new Result() { Error = Errors.UpdateFail, Message = "Update fail!" };
            }
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
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (phoneNumber[0] == '+')
            {
                phoneNumber = phoneNumber.Remove(0, 1);
            }
            try
            {
                int phone = Convert.ToInt32(phoneNumber);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}