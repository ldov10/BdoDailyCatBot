using BdoDailyCatBot.BusinessLogic.BusinessModels;
using BdoDailyCatBot.DataAccess.Interfaces;
using BdoDailyCatBot.MainBot.EventsArgs;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Views.Interfaces;
using BdoDailyCatBot.BusinessLogic.DTO;
using AutoMapper;
using System.Linq;
using BdoDailyCatBot.MainBot;

namespace BdoDailyCatBot.BusinessLogic.Services
{
    public class DiscordMessagesService
    {
        private readonly IViewDiscordChannel viewDiscordChannel;
        private readonly IChannels channels;
        private readonly IUnitOfWork dataBase;

        public string Prefix { get; private set; }
        public string NamePattern { get; private set; }

        public DiscordMessagesService(string prefix, string namePattern, IViewDiscordChannel viewDiscordChannel, IChannels channels, IUnitOfWork dataBase)
        {
            this.viewDiscordChannel = viewDiscordChannel;
            this.channels = channels;
            this.Prefix = prefix;
            this.NamePattern = namePattern;
            this.dataBase = dataBase;

            viewDiscordChannel.MessageSended += MessageSended;
        }

        private void MessageSended(MessageSendedEventArgs e)
        {
            Message mes = new Message() 
            { Content = e.Message.Content, Channel = new Channel() { Id = e.Message.ChannelId, Name = e.Message.Channel.Name }, SenderID = e.Message.Author.Id }; // TODO: Automap?

            if (mes.Content.StartsWith(Prefix))
            {
                if (mes.Content == (Prefix + "рег_тут")) // TODO: change select
                {
                    viewDiscordChannel.AddReactionToMes(e, AddChannelToReg(mes.Channel));
                }

                if (Regex.IsMatch(mes.Content, $"{Prefix}рег .+"))
                {
                    if (channels.GetAll(DataAccess.Entities.FilesType.ChannelsToReg).Result.Contains
                        (new DataAccess.Entities.Channels(e.Message.ChannelId, e.Message.Channel.Name)))
                    {
                        viewDiscordChannel.AddReactionToMes(e, false);
                    }
                    else
                    {
                        return;
                    }
                    string Name = Regex.Match(mes.Content, NamePattern).Value;

                    var flag = AddUserToDB(new UserDTO() 
                    {IdDiscord = mes.SenderID, IsCaptain = false, LastRaidDate = null, Name = Name, RaidsVisited = 0 });

                    viewDiscordChannel.AddReactionToMes(e, flag);
                }
            }
        }

        private bool AddUserToDB(UserDTO user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, Users>());
            var mapper = new Mapper(config);
            Users users = mapper.Map<UserDTO, Users>(user);
            var flag =  dataBase.Users.Add(users);
            dataBase.Save();
            return flag;
        }

        private bool AddChannelToReg(Channel channel)
        {
            var result = channels.Add(new DataAccess.Entities.Channels(channel.Id, channel.Name), DataAccess.Entities.FilesType.ChannelsToReg); // TODO: Automap?
            return result.Result.Item1;
        }
    }
}
