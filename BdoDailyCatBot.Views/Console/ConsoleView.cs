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
            ConsoleListnerAsync().ConfigureAwait(false);
        }

        private async Task ConsoleListnerAsync()
        {
            while (true)
            {
                this.Message = await System.Console.In.ReadLineAsync();

                SendMessage?.Invoke();
            }
        }
    }
}
