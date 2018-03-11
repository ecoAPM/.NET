using System;
using Microsoft.Extensions.Configuration;

namespace CoreAPM.NET.Agent
{
    public class Config : IConfig
    {
        public Uri BaseURL { get; }
        public Guid APIKey { get; }

        public Config()
            : this(Environment.GetEnvironmentVariable("CoreAPM_BaseURL"),
                Environment.GetEnvironmentVariable("CoreAPM_APIKey"))
        {
        }

        public Config(IConfiguration config)
            : this(config["CoreAPM:BaseURL"] ?? config["CoreAPM_BaseURL"],
                  config["CoreAPM:APIKey"] ?? config["CoreAPM_APIKey"])
        {
        }

        public Config(string baseURL, string apiKey)
            : this(new Uri(baseURL), new Guid(apiKey))
        {
        }

        public Config(Uri baseURL, Guid apiKey)
        {
            BaseURL = baseURL;
            APIKey = apiKey;
        }
    }
}