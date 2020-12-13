using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.BusinessLogic.Utils
{
    public class TimerCallback
    {
        private Timer timer;

        public TimerCallback(DateTime alertTime, Action callback)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = alertTime - current;

            if (timeToGo < TimeSpan.Zero)
            {
                callback();
                return;
            }

            this.timer = new Timer(x => { callback(); }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }
    }
}
