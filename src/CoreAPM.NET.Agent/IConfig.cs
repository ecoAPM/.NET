using System;

namespace CoreAPM.NET.Agent
{
    public interface IConfig
    {
        Uri EventsAPI { get; }
        Guid APIKey { get; }
    }
}