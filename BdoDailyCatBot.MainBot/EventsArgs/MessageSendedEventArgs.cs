using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.MainBot.EventsArgs
{
    public class MessageSendedEventArgs : EventArgs
    {
        public DiscordMessage Message { get; set; }
    }
}
