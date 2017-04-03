using System;
using System.Collections.Generic;
using CoreAPM.DotNet.Agent;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using NSubstitute;
using Xunit;

namespace CoreAPM.DotNet.ASPNETCoreMiddleware.Tests
{
    public class CoreAPMMiddlewareExtensionsTests
    {
        [Fact]
        public void CanCreateDefault()
        {
            //arrange
            var app = Substitute.For<IApplicationBuilder>();

            //act
            app.UseCoreAPM();

            //assert
            app.Received().UseMiddleware<CoreAPMMiddleware>();
        }

        [Fact]
        public void CanCreateWithConfig()
        {
            //arrange
            var app = Substitute.For<IApplicationBuilder>();
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
            app.UseCoreAPM(baseConfig.Build());

            //assert
            app.Received().UseMiddleware<CoreAPMMiddleware>(Arg.Any<IConfig>());
        }
    }
}