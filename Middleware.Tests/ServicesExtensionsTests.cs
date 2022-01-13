using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ecoAPM.Middleware.Tests;

public class ServicesExtensionsTests
{
	[Fact]
	public void CanSetupDI()
	{
		//arrange
		var di = new ServiceCollection();
		var config = new ConfigurationBuilder().Build();

		//act
		di.AddEcoAPM();

		//assert
		Assert.Contains(typeof(Middleware), di.Select(d => d.ImplementationType));
	}
}