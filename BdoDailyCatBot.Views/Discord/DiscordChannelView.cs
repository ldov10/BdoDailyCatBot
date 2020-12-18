using BdoDailyCatBot.MainBot.Interfaces;
using BdoDailyCatBot.MainBot.Models;
using BdoDailyCatBot.Views.Entites;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BdoDailyCatBot.Views.Interfaces;
using System.Linq;
using Microsoft.VisualBasic;
using DSharpPlus.EventArgs;
using BdoDailyCatBot.Views.EventsArgs;

namespace BdoDailyCatBot.Views.Discord
{
    public class DiscordChannelView : IViewDiscordChannel
    {
        private readonly IBot Bot;
        public event Action<Message> MessageSended;
        public event Action<MessageReactionAddedEventArgs> MessageReactionAdded;
        public event Action<MessageReactionRemovedEventArgs> MessageReactionRemoved;

        public DiscordChannelView(IBot bot, string Prefix)
        {
            this.Bot = bot;

            Bot.MessageSended += MessageCreated;
            bot.MessageReactionAdded += ReactionAdded;
            bot.MessageReactionRemoved += ReactionRemoved;
        }
        public ulong SendMessage(string mes, ulong channelId)
        {
            return Bot.SendMessageAsync(Bot.GetChannelById(channelId), mes).Result;
        }

        public ulong SendEmbedMessage(string title, string fieldName, string fieldValue, ulong channelId)
        {
            return Bot.SendMessageAsync(Bot.GetChannelById(channelId), Bot.BuildEmbed(title, fieldName, fieldValue)).Result;
        }

        public void DeleteChannel(ulong channeld)
        {
            Bot.DeleteChannel(Bot.GetChannelById(channeld));
        }

        public bool DoesChannelExist(ulong id)
        {
            return Bot.DoesChannelExist(id);
        }

        public bool DoesMessageExist(ulong mesId, ulong channelId)
        {
            return Bot.DoesMessageExist(mesId, channelId).Result;
        }

        public void AddReactionToMes(Message mes, Reactions reaction)
        {
            Bot.AddReactionToMesAsync(Bot.GetMessageById(mes.ChannelId, mes.ID), reaction);
        }

        public void AddReactionToMes(ulong messageId, ulong channelId, Reactions reaction)
        {
            Bot.AddReactionToMesAsync(Bot.GetMessageById(channelId, messageId), reaction);
        }

        public string GetEmoji(Reactions reaction)
        {
            return Bot.GetEmoji(reaction);
        }

        public List<string> GetUserRoles(ulong userId, ulong guildId)
        {
            var member = Bot.GetMemberByIdAsync(userId, guildId).Result;

            return member.Roles.Select(x => x.Name).ToList();
        }

        public void EditMessage(ulong messageId, ulong channelId, string mes)
        {
            Bot.EditMessage(messageId, channelId, mes);
        }

        public void EditMessage(ulong messageId, ulong channelId, string embedTitle, string embedFieldName, string embedFieldValue)
        {
            var embed = Bot.BuildEmbed(embedTitle, embedFieldName, embedFieldValue);
            Bot.EditMessage(messageId, channelId, default, embed);
        }

        public void EditMessage(ulong messageId, ulong channelId, string embedTitle, string embedFieldName, string embedFieldValue, string message)
        {
            var embed = Bot.BuildEmbed(embedTitle, embedFieldName, embedFieldValue);
            Bot.EditMessage(messageId, channelId, message, embed);
        }

        public ulong GetGuildIdByChannel(ulong channelId)
        {
            return Bot.GetChannelById(channelId).GuildId;
        }

        public ulong CreateChannel(ulong channelNeighborId, string name)
        {
            var channel = Bot.GetChannelById(channelNeighborId);
            return Bot.CreateTextChannelAsync(channel.GuildId, name, channel).Result.Id;
        }

        private void MessageCreated(MessageCreateEventArgs e)
        {
            MessageSended?.Invoke(new Message() {ChannelId = e.Message.ChannelId, ChannelName = e.Message.Channel.Name, 
               Content = e.Message.Content, ID = e.Message.Id, SenderID = e.Message.Author.Id });
        }

        private void ReactionAdded(MessageReactionAddEventArgs e)
        {
            var mes = Bot.GetMessageById(e.Channel.Id, e.Message.Id);

            var message = new Message()
            {
                ChannelId = mes.ChannelId,
                ChannelName = mes.Channel.Name,
                Content = mes.Content,
                ID = mes.Id,
                SenderID = mes.Author.Id
            };

            Reactions reaction = Bot.GetEmojiDictionary().FirstOrDefault(x => x.Value.Name == e.Emoji.Name).Key;

            if (reaction == default)
            {
                reaction = Reactions.INVALID;
            }

            MessageReactionAdded?.Invoke(new MessageReactionAddedEventArgs() {Message = message, Reaction = reaction, ReactionSenderId = e.User.Id });
        }

        private void ReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            var mes = Bot.GetMessageById(e.Channel.Id, e.Message.Id);

            var message = new Message()
            {
                ChannelId = mes.ChannelId,
                ChannelName = mes.Channel.Name,
                Content = mes.Content,
                ID = mes.Id,
                SenderID = mes.Author.Id
            };

            Reactions reaction = Bot.GetEmojiDictionary().FirstOrDefault(x => x.Value.Name == e.Emoji.Name).Key;

            if (reaction == default)
            {
                reaction = Reactions.INVALID;
            }

            MessageReactionRemoved?.Invoke(new MessageReactionRemovedEventArgs() { Message = message, Reaction = reaction, ReactionSenderId = e.User.Id });
        }

    }
}
