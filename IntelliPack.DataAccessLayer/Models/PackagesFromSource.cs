using IntelliPack.DataAccessLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliPack.DataAccessLayer.Models
{
    public class PackagesFromSource : ModelBase
    {
        public string fpkuno { get; set; }
        public string tpeso { get; set; }
        public string fpkdes { get; set; }
        public string fpkcon { get; set; }
        public string fpkcmm { get; set; }
        public string fpksup { get; set; }
        public decimal fpklb { get; set; }
        public int fpkbul { get; set; }
        public decimal fpkval { get; set; }
        public DateTime fpkarr { get; set; }
        public int fpkh { get; set; }
        public int fpkl { get; set; }
        public int fpkz { get; set; }
        public string fpktck { get; set; }
        public string pksdesc { get; set; }
        public decimal cstas { get; set; }
        public string tcgDesc { get; set; }
        public string nac_desc { get; set; }
        public string mon_desc { get; set; }
        public string clicuenta { get; set; }
        public string cnombrec { get; set; }
        public string sucdesc { get; set; }
        public decimal pktot { get; set; }
        public string cedula { get; set; }
    }
}
