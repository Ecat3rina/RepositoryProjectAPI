using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoApp.BLL.Models.AuthenticationModels
{
    public class LoginUserModel
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Insert user name")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Insert password")]
        public string Password { get; set; }
        
    }
}
