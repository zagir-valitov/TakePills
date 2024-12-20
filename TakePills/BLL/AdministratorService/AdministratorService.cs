using TakePills.Domain;

namespace TakePills.BLL.AdministratorService;

public class AdministratorService
{
    private AdministratorRepository _administratorRepository;

    public AdministratorService(AdministratorRepository administratorRepository)
    {
        _administratorRepository = administratorRepository;
    }


    // Операции с объектом администратор
    public Task AddAdministrator(Administrator administrator)
    {
        return _administratorRepository.Add(administrator);
    }
    public Task<Administrator> GetAdministratorById(int administratorId)
    {
        return _administratorRepository.Get(administratorId);
    }
    public Task UpdateAdministratorById(Administrator administratorId)
    {
        return _administratorRepository.Update(administratorId);
    }
    public Task DeleteAdministrator(int administratorId)
    {
        return _administratorRepository.Delete(administratorId);
    }
}
