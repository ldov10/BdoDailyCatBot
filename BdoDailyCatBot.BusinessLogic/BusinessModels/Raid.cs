using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.BusinessLogic.BusinessModels
{
    public class Raid
    {
        public static int Count { get; set; } = 0;

        public ulong Id { get; set; } = 0;
        public string CaptainName { get; set; } = "";
        public string Channel { get; set; } = "";
        public ulong ChannelAssemblyId { get; set; } = 0;
        public DateTime TimeStart { get; set; } = new DateTime();
        public DateTime TimeStartAssembly { get; set; } = new DateTime();
        public int ReservedUsers { get; set; } = 1;
        public int UsersInRaid { get; set; } = 0;
        public ulong MessageId { get; set; } = 0;
        public List<User> Users { get; set; } = new List<User>();
    }
}
