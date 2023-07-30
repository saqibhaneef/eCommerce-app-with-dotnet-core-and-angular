using System.ComponentModel.DataAnnotations;

namespace api.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Displayname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",ErrorMessage ="Must have 1 uppercase,1 lowercase,1 none alpha-numeric and atleast 6 characters")]
        public string  Password{ get; set; } 
    }
}
