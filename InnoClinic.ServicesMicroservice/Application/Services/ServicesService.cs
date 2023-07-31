using Application.Abstractions;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.RequestParameters;
using InnoClinic.SharedModels.DTOs.Services.Incoming;
using InnoClinic.SharedModels.DTOs.Services.Outgoing;
using InnoClinic.SharedModels.Messages;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ServicesService : IServicesService
{
    private readonly IServicesRepository _servicesRepository;
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IMapper _mapper;

    public ServicesService(IServicesRepository servicesRepository, IBus bus, IMapper mapper, IConfiguration configuration)
    {
        _servicesRepository = servicesRepository;
        var uri = configuration.GetSection("RabbitMq:Uri").Value;
        var doctorUpdatedQueue = configuration.GetSection("RabbitMq:QueueNames:ServiceUpdated").Value;
        _sendEndpoint = bus.GetSendEndpoint(new Uri(uri + doctorUpdatedQueue)).GetAwaiter().GetResult();
        _mapper = mapper;
    }   

    public async Task<Guid> CreateAsync(AddServiceIncomingDto incomingDto)
    {
        var service = _mapper.Map<Service>(incomingDto);
        var category = await _serviceCategoriesRepository.GetByNameAsync(incomingDto.Category);
        if (category is null)
            throw new EntityNotFoundException();
        service.CategoryId = category.Id;
        var id = await _servicesRepository.CreateAsync(service);
        return id;
    }

    public async Task<IEnumerable<ServiceMinOutgoingDto>> GetAsync(ServiceParameters parameters)
    {
        var services = await _servicesRepository.GetAsync(parameters);
        var mappedServices = _mapper.Map<IEnumerable<ServiceMinOutgoingDto>>(services);
        return mappedServices;
    }

    public async Task<IEnumerable<ServiceMinOutgoingDto>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        var entities = new List<ServiceMinOutgoingDto>();
        foreach (var id in ids)
        {
            var entity = await _servicesRepository.GetByIdAsync(id);
            var mappedEntity = _mapper.Map<ServiceMinOutgoingDto>(entity);
            entities.Add(mappedEntity);
        }
        return entities;
    }

    public async Task<ServiceOutgoingDto> GetByIdAsync(Guid id)
    {
        var service = await _servicesRepository.GetByIdAsync(id);
        var mappedService = _mapper.Map<ServiceOutgoingDto>(service);
        return mappedService;
    }

    public async Task<ServiceMinOutgoingDto> GetMinByIdAsync(Guid id)
    {
        var service = await _servicesRepository.GetMinByIdAsync(id);
        if (service is null)
            throw new EntityNotFoundException();
        var mappedService = _mapper.Map<ServiceMinOutgoingDto>(service);
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

        await _sendEndpoint.Send(new ServiceUpdatedMessage
        {
            Id = service.Id,
            Name = service.Name
        });
    }

    public async Task ChangeStatusAsync(Guid id, string status)
    {
        var exists = await _servicesRepository.Exists(id);
        if (!exists)
            throw new EntityNotFoundException();
        await _servicesRepository.ChangeStatusAsync(id, status);
    }
}
