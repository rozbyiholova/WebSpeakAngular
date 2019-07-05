using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.ViewModels 
{
    public class PersonalInfoViewModel
    { 
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        public IFormFile Avatar { get; set; }

        public string ErrorMessage { get; set; }
    }
}
