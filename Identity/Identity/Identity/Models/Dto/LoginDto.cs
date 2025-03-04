using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Dto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Remember me")]
        public bool IsPersistent { get; set; } = false;

        public string ReturnUrl { get; set; }
    }
}
