using Microsoft.AspNetCore.Builder;
using NSubstitute;
using Xunit;

namespace ecoAPM.NET.CoreMiddleware.Tests;

public class ecoAPMMiddlewareExtensionsTests
{
	[Fact]
	public void CanCreateDefault()
	{
		//arrange
		var app = Substitute.For<IApplicationBuilder>();

		//act
		app.UseecoAPM();

		//assert
		app.Received().UseMiddleware<ecoAPMMiddleware>();
	}
}