using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoApp.BLL.Models.EditModels
{
    public class RepositoryEditModel
    {
        public string URL { get; set; }
        public Guid TypeId { get; set; }
        public Guid ProjectId { get; set; }

        public int Index { get; set; }

    }
}
