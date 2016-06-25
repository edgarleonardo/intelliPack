using IntelliPack.DataAccessLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntelliPack.DataAccessLayer.Models
{
    public class Status_Table : ModelBase
    {
        public int status_id { get; set; }
        public string status_description { get; set; }
    }
}