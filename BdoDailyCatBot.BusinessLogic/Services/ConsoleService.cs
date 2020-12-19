using System;
using BdoDailyCatBot.MainBot.Interfaces;
using BdoDailyCatBot.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Text;
using BdoDailyCatBot.Views.Interfaces;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace BdoDailyCatBot.BusinessLogic.Services
{
    public class ConsoleService
    {
        private readonly IViewConsole viewConsole;
        private readonly IViewDiscordChannel viewDiscordChannel;
        private readonly IBot bot;
        private readonly IFilesRepository files;
        private readonly ResourceManager resourceCC;
        private readonly ResourceManager resourceCO;

        public ConsoleService(IViewConsole viewConsole, IViewDiscordChannel viewDiscordChannel, IBot bot, IFilesRepository filesRepository,
            ResourceManager resourceConsoleCommands, ResourceManager resourceConsoleOutput)
        {
            this.viewConsole = viewConsole;
            this.viewDiscordChannel = viewDiscordChannel;
            this.bot = bot;
            this.files = filesRepository;
            this.resourceCC = resourceConsoleCommands;
            this.resourceCO = resourceConsoleOutput;

            viewConsole.SendMessage += MessageSended;
            
        }

        private void MessageSended()
        {
            if (this.viewConsole.Message == resourceCC.GetString("SendMessage"))
            {
                SendMessageToChannel();
                return;
            }

            if (this.viewConsole.Message == resourceCC.GetString("AddChannelToReg"))
            {
                AddChannelToReg();
                return;
            }
        }

        private void SendMessageToChannel()
        {
            viewConsole.SendConsoleMessage(resourceCO.GetString("Select guild:"));

            int i = 0;

            var guilds = bot.GetBotGuilds();

            foreach (var item in guilds)
            {
                viewConsole.SendConsoleMessage($"{i}. {item.Name}");
                i++;
            }

            string Input = System.Console.ReadLine();

            if (Int32.TryParse(Input, out int guildInput) && (guildInput <= i && guildInput >= 0))
            {
                viewConsole.SendConsoleMessage(resourceCO.GetString("Select channle:"));

                var Channels = guilds[guildInput].Channels.Values.ToList();

                i = 0;
                foreach (var item in Channels)
                {
                    viewConsole.SendConsoleMessage($"{i}. {item.Name}");
                    i++;
                }

                Input = System.Console.ReadLine();


                if (Int32.TryParse(Input, out int channleInput) && (channleInput <= i && channleInput >= 0))
                {
                    viewConsole.SendConsoleMessage(resourceCO.GetString("Write message:"));
                    string message = System.Console.ReadLine();

                    viewDiscordChannel.SendMessage(message, Channels[channleInput].Id);
                }
                else
                {
                    viewConsole.SendConsoleMessage(resourceCO.GetString("Wrong choise"));
                    return;
                }
            }
            else
            {
                viewConsole.SendConsoleMessage(resourceCO.GetString("Wrong choise"));
                return;
            }
        }

        private void AddChannelToReg()
        {
            viewConsole.SendConsoleMessage(resourceCO.GetString("Select guild:"));

            int i = 0;

            var guilds = bot.GetBotGuilds();

            foreach (var item in guilds)
            {
                viewConsole.SendConsoleMessage($"{i}. {item.Name}");
                i++;
            }

            string Input = System.Console.ReadLine();

            if (Int32.TryParse(Input, out int guildInput) && (guildInput <= i && guildInput >= 0))
            {
                viewConsole.SendConsoleMessage(resourceCO.GetString("Select channle:"));

                var Channels = guilds[guildInput].Channels.Values.ToList();

                i = 0;
                foreach (var item in Channels)
                {
                    viewConsole.SendConsoleMessage($"{i}. {item.Name}");
                    i++;
                }

                Input = System.Console.ReadLine();


                if (Int32.TryParse(Input, out int channleInput) && (channleInput <= i && channleInput >= 0))
                {
                    files.Add<DataAccess.Entities.Channels>(
                        new DataAccess.Entities.Channels(Channels[channleInput].Id, Channels[channleInput].Name), DataAccess.Entities.FileTypes.ChannelsToReg);
                }
                else
                {
                    viewConsole.SendConsoleMessage(resourceCO.GetString("Wrong choise"));
                    return;
                }
            }
            else
            {
                viewConsole.SendConsoleMessage(resourceCO.GetString("Wrong choise"));
                return;
            }
        }
    }
}
