using Microsoft.EntityFrameworkCore;
using todoApp.Data;
using todoApp.Models;

namespace todoApp.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        // 생성자: EF Core의 DbContext 주입
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // 이메일로 사용자 검색
        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // 사용자 ID로 사용자 검색
        public async Task<User?> FindByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        // 사용자 등록 (회원가입)
        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
