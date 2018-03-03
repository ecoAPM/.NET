using System;

namespace CoreAPM.DotNet.Agent
{
    public interface IAgent : IDisposable
    {
        void Send(Event e);
    }
}