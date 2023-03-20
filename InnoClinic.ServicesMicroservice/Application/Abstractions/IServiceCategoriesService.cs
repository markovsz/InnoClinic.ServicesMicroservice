using Domain.Entities;

namespace Application.Abstractions;

public interface IServiceCategoriesService
{
    Task<Service> GetByIdAsync(Guid id);
    Task<IEnumerable<Service>> GetAsync();
}
