using LinqToDB.Data;
using TakePills.Infrastructure.DAL.Configuration;
using TakePills.Infrastructure.TelegramBotServices;
using TakePills.Infrastructure.TelegramBotServices.Configuration;



Console.WriteLine(" ---- Take A Pills Project ----\n\n");


DataConnection.DefaultSettings = new DbConnectionSettings();
/*
var dbAdministratorRepository = new DbAdministratorRepository();
var administratorService = new AdministratorService(dbAdministratorRepository);

var dbDoctorRepository = new DbDoctorRepository();
var doctorService = new DoctorService(dbDoctorRepository);

var dbPatientRepository = new DbPatientRepository();
var patientService = new PatientService(dbPatientRepository);

var dbRemainderRepository = new DbRemainderRepository();
var remainderService = new RemainderService(dbRemainderRepository);
*/
//var telegramBotToken = TelegramBotToken.Set();

var telegramBotService = new MainService(TelegramBotToken.Set()!);
telegramBotService.Start();

//var dbRemainderRepository = new DbRemainderRepository();
//var remainderService = new RemainderService(dbRemainderRepository);

//var remainders = dbRemainderRepository.GetRemainders().Result;

Console.WriteLine("THE END");




/*
private static long getChatId(Update update)
{
    return update.Type == UpdateType.Message ? update.Message!.Chat.Id : update.CallbackQuery.Message!.Chat.Id;
}
*/


//DateTime dateTime = DateTime.Now;
//var d = dateTime.DayOfWeek;

//;
/*
var task1 = administratorService.AddAdministrator(new Administrator()
{
    FirstName = "Amina",
    LastName = "Valitova",
    PhoneNumber = "+7-917-363-98-77",
    DateOfAddition = DateTime.Now    
});
Console.WriteLine("Administrator added...");
Console.ReadLine();

var task2 = doctorService.AddDoctor(new Doctor()
{
    FirstName = "Zaliya",
    LastName = "Valitova",
    Birthday = new DateOnly(1993, 07, 18),
    Specialization = "Neurologist",
    Qualification = Qualification.HigestCategory,
    WorkExperienceInMonths = 365,
    PhoneNumber = "+7-987-618-39-95",
    DateOfAddition = DateTime.Now
});
Console.WriteLine("Doctor added...");
Console.ReadLine();

var task3 = patientService.AddPatient(new Patient()
{
    FirstName = "Zagir",
    LastName = "Valitov",
    Birthday = new DateOnly(1985, 04, 06),
    HealthComplaints = "good",
    PhoneNumber = "+7-919-141-90-28",
    DateOfAddition = DateTime.Now
});
Console.WriteLine("Patient added...");
Console.ReadLine();
*/



//var task5 = remainderService.GetRemainders().Result;
//var _dbr = new DbConnection();

//var remainders = 
//    from r in _dbr.Remainders
//    where r.RemainderId == 1224954883
//    select r;

;