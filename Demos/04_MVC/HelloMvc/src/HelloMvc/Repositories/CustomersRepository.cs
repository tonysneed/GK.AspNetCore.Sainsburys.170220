using HelloMvc.Models;

namespace HelloMvc.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        public Customer GetCustomer()
        {
            return new Customer
            {
                FirstName = "Jack",
                LastName = "Doe"
            };
        }
    }
}
