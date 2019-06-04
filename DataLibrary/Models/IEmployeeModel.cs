using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public interface IEmployeeModel
    {
        int Id { get; set; }
        int EmployeeId { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string EmailAddress { get; set; }
    }
}
