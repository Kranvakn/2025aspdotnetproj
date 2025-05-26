using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using todoApp.Models;

namespace aspApp.Controllers;

public interface Interface
{
    string Speak();
}

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["Message"] = "안녕";

        var list = new List<string>();

        var filter = list.Where(x => x.StartsWith("a")).ToList();
        // foreach (var f in list)
        // {
        //     Console.WriteLine($"list 값 {f}");
        // }y
        // filter.ForEach(x => list.Add("X"));
        // foreach (var f in list)
        // {
        //     Console.WriteLine($"list 값 {f}");
        // }
        var res = GetMsg();
        /*
            filter -> Where
            map -> Select
            find -> FirstOrDefault
            some -> Any
            every -> All
            sort -> OrderBy 혹은 ThenBy(다중정렬필요시)
            fruits.OrderBy(f => f.Length).ThenBy(f => f).ToList();
        */

        return View();
    }

    public async Task<string> GetMsg()
    {
        var client = new HttpClient();
        var json = await client.GetStringAsync("https://api.chucknorris.io/jokes/random");
        return json;
    }
}
