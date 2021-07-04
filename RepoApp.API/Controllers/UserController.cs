using RepoApp.BLL.Models.AddModels;
using RepoApp.BLL.Models.EditModels;
using RepoApp.BLL.Repositories;
using RepoApp.Common.DataTables;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace RepoApp.API.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost]
        public IHttpActionResult GetUsers(DataTablesParameters parameters)
        {
            try
            {
                using (UserRepository repo = new UserRepository())
                {
                    var userList = repo.GetUsers(parameters);
                    return CreateDataTablesResult(userList, parameters);

                }
            }
            catch (Exception ex)
            {
                return CreateExceptionLog(ex);
            }

        }
        //[Route("api/user/GetSuperiorRole/{name}")]
        [HttpGet]
        public IHttpActionResult GetRole(string name)
        {

            using (UserRepository repo = new UserRepository())
            {
                var details = repo.GetSuperiorRoleName(name);

                return Json(details);
            }
        }


        [HttpGet]
        public IHttpActionResult GetEdit(Guid id)
        {

            using (UserRepository repo = new UserRepository())
            {
                if (repo.UserExists(id))
                {
                    var model = repo.GetEdit(id);
                    return Json(model);

                }
                return BadRequest();
            }
        }
        [HttpPost]
        public IHttpActionResult Add(UserAddModel model)
        {
            try
            {
                //ExecutionResult execResult = new ExecutionResult();

                using (UserRepository repo = new UserRepository())
                {
                    Dictionary<string, string> errors = new Dictionary<string, string>();
                    if (ModelState.IsValid)
                    {
                        if (repo.CheckUserName(model.UserName))
                        {
                            //execResult.ExecutionStatus = ResultOutcome.NOTVALID;
                            //execResult.ValidationMessages.Add("UserName", "User name already exists");
                            errors.Add("UserName", "User name already exists");
                        }
                        if (repo.CheckEmail(model.Email))
                        {
                            // execResult.ExecutionStatus = ResultOutcome.NOTVALID;
                            //execResult.ValidationMessages.Add("Email", "Email already exists");
                            errors.Add("Email", "Email already exists");

                        }
                        if (!repo.CheckUserName(model.UserName) && !repo.CheckEmail(model.Email))
                        {
                            repo.Add(model);
                            return CreateJsonOk();

                        }
                    }
                    return CreateJsonValidationError(errors);

                }

            }


            catch (Exception ex)
            {
                return CreateExceptionLog(ex);
            }
        }



        [HttpPost]
        public IHttpActionResult Edit(UserEditModel model)
        {

            var changePassword = false;
            var equalPasswords = false;
            var changeRoles = false;

            try
            {
                // ExecutionResult execResult = new ExecutionResult();
                Dictionary<string, string> errors = new Dictionary<string, string>();

                if (ModelState.IsValid)
                {
                    using (UserRepository repo = new UserRepository())
                    {

                        if (model.IsChangePassword)
                        {


                            if (string.IsNullOrEmpty(model.Password))
                            {
                                //execResult.ExecutionStatus = ResultOutcome.NOTVALID;
                                errors.Add("Password", "Insert password");
                                changePassword = true;
                                equalPasswords = true;

                            }


                            else if (!model.Password.Equals(model.ConfirmPassword))
                            {

                                //execResult.ExecutionStatus = ResultOutcome.NOTVALID;
                                errors.Add("ConfirmPassword", "Passwords don't match");
                                equalPasswords = true;

                            }

                        }

                        if (model.IsChangeRoles)
                        {
                            if (model.Roles.Count == 0)
                            {
                                //execResult.ExecutionStatus = ResultOutcome.NOTVALID;
                                errors.Add("IsChangeRoles", "Select at least one role");
                                changeRoles = true;
                            }
                        }

                        if (repo.CheckUserNameForEdit(model.UserName, model.Id))
                        {
                            // execResult.ExecutionStatus = ResultOutcome.NOTVALID;
                            errors.Add("UserName", "User name already exists");
                        }
                        if (repo.CheckUserEmailForEdit(model.Email, model.Id))
                        {
                            //execResult.ExecutionStatus = ResultOutcome.NOTVALID;
                            errors.Add("Email", "Email already exists");
                        }
                        if (!repo.CheckUserNameForEdit(model.UserName, model.Id)
                            && !repo.CheckUserEmailForEdit(model.Email, model.Id)
                            && !changePassword && !changeRoles && !equalPasswords)
                        {
                            // model.Id = GetCurrentUserId();
                            repo.Edit(model);
                            return CreateJsonOk();


                        }


                    }
                }
                return CreateJsonValidationError(errors);


            }

            catch (Exception ex)
            {
                return CreateExceptionLog(ex);
            }
        }
        [HttpGet]
        public IHttpActionResult GetDetails(Guid id)
        {
            try
            {

                using (UserRepository repo = new UserRepository())
                {

                    if (repo.UserExists(id))
                    {
                        var details = repo.GetDetails(id);
                        return Json(details);
                    }
                    else
                        return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return CreateExceptionLog(ex);
            }

        }

    }
}
