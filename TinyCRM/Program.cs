using System;
using System.Collections.Generic;

namespace TinyCRM
{
    class Program
    {
        static int Main(string[] args)
        {
            var ui = new UserInterface();
            var logic = new CustomerService();

            while (true)
            {
                ui.ShowMainMenu();
                int selectedItem = ui.GetEnteringInputNumber();

                switch (selectedItem)
                {
                    case 1: //Add new customer
                        ui.ShowEnteringCustomer();
                        var newCustomer = ui.GetEnteringCustomer();
                        var result = logic.Save(newCustomer);
                        ui.Inform(result.Message);                        
                        break;
                    case 2: //Edit a customer
                        ui.ShowEnteringId("Input id to edit customer: ");
                        int idCustomerEdit = ui.GetEnteringInputNumber();
                        Customer editCustomer = logic.GetCustomer(idCustomerEdit);
                        ui.ShowCustomer(editCustomer);
                        ui.Inform("===>Start update customer");
                        ui.ShowEnteringCustomer();
                        editCustomer = ui.GetEnteringCustomer();
                        if (logic.IsValidEmail(editCustomer.EmailHome) && logic.IsValidEmail(editCustomer.EmailOffice))
                        {
                            logic.Save(editCustomer);
                            ui.Inform("Updated!");
                        }
                        else
                        {
                            ui.Inform("Invalid email address!");
                        }
                        break;
                    case 3: //delete a customer
                        ui.ShowEnteringId("Input id to delete customer: ");
                        int customerId = ui.GetEnteringInputNumber();
                        var customer = logic.GetCustomer(customerId);
                        if (customer != null)
                        {
                            if (ui.IsConfirmed())
                            {
                                logic.Delete(customer);
                                ui.Inform("Customer is deleted!");
                            }
                        }
                        else
                        {
                            ui.Inform("Invalid Customer Id!");
                        }
                        break;
                    case 4: //view customers
                        ui.ShowOptionViewCustomer();
                        int selectView = ui.GetEnteringInputNumber();
                        switch (selectView)
                        {
                            case 41: //show all customers
                                List<Customer> customers = logic.GetCustomers();
                                ui.ShowAllCustomers(customers);
                                break;
                            case 42: //show a customer
                                ui.ShowEnteringId("Input id to view customer: ");
                                int idCustomerShow = ui.GetEnteringInputNumber();
                                Customer customerShow = logic.GetCustomer(idCustomerShow);
                                if (customerShow != null)
                                {                                    
                                    ui.ShowCustomer(customerShow);
                                }
                                else
                                {
                                    ui.Inform("Invalid Customer Id!");
                                }
                                break;
                        }
                        break;
                    case 5: //quit
                        return 0;
                    default:
                        ui.Inform("Bad selection");
                        break;
                }
            }

        }
    }
}
