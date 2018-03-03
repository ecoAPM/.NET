using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreAPM.DotNet.AspNetCoreMiddleware.Tests
{
    public class CoreAPMServicesExtensionsTests
    {
        [Fact]
        public void CanSetupDI()
        {
            //arrange
            var di = new ServiceCollection();

            //act
            di.AddCoreAPM();

            //assert
            Assert.Contains(typeof(CoreAPMMiddleware), di.Select(d => d.ImplementationType));
        }

        [Fact]
        public void CanSetupWithConfig()
        {
            //arrange
            var di = new ServiceCollection();
            var baseConfig = new ConfigurationBuilder();
            var configSource = new MemoryConfigurationSource
            {
                InitialData = new Dictionary<string, string>
                {
                    {"CoreAPM:EventsAPI", "http://localhost"},
                    {"CoreAPM:APIKey", Guid.NewGuid().ToString()}
                }
            };
            baseConfig.Add(configSource);

            //act
            di.AddCoreAPM(baseConfig.Build());

            //assert
            Assert.Contains(typeof(IConfiguration), di.Select(d => d.ServiceType));
        }
    }
}