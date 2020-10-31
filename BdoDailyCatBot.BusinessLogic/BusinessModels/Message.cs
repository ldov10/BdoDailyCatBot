using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.BusinessLogic.BusinessModels
{
    class Message
    {
        public string Content { get; set; } = "";
        public Channel Channel { get; set; } = new Channel();

        public ulong SenderID { get; set; } = 0;
    }
}
