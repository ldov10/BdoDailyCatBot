using BdoDailyCatBot.MainBot.Models;
using BdoDailyCatBot.Views.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.Views.EventsArgs
{
    public class MessageReactionAddedEventArgs : EventArgs
    {
        public Message Message { get; set; }
        public Reactions Reaction { get; set; }
        public ulong ReactionSenderId { get; set; }
    }
}
