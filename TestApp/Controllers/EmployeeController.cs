using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TestApp.DAL;

namespace TestApp.Controllers
{
    public class EmployeeController : Controller
    {

        EmployeeContext context = new EmployeeContext();

        public EmployeeController()
        {
            if (context.Employees.Any())
                return;

            for (int i = 1; i < 5; i++)
            {
                var emp = new Employee
                {
                    PersonnelID = i,
                    EmployeeFullName = $"Employee_{i}",
                    DateOfBirth = DateTime.Now.AddYears(-20),
                    Gender = "male",
                    IsStaffMember = true
                };

                context.Employees.Add(emp);
                context.SaveChanges();
            }
        }

        // GET: Employee
        public ActionResult Index(int id = 0)
        {
            //return Json(new { status = "success", message = "customer created" }, JsonRequestBehavior.AllowGet);

            // получаем из бд все объекты Employee
            IEnumerable<Employee> employees = null;
            if (id > 0)
            {
                employees = employees = context.Employees.Where(t => t.EmployeeID == id);
            }
            else
            { employees = context.Employees; }
                
                
            // передаем все объекты в динамическое свойство Employees в ViewBag
            ViewBag.Employees = employees;

            return View();
        }

        public ActionResult EmployeeJSON()
        {
            //return Json(new { status = "success", message = "customer created" }, JsonRequestBehavior.AllowGet);

            // получаем из бд все объекты Employee
            IEnumerable<Employee> employees = context.Employees;
            // передаем все объекты в динамическое свойство Employees в ViewBag
            //ViewBag.Employees = employees;

            var result = Content(JsonConvert.SerializeObject(employees), "application/json");
            // возвращаем представление
            return Content(JsonConvert.SerializeObject(employees), "application/json");
        }
       
        [HttpPost]
        public ActionResult RemoveEmployee(int id = 0)
        {
            var employee = context.Employees.Where(t => t.EmployeeID == id).First();
            context.Entry(employee).State = EntityState.Deleted;
            context.SaveChanges();

            return Content("Success");
        }

        public ActionResult SaveEmployee(Employee employee)
        {
            //TODO save new item
            context.Entry(employee).State = EntityState.Added;
            context.SaveChanges();

            ResultState resultState = new ResultState();
            resultState.IsSuccess = true;
            resultState.Message = "Success";
            return Json(resultState);
        }

        public ActionResult EditEmployee(Employee employee)
        {
            //TODO save new item
            //var employee = context.Employees.Where(t => t.EmployeeID == id).First();

            context.Entry(employee).State = EntityState.Modified;
            context.SaveChanges();

            //return Content("Success");

            

            ResultState resultState = new ResultState();
            resultState.IsSuccess = true;
            resultState.Message = "Success";
            return Json(resultState);
        }

        public JsonResult UploadFile()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                //TODO save file here or read InputStream

                BinaryReader b = new BinaryReader(file.InputStream);
                byte[] binData = b.ReadBytes(file.ContentLength);

                string jsonString = System.Text.Encoding.UTF8.GetString(binData);

                JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                List<Employee> employees = oSerializer.Deserialize<List<Employee>>(jsonString);
                SaveEmployee(employees);

            }
            return Json(new { UploadedFileCount = Request.Files.Count });
        }

        private void SaveEmployee(List<Employee> employees)
        {

            foreach (var emp in employees)
            {
                var employee = context.Employees.Where(t => t.EmployeeID == id).First();
            }
            context.Employees.Add(emp);
            context.SaveChanges();
        }
    }

    public class ResultState
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}