using System;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Text;
using System.Threading.Tasks;
using BdoDailyCatBot.MainBot.EventsArgs;
using BdoDailyCatBot.MainBot.Models;

namespace BdoDailyCatBot.MainBot.Interfaces
{
    public interface IBot
    {
        void SendMessage(DiscordChannel discordChannel, string mes);
        DiscordChannel GetChannelById(ulong id);
        List<DiscordGuild> GetBotGuilds();
        void AddReactionToMesAsync(DiscordMessage mes, Reactions reaction);

        public event Action<MessageSendedEventArgs> MessageSended;
    }
}
