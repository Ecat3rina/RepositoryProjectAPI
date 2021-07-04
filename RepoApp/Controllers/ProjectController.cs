using RepoApp.BLL.Models;
using RepoApp.BLL.Models.AddModels;
using RepoApp.BLL.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepoApp.BLL.Models.DetailModels;
using RepoApp.BLL.Models.EditModels;
using System.Net.Http;
using Newtonsoft.Json;
using RepoApp.Common;
using RepoApp.Common.DataTables;
using RepoApp.BLL.Models.DeleteModels;

namespace RepoApp.Controllers
{
    [Authorize(Roles = "Admin, Operator, User")]
    public class ProjectController : BaseController
    {
        [Authorize(Roles = "Admin, Operator, User")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Add()
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                var departments = repo.GetDepartments();
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
                var users = repo.GetUsers();
                ViewBag.Users = new SelectList(users, "Id", "UserName");

            }
            return View("Add");
        }


        [HttpPost]
        public JsonResult GetProjects(DataTablesParameters parameters)
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                var projectList = repo.GetProjects(parameters);
                return CreateDataTablesResult(projectList, parameters);

            }

        }
        [HttpGet]
        public JsonResult GetRole(string name)
        {

            var httpClient = new HttpClient();
            var response = httpClient.GetStringAsync("https://localhost:44367/api/project/GetRole/" + name).Result;


            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetEdit(Guid id)
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                var httpClient = new HttpClient();
                var response = httpClient.GetStringAsync("https://localhost:44367/api/project/GetEdit/" + id).Result;
                ProjectViewModelEdit model = JsonConvert.DeserializeObject<ProjectViewModelEdit>(response);

                var departments = repo.GetDepartments();
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
                var users = repo.GetUsers();
                ViewBag.Users = new SelectList(users, "Id", "UserName");

                return View("_Edit", model);
            }
        }
        [HttpGet]
        public ActionResult GetDetails(Guid id)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = httpClient.GetStringAsync("https://localhost:44367/api/project/getdetails/" + id).Result;

                ProjectDetailsModel model = JsonConvert.DeserializeObject<ProjectDetailsModel>(response);

                return PartialView("_Details", model);

            }

            catch (Exception ex)
            {
                return CreateExceptionView(ex);
            }
        }
        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                var model = repo.GetDelete(id);
                return PartialView("_Delete", model);
            }
        }
        [HttpPost]
        public ActionResult Delete(ProjectDetailsModel model)
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                repo.Delete(model);
            }

            return JsonViewValidResult("~/Views/Project/Index.cshtml");

        }

        [Authorize(Roles = "Admin, Operator")]


        [HttpPost]
        public ActionResult Edit(List<string> projectData)
        {
            HttpResponseMessage postTask = null, postTask1 = null;
            ExecutionResult result = null, result1 = null;
            var succesRepo = false;
            var hasRepos = false;
            var countProject = 0;
            if (projectData.Count <= 5)
            {
                hasRepos = false;
            }
            else
            {
                hasRepos = true;
            }
            try
            {
                Guid projectId = Guid.Parse(projectData[0]);
                string projectName = projectData[1];
                Guid department = Guid.Parse(projectData[2]);
                Guid user = Guid.Parse(projectData[3]);
                string userName = projectData[4];

                var project = new ProjectEditModel
                {
                    Id = projectId,
                    Name = projectName,
                    Department = department,
                    User = user,
                    Username = userName
                };


                foreach (var item in projectData)
                {
                    if (item.Contains('^'))
                    {
                        string[] repoData = item.Split('^');
                        Guid id = Guid.Parse(repoData[0]);
                        string url = repoData[1];
                        int repoNr = Int16.Parse(repoData[2]);
                        var repo = new RepositoryEditModel
                        {
                            URL = url,
                            TypeId = id,
                            Index = repoNr,
                            ProjectId = projectId
                        };

                        using (var client = new HttpClient())
                        {
                            string startUrl = _WebServiceUrl + "/api/project/";
                            client.BaseAddress = new Uri(startUrl);

                            postTask1 = client.PostAsJsonAsync("EditOnlyRepository", repo).Result;
                            result1 = JsonConvert.DeserializeObject<ExecutionResult>(postTask1.Content.ReadAsStringAsync().Result);

                            if (postTask1.IsSuccessStatusCode)
                            {
                                succesRepo = true;
                            }

                        }

                    }
                    else
                    {
                        if (countProject < 1)
                        {
                            using (var client = new HttpClient())
                            {
                                string startUrl = _WebServiceUrl + "/api/project/";
                                client.BaseAddress = new Uri(startUrl);

                                postTask = client.PostAsJsonAsync("EditOnlyProject", project).Result;
                                result = JsonConvert.DeserializeObject<ExecutionResult>(postTask.Content.ReadAsStringAsync().Result);
                            }
                            countProject++;

                        }
                    }

                }

                if (hasRepos && succesRepo && postTask.IsSuccessStatusCode)
                {
                    return JsonViewValidResult("~/Views/Project/Index.cshtml");
                }
                if (!hasRepos && postTask.IsSuccessStatusCode)
                {
                    return JsonViewValidResult("~/Views/Project/Index.cshtml");
                }

                return JsonViewUnValidResult("~/Views/Project/_Edit.cshtml");
            }
            catch (Exception ex)
            {
                return CreateExceptionView(ex);
            }
        }

        [HttpPost]
        public ActionResult AddRepository()
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                string res = repo.GetRepository();
                return Json(res);

            }
        }


        [HttpPost]
        public ActionResult DeleteRepository(List<string> info)
        
        {

            var repo = new RepositoryDeleteModel()
            {
                Id = Guid.Parse(info[1]),
                URL = info[0]
            };
            using (var client = new HttpClient())
            {
                string startUrl = _WebServiceUrl + "/api/project/";
                client.BaseAddress = new Uri(startUrl);

                var postTask1 = client.PostAsJsonAsync("DeleteRepository", repo).Result;
                // var result1 = JsonConvert.DeserializeObject<ExecutionResult>(postTask1.Content.ReadAsStringAsync().Result);

                if (postTask1.IsSuccessStatusCode)
                {
                    return JsonViewValidResult("~/Views/Project/Index.cshtml");
                }

            }
            return JsonViewUnValidResult("~/Views/Project/_Edit.cshtml");

        }

        [HttpPost]
        public ActionResult SubmitProject(List<string> projectData)
        {
            HttpResponseMessage postTask = null, postTask1 = null;
            ExecutionResult result = null, result1 = null;
            var succesRepo = false;
            var hasRepos = false;
            var countProject = 0;

            if (projectData.Count <= 4)
            {
                hasRepos = false;
            }
            else
            {
                hasRepos = true;
            }
            try
            {
                string projectName = projectData[0];
                Guid department = Guid.Parse(projectData[1]);
                Guid user = Guid.Parse(projectData[2]);
                string userName = projectData[3];

                var project = new ProjectAddModel
                {
                    Name = projectName,
                    Department = department,
                    User = user,
                    Username = userName
                };


                foreach (var item in projectData)
                {
                    if (item.Contains('^'))
                    {
                        string[] repoData = item.Split('^');
                        Guid id = Guid.Parse(repoData[0]);
                        string url = repoData[1];
                        var repo = new RepositoryAddModel
                        {
                            URL = url,
                            TypeId = id,
                            UserId = user,
                            ProjectName = projectName
                        };

                        using (var client = new HttpClient())
                        {
                            string startUrl = _WebServiceUrl + "/api/project/";
                            client.BaseAddress = new Uri(startUrl);

                            postTask1 = client.PostAsJsonAsync("SubmitOnlyRepository", repo).Result;
                            result1 = JsonConvert.DeserializeObject<ExecutionResult>(postTask1.Content.ReadAsStringAsync().Result);

                            if (postTask1.IsSuccessStatusCode)
                            {
                                succesRepo = true;
                            }

                        }
                    }
                    else
                    {
                        if (countProject < 1)
                        {
                            using (var client = new HttpClient())
                            {
                                string startUrl = _WebServiceUrl + "/api/project/";
                                client.BaseAddress = new Uri(startUrl);

                                postTask = client.PostAsJsonAsync("SubmitOnlyProject", project).Result;
                                result = JsonConvert.DeserializeObject<ExecutionResult>(postTask.Content.ReadAsStringAsync().Result);
                            }
                            countProject++;

                        }
                    }

                }
                if (hasRepos && succesRepo && postTask.IsSuccessStatusCode)
                {
                    return JsonViewValidResult("~/Views/Project/Index.cshtml");
                }
                if (!hasRepos && postTask.IsSuccessStatusCode)
                {
                    return JsonViewValidResult("~/Views/Project/Index.cshtml");
                }

                return JsonViewUnValidResult("~/Views/Project/Add.cshtml");

            }
            catch (Exception ex)
            {
                return CreateExceptionView(ex);
            }
        }


        public JsonResult IsProjectNameValid(string name)
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                var res = repo.CheckProjectName(name);
                return Json(res == false, JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult IsProjectNameValidForEdit(string name, Guid id)
        {
            using (ProjectRepository repo = new ProjectRepository())
            {
                var res = repo.CheckProjectNameForEdit(name, id);
                return Json(res == false, JsonRequestBehavior.AllowGet);

            }
        }

        //[HttpGet]
        //public string AddRepositoryForEdit(string id)
        //{
        //    var httpClient = new HttpClient();
        //    var response = httpClient.GetStringAsync("https://localhost:44367/api/project/AddRepositoryForEdit/" + id).Result;


        //    return response;
        //}

        [HttpGet]
        public string GetAllUserRepositories(Guid id)
        {

            var httpClient = new HttpClient();

            var response = httpClient.GetStringAsync("https://localhost:44367/api/project/GetAllUserRepositories/" + id).Result;


            return response;

        }
    }
}
