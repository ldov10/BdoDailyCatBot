using System;

namespace BdoDailyCatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var bot = new MainBot.Bot();
                bot.Run(Secrets.botToken).GetAwaiter().GetResult();

                var consoleView = new Views.Console.ConsoleView();
                var discordChannelView = new Views.Discord.DiscordChannelView(bot, GeneralResource.Prefix);
                var filesReposiroty = new DataAccess.Repositories.FilesReposiroty(ForFiles.ResourceManager);
                var dataBase = new DataAccess.Repositories.EFUnitOfWork();
                var raidsService = new BusinessLogic.Services.RaidsService(filesReposiroty, RaidAssembling.ResourceManager, discordChannelView, dataBase);
                var consoleService = new BusinessLogic.Services.ConsoleService
                    (consoleView, discordChannelView, bot, filesReposiroty, ConsoleCommands.ResourceManager, ConsoleOutput.ResourceManager);
                var discordMessagesService = new BusinessLogic.Services.DiscordMessagesService
                    (GeneralResource.ResourceManager, DiscordMessagesCommands.ResourceManager, discordChannelView, filesReposiroty,
                    dataBase, raidsService, Patterns.ResourceManager, DiscordMessageOutput.ResourceManager);

                consoleView.RunConsoleListner();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEXCEPTION: {ex.Message}" +
                    $"\n{ex.StackTrace}" +
                    $"\n\n");
            }
        }
    }
}
