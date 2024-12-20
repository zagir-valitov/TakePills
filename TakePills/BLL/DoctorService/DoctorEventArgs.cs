namespace TakePills.BLL.DoctorService;

public class DoctorEventArgs
{
    public string? Message { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public DoctorEventArgs(string? message, string firstName, string lastName)
    {
        Message = message;
        FirstName = firstName;
        LastName = lastName;
    }
}
