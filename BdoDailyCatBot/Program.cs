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
            Views.Discord.DiscordChannelView discordChannelView = new Views.Discord.DiscordChannelView(bot, GeneralResource.Prefix);
            DataAccess.Repositories.FilesReposiroty filesReposiroty = new DataAccess.Repositories.FilesReposiroty(GeneralResource.ResourceManager);
            DataAccess.Repositories.EFUnitOfWork dataBase = new DataAccess.Repositories.EFUnitOfWork();
            BusinessLogic.Services.RaidsService raidsService = new BusinessLogic.Services.RaidsService(filesReposiroty, RaidAssembling.ResourceManager, discordChannelView, dataBase);
            BusinessLogic.Services.ConsoleService consoleService = new BusinessLogic.Services.ConsoleService
                (consoleView, discordChannelView, bot, filesReposiroty, ConsoleCommands.ResourceManager, ConsoleOutput.ResourceManager);
            BusinessLogic.Services.DiscordMessagesService discordMessagesService = new BusinessLogic.Services.DiscordMessagesService
                (GeneralResource.ResourceManager, DiscordMessagesCommands.ResourceManager, discordChannelView, filesReposiroty, dataBase, raidsService);

            consoleView.RunConsoleListner();
        }
    }
}
