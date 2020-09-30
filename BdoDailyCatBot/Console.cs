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
                Commands.Console.ConsoleCommands[command].Invoke(bot);
            }
            else
            {
                System.Console.WriteLine("Wrong command");
            }
        }
    }
}
