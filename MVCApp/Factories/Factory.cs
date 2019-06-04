using MVCApp.Factories.Interfaces;
using MVCApp.Models;
using MVCApp.Models.Intrefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCApp.Factories
{
    public class Factory //: IFactory
    {
        public static IEmployeeModel CreateEmployee()
        {
            return new EmployeeModel();
        }

        public static List<IEmployeeModel> LoadEmployee()
        {
            return new List<IEmployeeModel>();
        }
    }
}