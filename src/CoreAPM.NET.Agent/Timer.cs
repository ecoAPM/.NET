using System;
using System.Diagnostics;

namespace CoreAPM.NET.Agent
{
    public class Timer : ITimer
    {
        private readonly Stopwatch _stopwatch;

        public Timer(Stopwatch stopwatch = null)
        {
            _stopwatch = stopwatch ?? new Stopwatch();
        }

        public double Time(Action action)
        {
            Start();
            action.Invoke();
            Stop();
            return CurrentTime;
        }

        public void Start() => _stopwatch.Start();
        public void Stop() => _stopwatch.Stop();
        public double CurrentTime => _stopwatch.Elapsed.TotalMilliseconds;
    }
}
