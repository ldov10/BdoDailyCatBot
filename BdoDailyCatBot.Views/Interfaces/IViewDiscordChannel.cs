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
        void AddReactionToMes(Message mes, Reactions reaction);
        void AddReactionToMes(ulong messageId, ulong channelId, Reactions reaction);
        ulong CreateChannel(ulong channelNeighborId, string channelName);
        List<string> GetUserRoles(ulong userId, ulong guildId);
        ulong GetGuildIdByChannel(ulong channelId);
        string GetEmoji(Reactions reaction);

        event Action<Message> MessageSended;
        event Action<MessageReactionAddedEventArgs> MessageReactionAdded;
    }
}
