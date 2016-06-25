using System;
using System.ComponentModel.DataAnnotations;

namespace IntelliPack.DataAccessLayer.Models
{
    public class AccountViewModel
    {
        public class RegisterViewModel
        { 
            [Display(Name = "User ID")]
            public int usersId { get; set; }

            [Display(Name = "Is Admin")]
            public int is_admin { get; set; }
            
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
            [Display(Name = "Nombre")]
            public string name { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
            [Display(Name = "Apellidos")]
            public string last_name { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [Display(Name = "E-mail")]
            public string email { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 11)]
            [Display(Name = "Cedula")]
            public string ID { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 11)]
            [Display(Name = "Usuario")]
            public string username { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 11)]
            public string addresss { get; set; }

            public string package_address { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string passwords { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string city_code { get; set; }


            public string validation_key { get; set; }
            public DateTime creation_date { get; set; }
            public DateTime update_date { get; set; }
            public DateTime date_of_birth { get; set; }
            


            
        }
    }
}