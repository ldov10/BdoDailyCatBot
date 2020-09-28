    using System;

namespace BdoDailyCatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.Run().GetAwaiter().GetResult();

            Console.RunConsoleListening(bot);
        }
    }
}
