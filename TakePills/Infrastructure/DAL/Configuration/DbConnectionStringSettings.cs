using LinqToDB.Configuration;

namespace TakePills.Infrastructure.DAL.Configuration;

public class DbConnectionStringSettings : IConnectionStringSettings
{
    public string? ConnectionString { get; set; }
    public string? Name { get; set; }
    public string? ProviderName { get; set; }
    public bool IsGlobal => false;


}
