using TakePills.Domain;

namespace TakePills.BLL.DoctorService;

public abstract class DoctorRepository : IRepository<Doctor>
{
    public abstract Task Add(Doctor doctor);
    public abstract Task<Doctor> Get(int id);
    public abstract Task Update(Doctor doctor);
    public abstract Task Delete(int id);
}
