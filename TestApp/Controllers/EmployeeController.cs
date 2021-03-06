using Newtonsoft.Json;
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
using TestApp.Models.Validators;

namespace TestApp.Controllers
{
    public class EmployeeController : Controller
    {

        EmployeeDBContext context = new EmployeeDBContext();

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
            ResultState resultState = new ResultState();

            var employee = context.Employees.Where(t => t.EmployeeID == id).FirstOrDefault();

            if (employee == null)
            {
                resultState.IsSuccess = true;
                resultState.MessageHeader = "Удаление записи";
                resultState.MessageText   = "Ошибка: Запись не найдена";
                return Json(resultState);
            }

            context.Entry(employee).State = EntityState.Deleted;
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                resultState.IsSuccess = false;
                resultState.MessageHeader = "Удаление записи";
                resultState.MessageText   = $"Ошибка: {e.Message}";
                return Json(resultState);
            }           

            resultState.IsSuccess = true;
            resultState.MessageHeader = "Удаление записи";
            resultState.MessageText   = "Запись успешно удалена";
            return Json(resultState);
        }

        /// <summary>
        /// Save new Enployee record
        /// </summary>
        /// <param name="employee">New Employee entity</param>
        /// <returns></returns>
        public ActionResult SaveEmployee([Bind(Exclude = "EmployeeID")] Employee employee)
        {
            //TODO save new item          
            ResultState resultState = new ResultState();            

            string validationReport = String.Empty;
            
            bool isValidated = this.TryValidateModel(employee);

            if (!isValidated)
            {
                validationReport = GetValidationMessages(ModelState);
            }

            if (isValidated)
            {
                context.Entry(employee).State = EntityState.Added;

                context.SaveChanges();
                resultState.IsSuccess = true;
                resultState.MessageHeader = "Создание новой записи";
                resultState.MessageText   = "Данные успешно сохранены";
            }
            else
            {
                resultState.IsSuccess = false;
                resultState.MessageHeader = "Ошибка правил валидации";
                resultState.MessageText = validationReport;
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
            ResultState resultState = new ResultState();            

            string validationReport = String.Empty;

            bool isValidated = ModelState.IsValid;

            if (isValidated)
            {
                resultState.IsSuccess = true;
                resultState.MessageHeader = "Изменение записи";
                resultState.MessageText = "Изменения успешно сохранены";                
            }
            else
            {
                validationReport = GetValidationMessages(ModelState);

                resultState.IsSuccess = false;
                resultState.MessageHeader = "Ошибка Сохранения";
                resultState.MessageText = validationReport;
                return Json(resultState);
            }

            try
            {
                context.Entry(employee).State = EntityState.Modified;

                context.SaveChanges();
            }
            catch (Exception e)
            {
                resultState.IsSuccess = false;
                resultState.MessageHeader = "Ошибка Сохранения";
                resultState.MessageText   = e.Message;
                return Json(resultState);
            }

            return Json(resultState);
        }

        /// <summary>
        /// Upload file with serialized Employee collection
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile()
        {
            ResultState resultState = new ResultState();

            string loadingResult = String.Empty;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];

                //TODO save file here or read InputStream
                FileLoader fileLoader = new FileLoader(file.InputStream, file.ContentLength);
             
                string jsonString = fileLoader.GetTextData();

                List<Employee> employees = new List<Employee>();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                try
                {
                    employees = serializer.Deserialize<List<Employee>>(jsonString);
                }
                catch (Exception exp)
                {
                    resultState.IsSuccess = false;
                    resultState.MessageHeader = "Загрузка файла";
                    resultState.MessageText = $"Ошибка при загрузке: {exp.Message}";

                    return Json(resultState);
                }

                loadingResult = SaveEmployeeFromList(employees);
            }

            resultState.IsSuccess = true;
            resultState.MessageHeader = "Загрузка файла";
            resultState.MessageText   = "Детали выполнения смотрите в журнале";
            resultState.ResponseBody  = loadingResult;

            return Json(resultState);
        }


        /// <summary>
        /// Save list into Employee table
        /// </summary>
        /// <param name="employees">Employee collection</param>
        private string SaveEmployeeFromList(List<Employee> employees)
        {
            List<LoadingResult> loadingResults = new List<LoadingResult>();           

            foreach (var emp in employees)
            {
                bool alreadyExists = false;
                string validationReport = String.Empty;

                //Clear previous validation errors
                ModelState.Clear();

                bool isValidated = this.TryValidateModel(emp);
   
              
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

                if (isValidated)
                {
                    if (!alreadyExists)
                    { context.Employees.Add(emp); }

                    context.SaveChanges();
                }
                else
                {
                    validationReport = GetValidationMessages(ModelState);
                }

                LoadingResult result = new LoadingResult
                {
                    Entity = $"{emp.PersonnelID}|{emp.EmployeeFullName}|{emp.DateOfBirth}|{emp.Gender}|{emp.IsStaffMember}|",
                    AlreadyExists = alreadyExists,
                    ErrorMessage = validationReport
                };

                loadingResults.Add(result);               
            }

            return loadingResults.ToJSON();
        }

        private string GetValidationMessages(ModelStateDictionary controllerModelState)
        {
            StringBuilder sbErr = new StringBuilder();

            //Get errors from ModelState
            foreach (KeyValuePair<string, ModelState> kvp in controllerModelState)
            {
                var modelState = kvp.Value;

                foreach (var error in modelState.Errors)
                {
                    sbErr.AppendLine($"{ kvp.Key} : { error.ErrorMessage} ");
                }
            }

            return sbErr.ToString();
        }
    }
}