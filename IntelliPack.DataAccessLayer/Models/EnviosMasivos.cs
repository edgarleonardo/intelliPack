using IntelliPack.DataAccessLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliPack.DataAccessLayer.Models
{
    public class EnviosMasivos : ModelBase
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string HtmlInfo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
