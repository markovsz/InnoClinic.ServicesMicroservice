using InnoClinic.SharedModels.DTOs.Services.Outgoing;

namespace Application.Abstractions;

public interface ICategoriesService
{
    Task<ServiceCategoryOutgoingDto> GetByNameAsync(string name);
}
