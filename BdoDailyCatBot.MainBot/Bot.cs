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
    public class Bot : Interfaces.IBot //Inotifypropchanged / collection
    {
        public DiscordClient Client { get; private set; }
        public event Action<MessageCreateEventArgs> MessageSended;
        public event Action<MessageReactionAddEventArgs> MessageReactionAdded;

        private Emoji emoji;

        public async Task Run(string Token)
        {
            var config = new DiscordConfiguration { Token = Token, TokenType = TokenType.Bot };

            Client = new DiscordClient(config);

            Client.MessageCreated += MessageCreated;
            Client.MessageReactionAdded += ReactionAdded;

            await Client.ConnectAsync();

            this.emoji = new Emoji(Client);

            //DelChannelsByName("2200-ldov10", 619240990858936321);
        }

        private async Task ReactionAdded(MessageReactionAddEventArgs e)
        {
            MessageReactionAdded?.Invoke(e);
        }

        public Dictionary<Reactions, DiscordEmoji> GetEmojiDictionary()
        {
            return emoji.EmojiDictionary;
        }

        private void DelChannelsByName(string name, ulong guildId)
        {
            var guild = Client.GetGuildAsync(guildId).Result;

            var channels = guild.Channels.Values.Where(x => x.Name == name).ToList();

            channels.ForEach(x => x.DeleteAsync());
        }

        public async Task<ulong> SendMessageAsync(DiscordChannel discordChannel, string mes)
        {
            return (await Client.SendMessageAsync(discordChannel, mes)).Id;
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
            return Channels.GetChannelById(id, this).Result;
        }

        public DiscordMessage GetMessageById(ulong channelId, ulong id)
        {
            return Channels.GetChannelById(channelId, this).Result.GetMessageAsync(id).Result;
        }

        public List<DiscordGuild> GetBotGuilds()
        {
            return Guilds.GetBotGuilds(this);
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
