using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToolsForEver.Models
{
    public class EmployeeViewModel
    {
        [DataType(DataType.Text)]
        public string ID { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Emailadres")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord bevestigen")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Voornaam")]
        public string Firstname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Achternaam")]
        public string Lastname { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Voorvoegsels")]
        public string Middlename { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Provincie")]
        public string District { get; set; }

        [Display(Name = "Is buiten gebruik")]
        public bool IsLockedOut { get; set; }
    }
}