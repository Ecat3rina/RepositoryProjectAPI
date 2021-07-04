using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RepoApp.BLL.Models.EditModels
{
    public class ProjectEditModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Department { get; set; }
        public Guid User { get; set; }
        public string Username { get; set; }


        //[Required(ErrorMessage = "Insert project name")]
        //[DisplayName("Project name")]
        //[Remote("IsProjectNameValid", "Project", ErrorMessage = "This name already exists")]
        //public string Name { get; set; }

        //[DisplayName("Department")]
        //public ICollection<Guid> Departments { get; set; }

        //[DisplayName("Cedacri International responsible user")]
        //public ICollection<Guid> Users { get; set; }

        //[Required(ErrorMessage = "Insert Cedacri International responsible user")]
        //[DisplayName("Cedacri Italia responsible user name")]
        //public string Username { get; set; }
        

    }
}
