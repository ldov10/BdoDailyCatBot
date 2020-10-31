using System;
using BdoDailyCatBot.MainBot.EventsArgs;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Views.Interfaces
{
    public interface IViewDiscordChannel
    {
        void SendMessage(string mes, ulong channelId);
        void AddReactionToMes(MessageSendedEventArgs args, bool flag);

        public event Action<MessageSendedEventArgs> MessageSended;
    }
}
