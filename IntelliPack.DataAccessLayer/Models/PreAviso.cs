using System;

namespace IntelliPack.DataAccessLayer.Models
{
    public class PreAviso
    {
        public int usersId { get; set; }
        public string tracking_code { get; set; }
        public decimal Amount { get; set; }
        public decimal Weights { get; set; }
        public string invoice { get; set; }
        public int estatusId { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime update_date { get; set; }
        public string status_description { get; set; }
    }
}
