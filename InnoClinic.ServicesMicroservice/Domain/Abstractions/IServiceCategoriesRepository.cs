using Domain.Entities;

namespace Domain.Abstractions;

public interface IServiceCategoriesRepository
{
    Task<ServiceCategory> GetByNameAsync(string name);
}
