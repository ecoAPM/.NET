using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Xunit;

namespace CoreAPM.DotNet.Agent.Tests
{
    public class ConfigTests
    {
        [Fact]
        public void CanCreateFromEnvVars()
        {
            //arrange
            var url = "http://localhost/123";
            var apiKey = Guid.NewGuid();
            Environment.SetEnvironmentVariable("CoreAPM_EventsAPI", url);
            Environment.SetEnvironmentVariable("CoreAPM_APIKey", apiKey.ToString());

            //act
            var config = new Config();

            //assert
            Assert.Equal(url, config.EventsAPI.AbsoluteUri);
            Assert.Equal(apiKey, config.APIKey);
        }

        [Fact]
        public void CanCreateFromStringArgs()
        {
            //arrange
            var url = "http://localhost/123";
            var apiKey = Guid.NewGuid();

            //act
            var config = new Config(url, apiKey.ToString());

            //assert
            Assert.Equal(url, config.EventsAPI.AbsoluteUri);
            Assert.Equal(apiKey, config.APIKey);
        }

        [Fact]
        public void CanCreateFromTypedArgs()
        {
            //arrange
            var url = new Uri("http://localhost/123");
            var apiKey = Guid.NewGuid();

            //act
            var config = new Config(url, apiKey);

            //assert
            Assert.Equal(url, config.EventsAPI);
            Assert.Equal(apiKey, config.APIKey);
        }

        [Fact]
        public void CanCreateFromConfigurationRoot()
        {
            //arrange
            var url = "http://localhost/123";
            var apiKey = Guid.NewGuid();
            var baseConfig = new ConfigurationBuilder();
            var configSource = new MemoryConfigurationSource
            {
                InitialData = new Dictionary<string, string>
                {
                    {"CoreAPM_EventsAPI", url},
                    {"CoreAPM_APIKey", apiKey.ToString()}
                }
            };
            baseConfig.Add(configSource);

            //act
            var config = new Config(baseConfig.Build());

            //assert
            Assert.Equal(url, config.EventsAPI.AbsoluteUri);
            Assert.Equal(apiKey, config.APIKey);
        }

        [Fact]
        public void CanCreateFromNestedConfigurationRoot()
        {
            //arrange
            var url = "http://localhost/123";
            var apiKey = Guid.NewGuid();
            var baseConfig = new ConfigurationBuilder();
            var configSource = new MemoryConfigurationSource
            {
                InitialData = new Dictionary<string, string>
                {
                    {"CoreAPM:EventsAPI", url},
                    {"CoreAPM:APIKey", apiKey.ToString()}
                }
            };
            baseConfig.Add(configSource);

            //act
            var config = new Config(baseConfig.Build());

            //assert
            Assert.Equal(url, config.EventsAPI.AbsoluteUri);
            Assert.Equal(apiKey, config.APIKey);
        }
    }
}