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
        public Task<ulong> SendMessageAsync(DiscordChannel discordChannel, DiscordEmbed embed);
        DiscordChannel GetChannelById(ulong id);
        DiscordEmbed BuildEmbed(string title, string fieldName, string fieldValue);
        List<DiscordGuild> GetBotGuilds();
        string GetEmoji(Reactions reaction);
        public Task<DiscordMember> GetMemberByIdAsync(ulong userId, ulong guildId);
        public Task<DiscordChannel> CreateTextChannelAsync(ulong guildId, string name, DiscordChannel parent);
        public DiscordMessage GetMessageById(ulong channelId, ulong id);
        void AddReactionToMesAsync(DiscordMessage mes, Reactions reaction);
        void EditMessage(ulong messageId, ulong channelId, string mes = default, DiscordEmbed discordEmbed = default);
        Task DeleteChannel(DiscordChannel channel);
        Task<bool> DoesMessageExist(ulong mesId, ulong channelId);
        bool DoesChannelExist(ulong channelId);
        DiscordGuild GetGuildByChannelId(ulong channelId);

        public event Action<MessageCreateEventArgs> MessageSended;
        public event Action<MessageReactionAddEventArgs> MessageReactionAdded;
        public event Action<MessageReactionRemoveEventArgs> MessageReactionRemoved;
    }
}
