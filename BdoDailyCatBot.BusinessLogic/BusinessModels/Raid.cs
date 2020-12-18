using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using BdoDailyCatBot.BusinessLogic.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BdoDailyCatBot.BusinessLogic.Services;

namespace BdoDailyCatBot.BusinessLogic.BusinessModels
{
    public class Raid
    {
        public ulong Id { get; set; } = 0;
        public string CaptainName { get; set; } = "";
        public int CaptainUserId { get; set; } = 0;
        public string Channel { get; set; } = "";
        public ulong ChannelAssemblyId { get; set; } = 0;
        public DateTime TimeStart { get; set; } = new DateTime();
        public DateTime TimeStartAssembly { get; set; } = new DateTime();
        public int ReservedUsers { get; set; } = 0;
        public int UsersInRaid { get; set; } = 0;
        public ulong MessageId { get; set; } = 0;
        public ulong TableMessageId { get; set; } = 0;
        public bool IsAssembling { get; set; } = false;
        public bool IsStarted { get; set; } = false;
        public List<User> Users { get; set; } = new List<User>();

        [NotMapped]
        public TimerCallback timerAssembly { get; set; }
        [NotMapped]
        public TimerCallback timerStart { get; set; }
        [NotMapped]
        public TimerCallback timerHourAfterStart { get; set; }

        public void SetIsAssemblingTrue()
        {
            RaidsService.GetInstance().SetAssemblingTrue(this);
        }

        public void Start()
        {
            RaidsService.GetInstance().StartRaid(this);
        }

        public void HourAfterStart()
        {
            RaidsService.GetInstance().HourAfterStart(this);
        }
    }
}
