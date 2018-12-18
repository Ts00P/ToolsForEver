using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToolsForEver.Models
{
    public class EmployeeEditModel
    {
        [DataType(DataType.Text)]
        public string ID { get; set; }

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
    }
}