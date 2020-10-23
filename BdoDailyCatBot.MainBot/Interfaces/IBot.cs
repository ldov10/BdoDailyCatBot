using System;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Text;
using System.Threading.Tasks;

namespace BdoDailyCatBot.MainBot.Interfaces
{
    public interface IBot
    {
        void SendMessage(DiscordChannel discordChannel, string mes);
        DiscordChannel GetChannelById(ulong id);
        List<DiscordGuild> GetBotGuilds();
    }
}
