using System;

namespace ecoAPM.NET.Agent
{
    public interface IAgent : IDisposable
    {
        void Send(Event e);
    }
}