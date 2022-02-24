using Microsoft.AspNetCore.Identity;

namespace MVC_FILE_VIEW_.ViewModel
{
    public class ApplicationUser:IdentityUser
    {
        public string City { get; set; }
    }
}
