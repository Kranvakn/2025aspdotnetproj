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

        // 자동 로그인 쿠키 확인
        if (!HttpContext.Session.Keys.Contains("UserId"))
        {
            if (Request.Cookies.TryGetValue("AutoLogin", out var userId))
            {
                HttpContext.Session.SetString("UserId", userId);
                return RedirectToAction("Index", "Todo");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // 세션이 이미 존재할 경우 바로 Todo로 이동
        return RedirectToAction("Index", "Todo");
    }

    public async Task<string> GetMsg()
    {
        var client = new HttpClient();
        var json = await client.GetStringAsync("https://api.chucknorris.io/jokes/random");
        return json;
    }
}
