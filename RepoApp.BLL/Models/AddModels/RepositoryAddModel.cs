using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoApp.BLL.Models.AddModels
{
    public class RepositoryAddModel
    {
        public string URL { get; set; }
        public Guid TypeId { get; set; }
        public Guid UserId { get; set; }

        public string ProjectName { get; set; }

    }
}
