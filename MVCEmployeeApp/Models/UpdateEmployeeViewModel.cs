namespace MVCEmployeeApp.Models
{
    public class UpdateEmployeeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string JobPosition { get; set; }
        public string City { get; set; }
    }
}
