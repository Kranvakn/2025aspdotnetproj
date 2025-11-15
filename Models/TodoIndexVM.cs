namespace todoApp.Models
{
    public class TodoIndexVM
    {
        public IEnumerable<TodoItem> Items { get; set; } = Enumerable.Empty<TodoItem>();
        public IEnumerable<TodoGroup> Groups { get; set; } = Enumerable.Empty<TodoGroup>();
        public string? SelectedGroupNo { get; set; }
    }
}
