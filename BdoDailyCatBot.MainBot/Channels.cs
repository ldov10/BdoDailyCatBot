using System;
using System.Collections.Generic;
using DSharpPlus;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Linq;
using System.Threading.Tasks;

namespace BdoDailyCatBot.MainBot
{
    public static class Channels
    {
        public static async Task<DiscordChannel> GetChannelById(ulong id, Bot bot)
        {
            return await bot.Client.GetChannelAsync(id);
        }
    }
}
