using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreAPM.NET.CoreMiddleware.Tests
{
    public class CoreAPMServicesExtensionsTests
    {
        [Fact]
        public void CanSetupDI()
        {
            //arrange
            var di = new ServiceCollection();
            var config = new ConfigurationBuilder().Build();

            //act
            di.AddCoreAPM(config);

            //assert
            Assert.Contains(typeof(CoreAPMMiddleware), di.Select(d => d.ImplementationType));
        }
    }
}