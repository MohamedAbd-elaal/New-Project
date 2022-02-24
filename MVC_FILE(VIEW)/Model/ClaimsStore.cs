using System.Collections.Generic;
using System.Security.Claims;

namespace MVC_FILE_VIEW_.Model
{
    public static class ClaimsStore
    {
        public static  List<Claim> AllClaims = new List<Claim>
        {
            new Claim("Create Role", "Create Role"),
            new Claim("Edit Role","Edit Role"),
            new Claim("Delete Role","Delete Role")
        };
    }
}
