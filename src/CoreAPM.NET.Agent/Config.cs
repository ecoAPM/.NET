using System;
using Microsoft.Extensions.Configuration;

namespace CoreAPM.NET.Agent
{
    public class Config : IConfig
    {
        public Uri EventsAPI { get; }
        public Guid APIKey { get; }

        public Config()
            : this(Environment.GetEnvironmentVariable("CoreAPM_EventsAPI"),
                Environment.GetEnvironmentVariable("CoreAPM_APIKey"))
        {
        }

        public Config(IConfiguration config)
            : this(config["CoreAPM:EventsAPI"] ?? config["CoreAPM_EventsAPI"],
                  config["CoreAPM:APIKey"] ?? config["CoreAPM_APIKey"])
        {
        }

        public Config(string eventsAPI, string apiKey)
            : this(new Uri(eventsAPI), new Guid(apiKey))
        {
        }

        public Config(Uri eventsAPI, Guid apiKey)
        {
            EventsAPI = eventsAPI;
            APIKey = apiKey;
        }
    }
}