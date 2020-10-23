using BdoDailyCatBot.MainBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Views.Interfaces;

namespace Views.Discord
{
    public class DiscordChannelView : IViewDiscordChannel
    {
        private readonly IBot Bot;

        public DiscordChannelView(IBot bot)
        {
            this.Bot = bot;
        }
        public void SendMessage(string mes, ulong channelId)
        {
            Bot.SendMessage(Bot.GetChannelById(channelId), mes);
        }
    }
}
