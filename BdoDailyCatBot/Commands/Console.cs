using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace BdoDailyCatBot.Commands
{
    public static class Console
    {
        public static Dictionary<string, Action<DiscordClient, DiscordChannel, string>> ConsoleCommands = new Dictionary<string, Action<DiscordClient, DiscordChannel, string>>() 
        {
            ["SendMessage"] = (client, channel, mes) => Console.SendMessageToChannle(client, channel, mes)
        };

        public static async void SendMessageToChannle(DiscordClient client, DiscordChannel channel, string mes)
        {
            await Base.SendMessage(client, channel, mes);
        }
    }
}
