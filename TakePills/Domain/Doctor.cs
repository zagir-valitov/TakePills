using LinqToDB.Mapping;

namespace TakePills.Domain;

public enum Qualification
{
    SecondCategory,
    FirstCategory,
    HigestCategory
}

[Table("doctors")]
public class Doctor
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


    [Column("specialization"), NotNull]
    public string? Specialization { get; set; }


    [Column("qualification")]
    public Qualification Qualification { get; set; }


    [Column("workexperience_in_months")]
    public short WorkExperienceInMonths { get; set; }


    [Column("phone_number"), NotNull]
    public string? PhoneNumber { get; set; }


    [Column("date_of_addition")]
    public DateTime DateOfAddition { get; set; }


    [Column("remainder_id")]
    public int RemainderId { get; set; }
}
