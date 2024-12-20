using TakePills.Domain;

namespace TakePills.BLL.PatientService;

public abstract class PatientRepository : IRepository<Patient>
{
    public abstract Task Add(Patient patient);
    public abstract Task<Patient> Get(int id);
    public abstract Task Update(Patient patient);
    public abstract Task Delete(int id);
}
