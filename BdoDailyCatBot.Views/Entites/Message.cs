 using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.Views.Entites
{
    public class Message
    {
        public string Content { get; set; } = "";
        public ulong ID { get; set; } = 0;
        public ulong ChannelId { get; set; } = 0;
        public string ChannelName { get; set; } = "";

        public ulong SenderID { get; set; } = 0;
    }
}
