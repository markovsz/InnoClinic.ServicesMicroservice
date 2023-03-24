using Application.DTOs.Incoming;
using Application.DTOs.Outgoing;
using Domain.RequestParameters;

namespace Application.Abstractions;

public interface IServicesService
{
    Task<Guid> CreateAsync(ServiceIncomingDto incomingDto);
    Task<IEnumerable<ServiceMinOutgoingDto>> GetAsync(ServiceParameters parameters);
    Task<ServiceOutgoingDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, ServiceIncomingDto incomingDto);
    Task ChangeStatusAsync(Guid id, string status);
}
