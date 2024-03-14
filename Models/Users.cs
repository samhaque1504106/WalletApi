using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace WalletApi.Models
{
    [Index(nameof(UserName),IsUnique = true)] // this is done through EF core. but also can be done using fluent api.
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "UserName is Must!")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Use at least 5 characters(Use symbols for unique UserName)")] 
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is Must!")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Use at least 8 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is Must!")]
        [RegularExpression(@"^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})$", ErrorMessage = "Invalid email format.")]
        [EmailAddress]
        [StringLength(500)]
        public string? Email { get; set; }

        public string? UserStatus { get; set; }
        
        //[JsonIgnore] //to avoid in request payload
        public DateTimeOffset? CreatedAt { get; set; }
        //[JsonIgnore]
        public DateTimeOffset? UpdatedAt { get; set; }

        public string? SignInStatus { get; set; }
    }
}