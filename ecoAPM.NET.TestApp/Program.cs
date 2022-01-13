using ecoAPM.NET.CoreMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEcoAPM(builder.Configuration);

var app = builder.Build();
app.UseEcoAPM();

app.MapGet("/", () => "Hello, world!");
app.MapGet("/slow", async () =>
{
	await Task.Delay(1_000);
	return "HellaSlow, world!";
});

app.Run();