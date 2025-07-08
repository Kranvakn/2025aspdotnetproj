using Microsoft.AspNetCore.Mvc;
using todoApp.Services;

public class TodoController : Controller
{
    private readonly TodoService _todoService;

    public TodoController(TodoService todoService)
    {
        _todoService = todoService;
    }

    // 할일 리스트
    public async Task<IActionResult> Index()
    {
        // 세션에서 사용자 ID 가져오기
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            // 자동 로그인 쿠키가 있는지 확인
            if (Request.Cookies.TryGetValue("AutoLogin", out var cookieUserId))
            {
                // 쿠키에서 사용자 ID 가져오기
                userId = cookieUserId;
                HttpContext.Session.SetString("UserId", userId);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // 로그인 된 유저의 todo 리스트 가져오기
        var todoList = await _todoService.GetTodoListAsync(userId);
        return View(todoList);
    }

    // 할일 추가
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var todoText = Request.Form["todoText"].ToString();

        if (string.IsNullOrEmpty(todoText))
        {
            ViewBag.Error = "텍스트를 입력해주세요.";
            return View("Index");
        }
        else if (todoText.Length > 100)
        {
            ViewBag.Error = "100자 이하로 입력해주세요.";
            return View("Index");
        }

        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        await _todoService.AddTodo(userId, todoText);
        return RedirectToAction("Index");
    }

    // 완료 전환
    [HttpPost]
    public async Task<IActionResult> Done()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var todoText = Request.Form["Content"].ToString();
        int todoId = 0;

        if (int.TryParse(Request.Form["Id"], out int idValue))
        {
            todoId = idValue;
        }

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }
        await _todoService.UpdateTodo(userId, todoText, todoId, true);
        return RedirectToAction("Index");
    }

    // 할일 삭제
    [HttpPost]
    public async Task<IActionResult> DeleteTodo()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var idDone = HttpContext.Request.Form["IdDone"].ToString();
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        if (int.TryParse(Request.Form["Id"], out int todoId))
        {
            await _todoService.DeleteTodo(userId, todoId);
        }

        if (bool.TryParse(idDone, out bool isDone))
        {
            await _todoService.DeleteTodo(userId, -1);
        }

        TempData["DeleteConfirm"] = true; // 삭제 확인 메시지 출력
        return RedirectToAction("Index");
    }

    // // 체크박스 클릭 이벤트
    // public IActionResult onClickCheckbox()
    // {
    //     return View();
    // }

    // 약관 페이지
    public IActionResult Privacy()
    {
        return View();
    }
}
