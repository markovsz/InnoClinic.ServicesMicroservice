using Application.DTOs.Incoming;
using Application.DTOs.Outgoing;

namespace Application.Abstractions;

public interface ISpecializationsService
{
    Task<Guid> CreateAsync(SpecializationIncomingDto incomingDto);
    Task<IEnumerable<SpecializationMinOutgoingDto>> GetAsync();
    Task<SpecializationOutgoingDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, SpecializationIncomingDto incomingDto);
    Task ChangeStatusAsync(Guid id, string status);
}
