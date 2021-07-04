using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoApp.BLL.Models.DetailModels
{
    public class ProjectDetailsModel
    {
        public Guid Id { get; set; }
        [DisplayName("Project name")]
        public string Name { get; set; }
        [DisplayName("Department")]

        public string Department { get; set; }
        [DisplayName("Cedacri International responsible user")]


        public string User { get; set; }
        [DisplayName("Cedacri Italia responsible user name")]
        public string Username { get; set; }
        [DisplayName("Repositories")]
        public List<string> Repositories { get; set; }
    }
}
