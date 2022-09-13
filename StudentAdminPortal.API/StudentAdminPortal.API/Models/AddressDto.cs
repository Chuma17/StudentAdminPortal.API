namespace StudentAdminPortal.API.Models
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public string? PhysicalAddress { get; set; }
        public string? PostalAddress { get; set; }
        public Guid StudentId { get; set; }
    }
}
