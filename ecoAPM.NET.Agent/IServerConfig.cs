﻿using System;

namespace ecoAPM.NET.Agent
{
    public interface IServerConfig
    {
        Uri BaseURL { get; }
        Guid APIKey { get; }
    }
}