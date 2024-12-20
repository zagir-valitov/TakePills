using TakePills.Domain;
using TakePills.Infrastructure.DAL.Configuration;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TakePills.Infrastructure.TelegramBotServices
{
    public class General
    {
        public static long ChatIdFromUpdate(Update update)
        {
            return update.Type == UpdateType.Message ? update.Message!.Chat.Id : update.CallbackQuery!.Message!.Chat.Id;
        }
        public static string? TextFromUpdate(Update update)
        {
            return update.Type == UpdateType.Message ? update.Message!.Text : update.CallbackQuery!.Data;
        }
        public static MessageId MessageIdFromUpdate(Update update)
        {
            return update.Type == UpdateType.Message ? update.Message!.MessageId : update.CallbackQuery!.Message!.MessageId;
        }

        public static Message? MessageFromUpdate(Update update)
        {
            return update.Type == UpdateType.Message ? update.Message! : update.CallbackQuery!.Message;
        }        
    }
}
