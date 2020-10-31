﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.BusinessLogic.DTO
{
    class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RaidsVisited { get; set; }
        public DateTime? LastRaidDate { get; set; }
        public bool IsCaptain { get; set; }
        public ulong IdDiscord { get; set; }
    }
}
