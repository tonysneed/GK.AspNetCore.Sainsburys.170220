namespace HelloDepInjection
{
    public class CustomerRepository : ICustomerRepository
    {
        public string GetCity(string customerName)
        {
            switch (customerName)
            {
                case "Peter":
                    return "London";
                case "Paul":
                    return "New York";
                case "Mary":
                    return "Los Angeles";
                default:
                    return "Madrid";
            }
        }
    }
}
