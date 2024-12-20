using LinqToDB.Mapping;

namespace TakePills.Domain;

[Table("patients")]
public class Patient
{
    [Column("id")]
    [PrimaryKey, Identity]
    public int Id { get; set; }


    [Column("first_name"), NotNull]
    public string? FirstName { get; set; }


    [Column("last_name"), NotNull]
    public string? LastName { get; set; }


    [Column("birthday")]
    public DateOnly Birthday { get; set; }


    [Column("health_complaints"), NotNull]
    public string? HealthComplaints { get; set; }


    [Column("phone_number"), NotNull]
    public string? PhoneNumber { get; set; }


    [Column("date_of_addition")]
    public DateTime DateOfAddition { get; set; }


    [Column("remainder_id")]
    public int RemainderId { get; set; }
}
