using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ScopeSignalControl
{
    class TimerClass
    {
        //Dispacher timer allow access to UI thread from timer thread
        public DispatcherTimer dispatcherTimer = null;

        public int RefreshTime { get; private set; } = 1;

        public TimerClass()
        {
            SetupTimer();
        }

        /// <summary>
        /// DispacherTimer allow access cross threading draw to canvas from timer thread
        /// </summary>
        private void SetupTimer()
        {
            //  DispatcherTimer setup
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, RefreshTime);
            dispatcherTimer.Start();
        }
    }
}
