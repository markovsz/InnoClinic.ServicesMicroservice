namespace Application.DTOs.Incoming;

public class ServiceIncomingDto
{
    public string Name { get; set; }
    public Decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Guid SpecializationId { get; set; }
    public string Status { get; set; }
}
