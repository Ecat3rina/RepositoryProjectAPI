using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoApp.BLL.Models.AddModels
{
    public class ProjectAddModel
    {
        public string Name { get; set; }
        public Guid Department { get; set; }
        public Guid User { get; set; }
        public string Username { get; set; }

        //public ICollection<string> repoData { get; set; } = new HashSet<string>();


    }
}
