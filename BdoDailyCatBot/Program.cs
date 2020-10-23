    using System;

namespace BdoDailyCatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            MainBot.Bot bot = new MainBot.Bot();
            bot.Run(Secrets.botToken).GetAwaiter().GetResult();

            Views.Console.ConsoleView consoleView = new Views.Console.ConsoleView();
            Views.Discord.DiscordChannelView discordChannelView = new Views.Discord.DiscordChannelView(bot);
            BusinessLogic.Services.ConsoleService consoleService = new BusinessLogic.Services.ConsoleService(consoleView, discordChannelView, bot);

            consoleView.RunConsoleListner();
        }
    }
}
