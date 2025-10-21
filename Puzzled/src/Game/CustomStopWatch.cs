using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace Puzzled
{
    public class CustomStopWatch
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public CustomStopWatch()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Tick += Timer_Tick;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = _stopwatch.Elapsed;
            StartTimeDisplay = $"{elapsed.Minutes:00}:{elapsed.Seconds:00}.{elapsed.Milliseconds / 10:00}";

            if (TimeUpdated != null)
            {
                TimeUpdated.Invoke(StartTimeDisplay);
            }
                
        }

        public void StopWatchStart()
        {
            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
                _timer.Start();
            }
        }

        public void StopWatchPauze()
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Stop();
                _timer.Stop();
            }
        }

        public void StopWatchReset()
        {
            _stopwatch.Reset();
            StartTimeDisplay = "00:00.00";

            if (TimeUpdated != null)
            {
                TimeUpdated.Invoke(StartTimeDisplay);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Stopwatch _stopwatch = new Stopwatch();
        private DispatcherTimer _timer;

        public string StartTimeDisplay { get; private set; } = "00:00.00";

        public event Action<string> TimeUpdated;
    }
}
