using Domain.Entities;
using Domain.RequestParameters;

namespace Domain.Abstractions;

public interface IServicesRepository
{
    Task<Guid> CreateAsync(Service service);
    Task ChangeStatusAsync(Guid id, string status);
    Task<Service> GetByIdAsync(Guid id);
    Task<Service> GetByNameAsync(string name);
    Task<bool> Exists(Guid id);
    Task<IEnumerable<Service>> GetAsync(ServiceParameters parameters);
    Task UpdateAsync(Service service);
    Task LinkWithSpecializationByIdAsync(Guid id, Guid specializationId);
}
