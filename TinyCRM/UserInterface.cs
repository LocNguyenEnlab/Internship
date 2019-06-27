using System;
using System.Collections.Generic;

namespace TinyCRM
{
    public class UserInterface
    {
        private int _enteringNumber;
        private Customer _customer;
        private CustomerService _service;


        public UserInterface()
        {            
        }

        internal void ShowMainMenu()
        {
            Console.WriteLine("\n====>MENU<====");
            Console.WriteLine("1. Add new customer");
            Console.WriteLine("2. Edit a customer");
            Console.WriteLine("3. Delete a customer");
            Console.WriteLine("4. Show customers");
            Console.WriteLine("5. Exit");
            Console.Write("\nInput your option: ");
            try
            {
                _enteringNumber = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                
            }
        }

        internal int GetEnteringInputNumber()
        {
            return _enteringNumber;
        }

        internal void ShowEnteringCustomer()
        {
            Console.WriteLine("\n===>Enter customer info:");
            _service = new CustomerService();
            _customer = new Customer();
            Console.Write("Name: ");
            _customer.Name = Console.ReadLine();
            
            while(true)
            {
                Console.Write("Email Office: ");
                _customer.EmailOffice = Console.ReadLine();
                if (_service.IsValidEmail(_customer.EmailOffice))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid email address, please input again");
                }
            }
            while(true)
            {
                Console.Write("Phone Office: ");
                _customer.PhoneOffice = Console.ReadLine();
                if (_service.IsValidPhoneNumber(_customer.PhoneOffice))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid phone number, please input again");
                }
            }
            while (true)
            {
                Console.Write("Email Home: ");
                _customer.EmailHome = Console.ReadLine();
                if (_service.IsValidEmail(_customer.EmailHome))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid email address, please input again");
                }
            }
            
            while(true)
            {
                Console.Write("Phone Home: ");
                _customer.PhoneHome = Console.ReadLine();
                if (_service.IsValidPhoneNumber(_customer.PhoneHome))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid phone number, please input again");
                }
            }
        }


        internal Customer GetEnteringCustomer()
        {
            return _customer;
        }

        internal bool IsConfirmed()
        {
            Console.WriteLine("Are you sure want to delete! 1. YES          2. NO");
            _enteringNumber = Convert.ToInt32(Console.ReadLine());
            if (_enteringNumber == 1)
            {
                return true;
            }
            return false;
        }

        internal void ShowCustomer(Customer customer)
        {
            Console.WriteLine();
            Console.WriteLine("Id: {0}", customer.Id);
            Console.WriteLine("Name: {0}", customer.Name);
            Console.WriteLine("Email Office: {0}", customer.EmailOffice);
            Console.WriteLine("Phone Office: {0}", customer.PhoneOffice);
            Console.WriteLine("Email Home: {0}", customer.EmailHome);
            Console.WriteLine("Phone Home: {0}", customer.PhoneHome);
        }

        internal void Inform(string v)
        {
            Console.WriteLine(v);
        }

        internal void ShowOptionViewCustomer()
        {
            Console.WriteLine("\n====>Menu show customer<====");
            Console.WriteLine("41. Show all customers");
            Console.WriteLine("42. Show a customer");
            Console.Write("\nYour selection: ");
            _enteringNumber = Convert.ToInt32(Console.ReadLine());
        }

        internal void ShowEnteringId(string inform)
        {
            Console.Write(inform);
            while(true)
            {
                try
                {
                    _enteringNumber = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid Id Type! Please input again!");
                    Console.Write("\nInput id to edit customer: ");
                }
            }
        }

        internal void ShowAllCustomers(List<Customer> listCustomer)
        {            
            if (listCustomer.Count != 0)
            {
                foreach (var item in listCustomer)
                {
                    ShowCustomer(item);
                }
            }
            else
            {
                Console.WriteLine("\nList customer is empty!");
            }
        }
    }
}