using Microsoft.AspNetCore.Builder;
using NSubstitute;
using Xunit;

namespace ecoAPM.Middleware.Tests;

public class MiddlewareExtensionsTests
{
	[Fact]
	public void CanCreateDefault()
	{
		//arrange
		var app = Substitute.For<IApplicationBuilder>();

		//act
		app.UseEcoAPM();

		//assert
		app.Received().UseMiddleware<Middleware>();
	}
}