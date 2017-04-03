using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreAPM.DotNet.ASPNETCoreMiddleware.Tests
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
    }
}