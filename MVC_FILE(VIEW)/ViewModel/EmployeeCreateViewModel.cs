using Microsoft.AspNetCore.Http;
using MVC_FILE_VIEW_.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC_FILE_VIEW_.ViewModel
{
    public class EmployeeCreateViewModel
    {
        [Required]
        [MaxLength(50, ErrorMessage = "not allow")]
        [MinLength(6, ErrorMessage = "must more 6 chars")]
        [Display(Name = "New Name")]
        public string Name { get; set; }
        [Required]
        //without attribut it required because option we send to it isnot in enum values
        public Dept? Hoppies { get; set; }//? for optional without attribute
        public IFormFile photopath { get; set; }
        // when we want choose multi photo
        //public List<IFormFile> photopath { get; set; }
    }
}
