using System;
using CoreAPM.Events.Model;

namespace CoreAPM.DotNet.Agent
{
    public interface IAgent : IDisposable
    {
        void Send(Event e);
    }
}