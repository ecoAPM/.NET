using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ecoAPM.NET.CoreMiddleware.Tests
{
    public class ecoAPMServicesExtensionsTests
    {
        [Fact]
        public void CanSetupDI()
        {
            //arrange
            var di = new ServiceCollection();
            var config = new ConfigurationBuilder().Build();

            //act
            di.AddecoAPM(config);

            //assert
            Assert.Contains(typeof(ecoAPMMiddleware), di.Select(d => d.ImplementationType));
        }
    }
}