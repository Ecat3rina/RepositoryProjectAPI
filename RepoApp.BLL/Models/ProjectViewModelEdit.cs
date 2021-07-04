using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RepoApp.BLL.Models
{
    public class ProjectViewModelEdit
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Insert project name")]
        [DisplayName("Project name")]
        [Remote("IsProjectNameValidForEdit", "Project", ErrorMessage = "This name already exists", AdditionalFields ="Id")]
        public string Name { get; set; }

        [DisplayName("Department")]
        public ICollection<Guid> Departments { get; set; }
        [DisplayName("Department")]
        public Guid DepartmentId { get; set; }

        [DisplayName("Cedacri International responsible user")]
        public ICollection<Guid> Users { get; set; }
        [DisplayName("Cedacri International responsible user")]

        public Guid UserId { get; set; }


        [Required(ErrorMessage = "Insert Cedacri International responsible user")]
        [DisplayName("Cedacri Italia responsible user name")]
        public string Username { get; set; }


    }
}
