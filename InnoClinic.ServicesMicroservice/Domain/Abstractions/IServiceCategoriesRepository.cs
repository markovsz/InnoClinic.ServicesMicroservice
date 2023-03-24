using Domain.Entities;

namespace Domain.Abstractions;

public interface IServiceCategoriesRepository
{
    Task<ServiceCategory> GetByIdAsync(Guid id);
    Task<IEnumerable<ServiceCategory>> GetAsync();
}
