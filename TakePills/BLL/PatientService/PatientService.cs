using TakePills.Domain;

namespace TakePills.BLL.PatientService;

public class PatientService
{
    private PatientRepository _patientRepository;

    public PatientService(PatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }


    // Операции с объектом пациент
    public Task AddPatient(Patient patient)
    {
        return _patientRepository.Add(patient);
    }
    public Task<Patient> GetPatientById(int patientId)
    {
        return _patientRepository.Get(patientId);
    }
    public Task UpdatePatientById(Patient patientId)
    {
        return _patientRepository.Update(patientId);
    }
    public Task DeletePatient(int patientId)
    {
        return _patientRepository.Delete(patientId);
    }
}
