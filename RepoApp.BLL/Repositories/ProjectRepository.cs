using RepoApp.BLL.Models;
using RepoApp.BLL.Models.AddModels;
using RepoApp.BLL.Models.DetailModels;
using RepoApp.BLL.Models.EditModels;
using RepositoryApplication.DAL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoApp.Common;
using RepoApp.Common.DataTables;
using RepoApp.BLL.Models.DeleteModels;

namespace RepoApp.BLL.Repositories
{
    public class ProjectRepository : BaseRepository
    {


        public string GetFirstDepartment(Guid id)
        {
            var depId = Context.Projects.FirstOrDefault(x => x.Id == id).DepartmentId;
            var name = Context.Departments.FirstOrDefault(x => x.Id == depId).Name;
            return name;
        }
        public List<DepartmentModel> GetDepartments()
        {

            var list = Context.Departments.Select(x => new DepartmentModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return list;
        }
        public List<UserModel> GetUsers()
        {

            var list = Context.Users.Select(x => new UserModel
            {
                Id = x.Id,
                UserName = x.UserName
            }).ToList();

            return list;
        }

        public List<ProjectGridModel> GetProjects(DataTablesParameters parameters)
        {

            var projects = Context.Projects.Select(u => new ProjectGridModel
            {
                Id = u.Id,
                Name = u.Name,
                Department = Context.Departments.FirstOrDefault(x => x.Id == u.DepartmentId).Name,
                User = Context.Users.FirstOrDefault(x => x.Id == u.UserId).UserName,
                Username = u.UserName,
            });

            if (!string.IsNullOrEmpty(parameters.Search.Value))
            {
                projects = projects.Where(x => x.Name.Contains(parameters.Search.Value) ||
                x.Department.Contains(parameters.Search.Value) ||
                x.User.Contains(parameters.Search.Value) ||
                x.Username.Contains(parameters.Search.Value));
            }


            return projects.OrderBy(parameters).Page(parameters).ToList();
        }



        public ProjectDetailsModel GetDetails(Guid id)
        {

            var projectDetails = Context.Projects.Select(u => new ProjectDetailsModel
            {
                Id = u.Id,
                Name = u.Name,
                Department = Context.Departments.FirstOrDefault(x => x.Id == u.DepartmentId).Name,
                User = Context.Users.FirstOrDefault(x => x.Id == u.UserId).UserName,
                Username = u.UserName,
                Repositories = Context.Repositories.Where(x => x.ProjectId == id).
                Select(n => Context.RepositoryTypes.FirstOrDefault(x => x.RepositoryTypeId == n.TypeId).RepositoryTypeName + " : " + n.URL).ToList()
            }).FirstOrDefault(x => x.Id == id);

            return projectDetails;

        }

        public ProjectDetailsModel GetDelete(Guid id)
        {
            var project = Context.Projects.Select(u => new ProjectDetailsModel
            {
                Id = u.Id,
                Name = u.Name,
                Department = Context.Departments.FirstOrDefault(x => x.Id == u.DepartmentId).Name,
                User = Context.Users.FirstOrDefault(x => x.Id == u.UserId).UserName,
                Username = u.UserName,
                Repositories = Context.Repositories.Where(x => x.ProjectId == id).
                Select(n => Context.RepositoryTypes.FirstOrDefault(x => x.RepositoryTypeId == n.TypeId).RepositoryTypeName + " : " + n.URL).ToList()
            }).FirstOrDefault(x => x.Id == id);

            return project;
        }
        public void Delete(ProjectDetailsModel model)
        {
            var projectToDelete = Context.Projects.FirstOrDefault(x => x.Id == model.Id);

            Context.Projects.Remove(projectToDelete);

            var repoList = Context.Repositories.Where(x => x.ProjectId == model.Id).ToList();
            Context.Repositories.RemoveRange(repoList);
            Context.SaveChanges();
        }

        public ProjectViewModelEdit GetEdit(Guid id)
        {
            var project = Context.Projects.Select(u => new ProjectViewModelEdit
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.UserName,
                DepartmentId = u.DepartmentId,
                UserId = u.UserId
            }).FirstOrDefault(x => x.Id == id);

            return project;
        }


