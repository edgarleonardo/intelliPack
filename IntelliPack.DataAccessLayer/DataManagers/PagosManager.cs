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
    public class PagosManager : BaseManager<Pagos>
    {
        public PagosManager()
            : base()
        {

        }

        public void HacerPago(Pagos pago, long usersId)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@no_id", pago.no_id),
                    new SqlParameter("@monto_pagar", pago.monto_pagado),
                    new SqlParameter("@usersId", usersId)};
            Execute("SET_PAGOS @no_id, @monto_pagar, @usersId", parameters);           
        }

        //public List<Packages> GetHistoryUserId(int user_id)
        //{
        //    var parameters = new SqlParameter[]{
        //            new SqlParameter("@user_id", user_id)};
        //    var result = Get("GET_HIST_ACT_PACKAGE_BY_USER_ID @user_id", parameters);
        //    if (result == null || !string.IsNullOrEmpty(Error_Message))
        //    {
        //        throw new Exception(Error_Message);
        //    }
        //    else
        //    {
        //        return result;
        //    }
        //}
        //public Packages SetWorkFlow()
        //{
        //    var result = Get(@"INITIALIZA_PACKAGE_WORKFLOW");

        //    if (result != null && result.Count > 0)
        //    {
        //        Error_Message = result[0].errorMessage;
        //        return result[0];
        //    }
        //    if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
        //    {
        //        throw new Exception(Error_Message);
        //    }
        //    return null;
        //}

    }
}
