﻿using AutoMapper;
using Domain.Entities;
using InnoClinic.SharedModels.DTOs.Services.Incoming;
using InnoClinic.SharedModels.DTOs.Services.Outgoing;

namespace Application;

public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<ServiceIncomingDto, Service>()
            .ForMember(e => e.Category, opt => opt.Ignore());
        CreateMap<AddServiceIncomingDto, Service>()
            .ForMember(e => e.Category, opt => opt.Ignore());
        CreateMap<Service, ServiceOutgoingDto>();
        CreateMap<Service, ServiceMinOutgoingDto>();
        CreateMap<SpecializationIncomingDto, Specialization>();
        CreateMap<UpdateSpecializationIncomingDto, Specialization>();
        CreateMap<Specialization, SpecializationOutgoingDto>();
        CreateMap<Specialization, SpecializationMinOutgoingDto>();
        CreateMap<ServiceCategory, ServiceCategoryOutgoingDto>();
    }
}
