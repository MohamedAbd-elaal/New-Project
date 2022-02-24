using MVC_FILE_VIEW_.ViewModel;
using System.Collections.Generic;

namespace MVC_FILE_VIEW_.Model
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }
        public string RoleName  { get; set; }
        public List<string> Users { get; set; }
    }
}
