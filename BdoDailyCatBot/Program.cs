using System;
using System.Threading.Tasks;

namespace BdoDailyCatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            try
            {
                var bot = new MainBot.Bot();
                await bot.Run(Secrets.botToken);

                var discordChannelView = new Views.Discord.DiscordChannelView(bot, GeneralResource.Prefix);
                var filesReposiroty = new DataAccess.Repositories.FilesReposiroty(ForFiles.ResourceManager);
                var dataBase = new DataAccess.Repositories.EFUnitOfWork();
                var raidsService = new BusinessLogic.Services.RaidsService(filesReposiroty, RaidAssembling.ResourceManager, discordChannelView, dataBase);
                var discordMessagesService = new BusinessLogic.Services.DiscordMessagesService
                    (GeneralResource.ResourceManager, DiscordMessagesCommands.ResourceManager, discordChannelView, filesReposiroty,
                    dataBase, raidsService, Patterns.ResourceManager, DiscordMessageOutput.ResourceManager);

                await Task.Delay(-1);
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
