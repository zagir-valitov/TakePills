using TakePills.BLL.ReminderService;
using TakePills.Domain;
using TakePills.Infrastructure.DAL;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TakePills.Infrastructure.DAL.Configuration;

namespace TakePills.Infrastructure.TelegramBotServices;

public class ReminderOperations
{
    private Reminder? _reminder = new Reminder();
    private static DbReminderRepository dbReminderRepository = new();
    private static ReminderService reminderService = new(dbReminderRepository);
    public bool OperationFinished;
    public State OperationState;
    public enum State : byte
    {
        ChatId,
        SelectDescription,
        SelectDay,
        SelectTimeHour,
        SelectTimeMinute,
        SelectStartDate,
        SelectEndDate,
        SaveReminder
    }    
    
    public async Task CreateReminder(ITelegramBotClient botClient, Update update)
    {
        switch (OperationState)
        {
            case State.ChatId:
                OperationFinished = false;
                _reminder = new Reminder();

                _reminder!.ReminderId = General.ChatIdFromUpdate(update);
                await botClient.SendMessage(General.ChatIdFromUpdate(update), "[ ⏰ Создаем новое напоминание ⏰ ]\n");
                await Task.Delay(1000);
                await botClient.SendMessage(General.ChatIdFromUpdate(update), "Введите описание напоминания: ");

                OperationState = State.SelectDescription;
                break;

            case State.SelectDescription:
                _reminder!.Description = update.Message!.Text;
                //Удаляем ответные сообщения
                await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.Message.MessageId - 1);
                await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.Message.MessageId);
                await botClient.SendMessage(General.ChatIdFromUpdate(update), $"Описание напоминания: {General.TextFromUpdate(update)}");
                await ChooseDay(botClient, update);

                OperationState = State.SelectDay;
                break;

            case State.SelectDay:
                if (update.Type == UpdateType.CallbackQuery)
                {
                    _reminder!.Day = update.CallbackQuery!.Data;
                    //Удаляем ответные сообщения
                    await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery.Message!.MessageId);
                    await botClient.SendMessage(General.ChatIdFromUpdate(update), $"День недели: {General.TextFromUpdate(update)}");
                    await ChooseHour(botClient, update);

