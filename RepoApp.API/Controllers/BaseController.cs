using NLog;
using RepoApp.BLL.Repositories;
using RepoApp.Common;
using RepoApp.Common.DataTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Routing;

namespace RepoApp.API.Controllers
{
    public class BaseController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected IHttpActionResult CreateDataTablesResult<Record>(IEnumerable<Record> records, DataTablesParameters parameters)
        {
            return Json(new { draw = parameters.Draw, recordsTotal = parameters.TotalCount, recordsFiltered = parameters.TotalCount, data = records });
        }

        protected JsonResult<ExecutionResult> CreateJsonValidationError(Dictionary<string, string> valMessages)
        {
            ExecutionResult result = new ExecutionResult { ExecutionStatus = ResultOutcome.NOTVALID, ValidationMessages = valMessages };
            return Json(result);
        }

        protected JsonResult<ExecutionResult> CreateJsonOk()
        {
            ExecutionResult result = new ExecutionResult { ExecutionStatus = ResultOutcome.OK };
            return Json(result);
        }

        protected IHttpActionResult CreateExceptionLog(Exception exception, string ErrorMessage = "")
        {
            logger.Error(exception, "Custom CreateException (Base API Contoller");

            return Content(HttpStatusCode.BadRequest, ErrorMessage);
        }

        protected Guid GetCurrentUserId()
        {
            Guid id;
            using (UserRepository repo = new UserRepository())
            {
                id = repo.GetUser(User.Identity.Name).Id;
            }

            return id;
        }

        public virtual JsonResult<ExecutionResult> JsonViewUnValidResultAPI(string controllerName, string viewName, object viewData, ExecutionResult errors)
        {
            string Html = RenderViewToString(controllerName, viewName, viewData);

            return Json(new ExecutionResult { Data = Html, validationResult = ValidationResult.UnValid, ValidationMessages = errors.ValidationMessages });

        }

        public virtual JsonResult<ExecutionResult> JsonViewValidResultAPI(string controllerName, string viewName, object viewData, ExecutionResult errors)
        {
            string Html = RenderViewToString(controllerName, viewName, viewData);

            return Json(new ExecutionResult { Data = Html, validationResult = ValidationResult.Valid, ValidationMessages = errors.ValidationMessages });
        }

        public static string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);
                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new FakeController());
                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);

                var viewContext = new ViewContext(fakeControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }

        public class FakeController : ControllerBase { protected override void ExecuteCore() { } }


        //protected JsonResult<List<JsTreeNodeModel>> CreateJsTreeResult(List<JsTreeNodeModel> model)
        //{
        //    return Json(model);
        //}

        //public IHttpActionResult DownloadFile(byte[] bytes, string file_name)
        //{
        //    HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
        //    responseMsg.Content = new ByteArrayContent(bytes);
        //    responseMsg.Content.Headers.ContentLength = bytes.LongLength;
        //    responseMsg.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //    responseMsg.Content.Headers.ContentDisposition.FileName = file_name;
        //    responseMsg.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(file_name));

        //    return ResponseMessage(responseMsg);
        //}

    }
}
