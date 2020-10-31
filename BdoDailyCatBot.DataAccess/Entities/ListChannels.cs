using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Entities
{
    class ListChannels
    {
        public List<Channels> channels { get; set; }

        public ListChannels()
        {
            channels = new List<Channels>();
        }
    }
}
