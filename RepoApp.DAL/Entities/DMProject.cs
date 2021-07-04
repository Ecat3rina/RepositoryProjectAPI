using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryApplication.DAL.Entities
{
    public class DMProject
    {
        public DMProject()
        {
            Repositories = new List<DMRepository>();
        }
       [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
        public DMDepartment Department { get; set; }
        public Guid UserId { get; set; }
        public DMUser User { get; set; }
        public string UserName { get; set; }
        public ICollection<DMRepository> Repositories { get; set; }

    }
}
