using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCApp.Models.Intrefaces
{
    public interface IEmployeeModel
    {
        int EmployeeId { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string EmailAddress { get; set; }

        string ConfirmEmail { get; set; }

        string Password { get; set; }
        string ConfirmPassword { get; set; }
    }
}
