using System;
using Microsoft.Extensions.Configuration;

namespace ecoAPM.NET.Agent
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
            : this(config["ecoAPM:BaseURL"] ?? config["ecoAPM_BaseURL"] ?? envBaseURL,
                  config["ecoAPM:APIKey"] ?? config["ecoAPM_APIKey"] ?? envAPIKey)
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

        private static readonly string envBaseURL = Environment.GetEnvironmentVariable("ecoAPM_BaseURL");
        private static readonly string envAPIKey = Environment.GetEnvironmentVariable("ecoAPM_APIKey");
    }
}