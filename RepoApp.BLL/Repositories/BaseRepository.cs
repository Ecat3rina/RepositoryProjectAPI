using RepositoryApplication.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoApp.BLL.Repositories
{
    public class BaseRepository : IDisposable
    {
        protected readonly FirstContext Context = new FirstContext();

        public virtual void Dispose()
        {
            this.Context?.Dispose();
        }
    }
}
