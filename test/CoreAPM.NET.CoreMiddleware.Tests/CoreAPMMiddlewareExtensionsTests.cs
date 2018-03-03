using System;
using System.Collections.Generic;
using CoreAPM.DotNet.Agent;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using NSubstitute;
using Xunit;

namespace CoreAPM.DotNet.AspNetCoreMiddleware.Tests
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
    }
}