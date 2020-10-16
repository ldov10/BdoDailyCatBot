using System;
using System.Collections.Generic;

namespace BdoDailyCatBot
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RaidsVisited { get; set; }
        public DateTime? LastRaidDate { get; set; }
        public bool IsCaptain { get; set; }
        public decimal IdDiscord { get; set; }

        public virtual Captains Captains { get; set; }
    }
}
