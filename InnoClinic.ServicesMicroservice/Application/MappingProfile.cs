using Application.DTOs.Incoming;
using Application.DTOs.Outgoing;
using AutoMapper;
using Domain.Entities;

namespace Application;

public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<ServiceIncomingDto, Service>();
        CreateMap<Service, ServiceOutgoingDto>();
        CreateMap<Service, ServiceMinOutgoingDto>();
        CreateMap<SpecializationIncomingDto, Specialization>();
        CreateMap<Specialization, SpecializationOutgoingDto>();
        CreateMap<Specialization, SpecializationMinOutgoingDto>();
        CreateMap<ServiceCategory, ServiceCategoryOutgoingDto>();
    }
}
