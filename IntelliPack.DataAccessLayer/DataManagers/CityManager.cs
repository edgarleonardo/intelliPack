using IntelliPack.DataAccessLayer.Base;
using IntelliPack.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliPack.DataAccessLayer.DataManagers
{
    public class CityManager : BaseManager<City>
    {
        public List<City> GetCities()
        {
            var result = Get("GET_CITY");
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
    }
}
