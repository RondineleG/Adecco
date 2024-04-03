using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System.Data;

namespace Adecco.Persistence.Contexts;

public class DapperDataContext : DbContext
{
    public string _connectionString;

    public DapperDataContext(IConfiguration configuration)
    {
        Configuration = configuration;
        _connectionString = Configuration.GetConnectionString("ConnectionString")!;
    }

    public IConfiguration Configuration { get; }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}