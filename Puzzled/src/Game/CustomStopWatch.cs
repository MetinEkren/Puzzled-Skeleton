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
            m_Timer = new DispatcherTimer();
            m_Timer.Interval = TimeSpan.FromMilliseconds(10);
            m_Timer.Tick += Timer_Tick;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = m_StopWatch.Elapsed;
            StartTimeDisplay = $"{elapsed.Minutes:00}:{elapsed.Seconds:00}.{elapsed.Milliseconds / 10:00}";

            if (TimeUpdated != null)
            {
                TimeUpdated.Invoke(StartTimeDisplay);
            }
                
        }

        public void Start()
        {
            if (!m_StopWatch.IsRunning)
            {
                m_StopWatch.Start();
                m_Timer.Start();
            }
        }

        public void Pauze()
        {
            if (m_StopWatch.IsRunning)
            {
                m_StopWatch.Stop();
                m_Timer.Stop();
            }
        }

        public void Reset()
        {
            m_StopWatch.Reset();
            StartTimeDisplay = "00:00.00";

            if (TimeUpdated != null)
            {
                TimeUpdated.Invoke(StartTimeDisplay);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Stopwatch m_StopWatch = new Stopwatch();
        private DispatcherTimer m_Timer;

        public string StartTimeDisplay { get; private set; } = "00:00.00";

        public Action<string> TimeUpdated;
    }

}
