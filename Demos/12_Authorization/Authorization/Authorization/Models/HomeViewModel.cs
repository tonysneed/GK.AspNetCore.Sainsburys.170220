namespace Authorization.Models
{
    public class HomeViewModel
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Region { get; set; }
        public SecurityClearance SecurityClearance { get; set; }
        public bool CanViewSales { get; set; }
        public string[] ViewableRegions { get; set; }
    }
}
