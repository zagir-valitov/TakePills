using TakePills.Domain;
using TakePills.Infrastructure.DAL;

namespace TakePills.BLL.ReminderService;

public class ReminderService
{
    private ReminderRepository _reminderRepository;

    public ReminderService(ReminderRepository reminderRepository)
    {
        _reminderRepository = reminderRepository;
    }

    // Операции с объектом напоминание
    public Task Add(Reminder reminder)
    {
        return _reminderRepository.Add(reminder);
    }
    public Task<Reminder> GetByReminderId(int reminderId)
    {
        return _reminderRepository.Get(reminderId);
    }
    public Task UpdateByRemainderId(Reminder reminderId)
    {
        return _reminderRepository.Update(reminderId);
    }
    public Task Delete(int reminderId)
    {
        return _reminderRepository.Delete(reminderId);
    }
    
    public Task <List<Reminder>> GetReminders()
    {
        return _reminderRepository.GetReminders();
    }
}
