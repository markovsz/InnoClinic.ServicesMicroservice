using Dapper;
using Domain.Abstractions;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Repositories;

public class ServiceCategoriesRepository : IServiceCategoriesRepository
{
    private readonly ISqlDataAccess _sqlDataAccess;
    private const string GetServiceCategoryByName = "GetServiceCategoryByName";

    public ServiceCategoriesRepository(ISqlDataAccess sqlDataAccess)
    {
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<ServiceCategory> GetByNameAsync(string name)
    {
        var parameters = new DynamicParameters();
        parameters.Add("name", name, DbType.String, ParameterDirection.Input);
        var response = await _sqlDataAccess.QueryAsync<ServiceCategory>(GetServiceCategoryByName, parameters);
        var category = response.FirstOrDefault();
        return category;
    }
}
