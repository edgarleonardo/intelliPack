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
    public class ReclamacionesManager : BaseManager<Reclamaciones>
    {
        public List<Reclamaciones> GetReclamaciones(int courierId, int reclamacionesStatus)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@UserLogged", courierId),
                    new SqlParameter("@reclamacionesStatus", reclamacionesStatus)
            };
            var result = Get("get_reclamaciones @UserLogged, @reclamacionesStatus", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public Reclamaciones GetReclamacionById(int user_id, int recl_id)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@UserLogged", user_id),
                    new SqlParameter("@recla_id", recl_id)};
            var result = Get("get_reclamacionesById @UserLogged, @recla_id", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                Reclamaciones cargo = new Reclamaciones();
                if (result.Count > 0)
                {
                    cargo = result[0];
                }
                return cargo;
            }
        }
        public void Insert(Reclamaciones model)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@usersId", model.UsersId),
                    new SqlParameter("@UserLogged", model.CourierId),
                    new SqlParameter("@subject", model.Subject),
                    new SqlParameter("@description", model.Description),
                    new SqlParameter("@emailCust", model.EmailCust),
                    new SqlParameter("@emailCourier", model.EmailCourier)
            };
             Execute(@"INSERT_reclamaciones @usersId, @UserLogged,@subject ,@description,@emailCust,@emailCourier", parameters);
                        
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
        public void Update(Reclamaciones model)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@UserLogged", model.UsersId),
                    new SqlParameter("@recl_id", model.RECL_ID),
                    new SqlParameter("@statusId", model.StatusId),
                    new SqlParameter("@answerInfo", model.AnswerInfo)
            };
            Execute(@"UPDATE_reclamaciones @UserLogged, @recl_id,@statusId ,@answerInfo", parameters);
            
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }

        public void Delete(Reclamaciones model)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@UserLogged", model.UsersId),
                    new SqlParameter("@recl_id", model.RECL_ID)
            };
            Execute(@"DELETE_reclamaciones @UserLogged, @recl_id", parameters);

            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
    }
}
