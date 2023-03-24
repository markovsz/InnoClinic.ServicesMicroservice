using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Dapper.SqlMapper;

namespace Infrastructure;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _configuration;

    public SqlDataAccess(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SqlConnection GetConnection()
    {
        var connectionString = _configuration
            .GetSection("ConnectionStrings")
            .GetSection("sqlConnection").Value;

        var connection = new SqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    public async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string storedProcedure, DynamicParameters parameters)
    {
        using var connection = GetConnection();
        var entities = await connection.QueryAsync<TEntity>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        return entities;
    }

    public async Task<IEntityBuilder<TEntity>> QueryComplexAsync<TEntity>(string storedProcedure, DynamicParameters parameters) where TEntity : BaseEntity
    {
        var connection = GetConnection();
        var gridReader = await connection.QueryMultipleAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        var entityBuilder = new EntityBuilder<TEntity>(gridReader);
        return entityBuilder;
    }

    public async Task ExecuteAsync(string storedProcedure, DynamicParameters parameters)
    {
        using var connection = GetConnection();
        var rowsCount = await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
