using System;
using BdoDailyCatBot.MainBot.Interfaces;
using System.Collections.Generic;
using System.Text;
using Views.Interfaces;
using System.Linq;

namespace BdoDailyCatBot.BusinessLogic.Services
{
    public class ConsoleService
    {
        private readonly IViewConsole viewConsole;
        private readonly IViewDiscordChannel viewDiscordChannel;
        private readonly IBot bot;

        public ConsoleService(IViewConsole viewConsole, IViewDiscordChannel viewDiscordChannel, IBot bot)
        {
            this.viewConsole = viewConsole;
            this.viewDiscordChannel = viewDiscordChannel;
            this.bot = bot;

            viewConsole.SendMessage += MessageSended;
        }

        private void MessageSended()
        {
            if (this.viewConsole.Message == "SendMessage")
            {
                SendMessageToChannel();
            }
        }

        private void SendMessageToChannel()
        {
            System.Console.WriteLine("Select guild:");

            int i = 0;

            var guilds = bot.GetBotGuilds();

            foreach (var item in guilds)
            {
                System.Console.WriteLine($"{i}. {item.Name}");
                i++;
            }

            string Input = System.Console.ReadLine();

            if (Int32.TryParse(Input, out int guildInput) && (guildInput <= i && guildInput >= 0))
            {
                System.Console.WriteLine("Select channle:");

                var channels = guilds[guildInput].Channels.Values.ToList();

                i = 0;
                foreach (var item in channels)
                {
                    System.Console.WriteLine($"{i}. {item.Name}");
                    i++;
                }

                Input = System.Console.ReadLine();


                if (Int32.TryParse(Input, out int channleInput) && (channleInput <= i && channleInput >= 0))
                {
                    System.Console.WriteLine("Write message: ");
                    string message = System.Console.ReadLine();

                    viewDiscordChannel.SendMessage(message, channels[channleInput].Id);
                }
                else
                {
                    System.Console.WriteLine("Wrong choise");
                    return;
                }
            }
            else
            {
                System.Console.WriteLine("Wrong choise");
                return;
            }
        }
    }
}
