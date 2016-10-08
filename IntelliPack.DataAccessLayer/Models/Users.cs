using IntelliPack.DataAccessLayer.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliPack.DataAccessLayer.Models
{
    public class Users : ModelBase
    {
        [Display(Name = "User ID")]
        public int usersId { get; set; }

        [Display(Name = "Is Admin")]
        public int is_admin { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 2)]
        [Display(Name = "Apellidos")]
        public string last_name { get; set; }

        
        [EmailAddress]
        [StringLength(300, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 3)]
        [Display(Name = "E-mail")]
        public string email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 11)]
        [Display(Name = "Cedula")]
        public string ID { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 11)]
        [Display(Name = "Telefono")]
        public string Phone_No { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 3)]
        [Display(Name = "Usuario")]
        public string username { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 5)]
        public string addresss { get; set; }

        public int IsReseller { get; set; }
        public string Segundo_nombre { get; set; }
        public string Segundo_apellido { get; set; }
        public decimal TarifaUsuario { get; set; }
        public string package_address { get; set; }

        public double lat { get; set; }
        public double lng { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "passwords")]
        public string passwords { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("passwords", ErrorMessage = "La clave de confirmacion debe ser igual a la clave.")]
        public string ConfirmPassword { get; set; }

        public string city_code { get; set; }

        public string validation_key { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime update_date { get; set; }
        public DateTime date_of_birth { get; set; }
        public int Id_Rol { get; set; }
        public int CourierId { get; set; }
        public string CourierName { get; set; }
        public string Desc_rol { get; set; }
        
        public string SuccessMessage { get; set; }

        public Users()
        {
            date_of_birth = DateTime.Now;
        }
    }
}
