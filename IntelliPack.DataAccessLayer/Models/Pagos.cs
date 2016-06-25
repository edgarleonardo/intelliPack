using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliPack.DataAccessLayer.Models
{
    public class Pagos
    {
        public long comprobante_pago { get; set; }
        public long no_id { get; set; }
        public DateTime fecha_pago { get; set; }
        [Required]
        public decimal monto_pagado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

    }
}
