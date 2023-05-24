using InnoClinic.SharedModels.DTOs.Services.Incoming;
using InnoClinic.SharedModels.DTOs.Services.Outgoing;

namespace Application.Abstractions;

public interface ISpecializationsService
{
    Task<Guid> CreateAsync(SpecializationIncomingDto incomingDto);
    Task<IEnumerable<SpecializationMinOutgoingDto>> GetAsync();
    Task<IEnumerable<SpecializationMinOutgoingDto>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<SpecializationOutgoingDto> GetByIdAsync(Guid id);
    Task<SpecializationMinOutgoingDto> GetMinByIdAsync(Guid id);
    Task UpdateAsync(Guid id, SpecializationIncomingDto incomingDto);
    Task ChangeStatusAsync(Guid id, string status);
}
