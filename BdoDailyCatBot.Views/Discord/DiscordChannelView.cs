using BdoDailyCatBot.MainBot.EventsArgs;
using BdoDailyCatBot.MainBot.Interfaces;
using DSharpPlus.Entities;
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
        public event Action<MessageSendedEventArgs> MessageSended;

        public DiscordChannelView(IBot bot, string Prefix)
        {
            this.Bot = bot;

            Bot.MessageSended += MessageCreated;
        }
        public void SendMessage(string mes, ulong channelId)
        {
            Bot.SendMessage(Bot.GetChannelById(channelId), mes);
        }

        public void AddReactionToMes(MessageSendedEventArgs args, bool flag)
        {
            if (flag)
            {
                Bot.AddReactionToMesAsync(args.Message, BdoDailyCatBot.MainBot.Models.Reactions.OK);
            }
            else
            {
                Bot.AddReactionToMesAsync(args.Message, BdoDailyCatBot.MainBot.Models.Reactions.NO);
            }
        }

        public void MessageCreated(MessageSendedEventArgs e)
        {
            MessageSended?.Invoke(e);
        }
    }
}
