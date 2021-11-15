using ecoAPM.NET.CoreMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ecoAPM.DotNet.TestApp;

public class Startup
{
	public Startup(IWebHostEnvironment env)
	{
		var builder = new ConfigurationBuilder()
			.SetBasePath(env.ContentRootPath)
			.AddJsonFile("appsettings.json", true, true)
			.AddEnvironmentVariables();

		Configuration = builder.Build();
	}

	public IConfiguration Configuration { get; }

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddecoAPM(Configuration);
		services.AddMvc();
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
	{
		app.UseecoAPM();
		app.UseMvc(routes =>
		{
			routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
		});
	}
}