﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TestApp.DAL;
using TestApp.Helpers;
using TestApp.Models;

namespace TestApp.Controllers
{
    public class EmployeeController : Controller
    {

        EmployeeDBContext context = new EmployeeDBContext();

        public EmployeeController()
        {
            //if (context.Employees.Any())
            //    return;

            ////Populate Employee table
            //for (int i = 1; i < 5; i++)
            //{
            //    var emp = new Employee
            //    {
            //        PersonnelID = Convert.ToString(i),
            //        EmployeeFullName = $"Employee_{i}",
            //        DateOfBirth = Convert.ToString( DateTime.Now.AddYears(-20) ),
            //        Gender = "male",
            //        IsStaffMember = true
            //    };

            //    context.Employees.Add(emp);
            //    context.SaveChanges();
            //}
        }

        /// <summary>
        /// Retrieve Employee collection or concrete entity
        /// </summary>
        /// <param name="id">Employee inner identifier</param>
        /// <returns></returns>
        public ActionResult Index(int id = 0)
        {           
            IEnumerable<Employee> employees = null;
            if (id > 0)
            {
                employees = employees = context.Employees.Where(t => t.EmployeeID == id);
            }
            else
            { employees = context.Employees; }

            return View();
        }

        /// <summary>
        /// Generate json string for populate grid 
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployeeJSON()
        {
            // получаем из бд все объекты Employee
            IEnumerable<Employee> employees = context.Employees;
            // передаем все объекты в динамическое свойство Employees в ViewBag
            //ViewBag.Employees = employees;
            //

            // возвращаем представление
            return Content(JsonConvert.SerializeObject(employees), "application/json");
        }

        /// <summary>
        /// Delete Employee by inner identifier
        /// </summary>
        /// <param name="id">Employee inner identifier</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveEmployee(int id = 0)
        {
            var employee = context.Employees.Where(t => t.EmployeeID == id).First();
            context.Entry(employee).State = EntityState.Deleted;
            context.SaveChanges();

            ResultState resultState = new ResultState();
            resultState.IsSuccess = true;
            resultState.Message = "Success";
            return Json(resultState);
        }

        /// <summary>
        /// Save new Enployee record
        /// </summary>
        /// <param name="employee">New Employee entity</param>
        /// <returns></returns>
        public ActionResult SaveEmployee(Employee employee)
        {
            //TODO save new item
            ResultState resultState = new ResultState();

            context.Entry(employee).State = EntityState.Added;

            string report = employee.GetValidateReport();

            if (String.IsNullOrEmpty(report) )
            {
                context.SaveChanges();
                resultState.IsSuccess = true;
                resultState.Message = "Success";
            }
            else
            {
                resultState.IsSuccess = false;
                resultState.Message = report;
            }
            

            
            
            return Json(resultState);
        }

        /// <summary>
        /// Edit existing Employee
        /// </summary>
        /// <param name="employee">Selected Employee to edit</param>
        /// <returns></returns>
        public ActionResult EditEmployee(Employee employee)
        {
            //TODO save edited item
            //var employee = context.Employees.Where(t => t.EmployeeID == id).First();

            context.Entry(employee).State = EntityState.Modified;
            context.SaveChanges();

            //return Content("Success");            

            ResultState resultState = new ResultState();
            resultState.IsSuccess = true;
            resultState.Message = "Success";
            return Json(resultState);
        }

        /// <summary>
        /// Upload file with serialized Employee collection
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile()
        {
            string loadingResult = String.Empty;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                //TODO save file here or read InputStream

                FileLoader fileLoader = new FileLoader(file.InputStream, file.ContentLength);
             
                string jsonString = fileLoader.GetTextData();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Employee> employees = serializer.Deserialize<List<Employee>>(jsonString);

                loadingResult = SaveEmployeeFromList(employees);               

            }
            return Content(loadingResult);
        }


        /// <summary>
        /// Save list into Employee table
        /// </summary>
        /// <param name="employees">Employee collection</param>
        private string SaveEmployeeFromList(List<Employee> employees)
        {
            //TODO Move to Repository
            List<LoadingResult> loadingResults = new List<LoadingResult>();
            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                bool alreadyExists = false;
                string report = emp.GetValidateReport();
                var existedEntity = context.Employees
                    .Where(t => t.PersonnelID == emp.PersonnelID
                                && t.IsStaffMember == emp.IsStaffMember
                          )?.FirstOrDefault();

                if (existedEntity != null)
                { alreadyExists = true; }

                if (alreadyExists)
                {
                    context.Entry(existedEntity).State = EntityState.Modified;
                }

                if (String.IsNullOrEmpty(report))
                {
                    if (!alreadyExists)
                    { context.Employees.Add(emp); }

                    context.SaveChanges();
                }

                LoadingResult result = new LoadingResult
                {
                    Entity = $"{emp.PersonnelID}|{emp.EmployeeFullName}|{emp.DateOfBirth}|{emp.Gender}|{emp.IsStaffMember}|",
                    AlreadyExists = alreadyExists,
                    ErrorMessage = report
                };

                loadingResults.Add(result);
            }

            return loadingResults.ToJSON();
            //context.SaveChanges();
        }
    }

    public class ResultState
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}