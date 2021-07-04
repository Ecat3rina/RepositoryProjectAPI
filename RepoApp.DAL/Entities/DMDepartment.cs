using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryApplication.DAL.Entities
{
    public class DMDepartment
    {
        public DMDepartment()
        {
            Projects = new List<DMProject>();
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<DMProject> Projects { get; set; }

    }
}
