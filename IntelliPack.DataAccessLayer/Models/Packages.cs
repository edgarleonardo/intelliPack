using IntelliPack.DataAccessLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntelliPack.DataAccessLayer.Models
{
    public class Packages : ModelBase
    {
        public int usersId { get; set; }
        public string tracking_code { get; set; }
        public string correo { get; set; }
        public decimal peso { get; set; }
        public decimal manejo { get; set; }
        public decimal costoXLibra { get; set; }
        public decimal total { get; set; }
        public int courierId { get; set; }
        public int workflowid { get; set; }
        public string WH { get; set; }
        public int status_code { get; set; }
        public string status_description { get; set; }
        public string consignado { get; set; }
        public string contenido { get; set; }
        public string tienda { get; set; }
        public string origen { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime update_date { get; set; }

        public string ID { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string CourierName { get; set; }
        public string addresss { get; set; }
        public string phone_no { get; set; }
        public int secuecia { get; set; }
        public int packageStatus { get; set; }
        public string packageStatusDesc { get; set; }
        public int secuencia_id { get; set; }
        public string Create_Date_Str { get; set; }
    }
}