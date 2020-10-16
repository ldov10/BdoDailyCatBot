using System;
using System.Collections.Generic;

namespace BdoDailyCatBot
{
    public partial class Captains
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DroveRaids { get; set; }
        public DateTime? LastDrivenRaid { get; set; }

        public virtual Users User { get; set; }
    }
}
