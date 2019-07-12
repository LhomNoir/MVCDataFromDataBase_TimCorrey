using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Helpers;
using DataLibrary.Factories;
using NLog;
using Dapper;

namespace DataLibrary.BusinnesLogic
{
    public static class EmployeeProcessor
    {
        #region Looger

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        static List<IEmployeeModel> employees = null;

        public static int CreateEmployee(int employeeId, string firstName, string lastName, string emailAdress)
        {
            _logger.Debug("EmployeeProcessor ==> CreateEmployee -- Création d'un employé...");

            IEmployeeModel employeModel = Factory.CreateEmployee();
                employeModel.EmployeeId = employeeId;
                employeModel.FirstName = firstName;
                employeModel.LastName = lastName;
                employeModel.EmailAddress = emailAdress;

            string sqlQuery = @"Insert Into dbo.Employee (EmployeeId, FirstName, LastName, EmailAddress)
                                Values(@EmployeeId, @FirstName, @LastName, @EmailAddress);";

            _logger.Trace(
                $"EmployeeProcessor ==> CreateEmployee : Appel de la méthode d'enregistrement d'une employé...");

            // --  -- 
            return SqlDataAccess.SaveData<IEmployeeModel>(sqlQuery, employeModel);
        }

        public static List<IEmployeeModel> LoadEmployees()
        {
            try
            {
                string sqlQuery = @"Select * from dbo.Employee;";

                employees = Factory.LoadEmployee();

                var model = SqlDataAccess.LoadData<EmployeeModel>(sqlQuery);

                employees.AddRange(model);

                return employees;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        public static List<IEmployeeModel> PullEmployees()
        {
            string sqlQuery = @"Select * from dbo.Employee;";

            employees = Factory.LoadEmployee();

            var dataTable = SqlDataAccess.PullDataOnDataTable(sqlQuery);

            // -- Convert datatable to list of employee --
            var employeeList = Helper.DataTableToList<EmployeeModel>(dataTable);

            employees.AddRange(employeeList);

            //// -- Call Insert employee datatable to insert list of employee --
            //SqlDataAccess.InsertListOfObject("dbo.Employee", employeeList); 

            return employees;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeId"></param>
        public static int DeleteEmploye(int employeId)
        {
            try
            {
                int result = 0;

                // -- Check for existing data in DB --
                //var sqlQuery = @"Select count(*) from dbo.Employee where EmployeeId = @EmployeeId;";
                var sqlQuery = @"Select count(*) from dbo.Employee where EmployeeId = " + employeId  + ";";

                //var param = new DynamicParameters();
                //param.Add("@EmployeeId", employeId); 

                var dataExist = SqlDataAccess.SelectOneData<IEmployeeModel>(sqlQuery, employeId);

                if (dataExist)
                {
                    // --  --
                    string sqlQuery2 = @"Delete from dbo.Employee where EmployeeId = @EmployeeId;";
                    result = SqlDataAccess.DeleteData<IEmployeeModel>(sqlQuery2, employeId);
                }

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

       
    }
}
