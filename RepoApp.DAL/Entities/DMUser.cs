using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryApplication.DAL.Entities
{
    public class DMUser
    {
        public DMUser()
        {
            Repositories = new List<DMRepository>();
            UserRoles = new List<DMUserRole>();
            Projects = new List<DMProject>();
        }
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsConnected { get; set; }
        public ICollection<DMProject> Projects { get; set; }
        public ICollection<DMRepository> Repositories { get; set; }
        public ICollection<DMUserRole> UserRoles { get; set; }

    }
}
