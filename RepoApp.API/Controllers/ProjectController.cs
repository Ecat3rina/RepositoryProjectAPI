using RepoApp.BLL.Models.AddModels;
using RepoApp.BLL.Models.DeleteModels;
using RepoApp.BLL.Models.EditModels;
using RepoApp.BLL.Repositories;
using RepoApp.Common.DataTables;
using System;
using System.Web.Http;

namespace RepoApp.API.Controllers
{
    public class ProjectController : BaseController
    {
        [HttpPost]
        public IHttpActionResult GetProjects(DataTablesParameters parameters)
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                var projectList = repo.GetProjects(parameters);
                return CreateDataTablesResult(projectList, parameters);

            }

        }
        [Route("api/project/GetRole/{name}")]
        [HttpGet]
        public IHttpActionResult GetRole(string name)
        {

            using (UserRepository repo = new UserRepository())
            {
                var details = repo.GetSuperiorRoleName(name);



                return Json(details);
            }
        }

        [HttpPost]
        public IHttpActionResult SubmitOnlyProject(ProjectAddModel model)
        {

            using (ProjectRepository repo = new ProjectRepository())
            {
                repo.AddOnlyProject(model);
            }

            return CreateJsonOk();
        }
        [HttpPost]
        public IHttpActionResult SubmitOnlyRepository(RepositoryAddModel model)
        {

            using (ProjectRepository repo = new ProjectRepository())
            {
                repo.AddOnlyRepository(model);
            }

            return CreateJsonOk();
        }
        [HttpPost]
        public IHttpActionResult EditOnlyProject(ProjectEditModel model)
        {

            using (ProjectRepository repo = new ProjectRepository())
            {
                repo.EditOnlyProject(model);
            }

            return CreateJsonOk();
        }
        [HttpPost]
        public IHttpActionResult EditOnlyRepository(RepositoryEditModel model)
        {

            using (ProjectRepository repo = new ProjectRepository())
            {
                repo.EditOnlyRepository(model);
            }

            return CreateJsonOk();
        }

        [HttpPost]
        public IHttpActionResult DeleteRepository(RepositoryDeleteModel repoData)
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                repo.DeleteRepo(repoData);
                return CreateJsonOk();
            }
        }

        [HttpGet]
        public IHttpActionResult GetDetails(Guid id)
        {
            try
            {
                using (ProjectRepository repo = new ProjectRepository())
                {
                    var details = repo.GetDetails(id);

                    return Json(details);
                }
            }

            catch (Exception ex)
            {
                return CreateExceptionLog(ex);
            }
        }


        [HttpGet]
        public IHttpActionResult GetAllUserRepositories(string Id)
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                string res = repo.GetUserRepositories(Id);
                return Json(res);

            }
        }
        [HttpGet]

        public IHttpActionResult AddRepositoryForEdit([FromUri] string id)
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                string res = repo.GetRepository(id);
                return Json(res);

            }
        }
        [HttpGet]
        public IHttpActionResult GetEdit(Guid id)
        {
            try
            {
                using (ProjectRepository repo = new ProjectRepository())
                {
                    var model = repo.GetEdit(id);

                    return Json(model);
                }
            }

            catch (Exception ex)
            {
                return CreateExceptionLog(ex);
            }
        }
    }
}
