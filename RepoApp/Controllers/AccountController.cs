using RepoApp.BLL.Models.AuthenticationModels;
using RepoApp.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RepoApp.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Initial()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUserModel model)
        {
            if (ModelState.IsValid)
            {
                using (UserRepository repo = new UserRepository())
                {
                    var user = repo.GetUser(model.UserName, model.Password);
                    if (user == null)
                    {
                        ModelState.AddModelError("Password", "Incorrect user name or password");
                    }
                    else
                    {
                        if (!repo.GetConnection(model.UserName))
                        {
                            ModelState.AddModelError("Password", "User is not connected");
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(user.UserName, true);
                            return RedirectToAction("Index", "Project");
                        }
                    }
                }
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }


    }
}