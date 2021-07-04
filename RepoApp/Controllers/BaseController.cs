using NLog;
using RepoApp.BLL.Repositories;
using RepoApp.Common;
using RepoApp.Common.DataTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepoApp.Controllers
{

    public class BaseController : Controller
    {
        protected string _WebServiceUrl
        {
            get
            {
                return Request.Url.Scheme + "://" + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
            }
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected JsonResult CreateDataTablesResult<Record>(IEnumerable<Record> records, DataTablesParameters parameters)
        {
            return Json(new { draw = parameters.Draw, recordsTotal = parameters.TotalCount, recordsFiltered = parameters.TotalCount, data = records });
        }

       

        protected ViewResult CreateExceptionView(Exception exception)
        {
            logger.Error(exception, "Custom CreateExceptionView Exception (BaseController)");

            ViewBag.Message = exception.Message;
            return View("~/Views/Shared/Error.cshtml");

        }

        //protected int GetCurrentUserId()
        //{
        //    int id;
        //    using (UserRepository repo = new UserRepository())
        //    {
        //        id = repo.GetUser(User.Identity.Name).Id;
        //    }

        //    return id;
        //}

        protected JsonResult JsonNet(object data)
        {
            return new JsonNetResult
            {
                Data = data
            } as JsonResult;
        }

        protected JsonResult JsonNet(object data, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                JsonRequestBehavior = behavior
            } as JsonResult;
        }

        protected JsonResult JsonNet(object data, string contentType)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                Data = data
            } as JsonResult;
        }

        protected JsonResult JsonNet(object data, string contentType, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                Data = data,
                JsonRequestBehavior = behavior
            } as JsonResult;
        }

        protected JsonResult JsonNet(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                Data = data
            } as JsonResult;
        }

        protected JsonResult JsonNet(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                Data = data,
                JsonRequestBehavior = behavior
            } as JsonResult;
        }

        protected virtual JsonResult CreateJsonError()
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.ERROR, Message = "Обратись к администратору", LongMessage = "" });
        }

        protected virtual JsonResult CreateJsonError(string message)
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.ERROR, Message = message, LongMessage = "" });
        }

        protected virtual JsonResult CreateJsonError(string message, string longMessage)
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.ERROR, Message = message, LongMessage = longMessage });
        }

        protected virtual JsonResult CreateJsonOK()
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.OK, Message = "", LongMessage = "" });
        }

        protected virtual JsonResult CreateJsonOK(string message)
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.OK, Message = message, LongMessage = "" });
        }

        protected virtual JsonResult CreateJsonOK(string message, string longMessage)
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.OK, Message = message, LongMessage = "" });
        }

        protected virtual JsonResult CreateJsonResult<T>(T data) where T : class, new()
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.OK, Message = "", LongMessage = "", Data = data });
        }

        protected virtual JsonResult CreateJsonResult<T>(IEnumerable<T> data) where T : class, new()
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.OK, Message = "", LongMessage = "", Data = data });
        }

        protected virtual JsonResult CreateJsonResult<T>(T data, string message) where T : class, new()
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.OK, Message = message, LongMessage = "", Data = data });
        }


        protected virtual JsonResult CreateJsonResult<RecordModelT>(RecordModelT[] array) where RecordModelT : class, new()
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.OK, Message = "", LongMessage = "", Data = array });
        }

        protected virtual JsonResult CreateJsonResult(string[] array)
        {
            return JsonNet(new ExecutionResult { ExecutionStatus = ResultOutcome.OK, Message = "", LongMessage = "", Data = array });
        }

        public string ViewToHtml(ControllerContext context, string viewPath, object model = null)
        {
            context.Controller.ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(context, viewPath, null);
                var viewContext = new ViewContext(context, viewResult.View, context.Controller.ViewData, context.Controller.TempData, writer);

                viewResult.View.Render(viewContext, writer);
                viewResult.ViewEngine.ReleaseView(context, viewResult.View);
                return writer.GetStringBuilder().ToString();
            }
        }

        public virtual JsonResult JsonViewValidResult(string viewPath, object model = null)
        {
            string view = string.IsNullOrWhiteSpace(viewPath) ? string.Empty : ViewToHtml(this.ControllerContext, viewPath, model);
            return Json(new { validationResult = ValidationResult.Valid, viewPage = view }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult JsonViewUnValidResult(string viewPath, object model = null)
        {
            string view = string.IsNullOrWhiteSpace(viewPath) ? string.Empty : ViewToHtml(this.ControllerContext, viewPath, model);
            return Json(new { validationResult = ValidationResult.UnValid, viewPage = view }, JsonRequestBehavior.AllowGet);
        }
    }
}
