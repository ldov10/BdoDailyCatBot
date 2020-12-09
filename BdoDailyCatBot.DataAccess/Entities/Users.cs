using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BdoDailyCatBot.DataAccess.Entities
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RaidsVisited { get; set; }
        public DateTime? LastRaidDate { get; set; }
        public bool IsCaptain { get; set; }
        public ulong IdDiscord { get; set; }

        [JsonIgnore][NotMapped]
        public virtual Captains Captains { get; set; }
    }
}
