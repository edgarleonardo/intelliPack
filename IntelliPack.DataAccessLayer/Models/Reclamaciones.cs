using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliPack.DataAccessLayer.Models
{
    public class Reclamaciones
    {
        public int RECL_ID {get;set;}
        public int UsersId { get; set; }
        public int CourierId { get; set; }
        public int StatusId { get; set; }
        [Display(Name = "Enunciado")]
        public string Subject { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 2)]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        public string AnswerInfo { get; set; }
        [EmailAddress]
        [StringLength(100, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 3)]
        public string EmailCust { get; set; }
        [EmailAddress]
        [StringLength(100, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 3)]
        public string EmailCourier { get; set; }
        public string status_description { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaUpdate { get; set; }
        [Required]
        [StringLength(11, ErrorMessage = "El Campo {0} debe tener al menos {2} caracteres.", MinimumLength = 11)]
        public string ID { get; set; }

    }
}
