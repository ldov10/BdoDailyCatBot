using AutoMapper;
using BdoDailyCatBot.BusinessLogic.BusinessModels;
using BdoDailyCatBot.DataAccess.Entities;
using BdoDailyCatBot.DataAccess.Interfaces;
using BdoDailyCatBot.MainBot.Models;
using BdoDailyCatBot.BusinessLogic.Interfaces;
using BdoDailyCatBot.Views.Interfaces;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using BdoDailyCatBot.BusinessLogic.Utils;
using System.Linq;
using System.Resources;
using System.Text;

namespace BdoDailyCatBot.BusinessLogic.Services
{
    public class RaidsService : IRaidsService
    {
        private readonly IFilesRepository files;
        private readonly ResourceManager resourceManager;
        private readonly IViewDiscordChannel viewDiscordChannel;
        private readonly IUnitOfWork database;

        private readonly string captainRoleName = "";
        private readonly Mapper mapperRaidToRaids;
        private readonly Mapper mapperUserToUsers;
        private readonly Mapper mapperUsersToUser;
        private readonly Mapper mapperRaidsToRaid;

        private static RaidsService instance;

        private static List<Raid> currentRaids = new List<Raid>();

        public RaidsService(IFilesRepository files, ResourceManager resource, IViewDiscordChannel viewDiscordChannel, IUnitOfWork database)
        {
            RaidsService.instance = this;

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
                cfg.AllowNullCollections = true;
                cfg.CreateMap<Raids, Raid>();
                cfg.CreateMap<Users, User>();
            });
            this.mapperRaidsToRaid = new Mapper(config);

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

