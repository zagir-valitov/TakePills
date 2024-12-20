using LinqToDB;
using LinqToDB.Data;
using TakePills.Domain;

namespace TakePills.Infrastructure.DAL.Configuration;

public class DbConnection : DataConnection
{
    public DbConnection() : base("PostgresTakePillDB")
    {
        (this as IDataContext).CloseAfterUse = true;
    }

    public ITable<Administrator> Administrators => this.GetTable<Administrator>();
    public ITable<Doctor> Doctors => this.GetTable<Doctor>();
    public ITable<Patient> Patients => this.GetTable<Patient>();
    public ITable<Reminder> Reminders => this.GetTable<Reminder>();
}
