using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Entities
{
    public class Raids
    {
        public string CaptainName { get; private set; } = "";
        public string Channel { get; private set; } = "";
        public DateTime TimeStart { get; private set; } = new DateTime();
        public DateTime TimeStartAssembly { get; private set; } = new DateTime();
        public int ReservedUsers { get; private set; } = 1;
        public int UsersInRaid { get; private set; } = 0;

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
