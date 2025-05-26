namespace todoApp.Models
{
    // 할 일 리스트
    public class TodoList
    {

        // 고유번호
        public int Id { get; set; }

        // 리스트 소유자정보
        public required string UserId { get; set; }

        // 리스트 이름
        public required string Title { get; set; }
    }
}
