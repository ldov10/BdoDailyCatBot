using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Entities
{
    public class Raids : ToFile
    {
        public ulong Id { get; set; } = 0;
        public string CaptainName { get; set; } = "";
        public string Channel { get; set; } = "";
        public ulong ChannelAssemblyId { get; set; } = 0; 
        public DateTime TimeStart { get; set; } = new DateTime();
        public DateTime TimeStartAssembly { get; set; } = new DateTime();
        public int ReservedUsers { get; set; } = 1;
        public int UsersInRaid { get; set; } = 0;
        public ulong MessageId { get; set; } = 0;
        public bool IsAssembling { get; set; } = false;
        public bool IsStarted { get; set; } = false;
        public ulong TableMessageId { get; set; } = 0;
        public List<Users> Users { get; set; } = new List<Users>();

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Raids objAsRaids = obj as Raids;

            if (objAsRaids == null)
            {
                return false;
            }

            if (objAsRaids.CaptainName == this.CaptainName && objAsRaids.Channel == this.Channel && objAsRaids.ReservedUsers == this.ReservedUsers &&
                objAsRaids.TimeStart.ToLocalTime() == this.TimeStart.ToLocalTime() && objAsRaids.TimeStartAssembly.ToLocalTime() == this.TimeStartAssembly.ToLocalTime() &&
                objAsRaids.UsersInRaid == this.UsersInRaid)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            hash = this.CaptainName.GetHashCode() + this.Channel.GetHashCode() + this.ReservedUsers.GetHashCode() + this.TimeStartAssembly.ToLocalTime().GetHashCode() +
                this.TimeStartAssembly.ToLocalTime().GetHashCode() + this.UsersInRaid.GetHashCode();
            return hash;
        }
    }
}
