using Newtonsoft.Json;
using RepoApp.BLL.Models.AddModels;
using RepoApp.BLL.Models.DetailModels;
using RepoApp.BLL.Models.EditModels;
using RepoApp.BLL.Repositories;
using RepoApp.Common;
using RepoApp.Common.DataTables;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace RepoApp.Controllers
{
    [Authorize(Roles = "Admin, Operator, User")]

    public class UserController : BaseController
    {
        [Authorize(Roles = "Admin, Operator, User")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetUsers(DataTablesParameters parameters)
        {
            using (UserRepository repo = new UserRepository())
            {
                var userList = repo.GetUsers(parameters);
                return CreateDataTablesResult(userList, parameters);

            }

        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Add()
        {

            using (UserRepository repo = new UserRepository())
            {
                var roles = repo.GetRoles();
                ViewBag.Roles = new MultiSelectList(roles, "Id", "Name");
            }
            return PartialView("_Add");
        }

        [HttpPost]
        public ActionResult Add(UserAddModel model)
        {
            try
            {
                using (UserRepository repo = new UserRepository())
                {
                    var roles = repo.GetRoles();
                    ViewBag.Roles = new MultiSelectList(roles, "Id", "Name");
                }
                using (UserRepository repo = new UserRepository())
                {

                    if (ModelState.IsValid)
                    {
                        using (var client = new HttpClient())
                        {
                            string startUrl = _WebServiceUrl + "/api/user/";
                            client.BaseAddress = new Uri(startUrl);

                            var postTask = client.PostAsJsonAsync("Add", model).Result;
                            var result = JsonConvert.DeserializeObject<ExecutionResult>(postTask.Content.ReadAsStringAsync().Result);

                            if (postTask.IsSuccessStatusCode)
                            {
                                return JsonViewValidResult("~/Views/User/Index.cshtml");
                            }

                            foreach (var a in result.ValidationMessages)
                            {
                                ModelState.AddModelError(a.Key, a.Value);
                            }
                        }
                    }
                }

                return JsonViewUnValidResult("~/Views/User/_Add.cshtml", model);
            }
            catch (Exception ex)
            {
                return CreateExceptionView(ex);
            }

        }


        [Authorize(Roles = "Admin, Operator")]
        [HttpGet]
        public ActionResult GetEdit(Guid id)
        {
            using (UserRepository repo = new UserRepository())
            {
                var roles = repo.GetRoles();
                var model = repo.GetEdit(id);
                ViewBag.Roles = new MultiSelectList(roles, "Id", "Name");
                return PartialView("_Edit", model);
            }
        }
        //[HttpGet]
        //public ActionResult GetEdit(Guid id)
        //{

        //    try
        //    {

        //        using (UserRepository repo = new UserRepository())
        //        {
        //            var roles = repo.GetRoles();
        //            var model = repo.GetEdit(id);
        //            ViewBag.Roles = new MultiSelectList(roles, "Id", "Name");
        //        }

        //        using (var client = new HttpClient())
        //        {
        //            string startUrl = _WebServiceUrl + "/api/user/getedit/" + id;
        //            client.BaseAddress = new Uri(startUrl);
        //            var response = client.GetStringAsync(startUrl).Result;
        //            var model = JsonConvert.DeserializeObject<UserEditModel>(response);
        //            return PartialView("_Edit", model);

        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        return CreateExceptionView(ex);
        //    }
        //}

        [HttpPost]
        public ActionResult Edit(UserEditModel model)
        {

            try
            {
                using (UserRepository repo = new UserRepository())
                {
                    var roles = repo.GetRoles();
                    ViewBag.Roles = new MultiSelectList(roles, "Id", "Name");
                }


                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        string startUrl = _WebServiceUrl + "/api/user/";
                        client.BaseAddress = new Uri(startUrl);

                        var postTask = client.PostAsJsonAsync("Edit", model).Result;
                        var result = JsonConvert.DeserializeObject<ExecutionResult>(postTask.Content.ReadAsStringAsync().Result);

                        if (postTask.IsSuccessStatusCode)
                        {
                            return JsonViewValidResult("~/Views/User/Index.cshtml");
                        }

                        foreach (var a in result.ValidationMessages)
                        {
                            ModelState.AddModelError(a.Key, a.Value);
                        }
                    }

                }
                return JsonViewUnValidResult("~/Views/User/_Edit.cshtml", model);
            }

            catch (Exception ex)
            {
                return CreateExceptionView(ex);
            }

        }



        [HttpGet]
        public ActionResult GetDetails(Guid id)
        {
            using (UserRepository repo = new UserRepository())
            {
                var userDetails = repo.GetDetails(id);
                return PartialView("_Details", userDetails);
            }
        }

        //    [HttpGet]
        //public ActionResult GetDetails(Guid id)
        //{
        //    try
        //    {

        //        var httpClient = new HttpClient();
        //        var response = httpClient.GetStringAsync("https://localhost:44367/api/user/getdetails/" + id).Result;

        //        UserDetailsModel model = JsonConvert.DeserializeObject<UserDetailsModel>(response);

        //        return PartialView("_Details", model);

        //    }

        //    catch (Exception ex)
        //    {
        //        return CreateExceptionView(ex);
        //    }
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            using (UserRepository repo = new UserRepository())
            {
                var model = repo.GetDelete(id);
                return PartialView("_Delete", model);
            }
        }
        [HttpPost]
        public ActionResult Delete(UserDetailsModel model)
        {
            using (UserRepository repo = new UserRepository())
            {
                repo.Delete(model);
            }
            return JsonViewValidResult("~/Views/User/Index.cshtml");

        }
        //[HttpGet]
        //public JsonResult GetRole(string name)
        //{

        //    var httpClient = new HttpClient();
        //    var response = httpClient.GetStringAsync("https://localhost:44367/api/user/GetSuperiorRole/" + name).Result;


        //    return Json(response, JsonRequestBehavior.AllowGet);

        //}

    }
}



