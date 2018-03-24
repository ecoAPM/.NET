using System;

namespace CoreAPM.NET.Agent
{
    public interface ITimer
    {
        double CurrentTime { get; }

        void Start();
        void Stop();
        double Time(Action action);
    }
}