using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryApplication.DAL.Entities
{
   public class DMRepositoryType
    {
        public DMRepositoryType()
        { 
            Repositories = new List<DMRepository>();
        }
        [Key]
        public Guid RepositoryTypeId { get; set; }
        public string RepositoryTypeName { get; set; }
        public ICollection<DMRepository> Repositories { get; set; }
    }
}
