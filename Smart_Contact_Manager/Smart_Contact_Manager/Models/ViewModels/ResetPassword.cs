using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smart_Contact_Manager.Models.ViewModels
{
    public class ResetPassword
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("Password", ErrorMessage = "New Password and Confirm New Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}