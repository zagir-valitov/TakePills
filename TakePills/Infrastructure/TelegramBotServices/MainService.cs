using TakePills.BLL.ReminderService;
using TakePills.Domain;
using TakePills.Infrastructure.DAL;
using TakePills.Infrastructure.DAL.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;



namespace TakePills.Infrastructure.TelegramBotServices;

public class MainService
{
    private static ITelegramBotClient? _botClient;
    private static ReceiverOptions? _receiverOptions;
    private static CancellationTokenSource _cts = new();

    private DbReminderRepository _dbReminderRepository;
    private ReminderService _reminderService;

    private static object _sync = new();
    private static Dictionary<long, ReminderOperations>? _reminderOperation;

    private static List<Reminder> _reminders = [];
    public MainService(string token) => _botClient = new TelegramBotClient(token);
    public void Start()
    {
        Thread myThread1 = new Thread(Notification);
        myThread1.Start();

        using (_cts)
        {
            var me = _botClient!.GetMe().Result;
            Console.WriteLine($"{me.FirstName} STARTED!");

            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [],
                DropPendingUpdates = true
            };

            _botClient!.StartReceiving(UpdateHandlerAsync, ErrorHandlerAsync, _receiverOptions, _cts.Token);

            while (true)
            {
                Console.WriteLine("\nExit the service, push the button 'x'");
                if (Console.ReadKey().KeyChar == 'x')
                {
                    _cts.Cancel();
                    Console.WriteLine($"\n{me.FirstName} STOPPED!");
                    Console.WriteLine("\nExit\n");
                    break;
                }
            }
        }                
    }

    private void Notification()
    {
        bool trigger = true;
        while (trigger) 
        {
            GetReminders();
            var currentDateTime = DateTime.Now;
            foreach (var reminder in _reminders)
            {
                if ((ConvertDayToDayOfWeek(reminder.Day!) == currentDateTime.DayOfWeek) 
                    && (reminder.Time.Hours == currentDateTime.Hour) 
                    && (currentDateTime.Minute == reminder.Time.Minutes) 
                    && (currentDateTime.Second - reminder.Time.Seconds < 10))
                {
                    Console.WriteLine($"Remaind for {reminder.ReminderId}!!!");
                    _botClient!.SendMessage(reminder.ReminderId, $"Напоминание: {reminder.Description}!!!");
                    _botClient!.SendMessage(reminder.ReminderId, $"Напоминание: {reminder.Description}!!!");
                    _botClient!.SendMessage(reminder.ReminderId, $"Напоминание: {reminder.Description}!!!");
                    _botClient!.SendMessage(reminder.ReminderId, $"Напоминание: {reminder.Description}!!!");
                    _botClient!.SendMessage(reminder.ReminderId, $"Напоминание: {reminder.Description}!!!");
                    _botClient!.SendMessage(reminder.ReminderId, $"Напоминание: {reminder.Description}!!!");
                    trigger = false;
                }
            }
            Task.Delay(1000);
        }        
    }

    private static DayOfWeek ConvertDayToDayOfWeek(string day)
    {
        switch (day)
        {
            case "Понедельник":
                return DayOfWeek.Monday;

            case "Вторник":
                return DayOfWeek.Tuesday;

            case "Среда":
                return DayOfWeek.Wednesday;

            case "Четверг":
                return DayOfWeek.Thursday;

            case "Пятница":
                return DayOfWeek.Friday;

            case "Суббота":
                return DayOfWeek.Saturday;

            case "Воскресенье":
                return DayOfWeek.Sunday;

            default:
                return 0;
                
        }
    }    

    private static void GetReminders()
    {
        _reminders.Clear();
        var db = new DbConnection();
        var reminders = from r in db.Reminders
                         orderby r.ExpirationDate
                         select r;
        foreach (var rema in reminders)
        {
            _reminders.Add(rema);  
        }
    }

    private Task ErrorHandlerAsync(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    private async Task UpdateHandlerAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {        
        try
        {
            lock(_sync)
            {
                TelegramBotMessages.AddMessage(General.MessageFromUpdate(update)!);
            }
            switch (update.Type)
            {
                case UpdateType.Message:                    
                    await BotOnMessageReceived(botClient, update);
                    
                    break;

                case UpdateType.CallbackQuery:
                    await BotOnMessageReceived(botClient, update);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
        
    private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Update update)
    {
        Console.WriteLine($"Receive message from {General.MessageFromUpdate(update)!.Chat.FirstName}\t{General.TextFromUpdate(update)}");
        
        switch (General.TextFromUpdate(update))
        {
            case "/start":
                await Greeting(botClient, update.Message!);
                break;

            case "/myreminders":
                
                await ReminderOperations.ShowListReminders(botClient, update);
                //GetReminders();

                break;

            case "/createreminder":
                if (_reminderOperation is null)
                {
                    _reminderOperation = new();
                }
                bool operationExist = false;
                lock (_sync)
                {
                    operationExist = _reminderOperation.ContainsKey(General.ChatIdFromUpdate(update));
                }
                if (operationExist && !_reminderOperation[General.ChatIdFromUpdate(update)].OperationFinished)
                {
                    await _reminderOperation[General.ChatIdFromUpdate(update)].CreateReminder(botClient, update);
                }
                else
                {
                    if (_reminderOperation is null)
                    {
                        _reminderOperation = new();
                    }
                    var operations = new ReminderOperations();
                    _reminderOperation[General.ChatIdFromUpdate(update)] = operations;
                    await _reminderOperation[General.ChatIdFromUpdate(update)].CreateReminder(botClient, update);
                }                       
                break;

            default:
                if (_reminderOperation != null 
                    && _reminderOperation.ContainsKey(General.ChatIdFromUpdate(update)) 
                    && !_reminderOperation[General.ChatIdFromUpdate(update)].OperationFinished)
                {
                    operationExist = false;
                    lock (_sync)
                    {
                        operationExist = _reminderOperation.ContainsKey(General.ChatIdFromUpdate(update));
                    }
                    if (operationExist)
                    {
                        await _reminderOperation[General.ChatIdFromUpdate(update)].CreateReminder(botClient, update);
                    }
                }
                else
                {
                    await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.Message!.MessageId);
                    //await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.Message!.MessageId - 1);
                    await botClient.SendMessage(General.ChatIdFromUpdate(update), "👇🏻 Нажми кнопку \"Меню\"");
                }
                break;
        }
        return;
    }   
    private static async Task Greeting(ITelegramBotClient botClient, Message message)
    {
        await botClient.SendMessage(message.Chat.Id, $"Привет, {message.From!.FirstName} {message.From.LastName}!\n👇🏻 Нажми кнопку \"Меню\"");
        
    }   
}
