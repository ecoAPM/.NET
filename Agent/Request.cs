namespace ecoAPM.Agent;

/// <summary>A summary of metrics gathered from a request</summary>
public class Request
{
	/// <summary>A unique identifier</summary>
	public Guid ID { get; set; }

	/// <summary>The type of request</summary>
	public string? Type { get; set; }

	/// <summary>The source of the request</summary>
	public string? Source { get; set; }

	/// <summary>The time the request was initiated</summary>
	public DateTime Time { get; set; }

	/// <summary>The action performed on the request</summary>
	public string? Action { get; set; }

	/// <summary>The result of the request</summary>
	public string? Result { get; set; }

	/// <summary>The duration of the request in milliseconds</summary>
	public double Duration { get; set; }

	/// <summary>Any additional context that may be helpful when reviewing request data</summary>
	public string? Context { get; set; }
}