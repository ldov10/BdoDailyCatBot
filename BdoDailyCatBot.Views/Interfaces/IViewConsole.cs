using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.Views.Interfaces
{
    public interface IViewConsole
    {
        public string Message { get; }

        event Action SendMessage;
    }
}