            LoadRaidsFromFie();
        }

        public static RaidsService GetInstance()
        {
            return RaidsService.instance;
        }

        public bool AddRaid(Raid raid, ulong senderId, ulong channelSenderId)
        {
            var roles = viewDiscordChannel.GetUserRoles(senderId, viewDiscordChannel.GetGuildIdByChannel(channelSenderId));
            if (!roles.Contains(captainRoleName))
            {
                return false;
            }

            var captain = database.Users.GetAll().FirstOrDefault(p => p.IdDiscord == senderId);

            if (captain == default)
            {
                return false;
            }

            raid.CaptainName = captain.Name.Trim();

            if (!captain.IsCaptain)
            {
                captain.IsCaptain = true;
                database.Users.Update(captain);
                database.Captains.Add(new Captains() { DroveRaids = 0, LastDrivenRaid = null, User = captain });
                database.Save();
            }

            raid.Id = GenerateId();
            raid.CaptainUserId = captain.Id;
            
            string mes = BuildString(raid);

            raid.ChannelAssemblyId = viewDiscordChannel.CreateChannel(
                raid.ChannelAssemblyId, (raid.TimeStart.ToShortTimeString() + " " + raid.CaptainName));

            raid.MessageId = viewDiscordChannel.SendMessage(mes, raid.ChannelAssemblyId);
            raid.TableMessageId = default;

            raid.timerAssembly = new TimerCallback(raid.TimeStartAssembly, raid.SetIsAssemblingTrue);
            raid.timerStart = new TimerCallback(raid.TimeStart, raid.Start);
            raid.timerHourAfterStart = new TimerCallback(raid.TimeStart + new TimeSpan(1, 0, 0), raid.HourAfterStart);

            currentRaids.Add(raid);
            if (viewDiscordChannel.DoesMessageExist(raid.MessageId, raid.ChannelAssemblyId))
            {
                viewDiscordChannel.AddReactionToMes(raid.MessageId, raid.ChannelAssemblyId, Reactions.HEART);
            }

            Raids raids = mapperRaidToRaids.Map<Raid, Raids>(raid);
            files.Add<Raids>(raids, FileTypes.CurrentRaids);

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

        private string BuildStringWhenStart()
        {
            string result = $"**{resourceManager.GetString("RaidWhenStart")}**";

            return result;
        }

        public void SetAssemblingTrue(Raid raid)
        {
            if (raid.IsAssembling == true)
            {
                return;
            }

            raid.IsAssembling = true;

            Raids raids = mapperRaidToRaids.Map<Raid, Raids>(raid);
            files.Update<Raids>(raids, FileTypes.CurrentRaids);

            SendRaidTable(raid);
        }

        public void HourAfterStart(Raid raid)
        {
            DeleteRaid(raid.Id);
        }

        public void DeleteRaid(ulong raidId)
        {
            var raid = currentRaids.Find(x => x.Id == raidId);

            files.Delete(mapperRaidToRaids.Map<Raid, Raids>(raid), FileTypes.CurrentRaids);
            if (currentRaids.Contains(raid))
            {
                currentRaids.Remove(raid);
            }
            if (viewDiscordChannel.DoesChannelExist(raid.ChannelAssemblyId))
            {
                viewDiscordChannel.DeleteChannel(raid.ChannelAssemblyId);
            }
        }

        public ulong GetRaidId(ulong channelAssemblyId)
        {
            var raid = currentRaids.FirstOrDefault(x => x.ChannelAssemblyId == channelAssemblyId);
            if (raid == default)
            {
                return default;
            }
            return raid.Id;
        }

        public void StartRaid(Raid raid)
        {
            if (raid.IsStarted == true)
            {
                return;
            }

            raid.IsStarted = true;
            Raids raids = mapperRaidToRaids.Map<Raid, Raids>(raid);
            files.Update<Raids>(raids, FileTypes.CurrentRaids);

            if (viewDiscordChannel.DoesChannelExist(raid.ChannelAssemblyId))
            {
                viewDiscordChannel.SendMessage(BuildStringWhenStart(), raid.ChannelAssemblyId);
            }

            var cap = database.Captains.GetAll().FirstOrDefault(x => x.UserId == raid.CaptainUserId);
            if (cap != default)
            {
                cap.LastDrivenRaid = raid.TimeStart;
                cap.DroveRaids++;
                database.Captains.Update(cap);
            }
            database.Save();
            foreach (var item in raid.Users)
            {
                item.LastRaidDate = raid.TimeStart;
                item.RaidsVisited++;
                database.Users.Update(mapperUserToUsers.Map<User, Users>(item));
            }
            database.Save();
        }

        public void ReactionHeartChanged(Message mes, ulong ReactionSenderId, bool heartAdded)
        {
            var raidFromFile = files.GetAll<Raids>(FileTypes.CurrentRaids).Result.FirstOrDefault(x => x.ChannelAssemblyId == mes.Channel.Id);

            if (raidFromFile == default)
            {
                return;
            }

            var raid = currentRaids.FirstOrDefault(x => x.Id == raidFromFile.Id);

            if (raid == default)
            {
                return;
            }

            if (raid.IsAssembling == false)
            {
                return;
            }

            if (raid.IsStarted == true)
            {
                return;
            }

            if (raid.UsersInRaid + raid.ReservedUsers >= 20)
            {
                return;
            }

            var userFromDB = database.Users.GetAll().FirstOrDefault(x => x.IdDiscord == ReactionSenderId);

            if (userFromDB == default)
            {
                return;
            }

            userFromDB.Name = userFromDB.Name.Trim();

            if (heartAdded)
            {
                if (raid.Users.Contains(new User() { IdDiscord = ReactionSenderId }))
                {
                    return;
                }

                raid.UsersInRaid++;
                raid.Users.Add(mapperUsersToUser.Map<Users, User>(userFromDB));
                files.Update(mapperRaidToRaids.Map<Raid, Raids>(raid), FileTypes.CurrentRaids);
            }
            else
            {
                if (!raid.Users.Contains(new User() { IdDiscord = ReactionSenderId }))
                {
                    return;
                }

                raid.UsersInRaid--;
                raid.Users.Remove(mapperUsersToUser.Map<Users, User>(userFromDB));
                files.Update(mapperRaidToRaids.Map<Raid, Raids>(raid), FileTypes.CurrentRaids);
            }

            SendRaidTable(raid);
            EditMainString(raid);
        }

        private void EditMainString(Raid raid)
        {
            if (viewDiscordChannel.DoesMessageExist(raid.MessageId, raid.ChannelAssemblyId))
            {
                viewDiscordChannel.EditMessage(raid.MessageId, raid.ChannelAssemblyId, BuildString(raid));
            }
        }

        private void SendRaidTable(Raid raid)
        {
            if (viewDiscordChannel.DoesChannelExist(raid.ChannelAssemblyId))
            {
                string title = raid.CaptainName + " " + raid.TimeStart.ToShortTimeString() + " " + raid.Channel;

                if (raid.TableMessageId == default)
                {
                    raid.TableMessageId = viewDiscordChannel.SendEmbedMessage(title, "\u200B", BuildStringTable(raid), raid.ChannelAssemblyId);
                    Raids raids = mapperRaidToRaids.Map<Raid, Raids>(raid);
                    files.Update<Raids>(raids, FileTypes.CurrentRaids);
                }
                else
                {
                    viewDiscordChannel.EditMessage(raid.TableMessageId, raid.ChannelAssemblyId, title, "\u200B", BuildStringTable(raid));
                }
            }
        }

        private string BuildStringTable(Raid raid)
        {
            StringBuilder result = new StringBuilder();

            int count = 1;
            foreach (var item in raid.Users)
            {
                result.Append(count);
                result.Append(". ");
                result.Append(item.Name);
                result.Append("\n");

                count++;
            }

            if (result.ToString() == "")
            {
                result.Clear();
                result.Append("\u200B");
            }

            return result.ToString();
        }

        private ulong GenerateId()
        {
            Random rnd = new Random();
            byte[] buf = new byte[8];
            rnd.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (ulong)Math.Abs(longRand);
        }

        public void LoadRaidsFromFie()
        {
            var raids = files.GetAll<Raids>(FileTypes.CurrentRaids).Result;
            var mappedRaids = mapperRaidsToRaid.Map<List<Raids>, List<Raid>>(raids);

            foreach (var item in mappedRaids)
            {
                if (currentRaids.FindAll(x => x.Id == item.Id).Count == 0)
                {
                    currentRaids.Add(item);
                    item.timerAssembly = new TimerCallback(item.TimeStartAssembly, item.SetIsAssemblingTrue);
                    item.timerStart = new TimerCallback(item.TimeStart, item.Start);
                    item.timerHourAfterStart = new TimerCallback(item.TimeStart + new TimeSpan(1, 0, 0), item.HourAfterStart);
                }
            }
        }
    }
}
