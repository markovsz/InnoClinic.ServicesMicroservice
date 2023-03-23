using Dapper;
using Domain.Abstractions;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Repositories;

public class SpecializationsRepository : ISpecializationsRepository
{
    private readonly ISqlDataAccess _sqlDataAccess;
    private const string CreateSpecialization = "CreateSpecialization";
    private const string GetSpecializationById = "GetSpecializationById";
    private const string GetSpecializationByName = "GetSpecializationByName";
    private const string SpecializationExists = "SpecializationExists";
    private const string GetSpecializations = "GetSpecializations";
    private const string UpdateSpecialization = "UpdateSpecialization";
    private const string ChangeSpecializationStatus = "ChangeSpecializationStatus";

    public SpecializationsRepository(ISqlDataAccess sqlDataAccess)
    {
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task ChangeStatusAsync(Guid id, string status)
    {
        var parameters = new DynamicParameters();
        parameters.Add("id", id, DbType.Guid, ParameterDirection.Input);
        parameters.Add("status", status, DbType.String, ParameterDirection.Input);
        await _sqlDataAccess.ExecuteAsync(ChangeSpecializationStatus, parameters);
    }

    public async Task<Guid> CreateAsync(Specialization specialization)
    {
        var id = Guid.NewGuid();
        var parameters = new DynamicParameters();
        parameters.Add("id", id, DbType.Guid, ParameterDirection.Input);
        parameters.Add("name", specialization.Name, DbType.String, ParameterDirection.Input);
        parameters.Add("status", specialization.Status, DbType.String, ParameterDirection.Input);
        await _sqlDataAccess.ExecuteAsync(CreateSpecialization, parameters);
        return id;
    }

    public async Task<IEnumerable<Specialization>> GetAsync()
    {
        var parameters = new DynamicParameters();
        var specializations = await _sqlDataAccess.QueryAsync<Specialization>(GetSpecializations, parameters);
        return specializations;
    }

    public async Task<Specialization> GetByIdAsync(Guid id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("id", id, DbType.Guid, ParameterDirection.Input);
        var specialization = await _sqlDataAccess.QueryAsync<Specialization>(GetSpecializationById, parameters);
        return specialization.FirstOrDefault();
    }

    public async Task<Specialization> GetByNameAsync(string name)
    {
        var parameters = new DynamicParameters();
        parameters.Add("name", name, DbType.String, ParameterDirection.Input);
        var specializations = await _sqlDataAccess.QueryAsync<Specialization>(GetSpecializationByName, parameters);
        return specializations.FirstOrDefault();
    }

    public async Task<bool> Exists(Guid id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("id", id, DbType.Guid, ParameterDirection.Input);
        var response = await _sqlDataAccess.QueryAsync<int>(SpecializationExists, parameters);
        var specializationsCount = response.FirstOrDefault();
        if (specializationsCount > 0) return true;
        return false;
    }

    public async Task UpdateAsync(Specialization specialization)
    {
        var parameters = new DynamicParameters();
        parameters.Add("id", specialization.Id, DbType.Guid, ParameterDirection.Input);
        parameters.Add("name", specialization.Name, DbType.String, ParameterDirection.Input);
        parameters.Add("status", specialization.Status, DbType.String, ParameterDirection.Input);
        await _sqlDataAccess.ExecuteAsync(UpdateSpecialization, parameters);
    }
}
