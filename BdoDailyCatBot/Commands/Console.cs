using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace BdoDailyCatBot.Commands
{
    public static class Console
    {
        public static Dictionary<string, Action<Bot>> ConsoleCommands = new Dictionary<string, Action<Bot>>() 
        {
            ["SendMessage"] = (bot) => Console.SendMessageToChannle(bot)
        };

        public static async void SendMessageToChannle(Bot bot)
        {
            System.Console.WriteLine("Select guild:");

            int i = 0;

            var guilds = Guilds.GetBotGuilds(bot);

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

                    await Base.SendMessage(bot.Client, channels[channleInput], message);
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
