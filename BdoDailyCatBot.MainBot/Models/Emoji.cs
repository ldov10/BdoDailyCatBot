using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.MainBot.Models
{
    public static class Emoji
    {
        public static Dictionary<Reactions, DiscordEmoji> EmojiDictionary = new Dictionary<Reactions, DiscordEmoji>()
        {
            [Reactions.OK] = DiscordEmoji.FromUnicode("✅"),
            [Reactions.NO] = DiscordEmoji.FromUnicode("❌")
        };
    }
}