        public void EditOnlyProject(ProjectEditModel model)
        {
            var projectModel = Context.Projects.FirstOrDefault(x => x.Id == model.Id);

            projectModel.Name = model.Name;
            projectModel.DepartmentId = model.Department;
            projectModel.UserId = model.User;
            projectModel.UserName = model.Username;

            Context.Entry(projectModel).State = EntityState.Modified;
            Context.SaveChanges();

        }
        public void EditOnlyRepository(RepositoryEditModel model)
        {
            var repoList = Context.Repositories.Where(x => x.ProjectId == model.ProjectId).ToArray();
            var repoModel = new DMRepository();
            var prevIndex = -1;

            if (repoList.Length == 0)
            {
                var newRepo = new DMRepository()
                {
                    Id = Guid.NewGuid(),
                    URL = model.URL,
                    TypeId = model.TypeId,
                    UserId = Context.Projects.FirstOrDefault(x => x.Id == model.ProjectId).UserId,
                    ProjectId = model.ProjectId
                };
                Context.Repositories.Add(newRepo);
            }
            else
            {

                for (int i = 0; i < repoList.Length; i++)
                {
                    if (model.Index == i)
                    {
                        repoList[i].URL = model.URL;
                        repoList[i].TypeId = model.TypeId;
                        repoList[i].ProjectId = model.ProjectId;
                        Context.Entry(repoList[i]).State = EntityState.Modified;

                    }
                    else
                    {
                        if (model.Index >= repoList.Length && model.Index > prevIndex)
                        {
                            prevIndex = model.Index;

                            var newRepo = new DMRepository()
                            {
                                Id = Guid.NewGuid(),
                                URL = model.URL,
                                TypeId = model.TypeId,
                                UserId = Context.Projects.FirstOrDefault(x => x.Id == model.ProjectId).UserId,
                                ProjectId = model.ProjectId
                            };
                            Context.Repositories.Add(newRepo);
                        }
                    }

                }
            }
            Context.SaveChanges();

        }



        public void Edit(ArrayList arrayList, Guid id)
        {
            var projectModel = Context.Projects.FirstOrDefault(x => x.Id == id);

            foreach (var item in arrayList)
            {
                if (item.GetType() == typeof(ProjectEditModel))
                {
                    ProjectEditModel project = (ProjectEditModel)item;
                    projectModel.Name = project.Name;
                    projectModel.DepartmentId = project.Department;
                    projectModel.UserId = project.User;
                    projectModel.UserName = project.Username;

                    this.EditRepository(arrayList, id, project);
                }

            }
            Context.Entry(projectModel).State = EntityState.Modified;

            Context.SaveChanges();
        }
        public void EditRepository(ArrayList model, Guid id, ProjectEditModel project)
        {
            var repoList = Context.Repositories.Where(x => x.ProjectId == id).ToArray();
            var repoModel = new DMRepository();
            var prevIndex = -1;
            foreach (var item in model)
            {
                if (item.GetType() == typeof(RepositoryEditModel))
                {
                    RepositoryEditModel repository = (RepositoryEditModel)item;

                    if (repoList.Length == 0)
                    {
                        var newRepo = new DMRepository()
                        {
                            Id = Guid.NewGuid(),
                            URL = repository.URL,
                            TypeId = repository.TypeId,
                            UserId = project.User,
                            ProjectId = id
                        };
                        Context.Repositories.Add(newRepo);
                    }
                    else
                    {

                        for (int i = 0; i < repoList.Length; i++)
                        {
                            if (repository.Index == i)
                            {
                                repoList[i].URL = repository.URL;
                                repoList[i].TypeId = repository.TypeId;
                                Context.Entry(repoList[i]).State = EntityState.Modified;

                            }
                            else
                            {
                                if (repository.Index >= repoList.Length && repository.Index > prevIndex)
                                {
                                    prevIndex = repository.Index;

                                    var newRepo = new DMRepository()
                                    {
                                        Id = Guid.NewGuid(),
                                        URL = repository.URL,
                                        TypeId = repository.TypeId,
                                        UserId = project.User,
                                        ProjectId = id
                                    };
                                    Context.Repositories.Add(newRepo);
                                }
                            }

                        }
                    }
                }
            }


        }



