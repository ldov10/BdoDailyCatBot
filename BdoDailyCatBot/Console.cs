using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Linq;

namespace BdoDailyCatBot
{
    public static class Console
    {
        private delegate void ConsoleCommandsHandler(string mes);
        static event ConsoleCommandsHandler Notify;
        public static Bot bot { get; private set; }

        public static async void RunConsoleListening(Bot bot)
        {
            Notify += Console.CommandSelection;
            Console.bot = bot;

            await Console.ConsoleListner();
        } 

        private static Task ConsoleListner()
        {
            while (true)
            {
                string Input = System.Console.ReadLine();

                Notify?.Invoke(Input);
            }
        }

        private static void CommandSelection(string command)
        {
            if (Commands.Console.ConsoleCommands.ContainsKey(command))
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

                        var Action = Commands.Console.ConsoleCommands[command];
                        Action.Invoke(bot.Client, channels[channleInput], message);
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
            else
            {
                System.Console.WriteLine("Wrong command");
            }
        }
    }
}
