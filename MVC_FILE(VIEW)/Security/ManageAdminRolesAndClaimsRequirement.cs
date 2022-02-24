using Microsoft.AspNetCore.Authorization;

namespace MVC_FILE_VIEW_.Security
{
    //custom Requirement
    public class ManageAdminRolesAndClaimsRequirement:IAuthorizationRequirement
    {
    }
}
