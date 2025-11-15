using Microsoft.EntityFrameworkCore;
using todoApp.Data;
using todoApp.Models;

namespace todoApp.Services
{
    public class TodoService
    {
        private readonly AppDbContext _context;

        public TodoService(AppDbContext context)
        {
            _context = context;
        }

        // 로그인된 유저의 할 일 그룹을 가져옴
        public async Task<List<TodoGroup>> GetTodoGroupsAsync(string userId)
        {
            return await _context.TodoGroups.Where(t => t.UserId == userId).ToListAsync();
        }

        // 할 일 그룹 추가
        public async Task AddTodoGroup(string userId, string groupName)
        {
            var todoGroup = new TodoGroup { Title = groupName, UserId = userId };
            _context.TodoGroups.Add(todoGroup);
            await _context.SaveChangesAsync();
        }

        // 로그인된 유저의 할 일 목록을 가져옴
        public async Task<List<TodoItem>> GetTodoListAsync(string userId, string groupNo)
        {
            return await _context
                .TodoItems.Where(t => t.UserId == userId && t.GroupNo == groupNo)
                .ToListAsync();
        }

        // 할 일 추가
        public async Task AddTodo(string userId, string content, string groupNo)
        {
            var todoItem = new TodoItem
            {
                UserId = userId,
                Content = content,
                GroupNo = groupNo,
                IsDone = false,
            };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
        }

        // 할 일 수정
        public async Task UpdateTodo(
            string userId,
            string content,
            int id,
            bool? isDone,
            string selectedGroupNo
        )
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(t =>
                (t.UserId == userId) && (t.Id == id) && (t.GroupNo == selectedGroupNo)
            );

            if (todoItem != null)
            {
                todoItem.Content = content;
                if (isDone == true)
                {
                    todoItem.IsDone = !todoItem.IsDone;
                }

                _context.TodoItems.Update(todoItem);
                await _context.SaveChangesAsync();
            }
        }

        // 할 일 삭제
        public async Task DeleteTodo(string userId, int id, string selectedGroupNo)
        {
            if (id == -1)
            {
                // 만약 id가 -1면 해당 유저의 완료된 할 일을 모두 삭제
                _context.TodoItems.RemoveRange(
                    _context.TodoItems.Where(t =>
                        (t.UserId == userId) && (t.IsDone == true) && (t.GroupNo == selectedGroupNo)
                    )
                );
                await _context.SaveChangesAsync();
                return;
            }

            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(t =>
                (t.UserId == userId) && (t.Id == id) && (t.GroupNo == selectedGroupNo)
            );

            if (todoItem != null)
            {
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
