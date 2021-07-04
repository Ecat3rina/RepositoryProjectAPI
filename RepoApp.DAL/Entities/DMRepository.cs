using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryApplication.DAL.Entities
{
   public class DMRepository
    {
        [Key]
        public Guid Id { get; set; }
        public string URL { get; set; }
        public Guid TypeId { get; set; }
        public DMRepositoryType Type { get; set; }
        public Guid ProjectId { get; set; }
        public DMProject Project { get; set; }
        public Guid UserId { get; set; }
        public DMUser User { get; set; }
    }
}
