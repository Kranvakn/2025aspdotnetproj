using Microsoft.AspNetCore.Mvc;
using todoApp.Models;
using todoApp.Services;
using todoApp.Utils;
using todoApp.Common;

public class AccountController : Controller
{
    private readonly UserService _userService;

    // 생성자: 외부에서 UserService를 주입받아 사용
    public AccountController(UserService userService)
    {
        _userService = userService;
    }

    // 회원가입 페이지
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // 회원가입 처리
    [HttpPost]
    public async Task<IActionResult> Register(string Email, string PasswordHash)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View();
        }

        // 이메일 중복 체크
        var existingUser = await _userService.FindByEmailAsync(Email);
        if (null != existingUser)
        {
            ViewBag.Error = CommonMessage.MessageNo3();
            return View();
        }

        // 비밀번호 유효성 검사
        if (8 > PasswordHash.Length)
        {
            ViewBag.Error = CommonMessage.MessageNo2("8", "이상");
            return View();
        }
        else if (20 < PasswordHash.Length)
        {
            ViewBag.Error = CommonMessage.MessageNo2("20", "이하");
            return View();
        }

        if (!PasswordHash.Any(char.IsDigit))
        {
            ViewBag.Error = CommonMessage.MessageNo1("숫자");
            return View();
        }

        if (!PasswordHash.Any(char.IsUpper))
        {
            ViewBag.Error = CommonMessage.MessageNo1("대문자");
            return View();
        }

        if (!PasswordHash.Any(char.IsLower))
        {
            ViewBag.Error = CommonMessage.MessageNo1("소문자");
            return View();
        }

        if (!PasswordHash.Any("!@#$%^&*()_+[]{}|;':\",.<>?/`~".Contains))
        {
            ViewBag.Error = CommonMessage.MessageNo1("특수문자");
            return View();
        }

        // 회원등록
        var newUser = new User { Email = Email, PasswordHash = Hash.setHashPassword(PasswordHash) };

        await _userService.CreateUserAsync(newUser);

        ViewBag.RegisterSuccess = true;
        return View();
    }

    // 로그인 페이지
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // 로그인 처리
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _userService.FindByEmailAsync(email);

        if (null == user)
        {
            ViewBag.Error = CommonMessage.MessageNo4();
            return View();
        }

        var hashedPassword = Hash.setHashPassword(password);
        if (hashedPassword != user.PasswordHash)
        {
            ViewBag.Error = CommonMessage.MessageNo4();
            return View();
        }

        // 로그인 성공: 세션에 사용자 ID 저장
        HttpContext.Session.SetString("UserId", user.Id.ToString());

        // 로그인 후 할 일 목록 페이지로 리디렉션

        // return RedirectToAction("Index", "Todo");
        ViewBag.LoginSuccess = true; // <<< boolean값은 return으로 반환 금지
        return View();
    }

    // 로그아웃 처리
    public IActionResult Logout()
    {
        // 세션 초기화
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}
