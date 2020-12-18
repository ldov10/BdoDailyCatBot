using BdoDailyCatBot.BusinessLogic.BusinessModels;
using BdoDailyCatBot.DataAccess.Interfaces;
using BdoDailyCatBot.BusinessLogic.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BdoDailyCatBot.Views.Interfaces;
using AutoMapper;
using System.Linq;
using BdoDailyCatBot.MainBot;
using System.Resources;
using BdoDailyCatBot.DataAccess.Entities;

namespace BdoDailyCatBot.BusinessLogic.Services
{
    public class DiscordMessagesService
    {
        private readonly IViewDiscordChannel viewDiscordChannel;
        private readonly IFilesRepository files;
        private readonly IUnitOfWork dataBase;
        private readonly IRaidsService raidsService;

        private readonly ResourceManager resourceDMC;
        private readonly ResourceManager resourceDMO;
        private readonly ResourceManager resourcePatterns;

        private readonly Mapper mapperUserToUsers;

        public string Prefix { get; private set; }
        public string NamePattern { get; private set; }
        public string CaptainRoleName { get; private set; }

        private List<string> AdminRolesName { get; set; } = new List<string>();

        public DiscordMessagesService(ResourceManager resourceGeneralManager, ResourceManager resourceDiscordMessagesCommands
            , IViewDiscordChannel viewDiscordChannel, IFilesRepository filesRepository,
            IUnitOfWork dataBase, IRaidsService raidsService, ResourceManager resourcePatterns, ResourceManager resourceDiscordMessageOutput)
        {
            this.viewDiscordChannel = viewDiscordChannel;
            this.files = filesRepository;
            this.Prefix = resourceGeneralManager.GetString("Prefix");
            this.NamePattern = resourceGeneralManager.GetString("NamePattern");
            this.dataBase = dataBase;
            this.raidsService = raidsService;
            this.resourceDMC = resourceDiscordMessagesCommands;
            this.resourcePatterns = resourcePatterns;
            this.resourceDMO = resourceDiscordMessageOutput;
            this.CaptainRoleName = resourceGeneralManager.GetString("CaptainRoleName");

            this.AdminRolesName.Add(resourceGeneralManager.GetString("AdminRoleName1"));
            this.AdminRolesName.Add(resourceGeneralManager.GetString("AdminRoleName2"));

            viewDiscordChannel.MessageSended += MessageSended;
            viewDiscordChannel.MessageReactionAdded += ReactionAdded;
            viewDiscordChannel.MessageReactionRemoved += ReactionRemoved;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, Users>());

            this.mapperUserToUsers = new Mapper(config);
        }

