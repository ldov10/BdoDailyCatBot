﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using BdoDailyCatBot.DataAccess.Interfaces;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Entities
{
    public class Channels : IToFile
    {
        public Channels(ulong id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public ulong Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Channels objAsChannels = obj as Channels;

            if (objAsChannels == null)
            {
                return false;
            }

            if (objAsChannels.Id == this.Id)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
