using MVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLibrary;
using DataLibrary.BusinnesLogic;
using MVCApp.Models.Intrefaces;
using MVCApp.Factories;
using System.Threading.Tasks;

// special C# 7 using static DataLibray.BusinessLogic.EmployeeProcessor

namespace MVCApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SignUp()
        {
            ViewBag.Message = "Employee Sign Up!";

            return View();
        }

        public ActionResult ViewEmployees()
        {
            ViewBag.Message = "Employee Sign Up!";

            List<EmployeeModel> employees = new List<EmployeeModel>();

            #region -- Utilise le DataTable --
            var pullEmmployees = EmployeeProcessor.PullEmployees();
            

            foreach (var item in pullEmmployees)
            {
                IEmployeeModel employeModel = Factory.CreateEmployee();
                employeModel.EmployeeId = item.EmployeeId;
                employeModel.FirstName = item.FirstName;
                employeModel.LastName = item.LastName;
                employeModel.EmailAddress = item.EmailAddress;
                
                employees.Add(employeModel as EmployeeModel);
            }
            #endregion

            #region -- Utilise une méthode générique SQL --
            var data = EmployeeProcessor.LoadEmployees();
            
            if (employees == null && employees.Count == 0)
            {
                foreach (var item in data)
                {
                    employees.Add(new EmployeeModel
                    {
                        EmployeeId = item.EmployeeId,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        EmailAddress = item.EmailAddress
                        //ConfirmEmail = item.EmailAddress
                    });
                }  
            }
            #endregion

            return View(employees.OrderBy(emp => emp.EmployeeId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                 var res =  EmployeeProcessor.CreateEmployee(
                                employee.EmployeeId,
                                employee.FirstName, 
                                employee.LastName, 
                                employee.EmailAddress);
                
                return RedirectToAction("ViewEmployees");
            }

            return View();
        }

        public ActionResult Delete()
        {
            ViewBag.Message = "Employee à supprimer";

            return View();
        }

        [HttpPost]
        public ActionResult Delete(EmployeeModel employee)
        {
            int id = 100002;

            var result = EmployeeProcessor.DeleteEmploye(id);
            if (result == 1)
            {
                Console.WriteLine("Correctely Deleted.");
            }
            else
            {
                Console.WriteLine("Error during delete operation!");
            }
            return View();
        }
    }
}