        private void MessageSended(Views.Entites.Message e)
        {
            Message mes = new Message() 
            { Content = e.Content, Channel = new Channel() { Id = e.ChannelId, Name = e.ChannelName }, SenderID = e.SenderID };

            if (mes.Content.StartsWith(Prefix))
            {
                if (mes.Content == (Prefix + resourceDMC.GetString("RegHere")))
                {
                    bool hasPermissions = false;

                    var roles = viewDiscordChannel.GetUserRoles(mes.SenderID, viewDiscordChannel.GetGuildIdByChannel(mes.Channel.Id));

                    foreach (var item in AdminRolesName)
                    {
                        if (roles.Contains(item))
                        {
                            hasPermissions = true;
                            break;
                        }
                    }

                    if (hasPermissions)
                    {
                        viewDiscordChannel.AddReactionToMes(e, AddChannelToReg(mes.Channel) ? MainBot.Models.Reactions.OK : MainBot.Models.Reactions.NO);
                        return;
                    }
                    viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                }

                if (Regex.IsMatch(mes.Content, $"^{Prefix}{resourceDMC.GetString("Reg")}" + " .+"))
                {
                    if (!Regex.IsMatch(mes.Content, $@"^{Prefix}{resourceDMC.GetString("Reg")}" + @" \w+$"))
                    {
                        viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                        return;
                    }

                    if (!files.GetAll<DataAccess.Entities.Channels>(FileTypes.ChannelsToReg).Result.Contains
                        (new DataAccess.Entities.Channels(e.ChannelId, e.ChannelName)))
                    {
                        viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                        return;
                    }
                    string Name = Regex.Match(mes.Content, NamePattern).Value;

                    if (Name == "") 
                    {
                        viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                        return;
                    }

                    var flag = AddUserToDB(new User() 
                    {IdDiscord = mes.SenderID, IsCaptain = false, LastRaidDate = null, Name = Name, RaidsVisited = 0 });

                    viewDiscordChannel.AddReactionToMes(e, flag ? MainBot.Models.Reactions.OK : MainBot.Models.Reactions.NO);
                }

                if (Regex.IsMatch(mes.Content, $"^{Prefix}({resourceDMC.GetString("CreateRaid1")}|" +
                    $"{resourceDMC.GetString("CreateRaid3")}|" +
                    $"{resourceDMC.GetString("CreateRaid2")})" + " .+"))
                {
                    string Channel = "";
                    DateTime TimeStart = DateTime.Now;
                    DateTime TimeStartAssembly = DateTime.Now.AddMinutes(1);
                    TimeStartAssembly = TimeStartAssembly.AddSeconds(TimeStartAssembly.Second * -1);

                    int ReservedUsers = 1;

                    if (TimeStartAssembly.ToShortTimeString() == "00:00")
                    {
                        TimeStartAssembly.AddDays(1);
                    }

                    if (Regex.IsMatch(mes.Content,
                        $@"^{Prefix}({resourceDMC.GetString("CreateRaid1")}|{resourceDMC.GetString("CreateRaid2")}|{resourceDMC.GetString("CreateRaid3")})" +
                        $@" {resourcePatterns.GetString("Channel-TimeStart")}$"))
                    {   // channel timeStart
                        var Args = mes.Content.Split(' ');

                        Channel = Args[1];

                        var HoursAndMins = Args[2].Split(':');
                        TimeSpan timeSpan = new TimeSpan(Int32.Parse(HoursAndMins[0]), Int32.Parse(HoursAndMins[1]), 0);
                        TimeStart = TimeStart.Date + timeSpan;

                        if (TimeStart < DateTime.Now)
                        {
                            TimeStart = TimeStart.AddDays(1);
                        }
                    }
                    else
                    {
                        if (Regex.IsMatch(mes.Content,
                            $@"^{Prefix}({resourceDMC.GetString("CreateRaid1")}|{resourceDMC.GetString("CreateRaid2")}|{resourceDMC.GetString("CreateRaid3")})" +
                            $@" {resourcePatterns.GetString("Channel-TimeStart-TimeStartAssembly")}$"))
                        {  // channel timeStart TimeStartAssembly
                            var Args = mes.Content.Split(' ');

                            Channel = Args[1];

                            var HoursAndMins = Args[2].Split(':');
                            TimeSpan timeSpan = new TimeSpan(Int32.Parse(HoursAndMins[0]), Int32.Parse(HoursAndMins[1]), 0);
                            TimeStart = TimeStart.Date + timeSpan;

                            HoursAndMins = Args[3].Split(':');
                            timeSpan = new TimeSpan(Int32.Parse(HoursAndMins[0]), Int32.Parse(HoursAndMins[1]), 0);
                            TimeStartAssembly = TimeStartAssembly.Date + timeSpan;

                            if (TimeStart < DateTime.Now)
                            {
                                TimeStart = TimeStart.AddDays(1);
                            }
                            if (TimeStartAssembly < DateTime.Now)
                            {
                                TimeStartAssembly = TimeStartAssembly.AddDays(1);
                            }
                        }
                        else
                        {
                            if (Regex.IsMatch(mes.Content,
                                $@"^{Prefix}({resourceDMC.GetString("CreateRaid1")}|{resourceDMC.GetString("CreateRaid2")}|{resourceDMC.GetString("CreateRaid3")})" +
                                $@" {resourcePatterns.GetString("Channel-TimeStart-ReservedUsers")}$"))
                            {  // channel timeStart reservedUsers
                                var Args = mes.Content.Split(' ');

                                Channel = Args[1];

                                var HoursAndMins = Args[2].Split(':');
                                TimeSpan timeSpan = new TimeSpan(Int32.Parse(HoursAndMins[0]), Int32.Parse(HoursAndMins[1]), 0);
                                TimeStart = TimeStart.Date + timeSpan;

                                if (TimeStart < DateTime.Now)
                                {
                                    TimeStart = TimeStart.AddDays(1);
                                }

                                ReservedUsers = Int32.Parse(Args[3]);
                            }
                            else
                            {
                                if (Regex.IsMatch(mes.Content,
                                    $@"^{Prefix}({resourceDMC.GetString("CreateRaid1")}|{resourceDMC.GetString("CreateRaid2")}|{resourceDMC.GetString("CreateRaid3")})" +
                                    $@" {resourcePatterns.GetString("Channel-TimeStart-TimeStartAssembly-ReserverUsers")}$"))
                                {  // channel timeStart timeStartAssembly reserverUsers
                                    var Args = mes.Content.Split(' ');

                                    Channel = Args[1];

                                    var HoursAndMins = Args[2].Split(':');
                                    TimeSpan timeSpan = new TimeSpan(Int32.Parse(HoursAndMins[0]), Int32.Parse(HoursAndMins[1]), 0);
                                    TimeStart = TimeStart.Date + timeSpan;

                                    if (TimeStart < DateTime.Now)
                                    {
                                        TimeStart =  TimeStart.AddDays(1);
                                    }

                                    HoursAndMins = Args[3].Split(':');
                                    timeSpan = new TimeSpan(Int32.Parse(HoursAndMins[0]), Int32.Parse(HoursAndMins[1]), 0);
                                    TimeStartAssembly = TimeStartAssembly.Date + timeSpan;

                                    if (TimeStartAssembly < DateTime.Now)
                                    {
                                        TimeStartAssembly = TimeStartAssembly.AddDays(1);
                                    }

                                    ReservedUsers = Int32.Parse(Args[4]);
                                }
                                else
                                {
                                    viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                                    return;
                                }
                            }
                        }
                    }

                    if (TimeStart <= TimeStartAssembly)
                    {
                        viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                        return;
                    }

                    bool IsRaidAdded = raidsService.AddRaid(
                        new Raid() { TimeStart = TimeStart, TimeStartAssembly = TimeStartAssembly,
                            Channel = Channel, ChannelAssemblyId = mes.Channel.Id, ReservedUsers = ReservedUsers },
                        mes.SenderID, mes.Channel.Id);

                    viewDiscordChannel.AddReactionToMes(e, IsRaidAdded ? MainBot.Models.Reactions.OK : MainBot.Models.Reactions.NO);
                }

                if (Regex.IsMatch(mes.Content, $"^{Prefix}{resourceDMC.GetString("Re-register")} .+"))
                {
                    if (!Regex.IsMatch(mes.Content, $@"^{Prefix}перерег \w+$"))
                    {
                        viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                        return;
                    }

                    if (!files.GetAll<DataAccess.Entities.Channels>(FileTypes.ChannelsToReg).Result.Contains
                        (new DataAccess.Entities.Channels(e.ChannelId, e.ChannelName)))
                    {
                        viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                        return;
                    }
                    string Name = Regex.Match(mes.Content, NamePattern).Value;

                    if (Name == "")
                    {
                        viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                        return;
                    }

                    var user = dataBase.Users.GetAll().FirstOrDefault(p => p.IdDiscord == e.SenderID);
                    if (user == default)
                    {
                        viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                        return;
                    }

                    user.Name = Name;
                    UpdateUser(user);

                    viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.OK);
                }

                if (Regex.IsMatch(mes.Content, $"^{Prefix}{resourceDMC.GetString("DeleteRaid1")}"))
                {
                    var roles = viewDiscordChannel.GetUserRoles(mes.SenderID, viewDiscordChannel.GetGuildIdByChannel(mes.Channel.Id));
                    if (!roles.Contains(CaptainRoleName))
                    {
                        viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                        return;
                    }

                    var raidId = raidsService.GetRaidId(mes.Channel.Id);
                    if (raidId == default)
                    {
                        viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.NO);
                        return;
                    }

                    raidsService.DeleteRaid(raidId);
                }

