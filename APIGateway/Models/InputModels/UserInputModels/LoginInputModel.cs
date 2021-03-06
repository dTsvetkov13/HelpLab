﻿using System.ComponentModel.DataAnnotations;

namespace APIGateway.Models.InputModels
{
    public class LoginInputModel
    {
        [MaxLength(100)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
