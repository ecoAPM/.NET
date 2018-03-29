using System;
using Microsoft.Extensions.Configuration;

namespace CoreAPM.NET.Agent
{
    public class ServerConfig : IServerConfig
    {
        public Uri BaseURL { get; }
        public Guid APIKey { get; }

        public ServerConfig()
            : this(envBaseURL, envAPIKey)
        {
        }

        public ServerConfig(IConfiguration config)
            : this(config["CoreAPM:BaseURL"] ?? config["CoreAPM_BaseURL"] ?? envBaseURL,
                  config["CoreAPM:APIKey"] ?? config["CoreAPM_APIKey"] ?? envAPIKey)
        {
        }

        public ServerConfig(string baseURL, string apiKey)
            : this(new Uri(baseURL ?? envBaseURL), new Guid(apiKey ?? envAPIKey))
        {
        }

        public ServerConfig(Uri baseURL, Guid? apiKey)
        {
            BaseURL = baseURL ?? new Uri(envBaseURL);
            APIKey = apiKey ?? new Guid(envAPIKey);
        }

        private static readonly string envBaseURL = Environment.GetEnvironmentVariable("CoreAPM_BaseURL");
        private static readonly string envAPIKey = Environment.GetEnvironmentVariable("CoreAPM_APIKey");
    }
}