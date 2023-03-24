namespace Application.DTOs.Outgoing;

public class SpecializationOutgoingDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public IEnumerable<ServiceMinOutgoingDto> Services { get; set; }
}
