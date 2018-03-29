using System;

namespace CoreAPM.NET.Agent
{
    public interface IServerConfig
    {
        Uri BaseURL { get; }
        Guid APIKey { get; }
    }
}