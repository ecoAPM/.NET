using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace ecoAPM.DotNet.TestApp.Controllers;

public class SlowController : Controller
{
	public string Index()
	{
		Thread.Sleep(1000);
		return "HellaSlow world!";
	}
}