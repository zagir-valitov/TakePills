using LinqToDB.Mapping;
using TakePills.BLL.ReminderService;
using TakePills.Infrastructure.TelegramBotServices;

namespace TakePills.Domain;

[Table("reminders")]
public class Reminder
{
    [Column("id")]
    [PrimaryKey, Identity]
    public int Id { get; set; }


    [Column("reminder_id")]
    public long ReminderId { get; set; }


    [Column("description")]
    public string? Description { get; set; }


    [Column("day")]
    public string? Day { get; set; }


    [Column("time")]
    public TimeSpan Time { get; set; }


    [Column("start_date")]
    public DateTime StartDate { get; set; }


    [Column("expiration_date")]
    public DateTime ExpirationDate { get; set; }
  
}


