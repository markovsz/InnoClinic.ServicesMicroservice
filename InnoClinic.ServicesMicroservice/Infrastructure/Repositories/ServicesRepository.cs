using Dapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.RequestParameters;
using System.Data;
using static Dapper.SqlMapper;

namespace Infrastructure.Repositories;

public class ServicesRepository : IServicesRepository
{
    private readonly ISqlDataAccess _sqlDataAccess;
    private const string CreateService = "CreateService";
    private const string ChangeServiceStatus = "ChangeServiceStatus";
    private const string GetServices = "GetServices";
    private const string GetServiceById = "GetServiceById";
    private const string GetServiceByName = "GetServiceByName";
    private const string ServiceExists = "ServiceExists";
    private const string UpdateService = "UpdateService";
    private const string LinkWithSpecialization = "LinkWithSpecialization";

    public ServicesRepository(ISqlDataAccess sqlDataAccess)
    {
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task ChangeStatusAsync(Guid id, string status)
    {
        var parameters = new DynamicParameters();
        parameters.Add("id", id, DbType.Guid, ParameterDirection.Input);
        parameters.Add("status", status, DbType.String, ParameterDirection.Input);
        await _sqlDataAccess.ExecuteAsync(ChangeServiceStatus, parameters);
    }

    public async Task<Guid> CreateAsync(Service service)
    {
        var id = Guid.NewGuid();
        var parameters = new DynamicParameters();
        parameters.Add("id", id, DbType.Guid, ParameterDirection.Input);
        parameters.Add("name", service.Name, DbType.String, ParameterDirection.Input);
        parameters.Add("price", service.Price, DbType.String, ParameterDirection.Input);
        parameters.Add("categoryId", service.CategoryId, DbType.Guid, ParameterDirection.Input);
        parameters.Add("specializationId", service.SpecializationId, DbType.Guid, ParameterDirection.Input);
        parameters.Add("status", service.Status, DbType.String, ParameterDirection.Input);
        await _sqlDataAccess.ExecuteAsync(CreateService, parameters);
        return id;
    }

    public async Task<IEnumerable<Service>> GetAsync(ServiceParameters requestParameters)
    {
        var parameters = new DynamicParameters();
        parameters.Add("categoryName", requestParameters.CategoryName, DbType.String, ParameterDirection.Input);
        parameters.Add("specializationName", requestParameters.SpecializationName, DbType.String, ParameterDirection.Input);
        var entitiesBuilder = await _sqlDataAccess.QueryComplexAsync<Service>(GetServices, parameters);
        await entitiesBuilder.RetrieveBaseEntityAsync();
        await entitiesBuilder.JoinRelatedEntityAsync<ServiceCategory>(e => e.CategoryId, (s, sc) => new Service(s) { Category = sc });
        await entitiesBuilder.JoinRelatedEntityAsync<Specialization>(e => e.SpecializationId, (s, sp) => new Service(s) { Specialization = sp });
        var services = entitiesBuilder.GetEntities();
        return services;
    }

    public async Task<Service> GetByIdAsync(Guid id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("id", id, DbType.Guid, ParameterDirection.Input);
        using var entityBuilder = await _sqlDataAccess.QueryComplexAsync<Service>(GetServiceById, parameters);
        await entityBuilder.RetrieveBaseEntityAsync();
        await entityBuilder.JoinRelatedEntityAsync<ServiceCategory>(e => e.CategoryId, (s, sc) => new Service(s) { Category = sc });
        await entityBuilder.JoinRelatedEntityAsync<Specialization>(e => e.SpecializationId, (s, sp) => new Service(s) { Specialization = sp });
        var service = entityBuilder.GetEntities()
            .FirstOrDefault();
        return service;
    }

    public async Task<Service> GetByNameAsync(string name)
    {
        var parameters = new DynamicParameters();
        parameters.Add("name", name, DbType.Guid, ParameterDirection.Input);
        var entityBuilder = await _sqlDataAccess.QueryComplexAsync<Service>(GetServiceByName, parameters);
        await entityBuilder.RetrieveBaseEntityAsync();
        await entityBuilder.JoinRelatedEntityAsync<ServiceCategory>(e => e.CategoryId, (s, sc) => new Service(s) { Category = sc });
        var service = entityBuilder.GetEntities()
            .FirstOrDefault();
        return service;
    }

    public async Task<bool> Exists(Guid id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("id", id, DbType.Guid, ParameterDirection.Input);
        var response = await _sqlDataAccess.QueryAsync<int>(ServiceExists, parameters);
        var servicesCount = response.FirstOrDefault();
        if (servicesCount > 0) return true;
        return false;
    }

    public async Task UpdateAsync(Service service)
    {
        var parameters = new DynamicParameters();
        parameters.Add("id", service.Id, DbType.Guid, ParameterDirection.Input);
        parameters.Add("name", service.Name, DbType.String, ParameterDirection.Input);
        parameters.Add("price", service.Price, DbType.String, ParameterDirection.Input);
        parameters.Add("categoryId", service.CategoryId, DbType.Guid, ParameterDirection.Input);
        parameters.Add("specializationId", service.SpecializationId, DbType.Guid, ParameterDirection.Input);
        parameters.Add("status", service.Status, DbType.String, ParameterDirection.Input);
        await _sqlDataAccess.ExecuteAsync(UpdateService, parameters);
    }

    public async Task LinkWithSpecializationByIdAsync(Guid id, Guid specializationId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("id", id, DbType.Guid, ParameterDirection.Input);
        parameters.Add("specializationId", specializationId, DbType.Guid, ParameterDirection.Input);
        await _sqlDataAccess.ExecuteAsync(LinkWithSpecialization, parameters);
    }
}
