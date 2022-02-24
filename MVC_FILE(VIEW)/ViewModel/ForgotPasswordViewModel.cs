using MVC_FILE_VIEW_.Utilities;
using System.ComponentModel.DataAnnotations;

namespace MVC_FILE_VIEW_.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [ValidEmailDominAttribute(AllowDomin: "gmail.com", ErrorMessage = "Must Have @gmail.com")]
        public string Email { get; set; }
    }
}
