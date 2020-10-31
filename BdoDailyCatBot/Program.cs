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
            Views.Discord.DiscordChannelView discordChannelView = new Views.Discord.DiscordChannelView(bot, Resource.Prefix);
            DataAccess.Repositories.ChannelsRepository channelsRepository = new DataAccess.Repositories.ChannelsRepository("Channels");
            DataAccess.Repositories.EFUnitOfWork dataBase = new DataAccess.Repositories.EFUnitOfWork();
            BusinessLogic.Services.ConsoleService consoleService = new BusinessLogic.Services.ConsoleService
                (consoleView, discordChannelView, bot, channelsRepository);
            BusinessLogic.Services.DiscordMessagesService discordMessagesService = new BusinessLogic.Services.DiscordMessagesService
                (Resource.Prefix, Resource.NamePattern, discordChannelView, channelsRepository, dataBase);

            consoleView.RunConsoleListner();
        }
    }
}
