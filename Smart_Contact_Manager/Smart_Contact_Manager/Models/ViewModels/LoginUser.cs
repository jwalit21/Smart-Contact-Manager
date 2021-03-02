﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smart_Contact_Manager.Models.ViewModels
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email (Username)")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}