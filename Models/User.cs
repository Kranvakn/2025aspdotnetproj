using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoApp.Models
{
    // 유저 정보
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 기본키, 자동 증가
        // 아이디 {get set} 읽기 쓰기 모두 가능하며, get만 하면 읽기 전용이 됨
        public int Id { get; set; }

        // 로그인용 이메일
        [Required]
        public string Email { get; set; } = string.Empty;

        // 비밀번호(HASH로 저장)
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
