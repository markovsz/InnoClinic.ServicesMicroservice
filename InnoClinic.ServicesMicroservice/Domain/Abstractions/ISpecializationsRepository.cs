using Domain.Entities;

namespace Domain.Abstractions;

public interface ISpecializationsRepository
{
    Task<Guid> CreateAsync(Specialization specialization);
    Task ChangeStatusAsync(Guid id, string status);
    Task<Specialization> GetByIdAsync(Guid id);
    Task<Specialization> GetByNameAsync(string name);
    Task<bool> Exists(Guid id);
    Task<IEnumerable<Specialization>> GetAsync();
    Task UpdateAsync(Specialization specialization);
}
