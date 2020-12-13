using System;
using BdoDailyCatBot.MainBot.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using BdoDailyCatBot.Views.Entites;
using BdoDailyCatBot.Views.EventsArgs;

namespace BdoDailyCatBot.Views.Interfaces
{
    public interface IViewDiscordChannel
    {
        ulong SendMessage(string mes, ulong channelId);
        ulong SendEmbedMessage(string title, string fieldName, string fieldValue, ulong channelId);
        void EditMessage(ulong messageId, ulong channelId, string mes);
        void EditMessage(ulong messageId, ulong channelId, string embedTitle, string embedFieldName, string embedFieldValue);
        void EditMessage(ulong messageId, ulong channelId, string embedTitle, string embedFieldName, string embedFieldValue, string message);
        void AddReactionToMes(Message mes, Reactions reaction);
        void AddReactionToMes(ulong messageId, ulong channelId, Reactions reaction);
        ulong CreateChannel(ulong channelNeighborId, string channelName);
        List<string> GetUserRoles(ulong userId, ulong guildId);
        ulong GetGuildIdByChannel(ulong channelId);
        string GetEmoji(Reactions reaction);
        void DeleteChannel(ulong channeld);


        event Action<Message> MessageSended;
        event Action<MessageReactionAddedEventArgs> MessageReactionAdded;
        event Action<MessageReactionRemovedEventArgs> MessageReactionRemoved;
    }
}
