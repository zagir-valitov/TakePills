using LinqToDB;
using TakePills.BLL.ReminderService;
using TakePills.Domain;
using TakePills.Infrastructure.DAL.Configuration;

namespace TakePills.Infrastructure.DAL;

public class DbReminderRepository : ReminderRepository
{
    private DbConnection _db = new DbConnection();


    public override Task Add(Reminder reminder)
    {
        return _db.InsertWithIdentityAsync(reminder);
    }

    public override Task Delete(int id)
    {
        return _db.Reminders.DeleteAsync(s => s.Id == id);
    }

    public override Task<Reminder> Get(int id)
    {
        return _db.Reminders.FirstOrDefaultAsync(s => s.Id == id)!;
    }

    public override Task<List<Reminder>> GetReminders()
    {
        return _db.Reminders.ToListAsync();
    }

    public override Task Update(Reminder reminder)
    {
        return _db.UpdateAsync(reminder);
    }

    
   
}
