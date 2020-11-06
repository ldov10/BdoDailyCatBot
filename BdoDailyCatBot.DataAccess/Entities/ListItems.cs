using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Entities
{
    class ListItems<T>
    {
        public List<T> listItems { get; set; }

        public ListItems()
        {
            listItems = new List<T>();
        }
    }
}
