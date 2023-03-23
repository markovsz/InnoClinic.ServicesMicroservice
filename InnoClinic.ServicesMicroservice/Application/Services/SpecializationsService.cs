using Application.Abstractions;
using Application.DTOs.Incoming;
using Application.DTOs.Outgoing;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.RequestParameters;

namespace Application.Services;

public class SpecializationsService : ISpecializationsService
{
    private ISpecializationsRepository _specializationsRepository;
    private IServicesRepository _servicesRepository;
    private IMapper _mapper;

    public SpecializationsService(ISpecializationsRepository specializationsRepository, IServicesRepository servicesRepository, IMapper mapper)
    {
        _specializationsRepository = specializationsRepository;
        _servicesRepository = servicesRepository;
        _mapper = mapper;
    }

    public async Task ChangeStatusAsync(Guid id, string status)
    {
        var entity = await _specializationsRepository.GetByIdAsync(id);
        if (entity is null)
            throw new EntityNotFoundException();
        await _specializationsRepository.ChangeStatusAsync(id, status);
    }

    public async Task<Guid> CreateAsync(SpecializationIncomingDto incomingDto)
    {
        var mappedEntity = _mapper.Map<Specialization>(incomingDto);
        var specializationId = await _specializationsRepository.CreateAsync(mappedEntity);

        foreach (var serviceId in incomingDto.ServiceIds)
        {
            var exists = await _servicesRepository.Exists(serviceId);
            if (!exists)
                throw new EntityNotFoundException($"there is no service with id = '{serviceId}'");
            await _servicesRepository.LinkWithSpecializationByIdAsync(serviceId, specializationId);
        }

        return specializationId;
    }

    public async Task<IEnumerable<SpecializationMinOutgoingDto>> GetAsync()
    {
        var entity = await _specializationsRepository.GetAsync();
        var mappedEntity = _mapper.Map<IEnumerable<SpecializationMinOutgoingDto>>(entity);
        return mappedEntity;
    }

    public async Task<SpecializationOutgoingDto> GetByIdAsync(Guid id)
    {
        var entity = await _specializationsRepository.GetByIdAsync(id);
        var mappedEntity = _mapper.Map<SpecializationOutgoingDto>(entity);
        var services = await _servicesRepository.GetAsync(new ServiceParameters { CategoryName = null, SpecializationName = entity.Name });
        var mappedServices = _mapper.Map<IEnumerable<ServiceMinOutgoingDto>>(services);
        mappedEntity.Services = mappedServices;
        return mappedEntity;
    }

    public async Task<SpecializationOutgoingDto> GetByNameAsync(string name)
    {
        var entity = await _specializationsRepository.GetByNameAsync(name);
        var mappedEntity = _mapper.Map<SpecializationOutgoingDto>(entity);
        return mappedEntity;
    }

    public async Task UpdateAsync(Guid id, SpecializationIncomingDto incomingDto)
    {
        var mappedEntity = _mapper.Map<Specialization>(incomingDto);

        mappedEntity.Id = id;
        await _specializationsRepository.UpdateAsync(mappedEntity);

        foreach (var serviceId in incomingDto.ServiceIds)
        {
            var exists = await _servicesRepository.Exists(serviceId);
            if (!exists)
                throw new EntityNotFoundException($"there is no service with id = '{serviceId}'");
            await _servicesRepository.LinkWithSpecializationByIdAsync(serviceId, id);
        }

    }
}