        public string GetUserRepositories(string id)
        {
            Guid projectId = Guid.Parse(id);
            var repolist = Context.Repositories.Where(x => x.ProjectId == projectId).Select(x => x.URL).ToArray();

            var list = Context.RepositoryTypes.Select(x => new RepositoryModel
            {
                RepositoryTypeId = x.RepositoryTypeId,
                RepositoryTypeName = x.RepositoryTypeName
            }).ToList();

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < repolist.Length; i++)
            {
                stringBuilder.Append(@"<div class='pb-10 repoClass' id='repoId'><div class='row'>
                        <div class='col-sm-9'>
                        <label>URL</label>

                        <input name='name" + i + "' type ='text' value='" + repolist[i] + @"' class='form-control url' />
                       <span class='spanClass' id='" + i + @"'></span>
                        </div>
                        <div class='col-sm-2'>

                        <label>Type</label>
                            <select id='select" + i + "' class='repository-selectpicker form-control form-control-sm selectpicker selectpicker-border show-tick'> "
                        + GetOptionRepositoryTypeSelected(list, repolist[i]) + @"
                            </select>
                            </div>
                        <div class='col-sm-1'>
                            <label>Remove</label>
                            <button class='btn btn-danger remove' id='" + i + @"'><span class='glyphicon glyphicon-minus-sign'></span></button>
                        </div>
                        </div></div>");


            }

            return stringBuilder.ToString();
        }


        public string GetRepository()
        {

            var list = Context.RepositoryTypes.Select(x => new RepositoryModel
            {
                RepositoryTypeId = x.RepositoryTypeId,
                RepositoryTypeName = x.RepositoryTypeName
            }).ToList();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"<div class='pb-10 repoClass' id='repoId'><div class='row'>
                    <div class='col-sm-9'>
                    <label>URL</label>
                    
                    <input name='name0' type ='text' class='form-control url' />
                   <span class='spanClass' id='0'></span>
                   
                    </div>
                    <div class='col-sm-2'>
                    
                    <label>Type</label>
                        <select class='repository-selectpicker form-control form-control-sm selectpicker selectpicker-border show-tick'> " + GetOptionRepositoryType(list) + @"
                        </select>
                        </div>
                    <div class='col-sm-1'>
                            <label>Remove</label>
                        
                        <button class='btn btn-danger remove' id='removeButton'><span class='glyphicon glyphicon-minus-sign'></span></button>
                    </div>
                    </div>");

            return stringBuilder.ToString();
        }

        public string GetRepository(string id)
        {

            var list = Context.RepositoryTypes.Select(x => new RepositoryModel
            {
                RepositoryTypeId = x.RepositoryTypeId,
                RepositoryTypeName = x.RepositoryTypeName
            }).ToList();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"<div class='pb-10 repoClass' id='repoId'><div class='row'>
                    <div class='col-sm-9'>
                    <label>URL</label>
                    
                    <input name='name" + id + @"' type ='text' class='form-control url' />
                   <span class='spanClass' id='" + id + @"'></span>
                   
                    </div>
                    <div class='col-sm-2'>
                    
                    <label>Type</label>
                        <select class='repository-selectpicker form-control form-control-sm selectpicker selectpicker-border show-tick'> " + GetOptionRepositoryType(list) + @"
                        </select>
                        </div>
                    <div class='col-sm-1'>
                        <label>Remove</label>
                        <button class='btn btn-danger remove' id='" + id + @"'><span class='glyphicon glyphicon-minus-sign'></span></button>
                    </div>
                    </div>");

            return stringBuilder.ToString();
        }

