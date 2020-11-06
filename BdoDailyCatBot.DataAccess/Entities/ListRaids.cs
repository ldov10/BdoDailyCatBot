using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Entities
{
    class ListRaids
    {
        public List<Raids> raids { get; set; }

        public ListRaids()
        {
            raids = new List<Raids>();
        }
    }
}
