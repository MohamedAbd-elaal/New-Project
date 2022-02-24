using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using MVC_FILE_VIEW_.Model;
using MVC_FILE_VIEW_.ViewModel;
using System;
using System.IO;

namespace MVC_File.Controllers
{
    //[Route("home")]
    // token Attribute
    //[Route("[Controller]/[action]")]  // global attribut in Route (Controller can be home or any controller name and the action can be any method   
    //when use it here means cannot do any thing without login first
    // it isnot having any meaningful because the globly Authorize in ConfigureServices in startrup page
    [Authorize]
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller  // to run controller for mvc must name (HomeController)
    {
        // this is anthor way to run (instance from class(manage data))
        //but Interface is Better Because If many (manage data we have)so we need to instance all these classes
        // but un interface we change only  
        readonly IEmployeeRepositry iEmployeeRepositry = new IEmployeeRepositry();
        private readonly IEmployee _iEmployee;
        // this for get path of wwwroot file
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger<HomeController> logger;

        public HomeController(IEmployee iEmployee,IHostingEnvironment hostingEnvironment,ILogger<HomeController> logger)
        {
            this._iEmployee = iEmployee;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }
        //path (Home/Index)in html page
        //index his name in path is Action method
        // we can remove home we put it in out side the controller method
       // [Route("/Index")]// to search on it
        //[Route("~/")] // With LocalHost Get Index Action
        //[Route("home")] //if we search by only home (wthout the global Roure["home"])
        public Dept? Index() 
        {
            return _iEmployee.GetName(5).Hoppies;

        }
        public ViewResult Index1()
        {
            return View("mine/Index1");

        }
       // [Route("~/")]
        // for IEnumebale
        [AllowAnonymous] // reach it without ligin
        public ViewResult IEnumerable()
        {
            var y = _iEmployee.IenumMethod();
            return View(y);

        }
        
        //path (Home/Details)
        public Microsoft.AspNetCore.Mvc.JsonResult Details()
        {
            Employee model = _iEmployee.GetName(5);
            return Json(model);

        }
        public ObjectResult Details1()
        {
            Employee model = _iEmployee.GetName(5);
            return new ObjectResult(model);

        }
        // [Route("home/details2/{id?}")]
        //[Route("details2/{id?}")]
        [Authorize]// mean must login or user exist by register
        public Microsoft.AspNetCore.Mvc.ViewResult Details2(int? id)
        {
            //throw new Exception("Error in Details2 Page");
            // LogLevel (see enum for log level by see definationto log level)
            logger.LogTrace("logger Trace");
            logger.LogDebug("logger Debug");
            logger.LogInformation("logger Information");
            logger.LogWarning("logger Warning");
            logger.LogError("logger Error");
            logger.LogCritical("logger Critical");
            Employee model = _iEmployee.GetName(id??5);
            //return View("~/myviews/test.cshtml");// absolute path (full path),(~) tilde (go to root of main project)
            // return View("../../myviews/test");//relative path (not full path(without cshtml))
           // ViewData has a big error (didnot know the error before running) and other in screenshoot(3)
            ViewData["Employee"] = model;//Viewdata["Key"]=>name dictionary key (key)
            ViewData["Title"] = "Hello From Our Title";
            ViewBag.Employee = model;
            ViewBag.Title = "Hello dynamic property(. ) to Our Title";
            // return View(model);
            // solve error(resource with specified id not found)
            Employee employee = _iEmployee.GetName(id.Value);
            if(employee==null)
            {
                Response.StatusCode = 404;
                return View("Employee Not Found",id);
                //anthor soluthion
            //    var statuscode = Response.StatusCode = 404;
            //    EmployeeNotFound employeeNotFound = new EmployeeNotFound
            //    {
            //        statuscode = statuscode,
            //        id = id.Value
            //    };


            //    return View("Employee Not Found", employeeNotFound);
            //}
            }
            // we create class and put on it all data because there a data cannot contain in passing para like string (title)
            HomeDetails2ViewModel homeDetails2ViewModel = new HomeDetails2ViewModel()
            {
                Employee = employee,
                titleClass="Hello From Class"

             };
            return View(homeDetails2ViewModel);
            //with out the solution of 404
            //HomeDetails2ViewModel homeDetails2ViewModel = new HomeDetails2ViewModel()
            //{
            //    Employee = _iEmployee.GetName(id ?? 9),
            //    titleClass = "Hello From Class"

            //};
            //return View(homeDetails2ViewModel);


        }
        [HttpGet]
        [Authorize]
        public ViewResult create()
        {
            return View();
        }
        
        
        //[HttpPost]
        ////public IActionResult create(Employee employee)
        //////IActionResult it has (RedirectToAction and view)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        Employee emp = _iEmployee.Add(employee);
        ////        return RedirectToAction("details2", new { id = emp.Id });
        ////    }
        ////    return View();

        ////}
        [HttpPost]
        [Authorize]
        public IActionResult create(EmployeeCreateViewModel model)
        //IActionResult it has (RedirectToAction and view)
        {
            if (ModelState.IsValid)
            {
                //
                string Uniquefilename = ProcessUploadFile(model);
                


                Employee emp = new Employee
                {
                    Name = model.Name,
                    Hoppies = model.Hoppies,
                    photopath = Uniquefilename
                };
                _iEmployee.Add(emp);
                 return RedirectToAction("details2", new { id = emp.Id });
            }
            return View();

        }
        [HttpGet]
        [Authorize]
        public ViewResult Edit(int id)
        {

            Employee model = _iEmployee.GetName(id);

            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                id = model.Id,
                Name = model.Name,
                Hoppies = model.Hoppies,
                ProperyPath = model.photopath
            };
            return View(employeeEditViewModel);




        }
        [HttpPost]
        [Authorize]
        public IActionResult Edit(EmployeeEditViewModel model)
        //IActionResult it has (RedirectToAction and view)
        {
            
            if (ModelState.IsValid)
            {
                Employee employee = _iEmployee.GetName(model.id);
                employee.Name = model.Name;
                employee.Hoppies = model.Hoppies;
                if(model.photopath != null)
                {
                    if (model.ProperyPath!=null)
                    {
                        string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Images", model.ProperyPath);
                        System.IO.File.Delete(filepath);
                    }
                    employee.photopath= ProcessUploadFile(model);
                }

                

             
                _iEmployee.Update(employee);
                return RedirectToAction("IEnumerable");
            }
            return View();

        }
        private string ProcessUploadFile(EmployeeCreateViewModel model)
        {
            string Uniquefilename = null;
            if (model.photopath != null )
           {
               
                    string uploadfolder = Path.Combine(hostingEnvironment.WebRootPath, "Images");
                    //way for unique file (file with same name override it(not two file with the same name))
                  Uniquefilename = Guid.NewGuid().ToString() + "_" + model.photopath.FileName;
                    string FilePath = Path.Combine(uploadfolder, Uniquefilename);
                //to copy file(image) from server to folder(images)
                using(var filestream= new FileStream(FilePath, FileMode.Create)) 
                { 
                 model.photopath.CopyTo(filestream);
                }
                

            }

            return Uniquefilename;
        }
    }
    // use foreach to  must choose multiable photos
    //private string ProcessUploadFile(EmployeeCreateViewModel model)
    //{
    //    string Uniquefilename = null;
    //    if (model.photopath != null && model.photopath.Count > 1)
    //    {
    //        foreach (IFormFile photo in model.photopath)
    //        {
    //            string uploadfolder = Path.Combine(hostingEnvironment.WebRootPath, "Images");
    //            //way for unique file (file with same name override it(not two file with the same name))
    //            Uniquefilename = Guid.NewGuid().ToString() + "_" + photo.FileName;
    //            string FilePath = Path.Combine(uploadfolder, Uniquefilename);
    //            //to copy file(image) from server to folder(images)
    //            photo.CopyTo(new FileStream(FilePath, FileMode.Create));
    //        }
    //    }

    //    return Uniquefilename;
    //}
}

