using MVCApp.Models.Intrefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCApp.Factories.Interfaces
{
    public interface IFactory
    {
        IEmployeeModel CreateEmployee();
    }
}
