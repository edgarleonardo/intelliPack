using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliPack.DataAccessLayer.Models
{
    public class Cuotas
    {
        public long no_id { get; set; }

        public int usersId { get; set; }

        public string usersName { get; set; }

        public decimal libras { get; set; }

        public int numero_paquetes { get; set; }

        public DateTime fecha_cuota { get; set; }

        public DateTime fecha_vencimiento { get; set; }

        public int status_id { get; set; }

        public string status_desc { get; set; }

        public decimal monto_unitario { get; set; }

        public decimal monto_precio { get; set; }

        public decimal monto_pagado { get; set; }

        public decimal monto_adeudado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
