using LinqToDB;
using TakePills.Domain;
using TakePills.BLL.AdministratorService;
using TakePills.Infrastructure.DAL.Configuration;

namespace TakePills.Infrastructure.DAL;

public class DbAdministratorRepository : AdministratorRepository
{
    private DbConnection _db = new DbConnection();


    public override Task Add(Administrator administrator)
    {
        return _db.InsertWithIdentityAsync(administrator);
    }

    public override Task Delete(int id)
    {
        return _db.Administrators.DeleteAsync(s => s.Id == id);
    }

    public override Task<Administrator> Get(int id)
    {
        return _db.Administrators.FirstOrDefaultAsync(s => s.Id == id)!;
    }

    public override Task Update(Administrator administrator)
    {
        return _db.UpdateAsync(administrator);
    }
}
