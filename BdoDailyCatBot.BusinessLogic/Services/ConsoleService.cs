using System;
using BdoDailyCatBot.MainBot.Interfaces;
using BdoDailyCatBot.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Text;
using Views.Interfaces;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BdoDailyCatBot.BusinessLogic.Services
{
    public class ConsoleService
    {
        private readonly IViewConsole viewConsole;
        private readonly IViewDiscordChannel viewDiscordChannel;
        private readonly IBot bot;
        private readonly IChannels channels;

        public ConsoleService(IViewConsole viewConsole, IViewDiscordChannel viewDiscordChannel, IBot bot, IChannels channels)
        {
            this.viewConsole = viewConsole;
            this.viewDiscordChannel = viewDiscordChannel;
            this.bot = bot;
            this.channels = channels;

            viewConsole.SendMessage += MessageSended;
            
        }

        private void MessageSended() // TODO: change select
        {
            if (this.viewConsole.Message == "SendMessage")
            {
                SendMessageToChannel();
                return;
            }

            if (this.viewConsole.Message == "AddChannelToReg")
            {
                AddChannelToReg();
                return;
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

                var Channels = guilds[guildInput].Channels.Values.ToList();

                i = 0;
                foreach (var item in Channels)
                {
                    System.Console.WriteLine($"{i}. {item.Name}");
                    i++;
                }

                Input = System.Console.ReadLine();


                if (Int32.TryParse(Input, out int channleInput) && (channleInput <= i && channleInput >= 0))
                {
                    System.Console.WriteLine("Write message: ");
                    string message = System.Console.ReadLine();

                    viewDiscordChannel.SendMessage(message, Channels[channleInput].Id);
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

        private void AddChannelToReg()
        {
            System.Console.WriteLine("Select guild:"); // TODO: If select > then count of guild program will end

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

                var Channels = guilds[guildInput].Channels.Values.ToList();

                i = 0;
                foreach (var item in Channels)
                {
                    System.Console.WriteLine($"{i}. {item.Name}");
                    i++;
                }

                Input = System.Console.ReadLine();


                if (Int32.TryParse(Input, out int channleInput) && (channleInput <= i && channleInput >= 0))
                {
                    channels.Add(new DataAccess.Entities.Channels(Channels[channleInput].Id, Channels[channleInput].Name), DataAccess.Entities.FilesType.ChannelsToReg); // TODO: Automap?
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
