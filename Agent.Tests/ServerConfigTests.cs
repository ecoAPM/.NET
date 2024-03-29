using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Xunit;

namespace ecoAPM.Agent.Tests;

public class ServerConfigTests
{
	[Fact]
	public void CanCreateFromEnvVars()
	{
		//arrange
		var url = "http://localhost/123";
		var apiKey = Guid.NewGuid().ToString();
		Environment.SetEnvironmentVariable("ecoAPM_BaseURL", url);
		Environment.SetEnvironmentVariable("ecoAPM_APIKey", apiKey);

		//act
		var config = new ServerConfig();

		//assert
		Assert.Equal(url, config.BaseURL.AbsoluteUri);
		Assert.Equal(apiKey, config.APIKey);
	}

	[Fact]
	public void CanCreateFromStringArgs()
	{
		//arrange
		var url = "http://localhost/123";
		var apiKey = Guid.NewGuid().ToString();

		//act
		var config = new ServerConfig(url, apiKey);

		//assert
		Assert.Equal(url, config.BaseURL.AbsoluteUri);
		Assert.Equal(apiKey, config.APIKey);
	}

	[Fact]
	public void CanCreateFromTypedArgs()
	{
		//arrange
		var url = new Uri("http://localhost/123");
		var apiKey = Guid.NewGuid().ToString();

		//act
		var config = new ServerConfig(url, apiKey);

		//assert
		Assert.Equal(url, config.BaseURL);
		Assert.Equal(apiKey, config.APIKey);
	}

	[Fact]
	public void CanCreateFromConfigurationRoot()
	{
		//arrange
		var url = "http://localhost/123";
		var apiKey = Guid.NewGuid().ToString();
		var baseConfig = new ConfigurationBuilder();
		var configSource = new MemoryConfigurationSource
		{
			InitialData = new Dictionary<string, string?>
			{
				{ "ecoAPM_BaseURL", url },
				{ "ecoAPM_APIKey", apiKey }
			}
		};
		baseConfig.Add(configSource);

		//act
		var config = new ServerConfig(baseConfig.Build());

		//assert
		Assert.Equal(url, config.BaseURL.AbsoluteUri);
		Assert.Equal(apiKey, config.APIKey);
	}

	[Fact]
	public void CanCreateFromNestedConfigurationRoot()
	{
		//arrange
		var url = "http://localhost/123";
		var apiKey = Guid.NewGuid().ToString();
		var baseConfig = new ConfigurationBuilder();
		var configSource = new MemoryConfigurationSource
		{
			InitialData = new Dictionary<string, string?>
			{
				{ "ecoAPM:BaseURL", url },
				{ "ecoAPM:APIKey", apiKey }
			}
		};
		baseConfig.Add(configSource);

		//act
		var config = new ServerConfig(baseConfig.Build());

		//assert
		Assert.Equal(url, config.BaseURL.AbsoluteUri);
		Assert.Equal(apiKey, config.APIKey);
	}
}