using System;
using System.Collections.Generic;
using DSharpPlus;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Linq;

namespace BdoDailyCatBot
{
    public static class Guilds
    {
        public static List<DiscordGuild> GetBotGuilds(Bot bot)
        {
            return bot.Client.Guilds.Values.ToList();
        }
    }
}
