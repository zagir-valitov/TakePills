namespace TakePills.BLL.PatientService;

public class PatientEventArgs
{
    public string? Message { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public PatientEventArgs(string? message, string firstName, string lastName)
    {
        Message = message;
        FirstName = firstName;
        LastName = lastName;
    }
}
