using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Views.Interfaces
{
    public interface IViewDiscordChannel
    {
        void SendMessage(string mes, ulong channelId);
    }
}
