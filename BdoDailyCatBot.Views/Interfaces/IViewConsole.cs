using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.Views.Interfaces
{
    public interface IViewConsole
    {
        public string Message { get; }

        void SendConsoleMessage(string mes);

        event Action SendMessage;
    }
}
