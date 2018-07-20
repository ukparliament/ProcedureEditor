using System.ComponentModel.DataAnnotations;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class UserLogin
    {
        [Required]
        [EmailAddress]
        public string EMail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}