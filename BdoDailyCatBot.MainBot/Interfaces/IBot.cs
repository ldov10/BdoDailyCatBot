using System;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Text;
using System.Threading.Tasks;
using BdoDailyCatBot.MainBot.Models;
using DSharpPlus.EventArgs;

namespace BdoDailyCatBot.MainBot.Interfaces
{
    public interface IBot
    {
        Dictionary<Reactions, DiscordEmoji> GetEmojiDictionary();

        public Task<ulong> SendMessageAsync(DiscordChannel discordChannel, string mes);
        DiscordChannel GetChannelById(ulong id);
        List<DiscordGuild> GetBotGuilds();
        string GetEmoji(Reactions reaction);
        public Task<DiscordMember> GetMemberByIdAsync(ulong userId, ulong guildId);
        public Task<DiscordChannel> CreateTextChannelAsync(ulong guildId, string name, DiscordChannel parent);
        public DiscordMessage GetMessageById(ulong channelId, ulong id);
        void AddReactionToMesAsync(DiscordMessage mes, Reactions reaction);

        public event Action<MessageCreateEventArgs> MessageSended;
        public event Action<MessageReactionAddEventArgs> MessageReactionAdded;
    }
}
