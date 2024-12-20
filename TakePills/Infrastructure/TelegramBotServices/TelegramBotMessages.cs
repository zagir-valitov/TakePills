
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TakePills.Infrastructure.TelegramBotServices;

public class TelegramBotMessages
{
    private static List<Message> _messageList = [];
    private static int _countMessages = 0;
    public static void AddMessage(Message message)
    {
        _messageList.Add(message);
        _countMessages++;
        ;
    }

    public static void RemoveRangeMessageFromChatId(ITelegramBotClient botClient, Update update, int index, int count)
    {
        int startIndexFromMessageId = _messageList.Count - 1;
        int endIndexFromMessageId = 0;
        int currentPos = 0;
        for (int i = _messageList.Count - 1; i >=0; i--)
        {
            if (_messageList[i].Chat.Id == General.ChatIdFromUpdate(update))
            {
                if (currentPos != index)
                {
                    currentPos++;
                }
                else
                {
                    startIndexFromMessageId = i;
                }
            }
        }
        for (int i = startIndexFromMessageId; i >= 0 && endIndexFromMessageId <= count; i--)
        {
            if (_messageList[i].Chat.Id == General.ChatIdFromUpdate(update))
            {
                _countMessages--;
                endIndexFromMessageId++;
                botClient.DeleteMessage(General.ChatIdFromUpdate(update), _messageList[i].MessageId);
                _messageList.RemoveAt(i);
            }
        }
    }

    public static int RemoveLatestMessageFromChatId(ITelegramBotClient botClient, Update update, int count)
    {
        int deletedMessage = 0;
        for (int i = _messageList.Count - 1; i >= 0 && deletedMessage < count; i--)
        {
            if (_messageList[i].Chat.Id == General.ChatIdFromUpdate(update))
            {
                botClient.DeleteMessage(General.ChatIdFromUpdate(update), _messageList[i].MessageId);
                _messageList.RemoveAt(i);
                _countMessages--;
                deletedMessage++;
            }
        } 
        return deletedMessage;
    }

    public static string? RemoveMessageFromChatId(ITelegramBotClient botClient, Update update, int index)
    {
        string? deletedMessage = null;
        int position = 0;
        for (int i = _messageList.Count - 1; i >= 0; i--)
        {
            if (_messageList[i].Chat.Id == General.ChatIdFromUpdate(update))
            {
                position++;
                if (position == index)
                {
                    botClient.DeleteMessage(General.ChatIdFromUpdate(update), _messageList[i].MessageId);
                    _messageList.RemoveAt(i);
                    _countMessages--;
                    deletedMessage = General.TextFromUpdate(update)!;
                    break;  
                }                
            }
        }
        return deletedMessage;
    }


}
