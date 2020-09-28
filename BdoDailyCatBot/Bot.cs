using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace BdoDailyCatBot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }

        public async Task Run()
        {
            var config = new DiscordConfiguration { Token = Secrets.botToken, TokenType = TokenType.Bot };

            Client = new DiscordClient(config);


            await Client.ConnectAsync();
            //await Task.Delay(-1);
        }
    }
}
