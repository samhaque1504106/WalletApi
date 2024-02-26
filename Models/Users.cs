using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletApi.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "UserName is Must!")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is Must!")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Use at least 8 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is Must!")]
        [RegularExpression(@"^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})$", ErrorMessage = "Invalid email format.")]
        [EmailAddress]
        public string? Email { get; set; }

        public string? UserStatus { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public string? SignInStatus { get; set; }
    }
}