        public string GetOptionRepositoryType(ICollection<RepositoryModel> repositoryModels)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in repositoryModels)
            {
                stringBuilder.Append("<option id=" + item.RepositoryTypeId + ">" + item.RepositoryTypeName + "</option>");
            }
            return stringBuilder.ToString();
        }

        public string GetOptionRepositoryTypeSelected(ICollection<RepositoryModel> repositoryModels, string url)
        {
            var typeId = Context.Repositories.FirstOrDefault(x => x.URL == url).TypeId;

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in repositoryModels)
            {
                if (item.RepositoryTypeId == typeId)
                {
                    stringBuilder.Append("<option value=" + item.RepositoryTypeId + " id =" + item.RepositoryTypeId + " selected>" + item.RepositoryTypeName + "</option>");
                }
                else
                {
                    stringBuilder.Append("<option id =" + item.RepositoryTypeId + ">" + item.RepositoryTypeName + "</option>");

                }
            }
            return stringBuilder.ToString();
        }


        public bool CheckProjectName(string projectName)
        {
            var projectNameExists = Context.Projects.Any(x => x.Name == projectName);

            if (projectNameExists)
                return true;
            else
                return false;
        }

        public bool CheckProjectNameForEdit(string projectName, Guid id)
        {
            var projectList = Context.Projects.Where(x => x.Name == projectName).ToList();

            foreach (var item in projectList)
            {
                if (item.Id == id)
                    return false;
                else
                    return true;
            }
            return false;

        }
        public void AddOnlyProject(ProjectAddModel model)
        {
            DMProject projectModel = new DMProject()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                DepartmentId = model.Department,
                UserId = model.User,
                UserName = model.Username
            };

            Context.Projects.Add(projectModel);
            Context.SaveChanges();

        }
        public void AddOnlyRepository(RepositoryAddModel model)
        {

            var repoModel = new DMRepository()
            {
                Id = Guid.NewGuid(),
                URL = model.URL,
                TypeId = model.TypeId,
                UserId = model.UserId,
                ProjectId = Context.Projects.FirstOrDefault(x => x.Name == model.ProjectName).Id
            };

            Context.Repositories.Add(repoModel);
            Context.SaveChanges();

        }



        public void AddProject(ArrayList model)
        {
            DMProject projectModel = new DMProject();

            foreach (var item in model)
            {
                if (item.GetType() == typeof(ProjectAddModel))
                {
                    ProjectAddModel project = (ProjectAddModel)item;
                    projectModel = new DMProject()
                    {
                        Id = Guid.NewGuid(),
                        Name = project.Name,
                        DepartmentId = project.Department,
                        UserId = project.User,
                        UserName = project.Username
                    };

                    this.AddRepositories(model, projectModel);
                }

            }
            Context.Projects.Add(projectModel);
            Context.SaveChanges();


        }

        public void AddRepositories(ArrayList model, DMProject project)
        {
            foreach (var item in model)
            {
                if (item.GetType() == typeof(RepositoryAddModel))
                {
                    RepositoryAddModel repository = (RepositoryAddModel)item;
                    var repoModel = new DMRepository()
                    {
                        Id = Guid.NewGuid(),
                        URL = repository.URL,
                        TypeId = repository.TypeId,
                        UserId = project.UserId,
                        ProjectId = project.Id
                    };

                    Context.Repositories.Add(repoModel);

                }
            }
        }


        public void DeleteRepo(RepositoryDeleteModel info)
        {
            string url = info.URL;
            Guid id = info.Id;

            if (Context.Repositories.Any(x => x.URL == url && x.TypeId == id))
            {
                var repoToDelete = Context.Repositories.FirstOrDefault(x => x.URL == url && x.TypeId == id);
                Context.Repositories.Remove(repoToDelete);
                Context.SaveChanges();
            }


        }

    }
}
