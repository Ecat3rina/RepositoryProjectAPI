using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoApp.BLL.Models.DetailModels
{
    public class UserDetailsModel
    {
        public Guid Id { get; set; }

        [DisplayName("User name")]
        public string UserName { get; set; }
        [DisplayName("Personal name")]
        public string FullName { get; set; }
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("Connection")]
        public bool IsConnected { get; set; }
        [DisplayName("Roles")]
        public List<string> Roles { get; set; }


    }
}
