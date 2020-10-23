using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace BdoDailyCatBot.MainBot
{
    public class Bot : Interfaces.IBot
    {
       public DiscordClient Client { get; private set; }

        public async Task Run(string Token)

        {
            var config = new DiscordConfiguration { Token = Token, TokenType = TokenType.Bot };

            Client = new DiscordClient(config);

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
    }
}