                if (Regex.IsMatch(mes.Content, $"{Prefix}{resourceDMC.GetString("Help")}"))
                {
                    viewDiscordChannel.SendMessage(resourceDMO.GetString("Help"), mes.Channel.Id);

                    viewDiscordChannel.AddReactionToMes(e, MainBot.Models.Reactions.OK);
                }
            }
        }

        private void ReactionAdded(Views.EventsArgs.MessageReactionAddedEventArgs e)
        {
            if (e.Reaction == MainBot.Models.Reactions.HEART)
            {
                var mes = new Message()
                {
                    Channel = new Channel()
                    {
                        Id = e.Message.ChannelId,
                        Name = e.Message.ChannelName
                    },
                    Content = e.Message.Content,
                    SenderID = e.Message.SenderID
                };

                raidsService.ReactionHeartChanged(mes, e.ReactionSenderId, true);
            }
        }

        private void ReactionRemoved(Views.EventsArgs.MessageReactionRemovedEventArgs e)
        {
            if (e.Reaction == MainBot.Models.Reactions.HEART)
            {
                var mes = new Message()
                {
                    Channel = new Channel()
                    {
                        Id = e.Message.ChannelId,
                        Name = e.Message.ChannelName
                    },
                    Content = e.Message.Content,
                    SenderID = e.Message.SenderID
                };

                raidsService.ReactionHeartChanged(mes, e.ReactionSenderId, false);
            }
        }

        private bool AddUserToDB(User user)
        {
            Users users = mapperUserToUsers.Map<User, Users>(user);

            if (dataBase.Users.GetAll().Where(p => p.IdDiscord == user.IdDiscord).ToList().Count > 0)
            {
                return false;
            }
            
            dataBase.Users.Add(users);
            dataBase.Save();
            return true;
        }

        private void UpdateUser(Users user)
        {
            dataBase.Users.Update(user);
            dataBase.Save();
        }

        private bool AddChannelToReg(Channel channel)
        {
            var result = files.Add(new DataAccess.Entities.Channels(channel.Id, channel.Name), FileTypes.ChannelsToReg);
            return result.Result;
        }
    }
}
