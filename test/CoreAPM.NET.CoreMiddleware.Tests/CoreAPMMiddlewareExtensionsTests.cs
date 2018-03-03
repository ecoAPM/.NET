using Microsoft.AspNetCore.Builder;
using NSubstitute;
using Xunit;

namespace CoreAPM.NET.CoreMiddleware.Tests
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