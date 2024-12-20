using LinqToDB;
using TakePills.BLL.DoctorService;
using TakePills.Domain;
using TakePills.Infrastructure.DAL.Configuration;

namespace TakePills.Infrastructure.DAL;

public class DbDoctorRepository : DoctorRepository
{
    private DbConnection _db = new DbConnection();


    public override Task Add(Doctor doctor)
    {
        return _db.InsertWithIdentityAsync(doctor);
    }

    public override Task Delete(int id)
    {
        return _db.Doctors.DeleteAsync(s => s.Id == id);
    }

    public override Task<Doctor> Get(int id)
    {
        return _db.Doctors.FirstOrDefaultAsync(s => s.Id == id)!;
    }

    public override Task Update(Doctor doctor)
    {
        return _db.UpdateAsync(doctor);
    }
}
