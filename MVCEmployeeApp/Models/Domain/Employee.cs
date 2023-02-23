namespace MVCEmployeeApp.Models.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string JobPosition { get; set;}
        public string City { get; set;}
    }
}
