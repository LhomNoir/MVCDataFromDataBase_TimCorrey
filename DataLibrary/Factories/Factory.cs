using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Factories
{
    public static class Factory 
    {
        public static IEmployeeModel CreateEmployee()
        {
            return new EmployeeModel();
        }

        public static List<IEmployeeModel> LoadEmployee()
        {
            return new List<IEmployeeModel>();
        }

        public static void Delete()
        {

        }
    }
}
