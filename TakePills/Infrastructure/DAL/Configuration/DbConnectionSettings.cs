using LinqToDB;
using LinqToDB.Configuration;
using Microsoft.Extensions.Configuration;
using TakePills.Domain;

namespace TakePills.Infrastructure.DAL.Configuration;

public class DbConnectionSettings : ILinqToDBSettings
{
    public IEnumerable<IDataProviderSettings> DataProviders
        => Enumerable.Empty<IDataProviderSettings>();

    public string DefaultConfiguration => "PostgreSQL";
    public string DefaultDataProvider => "PostgreSQL";

    public IEnumerable<IConnectionStringSettings> ConnectionStrings
    {
        get
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .AddUserSecrets<Patient>()
                .Build();
            var connectionString = config["ConnectionStringPostgreSQL"];

            yield return
                new DbConnectionStringSettings
                {
                    Name = "PostgresTakePillDB",
                    ProviderName = ProviderName.PostgreSQL,
                    ConnectionString = connectionString
                };
        }
    }
}
