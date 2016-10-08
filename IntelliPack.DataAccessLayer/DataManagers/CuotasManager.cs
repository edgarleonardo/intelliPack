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
    public class CuotasManager : BaseManager<Cuotas>
    {
        public void CorrerPagos(int user_id)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@user_id", user_id)};
            var result = Get("SET_CUOTAS @user_id", parameters);
        }

        public List<Cuotas> GetCuotas(int user_id)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@user_id", user_id)};
            var result = Get("GET_CUOTAS_DETAIL @user_id", parameters);
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
