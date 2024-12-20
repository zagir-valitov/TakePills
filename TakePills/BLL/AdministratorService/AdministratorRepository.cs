using TakePills.Domain;

namespace TakePills.BLL.AdministratorService;

public abstract class AdministratorRepository : IRepository<Administrator>
{
    public abstract Task Add(Administrator administrator);
    public abstract Task<Administrator> Get(int id);
    public abstract Task Update(Administrator administrator);
    public abstract Task Delete(int id);
}
