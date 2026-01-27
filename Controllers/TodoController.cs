using Microsoft.AspNetCore.Mvc;
using todoApp.Models;
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

        var initGroupNo = 0;

        // 로그인 된 유저의 todo 리스트 가져오기
        var todoList = await _todoService.GetTodoListAsync(userId, initGroupNo);
        var todoGroups = await _todoService.GetTodoGroupsAsync(userId);

        var vm = new TodoIndexVM { Items = todoList, Groups = todoGroups };
        return View(vm);
    }

    // 할일 그룹 추가
    [HttpPost]
    public async Task<IActionResult> CreateGroup()
    {
        var todoGroup = Request.Form["todoGroup"].ToString();

        if (string.IsNullOrEmpty(todoGroup))
        {
            ViewBag.ErrorGroup = "그룹의 이름을 입력해주세요.";
            return View("Index");
        }
        else if (todoGroup.Length > 20)
        {
            ViewBag.ErrorGroup = "20자 이하로 입력해주세요.";
            return View("Index");
        }

        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        await _todoService.AddTodoGroup(userId, todoGroup);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> GetTodoListByGroupNo(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        // 그룹에 해당하는 todo 리스트 가져오기
        var selectedGroupNo = id;
        var todoList = await _todoService.GetTodoListAsync(userId, selectedGroupNo);
        var todoGroups = await _todoService.GetTodoGroupsAsync(userId);

        var vm = new TodoIndexVM
        {
            Items = todoList,
            Groups = todoGroups,
            SelectedGroupNo = selectedGroupNo,
        };
        return View("Index", vm);
    }

    // 할일 추가
    [HttpPost]
    public async Task<IActionResult> Create(int groupNo, string todoText)
    {
        if (groupNo == 0)
        {
            TempData["Error"] = "グループを選択してください。";
            return RedirectToAction("GetTodoListByGroupNo", new { id = groupNo });
        }
        else if (string.IsNullOrEmpty(todoText))
        {
            TempData["Error"] = "テキストを入力してください。";
            return RedirectToAction("GetTodoListByGroupNo", new { id = groupNo });
        }
        else if (todoText.Length > 100)
        {
            TempData["Error"] = "100文字以下で入力してください。";
            return RedirectToAction("GetTodoListByGroupNo", new { id = groupNo });
        }

        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        await _todoService.AddTodo(userId, todoText, groupNo);
        return RedirectToAction("GetTodoListByGroupNo", new { id = groupNo });
    }

    // 완료 전환
    [HttpPost]
    public async Task<IActionResult> Done(int selectedGroupNo)
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

        await _todoService.UpdateTodo(userId, todoText, todoId, true, selectedGroupNo);
        return RedirectToAction("GetTodoListByGroupNo", new { id = selectedGroupNo });
    }

    // 할일 삭제
    [HttpPost]
    public async Task<IActionResult> DeleteTodo(int selectedGroupNo)
    {
        var userId = HttpContext.Session.GetString("UserId");
        var idDone = HttpContext.Request.Form["IdDone"].ToString();
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        if (int.TryParse(Request.Form["Id"], out int todoId))
        {
            await _todoService.DeleteTodo(userId, todoId, selectedGroupNo);
        }

        if (bool.TryParse(idDone, out bool isDone))
        {
            await _todoService.DeleteTodo(userId, -1, selectedGroupNo);
        }

        TempData["DeleteConfirm"] = true; // 삭제 확인 메시지 출력
        return RedirectToAction("GetTodoListByGroupNo", new { id = selectedGroupNo });
    }

    public async Task<IActionResult> DeleteTodoGroup(int SelectedGroupNo)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        await _todoService.DeleteTodoGroup(userId, SelectedGroupNo);
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
