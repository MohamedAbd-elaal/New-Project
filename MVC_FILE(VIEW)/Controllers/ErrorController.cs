using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC_FILE_VIEW_.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        //solve wrong URL
        [Route("Error/{statuscode}")]
        public IActionResult HttpStutasCodeHandlar(int statuscode)
        {
            var StatusCoderesult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statuscode)
            {
                case 404:
                    ViewBag.ErrorMessage = "The Resourse U Order Not found";
                    ViewBag.Path = StatusCoderesult.OriginalPath;
                    ViewBag.QS = StatusCoderesult.OriginalQueryString;
                    // we use way (string interploation )($"words{what want to appear like placeholder}"
                    // log Exception
                    logger.LogWarning($"path is {StatusCoderesult.OriginalPath}" + $" Query String is:{StatusCoderesult.OriginalQueryString}");
                    break;
            }
                
            return View("~/Views/Home/Not Found.cshtml");
        }
        // solve throw new exception in details2 method(bt go to meaningful page(view) 
        // reverse comment on throw in datails2
        [Route("Error")]
        [AllowAnonymous]
        public ViewResult Error()
        {
            var ExcepionsDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            // go to error Exception before  go to view (all it in output)
            // log Exception
            logger.LogError($"The Path is:{ExcepionsDetails.Path} throw an Exception {ExcepionsDetails.Error}");
            ViewBag.Exceptionpath = ExcepionsDetails.Path;
            ViewBag.ExceptionMessage = ExcepionsDetails.Error.Message;
            ViewBag.StackTrace = ExcepionsDetails.Error.StackTrace;
            return View("~/Views/Home/Error.cshtml");
        }
    }
}
