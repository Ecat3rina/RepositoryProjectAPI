using RepoApp.BLL.Models.DetailModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RepoApp.BLL.Models.AddModels
{
    public class UserAddModel
    {
        public UserAddModel()
        {
            Roles = new List<Guid>();
        }

       // [Remote("IsUserNameValid", "Project", ErrorMessage = "This name already exists")]
        [Required(ErrorMessage = "Insert username")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Insert personal name")]
        [DisplayName("Personal Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Insert password")]
        [DisplayName("Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Insert email")]
        [EmailAddress(ErrorMessage = "Wrong email")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Select at least a role")]
        [DisplayName("Roles")]
        public ICollection<Guid> Roles { get; set; }

    }
}
