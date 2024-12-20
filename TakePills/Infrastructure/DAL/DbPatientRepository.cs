using LinqToDB;
using TakePills.BLL.PatientService;
using TakePills.Domain;
using TakePills.Infrastructure.DAL.Configuration;

namespace TakePills.Infrastructure.DAL;

public class DbPatientRepository : PatientRepository
{
    private DbConnection _db = new DbConnection();


    public override Task Add(Patient patient)
    {
        return _db.InsertWithIdentityAsync(patient);
    }

    public override Task Delete(int id)
    {
        return _db.Patients.DeleteAsync(s => s.Id == id);
    }

    public override Task<Patient> Get(int id)
    {
        return _db.Patients.FirstOrDefaultAsync(s => s.Id == id)!;
    }

    public override Task Update(Patient patient)
    {
        return _db.UpdateAsync(patient);
    }
}
