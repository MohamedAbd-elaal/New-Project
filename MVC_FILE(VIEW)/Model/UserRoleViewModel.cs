namespace MVC_FILE_VIEW_.Model
{
    public class UserRoleViewModel
    {
        //we didnot add RoleId because Relation Between (RoleId and userId)One to Many (One role Can Have sevral Users)
        public string UserId { get; set; }
        public string UserName { get; set; }    
        public bool IsSelected { get; set; }
    }
}
