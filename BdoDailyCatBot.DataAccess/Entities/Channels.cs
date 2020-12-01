using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Entities
{
    public class Channels : ToFile
    {
        public Channels(ulong id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public ulong Id { get; private set; }
        public string Name { get; private set; }

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
