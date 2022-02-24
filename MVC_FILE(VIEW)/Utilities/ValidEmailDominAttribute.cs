using System.ComponentModel.DataAnnotations;

namespace MVC_FILE_VIEW_.Utilities
{
    public class ValidEmailDominAttribute:ValidationAttribute
    {
        private  string AllowDomin { get; }
        public ValidEmailDominAttribute(string AllowDomin)
        {
            this.AllowDomin = AllowDomin;
        }

        

        public override bool IsValid(object value)
        {
            string [] Resualt=value.ToString().Split("@");
            return Resualt[1].ToUpper()==AllowDomin.ToUpper();
        }
    }
}
