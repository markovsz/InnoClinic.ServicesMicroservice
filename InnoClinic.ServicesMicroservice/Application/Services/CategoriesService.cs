using Application.Abstractions;
using AutoMapper;
using Domain.Abstractions;
using Domain.Exceptions;
using InnoClinic.SharedModels.DTOs.Services.Outgoing;

namespace Application.Services;

public class CategoriesService : ICategoriesService
{
    private readonly IServiceCategoriesRepository _serviceCategoriesRepository;
    private readonly IMapper _mapper;

    public CategoriesService(IServiceCategoriesRepository serviceCategoriesRepository, IMapper mapper)
    {
        _serviceCategoriesRepository = serviceCategoriesRepository;
        _mapper = mapper;
    }   

    public async Task<ServiceCategoryOutgoingDto> GetByNameAsync(string name)
    {
        var category = await _serviceCategoriesRepository.GetByNameAsync(name);
        if (category is null)
            throw new EntityNotFoundException();
        var mappedCategory = _mapper.Map<ServiceCategoryOutgoingDto>(category);
        return mappedCategory;
    }
}
