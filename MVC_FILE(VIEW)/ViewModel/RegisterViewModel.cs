using Microsoft.AspNetCore.Mvc;
using MVC_FILE_VIEW_.Utilities;
using System.ComponentModel.DataAnnotations;

namespace MVC_FILE_VIEW_.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        //(jquery.validate.js, jquery.validate.unobtrusive.js) must run it in our Progarm
        //to use remote Attribute (with there Order)
        [Remote(action: "IsEmailInUse",controller:"Account")]
        [ValidEmailDominAttribute(AllowDomin:"gmail.com",ErrorMessage = "Must Have gmail.com")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Cofirm Password")]
        [Compare("Password",ErrorMessage ="Password And ConfirmPassword Not Valid")]
        public string ConfirmPassword { get; set; }
        public string City { get; set; }
    }
}
