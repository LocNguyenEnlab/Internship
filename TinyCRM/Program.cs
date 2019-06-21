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
                MenuItems selectedItem = (MenuItems)ui.GetEnteringInputNumber();
                
                switch (selectedItem)
                {
                    case MenuItems.Add: //Add new customer
                        ui.ShowEnteringCustomer();
                        var newCustomer = ui.GetEnteringCustomer();
                        var result = logic.Save(newCustomer);
                        ui.Inform(result.Message);                        
                        break;
                    case MenuItems.Edit: //Edit a customer
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
                    case MenuItems.Delete: //delete a customer
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
                    case MenuItems.View: //view customers
                        ui.ShowOptionViewCustomer();
                        MenuItems selectView = (MenuItems)ui.GetEnteringInputNumber();
                        switch (selectView)
                        {
                            case MenuItems.ViewAllCustomers: //show all customers
                                List<Customer> customers = logic.GetCustomers();
                                ui.ShowAllCustomers(customers);
                                break;
                            case MenuItems.ViewACustomer: //show a customer
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
                    case MenuItems.Quit: //quit
                        return 0;
                    default:
                        ui.Inform("Bad selection");
                        break;
                }
            }

        }
    }    
}
