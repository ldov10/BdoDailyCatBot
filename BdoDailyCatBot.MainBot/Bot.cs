using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BdoDailyCatBot.MainBot.Models;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.EventHandling;

namespace BdoDailyCatBot.MainBot
{
    public class Bot : Interfaces.IBot
    {
        public DiscordClient Client { get; private set; }
        public event Action<MessageCreateEventArgs> MessageSended;
        public event Action<MessageReactionAddEventArgs> MessageReactionAdded;
        public event Action<MessageReactionRemoveEventArgs> MessageReactionRemoved;

        private Emoji emoji;

        public async Task Run(string Token)
        {
            var config = new DiscordConfiguration { Token = Token, TokenType = TokenType.Bot };

            Client = new DiscordClient(config);

            Client.MessageCreated += MessageCreated;
            Client.MessageReactionAdded += ReactionAdded;
            Client.MessageReactionRemoved += ReactionRemoved;

            await Client.ConnectAsync();

            this.emoji = new Emoji(Client);
            Test();
        }

        public void Test()
        {
            var guild = Client.GetGuildAsync(619240990858936321).Result;
            var ch = guild.GetChannelsAsync().Result;
            foreach (var item in ch)
            {
                if (item.Name.Contains("ldov10"))
                {
                    item.DeleteAsync();
                }
            }
        }

        private async Task ReactionAdded(MessageReactionAddEventArgs e)
        {
            MessageReactionAdded?.Invoke(e);
        }

        public async Task<bool> DoesMessageExist(ulong mesId, ulong channelId)
        {
            var channel = await Client.GetChannelAsync(channelId);
            if (channel != default)
            {
                if (await channel.GetMessageAsync(mesId) != default)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task ReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            MessageReactionRemoved?.Invoke(e);
        }

        public Dictionary<Reactions, DiscordEmoji> GetEmojiDictionary()
        {
            return emoji.EmojiDictionary;
        }

        public DiscordGuild GetGuildByChannelId(ulong channelId)
        {
            return GetBotGuilds().FirstOrDefault(x => x.Channels.Keys.Contains(channelId));
        }

        public void EditMessage(ulong messageId, ulong channelId, string mes = default, DiscordEmbed discordEmbed = default)
        {
            var message = GetMessageById(channelId, messageId);
            message.ModifyAsync(mes, discordEmbed);
        }

        public DiscordEmbed BuildEmbed(string title, string fieldName, string fieldValue)
        {
            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();
            discordEmbedBuilder.Title = title;
            discordEmbedBuilder.AddField(fieldName, fieldValue);
            return discordEmbedBuilder.Build();
        }

        public async Task DeleteChannel(DiscordChannel channel)
        {
            await channel.DeleteAsync();
        }

        public bool DoesChannelExist(ulong id)
        {
            if (Client.GetChannelAsync(id).IsCompleted)
            {
                return true;
            }
            return false;
        }

        public async Task<ulong> SendMessageAsync(DiscordChannel discordChannel, string mes)
        {
            return (await Client.SendMessageAsync(discordChannel, mes)).Id;
        }

        public async Task<ulong> SendMessageAsync(DiscordChannel discordChannel, DiscordEmbed embed)
        {
            return (await Client.SendMessageAsync(discordChannel, null, false, embed)).Id;
        }

        public string GetEmoji(Reactions reaction)
        {
            return emoji.EmojiDictionary[reaction].Name;
        }

        public async Task<DiscordMember> GetMemberByIdAsync(ulong userId, ulong guildId)
        {
            var Guild = await Client.GetGuildAsync(guildId);
            return await Guild.GetMemberAsync(userId);
        }

        public async Task<DiscordChannel> CreateTextChannelAsync(ulong guildId, string name, DiscordChannel channelNeighbor)
        {
            var guild = await Client.GetGuildAsync(guildId);
            List<DiscordOverwriteBuilder> overwriteBuilder = new List<DiscordOverwriteBuilder>();

            foreach (var item in channelNeighbor.PermissionOverwrites)
            {
                overwriteBuilder.Add(await new DiscordOverwriteBuilder().FromAsync(item));
            }

            return await guild.CreateChannelAsync(name, ChannelType.Text, channelNeighbor.Parent, default, null, null, overwriteBuilder);
        }

        public DiscordChannel GetChannelById(ulong id)
        {
            return Client.GetChannelAsync(id).Result;
        }

        public DiscordMessage GetMessageById(ulong channelId, ulong id)
        {
            return Client.GetChannelAsync(channelId).Result.GetMessageAsync(id).Result;
        }

        public List<DiscordGuild> GetBotGuilds()
        {
            return Client.Guilds.Values.ToList();
        }

        public async Task MessageCreated(MessageCreateEventArgs e)
        {
            MessageSended?.Invoke(e);
        }

        public async void AddReactionToMesAsync(DiscordMessage mes, Reactions reaction)
        {
            await mes.CreateReactionAsync(emoji.EmojiDictionary[reaction]);
        }
    }
}
