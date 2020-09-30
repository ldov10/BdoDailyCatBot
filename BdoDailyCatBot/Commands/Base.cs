using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace BdoDailyCatBot.Commands
{
    public static class Base
    {
        public static async Task SendMessage(DiscordClient client, DiscordChannel channel, string mes)
        {
            await client.SendMessageAsync(channel, mes);
        }

    }
}
