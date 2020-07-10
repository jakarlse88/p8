﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CalHealth.Blazor.Client.Models
{
    public class PatientFormViewModel
    {
        public PatientFormViewModel()
        {
            ReligionId = 0;
            AllergyIdList = new List<int>();
            DateOfBirth = DateTime.Today;
        }
        
        [Required(ErrorMessage = "Required.")]
        public int GenderId { get; set; }
        
        public int ReligionId { get; set; }
        public IEnumerable<int> AllergyIdList { get; set; }
        
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