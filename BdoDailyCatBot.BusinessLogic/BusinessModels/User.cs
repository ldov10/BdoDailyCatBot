using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.BusinessLogic.BusinessModels
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RaidsVisited { get; set; }
        public DateTime? LastRaidDate { get; set; }
        public bool IsCaptain { get; set; }
        public ulong IdDiscord { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            User objAsUser = obj as User;

            if (objAsUser == null)
            {
                return false;
            }

            if (objAsUser.IdDiscord == this.IdDiscord)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.IdDiscord.GetHashCode();
        }
    }
}
