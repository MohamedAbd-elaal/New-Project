using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_FILE_VIEW_.Model
{
    public class Employee
    {
        // Having data
        public int Id { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage ="not allow")]
        [MinLength(6,ErrorMessage ="must more 6 chars")]
        [Display(Name="New Name")]
        public string Name { get; set; }
        [Required]
        //without attribut it required because option we send to it isnot in enum values
        public Dept? Hoppies { get; set; }//? for optional without attribute
        public string photopath { get; set; }

       
    }
}
