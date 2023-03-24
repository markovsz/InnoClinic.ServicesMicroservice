namespace Application.DTOs.Incoming;

public class SpecializationIncomingDto
{
    public string Name { get; set; }
    public string Status { get; set; }
    public IEnumerable<Guid> ServiceIds { get; set; }
}
