using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BdoDailyCatBot.MainBot.EventsArgs;
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
        public event Action<MessageSendedEventArgs> MessageSended;

        public async Task Run(string Token)
        {
            var config = new DiscordConfiguration { Token = Token, TokenType = TokenType.Bot };

            Client = new DiscordClient(config);

            Client.MessageCreated += MessageCreated;

            await Client.ConnectAsync();
        }

        public void SendMessage(DiscordChannel discordChannel, string mes)
        {
            Client.SendMessageAsync(discordChannel, mes);
        }

        public DiscordChannel GetChannelById(ulong id)
        {
            return Channels.GetChannelById(id, this).Result;
        }

        public List<DiscordGuild> GetBotGuilds()
        {
            return Guilds.GetBotGuilds(this);
        }

        public async Task MessageCreated(MessageCreateEventArgs e)
        {
            MessageSended?.Invoke(new MessageSendedEventArgs {Message = e.Message });
        }

        public async void AddReactionToMesAsync(DiscordMessage mes, Reactions reaction)
        {
            await mes.CreateReactionAsync(Emoji.EmojiDictionary[reaction]);
        }
    }
}
