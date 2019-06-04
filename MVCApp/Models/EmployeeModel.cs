using MVCApp.Models.Intrefaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCApp.Models
{
    public class EmployeeModel : IEmployeeModel
    {
        [Range(100000, 999999, ErrorMessage = "Vous devez saisir un id valide!")]
        [Display(Name = "Id Employé")]
        public int EmployeeId { get; set; }

        [Display(Name = "Prenom Employé")]
        [Required(ErrorMessage = "Le nom n'est pas valide!")]
        public string FirstName { get; set; }

        [Display(Name = "Prenom Employé")]
        [Required(ErrorMessage = "Le prénom n'est pas valide!")]
        public string LastName { get; set; }

        [Display(Name = "Adresse mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "L'adresse mail n'est pas valide!")]
        public string EmailAddress { get; set; }


        [Display(Name = "Confirmez adresse mail")]
        [DataType(DataType.EmailAddress)]
        [Compare("EmailAddress", ErrorMessage = "Les 2 emails doivent être identique!")]
        public string ConfirmEmail { get; set; }

        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Le mot de passe n'est pas valide!")]
        [StringLength(100, MinimumLength = 4, ErrorMessage ="Le mot de passe doit avoir au moins 4 caractères!")]
        public string Password { get; set; }

        [Display(Name = "Confirmez mot de passe")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Les 2 mot de passe doivent être identique!")]
        public string ConfirmPassword { get; set; }
    }
}