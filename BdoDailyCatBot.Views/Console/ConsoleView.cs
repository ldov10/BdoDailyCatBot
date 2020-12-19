using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BdoDailyCatBot.Views.Interfaces;

namespace BdoDailyCatBot.Views.Console
{
    public class ConsoleView : IViewConsole
    {
        public string Message { get; private set; } = "";

        public event Action SendMessage;

        public void RunConsoleListner()
        {
            ConsoleListner();
        }

        public void SendConsoleMessage(string mes)
        {
            System.Console.WriteLine(mes);
        }

        private void ConsoleListner()
        {
            while (true)
            {
                this.Message =  System.Console.ReadLine();

                SendMessage?.Invoke();
            }
        }
    }
}
