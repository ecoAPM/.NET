namespace ecoAPM.NET.Agent;

public class Request
{
	public Guid ID { get; set; }
	public string? Type { get; set; }
	public string? Source { get; set; }
	public DateTime Time { get; set; }
	public string? Action { get; set; }
	public string? Result { get; set; }
	public double Length { get; set; }
	public string? Context { get; set; }
}