using System;

namespace CoreAPM.DotNet.Agent
{
    public interface IConfig
    {
        Uri EventsAPI { get; }
        Guid APIKey { get; }
    }
}