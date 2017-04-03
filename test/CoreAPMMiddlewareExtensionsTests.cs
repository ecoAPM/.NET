using System;
using System.Linq;
using CoreAPM.DotNet.Agent;
using Microsoft.AspNetCore.Builder;
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
            var config = new Config(new Uri("http://localhost"), Guid.NewGuid());

            //act
            app.UseCoreAPM(config);

            //assert
            app.Received().UseMiddleware<CoreAPMMiddleware>(config);
        }
    }
}