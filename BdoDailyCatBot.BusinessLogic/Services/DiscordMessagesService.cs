using BdoDailyCatBot.BusinessLogic.BusinessModels;
using BdoDailyCatBot.DataAccess.Interfaces;
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
        private readonly RaidsService raidsService; // TODO: It's not good

        public string Prefix { get; private set; }
        public string NamePattern { get; private set; }

        public DiscordMessagesService(ResourceManager resourceManager, IViewDiscordChannel viewDiscordChannel, IFilesRepository filesRepository,
            IUnitOfWork dataBase, RaidsService raidsService)
        {
            this.viewDiscordChannel = viewDiscordChannel;
            this.files = filesRepository;
            this.Prefix = resourceManager.GetString("Prefix");
            this.NamePattern = resourceManager.GetString("NamePattern");
            this.dataBase = dataBase;
            this.raidsService = raidsService;

            viewDiscordChannel.MessageSended += MessageSended;
            viewDiscordChannel.MessageReactionAdded += ReactionAdded;
            viewDiscordChannel.MessageReactionRemoved += ReactionRemoved;
        }

        private void MessageSended(Views.Entites.Message e)
        {
            Message mes = new Message() 
            { Content = e.Content, Channel = new Channel() { Id = e.ChannelId, Name = e.ChannelName }, SenderID = e.SenderID }; // TODO: Automap?

            if (mes.Content.StartsWith(Prefix))
            {
                if (mes.Content == (Prefix + "рег_тут")) // TODO: change select
                {
                    viewDiscordChannel.AddReactionToMes(e, AddChannelToReg(mes.Channel) ? MainBot.Models.Reactions.OK : MainBot.Models.Reactions.NO);
                }

                if (Regex.IsMatch(mes.Content, $"^{Prefix}рег .+"))
                {
                    if (!Regex.IsMatch(mes.Content, $@"^{Prefix}рег \w+$"))
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

                if (Regex.IsMatch(mes.Content, $"^{Prefix}к .+")) // TODO: change "к"
                {
                    string Channel = "";
                    DateTime TimeStart = DateTime.Now;
                    DateTime TimeStartAssembly = DateTime.Now.AddMinutes(1);
                    int ReservedUsers = 1;

                    if (TimeStartAssembly.ToShortTimeString() == "00:00")
                    {
                        TimeStartAssembly.AddDays(1);
                    }

                    if (Regex.IsMatch(mes.Content, $@"^{Prefix}к (([A-Za-zА-Яа-я]-[1-4])|([A-Za-zА-Яа-я][1-4])) (([0-1]\d)|([2][0-3])|(\d)):[0-5]\d$"))
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
                        if (Regex.IsMatch(mes.Content, $@"^{Prefix}к (([A-Za-zА-Яа-я]-[1-4])|([A-Za-zА-Яа-я][1-4])) (([0-1]\d)|([2][0-3])|(\d)):[0-5]\d (([0-1]\d)|([2][0-3])|(\d)):[0-5]\d$"))
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
                            if (Regex.IsMatch(mes.Content, $@"^{Prefix}к (([A-Za-zА-Яа-я]-[1-4])|([A-Za-zА-Яа-я][1-4])) (([0-1]\d)|([2][0-3])|(\d)):[0-5]\d (([1][0-9])|([1-9]))$"))
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
                                if (Regex.IsMatch(mes.Content, $@"^{Prefix}к (([A-Za-zА-Яа-я]-[1-4])|([A-Za-zА-Яа-я][1-4])) (([0-1]\d)|([2][0-3])|(\d)):[0-5]\d (([0-1]\d)|([2][0-3])|(\d)):[0-5]\d (([1][0-9])|([1-9]))$"))
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
                        new Raid() { TimeStart = TimeStart, TimeStartAssembly = TimeStartAssembly, Channel = Channel, ChannelAssemblyId = mes.Channel.Id }, mes.SenderID, mes.Channel.Id); // TODO: del channel

                    viewDiscordChannel.AddReactionToMes(e, IsRaidAdded ? MainBot.Models.Reactions.OK : MainBot.Models.Reactions.NO);
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, Users>());
            var mapper = new Mapper(config);
            Users users = mapper.Map<User, Users>(user);
            var flag = dataBase.Users.Add(users).Result;
            dataBase.Save();
            return flag;
        }

        private bool AddChannelToReg(Channel channel)
        {
            var result = files.Add(new DataAccess.Entities.Channels(channel.Id, channel.Name), FileTypes.ChannelsToReg); // TODO: Automap?
            return result.Result;
        }
    }
}
