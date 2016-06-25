using IntelliPack.DataAccessLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntelliPack.DataAccessLayer.Models
{
    public class City : ModelBase
    {
        public string city_code  {get;set;}
        public string city_description { get; set; }
    }
}