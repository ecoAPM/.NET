using System;

namespace CoreAPM.NET.Agent
{
    public interface IConfig
    {
        Uri BaseURL { get; }
        Guid APIKey { get; }
    }
}