using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CalHealth.Blazor.Client.Models
{
    public class PatientFormViewModel
    {
        public PatientFormViewModel()
        {
            DateOfBirth = DateTime.Today;
        }
        
        [Required(ErrorMessage = "Required.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Required.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        public string LastName { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}