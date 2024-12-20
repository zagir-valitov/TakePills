using System;
using System.Collections.Generic;
using TakePills.Domain;

namespace TakePills.BLL.ReminderService;

public abstract class ReminderRepository : IRepository<Reminder>
{
    public abstract Task Add(Reminder reminder);
    public abstract Task Delete(int id);
    public abstract Task<Reminder> Get(int id);
    public abstract Task Update(Reminder reminder);
    public abstract Task <List<Reminder>> GetReminders();
}
