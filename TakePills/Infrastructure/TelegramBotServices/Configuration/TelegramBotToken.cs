using Microsoft.Extensions.Configuration;
using TakePills.Domain;

namespace TakePills.Infrastructure.TelegramBotServices.Configuration;

public class TelegramBotToken
{
    public static string? Set()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .AddUserSecrets<Patient>()
            .Build();
        return config["TelegramBotToken"];
    }
}
