using BdoDailyCatBot.BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.BusinessLogic.Interfaces
{
    public interface IRaidsService
    {
        bool AddRaid(Raid raid, ulong senderId, ulong channelSenderId);
        void ReactionHeartChanged(Message mes, ulong ReactionSenderId, bool heartAdded);
        ulong GetRaidId(ulong channelAssemblyId);
        void DeleteRaid(ulong raidId);
    }
}
