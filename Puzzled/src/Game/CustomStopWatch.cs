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
            m_Timer = new DispatcherTimer();// Microsoft timer
            m_Timer.Interval = TimeSpan.FromMilliseconds(10);// timer with a time span of 10 miliseconds
            m_Timer.Tick += Timer_Tick;// every 10 milisecond is calls Timer_Tick function
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = m_StopWatch.Elapsed;// calculates how much time passed 
            StartTimeDisplay = $"{elapsed.Minutes:00}:{elapsed.Seconds:00}";// shows how much time passed in minutes/seconds/miliseconds

            if (TimeUpdated != null)
            {
                TimeUpdated.Invoke(StartTimeDisplay);// activates a function in leveloverlay UpdateStopwatchDisplay and sends the time value
            }
                
        }

        public void Start()
        {
            if (!m_StopWatch.IsRunning)
            {
                m_StopWatch.Start();// start time/stopwatch
                m_Timer.Start();// start timer
            }
        }

        public void Pauze()
        {
            if (m_StopWatch.IsRunning)
            {
                m_StopWatch.Stop();// stops time/stopwatch
                m_Timer.Stop();// stop timer
            }
        }

        public void Reset()
        {
            m_StopWatch.Reset();// reset Microsoft Stopwatch
            StartTimeDisplay = "00:00";// changes the time value to 00:00

            if (TimeUpdated != null)
            {
                TimeUpdated.Invoke(StartTimeDisplay);// activates a function in leveloverlay UpdateStopwatchDisplay and sends the time value
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Helper methods
        ////////////////////////////////////////////////////////////////////////////////////
        public float Elapsed() // Note: In sseconds
        {
            return m_StopWatch.ElapsedMilliseconds / 1000.0f;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Stopwatch m_StopWatch = new Stopwatch();// Microsoft stopwatch
        private DispatcherTimer m_Timer;//  Microsoft timer

        public string StartTimeDisplay { get; private set; } = "00:00.00";// time text/value 

        public Action<string> TimeUpdated;
    }

}
