﻿using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string Username { get; set; } = null!;
        
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
        
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        
        public string? ReturnUrl { get; set; }
    }
}
