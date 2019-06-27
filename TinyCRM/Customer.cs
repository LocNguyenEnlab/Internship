namespace TinyCRM
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailOffice { get; set; }
        public string PhoneOffice { get; set; }
        public string EmailHome { get; set; }
        public string PhoneHome { get; set; }

        /// <summary>
        /// Compare 2 customer objects based on their name, emails, phones
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Return true if this.customer = customer, false if not equal</returns>
        public bool Compare(Customer customer)
        {
            if (customer.Name != this.Name)
                return false;
            if (customer.EmailHome != this.EmailHome)
                return false;
            if (customer.PhoneHome != this.PhoneHome)
                return false;
            if (customer.EmailOffice != this.EmailOffice)
                return false;
            if (customer.PhoneOffice != this.PhoneOffice)
                return false;
            return true;
        }
    }
}