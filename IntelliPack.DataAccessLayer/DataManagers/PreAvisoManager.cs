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
    public class PreAvisoManager : BaseManager<PreAviso>
    {
        public List<PreAviso> GetPreAdvisor(int courierId, int statusId)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@UserLogged", courierId),
                    new SqlParameter("@statusId", statusId)                    
            };
            var result = Get("get_preAdvisor @UserLogged, @statusId", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public PreAviso GetPreAdvisorById(int user_id, string tracking_code)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@UserLogged", user_id),
                    new SqlParameter("@trackingCode", tracking_code)};
            var result = Get("get_preAdvisorById @UserLogged, @trackingCode", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                PreAviso cargo = new PreAviso();
                if (result.Count > 0)
                {
                    cargo = result[0];
                }
                return cargo;
            }
        }
        public void Insert(PreAviso model)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@usersId", model.usersId),
                    new SqlParameter("@tracking_code", model.tracking_code),
                    new SqlParameter("@Amount", model.Amount),
                    new SqlParameter("@Weights", model.Weights),
                    new SqlParameter("@invoice", model.invoice)
            };
            Execute(@"INSERT_preAdvisor @usersId, @tracking_code,@Amount,@Weights,@invoice", parameters);

            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
        public void Update(PreAviso model)
        {
            var parameters = new SqlParameter[]{
                   new SqlParameter("@usersId", model.usersId),
                   new SqlParameter("@statusId", model.estatusId),
                    new SqlParameter("@tracking_code", model.tracking_code),
                    new SqlParameter("@Amount", model.Amount),
                    new SqlParameter("@Weights", model.Weights),
                    new SqlParameter("@invoice", model.invoice)
            };
            Execute(@"UPDATE_preAdvisor @usersId,@statusId,@tracking_code,@Amount,@Weights,@invoice", parameters);

            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }

        public void Delete(PreAviso model)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@usersId", model.usersId),
                    new SqlParameter("@statusId", model.estatusId),
                    new SqlParameter("@tracking_code", model.tracking_code)
            };
            Execute(@"DELETE_preAdvisor @usersId, @statusId, @tracking_code", parameters);

            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
    }
}
