using AutoMapper;
using BdoDailyCatBot.BusinessLogic.BusinessModels;
using BdoDailyCatBot.DataAccess.Entities;
using BdoDailyCatBot.DataAccess.Interfaces;
using BdoDailyCatBot.MainBot.Models;
using BdoDailyCatBot.Views.Interfaces;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;

namespace BdoDailyCatBot.BusinessLogic.Services
{
    public class RaidsService
    {
        private readonly IFilesRepository files;
        private readonly ResourceManager resourceManager;
        private readonly IViewDiscordChannel viewDiscordChannel;
        private readonly IUnitOfWork database;

        private readonly string captainRoleName = "";
        private readonly Mapper mapperRaidToRaids;
        private readonly Mapper mapperUserToUsers;
        private readonly Mapper mapperUsersToUser;

        private static List<Raid> currentRaids = new List<Raid>();

        public RaidsService(IFilesRepository files, ResourceManager resource, IViewDiscordChannel viewDiscordChannel, IUnitOfWork database)
        {
            this.files = files;
            this.resourceManager = resource;
            this.viewDiscordChannel = viewDiscordChannel;
            this.database = database;
            this.captainRoleName = resource.GetString("CaptainRoleName");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Raid, Raids>();
                cfg.CreateMap<User, Users>();
            });
            this.mapperRaidToRaids = new Mapper(config);

            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, Users>();
            });
            this.mapperUserToUsers = new Mapper(config);

            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Users, User>();
            });
            this.mapperUsersToUser = new Mapper(config);
        }

        public bool AddRaid(Raid raid, ulong senderId, ulong channelSenderId)
        {
            var roles = viewDiscordChannel.GetUserRoles(senderId, viewDiscordChannel.GetGuildIdByChannel(channelSenderId));
            if (!roles.Contains(captainRoleName))
            {
                return false;
            }

            var user = database.Users.GetAll().FirstOrDefault(p => p.IdDiscord == senderId); // TODO: map to userdto?
            raid.CaptainName = user.Name.Trim();

            if (!user.IsCaptain)
            {
                user.IsCaptain = true;
                database.Users.Update(user);
            }

            raid.Id = ++Raid.Count;
            
            string mes = BuildString(raid);

            raid.ChannelAssemblyId = viewDiscordChannel.CreateChannel(
                raid.ChannelAssemblyId, (raid.TimeStart.ToShortTimeString() + " " + raid.CaptainName));

            raid.MessageId = viewDiscordChannel.SendMessage(mes, raid.ChannelAssemblyId);

            Raids raids = mapperRaidToRaids.Map<Raid, Raids>(raid);
            files.Add<Raids>(raids, FileTypes.CurrentRaids);

            currentRaids.Add(raid);
            viewDiscordChannel.AddReactionToMes(raid.MessageId, raid.ChannelAssemblyId, Reactions.HEART);

            return true;
        }

        private string BuildString(Raid raid)
        {
            return $"{resourceManager.GetString("RaidAddedTimeStartAssembly")}**`{raid.TimeStartAssembly.ToShortTimeString()}`**" +
                $"{resourceManager.GetString("RaidAddedCaptainName")}**`{raid.CaptainName}`**" +
                $"{resourceManager.GetString("RaidAddedTimeStart")}**`{raid.TimeStart.ToShortTimeString()}`**" +
                $"{resourceManager.GetString("RaidAddedChannel")}**`{raid.Channel}`**" +
                $"{resourceManager.GetString("RaidAddedUsersLeft")}**`{20 - raid.ReservedUsers - raid.UsersInRaid}`**" +
                $"{resourceManager.GetString("RaidAddedReactionToAdd")}{viewDiscordChannel.GetEmoji(Reactions.HEART)}";
        }

        public void ReactionHeartAdded(Message mes)
        {
            var raidFromFile = files.GetAll<Raids>(FileTypes.CurrentRaids).Result.FirstOrDefault(x => x.ChannelAssemblyId == mes.Channel.Id);

            if (raidFromFile == default)
            {
                return;
            }

            var raid = currentRaids.FirstOrDefault(x => x.Id == raidFromFile.Id);

            if (raid == default || (raid.UsersInRaid + raid.ReservedUsers) >= 20)
            {
                return;
            }

            var userFromFile = database.Users.GetAll().FirstOrDefault(x => x.IdDiscord == mes.SenderID);

            if (userFromFile == default)
            {
                return;
            }

            raid.UsersInRaid++;
            raid.Users.Add(mapperUsersToUser.Map<Users, User>(userFromFile));

            foreach (var item in raid.Users)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
