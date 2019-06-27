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
                var selectedItem = (MenuItems)ui.GetEnteringInputNumber();
                
                switch (selectedItem)
                {
                    case MenuItems.Add:
                        Insert(ui, logic);
                        break;
                    case MenuItems.Edit:
                        Update(ui, logic);
                        break;
                    case MenuItems.Delete:
                        Delete(ui, logic);
                        break;
                    case MenuItems.View:
                        Show(ui, logic);
                        break;
                    case MenuItems.Quit:
                        return 0;
                    default:
                        ui.Inform("\nWrong selection, please input again!");
                        break;
                }
            }

        }

        private static void Insert(UserInterface ui, CustomerService logic)
        {
            ui.ShowEnteringCustomer();
            var newCustomer = ui.GetEnteringCustomer();
            var addResult = logic.Save(newCustomer);
            ui.Inform(addResult.Message);            
        }

        private static void Update(UserInterface ui, CustomerService logic)
        {
            ui.ShowEnteringId("\nInput id to edit customer: ");
            int customerIdEdit = ui.GetEnteringInputNumber();
            var editCustomer = logic.GetCustomer(customerIdEdit);
            if (editCustomer != null)
            {
                ui.ShowCustomer(editCustomer);
                ui.Inform("\n===>Start update customer");
                ui.ShowEnteringCustomer();
                editCustomer = ui.GetEnteringCustomer();
                editCustomer.Id = customerIdEdit;
                var editResult = logic.Update(editCustomer);
                ui.Inform(editResult.Message);
            }
            else
            {
                ui.Inform("\nInvalid Customer Id");
            }
        }

        private static void Delete(UserInterface ui, CustomerService logic)
        {
            ui.ShowEnteringId("\nInput id to delete customer: ");
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
                ui.Inform("\nInvalid Customer Id!");
            }
        }

        private static void Show(UserInterface ui, CustomerService logic)
        {
            ui.ShowOptionViewCustomer();
            var selectView = (MenuItems)ui.GetEnteringInputNumber();
            switch (selectView)
            {
                case MenuItems.ViewAllCustomers:
                    List<Customer> customers = logic.GetCustomers();
                    ui.ShowAllCustomers(customers);
                    break;
                case MenuItems.ViewACustomer:
                    ui.ShowEnteringId("\nInput id to view customer: ");
                    int idCustomerShow = ui.GetEnteringInputNumber();
                    Customer customerShow = logic.GetCustomer(idCustomerShow);
                    if (customerShow != null)
                    {
                        ui.ShowCustomer(customerShow);
                    }
                    else
                    {
                        ui.Inform("\nInvalid Customer Id!");
                    }
                    break;
            }
        }
    }    
}
