using System;

namespace CoreAPM.NET.Agent
{
    public interface IAgent : IDisposable
    {
        void Send(Event e);
    }
}