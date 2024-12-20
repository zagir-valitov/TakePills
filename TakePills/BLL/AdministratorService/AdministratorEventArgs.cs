namespace TakePills.BLL.AdministratorService;

public class AdministratorEventArgs
{
    public string? Message { get; }
    public string? FirstName { get; }    
    public string? LastName { get; }    
    public AdministratorEventArgs(string? message, string? firstName, string? lastName)
    {
        Message = message;
        FirstName = firstName;
        LastName = lastName;
    }
}
