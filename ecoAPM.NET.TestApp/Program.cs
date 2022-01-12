using ecoAPM.NET.CoreMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddecoAPM(builder.Configuration);

var app = builder.Build();
app.UseecoAPM();

app.MapGet("/", () => "Hello, world!");
app.MapGet("/slow", async () =>
{
	await Task.Delay(1_000);
	return "HellaSlow, world!";
});

app.Run();