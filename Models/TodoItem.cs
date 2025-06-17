namespace todoApp.Models
{
    // 할 일 상세 정보
    public class TodoItem
    {
        /* 구조
                 User ──┬─ TodoList ──┬─ TodoItem
                        └─────────────┘
        */

        // 고유 번호
        public int Id { get; set; }

        // 어느 리스트의 속한 항목인가(TodoList.Id와 연결)
        public required string UserId { get; set; }

        // 할 일 내용
        public required string Content { get; set; }

        // 할 일 완료 여부(기본값: false)
        public bool IsDone { get; set; } = false;
    }
}