                    OperationState = State.SelectTimeHour;
                }
                else
                {
                    await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.Message!.MessageId);
                }
                break;

            case State.SelectTimeHour:
                if (update.Type == UpdateType.CallbackQuery)
                {
                    _reminder!.Time = TimeSpan.Parse(General.TextFromUpdate(update) + ":00");
                    //Удаляем ответные сообщения
                    await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery!.Message!.MessageId);
                    await botClient.SendMessage(General.ChatIdFromUpdate(update), $"Время: {General.TextFromUpdate(update)} ");
                    await ChooseMinute(botClient, update);

                    OperationState = State.SelectTimeMinute;
                }
                else
                {
                    await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.Message!.MessageId);
                }
                break;

            case State.SelectTimeMinute:
                if (update.Type == UpdateType.CallbackQuery)
                {
                    var minute = TimeSpan.Parse("00:" + update.CallbackQuery?.Data);
                    _reminder!.Time = _reminder.Time.Add(minute);
                    //Удаляем ответные сообщения
                    await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery!.Message!.MessageId - 1);
                    await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery.Message.MessageId);
                    await botClient.SendMessage(General.ChatIdFromUpdate(update), $"Время: {_reminder.Time} ");
                    await ChooseExpirationDate(botClient, update);

                    OperationState = State.SelectEndDate;
                }
                else
                {
                    await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.Message.MessageId);
                }
                break;

            case State.SelectEndDate:   
                if (update.Type == UpdateType.CallbackQuery)
                {
                    switch (update.CallbackQuery!.Data)
                    {
                        case "1d":
                            _reminder!.StartDate = DateTime.Now;
                            _reminder!.ExpirationDate = DateTime.Now.AddDays(1);
                            //Удаляем ответные сообщения
                            await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery.Message!.MessageId);
                            await botClient.SendMessage(General.ChatIdFromUpdate(update), $"Срок напоминания до: {(_reminder!.ExpirationDate.Date).ToString("D")} ");
                            break;

                        case "7d":
                            _reminder!.StartDate = DateTime.Now;
                            _reminder!.ExpirationDate = DateTime.Now.AddDays(7);
                            //Удаляем ответные сообщения
                            await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery.Message!.MessageId);
                            await botClient.SendMessage(General.ChatIdFromUpdate(update), $"Срок напоминания до: {_reminder!.ExpirationDate.Date:D} ");
                            break;

                        case "1m":
                            _reminder!.StartDate = DateTime.Now;
                            _reminder!.ExpirationDate = DateTime.Now.AddMonths(1);
                            //Удаляем ответные сообщения
                            await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery.Message!.MessageId);
                            await botClient.SendMessage(General.ChatIdFromUpdate(update), $"Срок напоминания до: {_reminder!.ExpirationDate.Date:D} ");
                            break;

                        case "3m":
                            _reminder!.StartDate = DateTime.Now;
                            _reminder!.ExpirationDate = DateTime.Now.AddMonths(3);
                            //Удаляем ответные сообщения
                            await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery.Message.MessageId);
                            await botClient.SendMessage(General.ChatIdFromUpdate(update), $"Срок напоминания до: {_reminder!.ExpirationDate.Date:D} ");
                            break;

                        case "6m":
                            _reminder!.StartDate = DateTime.Now;
                            _reminder!.ExpirationDate = DateTime.Now.AddMonths(6);
                            //Удаляем ответные сообщения
                            await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery.Message!.MessageId);
                            await botClient.SendMessage(General.ChatIdFromUpdate(update), $"Срок напоминания до: {_reminder!.ExpirationDate.Date:D} ");
                            break;

                        case "1y":
                            _reminder!.StartDate = DateTime.Now;
                            _reminder!.ExpirationDate = DateTime.Now.AddYears(1);
                            //Удаляем ответные сообщения
                            await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery.Message!.MessageId);
                            await botClient.SendMessage(General.ChatIdFromUpdate(update), $"Срок напоминания до: {_reminder!.ExpirationDate.Date:D} ");
                            break;
                    }
                    OperationState = State.SaveReminder;
                    await SaveReminder(botClient, update);
                }
                else
                {
                    await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.Message!.MessageId);
                }
                break;

            case State.SaveReminder:
                if (update.Type == UpdateType.CallbackQuery)
                {
                    switch (update.CallbackQuery?.Data)
                    {
                        case "yes":
                            await reminderService.Add(_reminder);
                            //Удаляем ответные сообщения
                            await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery.Message!.MessageId);
                            await botClient.SendMessage(General.ChatIdFromUpdate(update), $"Напоминание \"{_reminder.Description!.ToUpper()}\" успешно сохранено!");
                            OperationState = 0;
                            OperationFinished = true;
                            break;

                        case "no":
                            await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.CallbackQuery.Message!.MessageId);
                            await botClient.SendMessage(General.ChatIdFromUpdate(update), "❗️ Напоминание не сохранено ❗️");
                            OperationState = 0;
                            OperationFinished = true;
                            break;
                    }
                    await botClient.SendMessage(General.ChatIdFromUpdate(update), "👇🏻 Нажми кнопку \"Меню\"");
                }
                else
                {
                    await botClient.DeleteMessage(General.ChatIdFromUpdate(update), update.Message!.MessageId);
                }
                break;
        }
    }

    private static async Task SaveReminder(ITelegramBotClient botClient, Update update)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("✍️ СОХРАНИТЬ", "yes"),
                InlineKeyboardButton.WithCallbackData("❌   ОТМЕНА", "no"),
            },
            });
        await botClient.SendMessage(General.ChatIdFromUpdate(update), "Сохранить напоминание?", replyMarkup: inlineKeyboard);
    }

    private static async Task ChooseDay(ITelegramBotClient botClient, Update update)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[Пн]", "Понедельник"),
                InlineKeyboardButton.WithCallbackData("[Вт]", "Вторник"),
                InlineKeyboardButton.WithCallbackData("[Ср]", "Среда"),
                InlineKeyboardButton.WithCallbackData("[Чт]", "Четверг"),
                InlineKeyboardButton.WithCallbackData("[Пт]", "Пятница"),
                InlineKeyboardButton.WithCallbackData("[Сб]", "Суббота"),
                InlineKeyboardButton.WithCallbackData("[Вс]", "Воскресенье"),
            },
            });
        await botClient.SendMessage(General.ChatIdFromUpdate(update), "Выберите день недели:", replyMarkup: inlineKeyboard);
    }

    private static async Task ChooseHour(ITelegramBotClient botClient, Update update)       
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[ 0 ]",     "0"),
                InlineKeyboardButton.WithCallbackData("[ 1 ]",     "1"),
                InlineKeyboardButton.WithCallbackData("[ 2 ]",     "2"),
                InlineKeyboardButton.WithCallbackData("[ 3 ]",     "3"),
                InlineKeyboardButton.WithCallbackData("[ 4 ]",     "4"),
                InlineKeyboardButton.WithCallbackData("[ 5 ]",     "5")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[ 6 ]",     "6"),
                InlineKeyboardButton.WithCallbackData("[ 7 ]",     "7"),
                InlineKeyboardButton.WithCallbackData("[ 8 ]",     "8"),
                InlineKeyboardButton.WithCallbackData("[ 9 ]",     "9"),
                InlineKeyboardButton.WithCallbackData("[ 10 ]",    "10"),
                InlineKeyboardButton.WithCallbackData("[ 11 ]",    "11")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[ 12 ]",    "12"),
                InlineKeyboardButton.WithCallbackData("[ 13 ]",    "13"),
                InlineKeyboardButton.WithCallbackData("[ 14 ]",    "14"),
                InlineKeyboardButton.WithCallbackData("[ 15 ]",    "15"),
                InlineKeyboardButton.WithCallbackData("[ 16 ]",    "16"),
                InlineKeyboardButton.WithCallbackData("[ 17 ]",    "17")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[ 18 ]",    "18"),
                InlineKeyboardButton.WithCallbackData("[ 19 ]",    "19"),
                InlineKeyboardButton.WithCallbackData("[ 20 ]",    "20"),
                InlineKeyboardButton.WithCallbackData("[ 21 ]",    "21"),
                InlineKeyboardButton.WithCallbackData("[ 22 ]",    "22"),
                InlineKeyboardButton.WithCallbackData("[ 23 ]",    "23")
            }
            });
        await botClient.SendMessage(General.ChatIdFromUpdate(update), "Выберите Часы:", replyMarkup: inlineKeyboard);
    }

    private static async Task ChooseMinute(ITelegramBotClient botClient, Update update)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[ 0 ]",     "0"),
                InlineKeyboardButton.WithCallbackData("[ 5 ]",     "5"),
                InlineKeyboardButton.WithCallbackData("[ 10 ]",    "10"),
                InlineKeyboardButton.WithCallbackData("[ 15 ]",    "15"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[ 20 ]",    "20"),
                InlineKeyboardButton.WithCallbackData("[ 25 ]",    "25"),
                InlineKeyboardButton.WithCallbackData("[ 30 ]",    "30"),
                InlineKeyboardButton.WithCallbackData("[ 35 ]",    "35"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[ 40 ]",    "40"),
                InlineKeyboardButton.WithCallbackData("[ 45 ]",    "45"),
                InlineKeyboardButton.WithCallbackData("[ 50 ]",    "50"),
                InlineKeyboardButton.WithCallbackData("[ 55 ]",    "55"),
            }
            });
        await botClient.SendMessage(General.ChatIdFromUpdate(update), "Выберите Минуты:", replyMarkup: inlineKeyboard);
    }

    private static async Task ChooseExpirationDate(ITelegramBotClient botClient, Update update)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[Сутки]",        "1d"),
                InlineKeyboardButton.WithCallbackData("[Неделя]",       "7d"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[Месяц]",        "1m"),
                InlineKeyboardButton.WithCallbackData("[Квартал]",      "3m"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[Полугодие]",    "6m"),
                InlineKeyboardButton.WithCallbackData("[Год]",          "1y"),
            },
            });
        await botClient.SendMessage(General.ChatIdFromUpdate(update), "Выберите срок действия напоминания:", replyMarkup: inlineKeyboard);
    }

    public static async Task ShowListReminders(ITelegramBotClient botClient, Update update)
    {
        var myRemaindersList = GetMyReminders(General.ChatIdFromUpdate(update));
        int count = 1;
        foreach(var r in myRemaindersList)
        {
            await botClient.SendMessage(General.ChatIdFromUpdate(update),
            $"<tg-emoji emoji-id=\"5368324170671202286\">⏰</tg-emoji>\r\n" +
            $"<b>{count}</b>. <b>{r.Description}</b>\r\n" +
            $"<a>    День</a>:<a>  {r.Day}</a>\r\n" +
            $"<a>    Время</a>:<a> {r.Time}</a>\r\n" +
            $"<a>    Срок</a>:<a>  {r.ExpirationDate.ToLongDateString()}</a>\r\n", parseMode: ParseMode.Html);
            count++;
        }        
    }

    public static List<Reminder> GetMyReminders(long chatId)
    {
        var db = new DbConnection();
        var reminders = from r in db.Reminders
                         where r.ReminderId == chatId
                         orderby r.ExpirationDate
                         select r;
        return reminders.ToList();
    }
}
