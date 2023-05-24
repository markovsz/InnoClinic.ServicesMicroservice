using Application.Abstractions;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.RequestParameters;
using InnoClinic.SharedModels.DTOs.Services.Incoming;
using InnoClinic.SharedModels.DTOs.Services.Outgoing;

namespace Application.Services;

public class ServicesService : IServicesService
{
    private readonly IServicesRepository _servicesRepository;
    private readonly IMapper _mapper;

    public ServicesService(IServicesRepository servicesRepository, IMapper mapper)
    {
        _servicesRepository = servicesRepository;
        _mapper = mapper;
    }   

    public async Task<Guid> CreateAsync(ServiceIncomingDto incomingDto)
    {
        var service = _mapper.Map<Service>(incomingDto);
        var id = await _servicesRepository.CreateAsync(service);
        return id;
    }

    public async Task<IEnumerable<ServiceMinOutgoingDto>> GetAsync(ServiceParameters parameters)
    {
        var services = await _servicesRepository.GetAsync(parameters);
        var mappedServices = _mapper.Map<IEnumerable<ServiceMinOutgoingDto>>(services);
        return mappedServices;
    }

    public async Task<ServiceOutgoingDto> GetByIdAsync(Guid id)
    {
        var service = await _servicesRepository.GetByIdAsync(id);
        var mappedService = _mapper.Map<ServiceOutgoingDto>(service);
        return mappedService;
    }

    public async Task UpdateAsync(Guid id, ServiceIncomingDto incomingDto)
    {
        var exists = await _servicesRepository.Exists(id);
        if (!exists)
            throw new EntityNotFoundException();
        var service = _mapper.Map<Service>(incomingDto);
        service.Id = id;
        await _servicesRepository.UpdateAsync(service);
    }

    public async Task ChangeStatusAsync(Guid id, string status)
    {
        var exists = await _servicesRepository.Exists(id);
        if (!exists)
            throw new EntityNotFoundException();
        await _servicesRepository.ChangeStatusAsync(id, status);
    }
}
