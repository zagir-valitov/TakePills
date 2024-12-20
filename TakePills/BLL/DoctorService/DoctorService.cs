using TakePills.Domain;

namespace TakePills.BLL.DoctorService;

public class DoctorService
{
    private DoctorRepository _doctorRepository;

    public DoctorService(DoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }


    // Операции с объектом доктор
    public Task AddDoctor(Doctor doctor)
    {
        return _doctorRepository.Add(doctor);
    }
    public Task<Doctor> GetDoctorById(int doctorId)
    {
        return _doctorRepository.Get(doctorId);
    }
    public Task UpdateDoctorById(Doctor doctorId)
    {
        return _doctorRepository.Update(doctorId);
    }
    public Task DeleteDoctor(int doctorId)
    {
        return _doctorRepository.Delete(doctorId);
    }
}
