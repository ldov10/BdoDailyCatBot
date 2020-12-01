using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.MainBot.Models
{
    public class Emoji
    {
        private DiscordClient client;

        public Dictionary<Reactions, DiscordEmoji> EmojiDictionary { get; }

        public Emoji(DiscordClient client)
        {
            this.client = client;

            EmojiDictionary = new Dictionary<Reactions, DiscordEmoji>()
            {
                [Reactions.OK] = DiscordEmoji.FromUnicode("✅"),
                [Reactions.NO] = DiscordEmoji.FromUnicode("❌"),
                [Reactions.HEART] = DiscordEmoji.FromName(client, ":sparkling_heart:")
            };
        }
    }

 }

