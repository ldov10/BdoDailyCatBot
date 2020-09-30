using System;
using System.Collections.Generic;
using DSharpPlus;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Linq;

namespace BdoDailyCatBot
{
    public static class Channels
    {
        public static List<DiscordChannel> GetChannelsWhereCanWrite(Bot bot)
        {
            return bot.Client.Guilds.Values.AsParallel().SelectMany(x => x.Channels.Values).ToList(); //TODO: need some logic
        }
    }
}
