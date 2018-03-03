using System;

namespace CoreAPM.DotNet.Agent
{
    public interface ITimer
    {
        double CurrentTime { get; }

        void Start();
        void Stop();
        double Time(Action action);
    }
}