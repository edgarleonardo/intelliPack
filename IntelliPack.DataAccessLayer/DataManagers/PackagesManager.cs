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
    public class PackagesManager : BaseManager<Packages>
    {
        public PackagesManager()
            : base()
        {

        }
        
             public List<Packages> GetEntregasSecuenciaByCourier(int userIdLogged, int courier)
        {
            var parameters = new SqlParameter[]{
            new SqlParameter("@UserLogged", userIdLogged),
            new SqlParameter("@userId", courier)};
            var result = Get("GET_Delivery_By_User_ID @UserLogged, @userId", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> GetEntregasByCourier(int userIdLogged, int courier)
        {
            var parameters = new SqlParameter[]{
            new SqlParameter("@UserLogged", userIdLogged),
            new SqlParameter("@userId", courier)};
            var result = Get("GET_PACKAGES_BY_Delivered_User_ID @UserLogged, @userId", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> GetEntregasByCourierEntregaId(int userIdLogged, int courier, int fechaEntrega)
        {
            var parameters = new SqlParameter[]{
            new SqlParameter("@UserLogged", userIdLogged),
            new SqlParameter("@userId", courier),
            new SqlParameter("@fechaEntrega", fechaEntrega)
            };
            var result = Get("GET_PACKAGES_BY_DeliveredId_User_ID @UserLogged, @userId, @fechaEntrega", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public Packages GetById(string wh, int userIdLogged)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@WH", wh),
            new SqlParameter("@UserLogged", userIdLogged)};
            var result = Get("GET_PACKAGES_BY_ID @WH, @UserLogged", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                Packages cargo = new Packages();
                if (result.Count > 0)
                {
                    cargo = result[0];
                }
                return cargo;
            }
        }
        public List<Packages> GetListById(string wh,string tracking, int userIdLogged)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@WH", wh),
                    new SqlParameter("@tracking", tracking),
            new SqlParameter("@UserLogged", userIdLogged)};
            var result = Get("GET_PACKAGES_BY_ID_invoice @WH, @tracking, @UserLogged", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> GetListById(string wh, int userIdLogged)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@WH", wh),
            new SqlParameter("@UserLogged", userIdLogged)};
            var result = Get("GET_PACKAGES_BY_ID @WH, @UserLogged", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public Packages GetById(string wh, string trackingCode,int userIdLogged)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@WH", wh),
                    new SqlParameter("@trackingCode", trackingCode),
            new SqlParameter("@UserLogged", userIdLogged)};
            var result = Get("GET_PACKAGES_BY_ID_trackin @WH,@trackingCode, @UserLogged", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                Packages cargo = new Packages();
                if (result.Count > 0)
                {
                    cargo = result[0];
                }
                return cargo;
            }
        }
        public List<Packages> GetByUsersIdPk(int userId, int userIdLogged)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@userId", userId),
            new SqlParameter("@UserLogged", userIdLogged)};
            var result = Get("GET_PACKAGES_BY_UserID @userId, @UserLogged", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> GetByUserId( int userIdLogged, int courier)
        {
            var parameters = new SqlParameter[]{
            new SqlParameter("@UserLogged", userIdLogged),
            new SqlParameter("@userId", courier)};
            var result = Get("GET_PACKAGES_BY_User_ID @UserLogged, @userId", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }

        public List<Packages> ApplyUserDelivery( int userIdLogged, int courier)
        {
            var parameters = new SqlParameter[]{
            new SqlParameter("@UserLogged", userIdLogged),
            new SqlParameter("@userId", courier)};
            var result = Get("ENTREGA_PAQUETES_CLIENTES @UserLogged, @userId", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> ApplyUserDelivery(int userIdLogged, int courier, string TipoFact, string Comprobante)
        {
            var parameters = new SqlParameter[]{
            new SqlParameter("@UserLogged", userIdLogged),
            new SqlParameter("@userId", courier),
            new SqlParameter("@TipoFact", TipoFact),
            new SqlParameter("@Comprobante", Comprobante)
            };
            var result = Get("ENTREGA_PAQUETES_CLIENTES @UserLogged, @userId, @TipoFact, @Comprobante", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> GetUnProccesed(int userIdLogged)
        {
            var parameters = new SqlParameter[]{
            new SqlParameter("@UserLogged", userIdLogged)};
            var result = Get("GET_ACTIVED_UNPROCCED_PACKAGES @UserLogged", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> GetActived()
        {
            var result = Get("GET_ACTIVED_PACKAGES");
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> GetReteinedUnproccesesFinished(int userIdLogged)
        {
            var parameters = new SqlParameter[]{
            new SqlParameter("@UserLogged", userIdLogged)};
            var result = Get("GET_Retained_UNPROCCED_PACKAGES @UserLogged", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> GetReteinedUnprocceses(int userIdLogged)
        {
            var parameters = new SqlParameter[]{
            new SqlParameter("@UserLogged", userIdLogged)};
            var result = Get("GET_ACTIVED_UNPROCCED_PACKAGES_UNPROC @UserLogged", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> GetRetornedPackages()
        {
            var result = Get("GET_Retorned_PACKAGES");
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Packages> GetinActived()
        {
            var result = Get("GET_INACTIVED_PACKAGES");
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }

        public List<Packages> GetUserId(int user_id)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@user_id", user_id)};
            var result = Get("GET_ACT_PACKAGE_BY_USER_ID @user_id", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }

        public List<Packages> GetHistoryUserId(int user_id)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@user_id", user_id)};
            var result = Get("GET_HIST_ACT_PACKAGE_BY_USER_ID @user_id", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public Packages SetWorkFlow()
        {
            var result = Get(@"INITIALIZA_PACKAGE_WORKFLOW");

            if (result != null && result.Count > 0)
            {
                Error_Message = result[0].ErrorMessage;
                return result[0];
            }
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            return null;
        }
        public void ReturnPackage(Packages model)
        {
            var parameters = new SqlParameter[]{
                 new SqlParameter("@usersId", model.usersId),
                  new SqlParameter("@courierid", model.courierId),
                    new SqlParameter("@tracking_code", model.tracking_code),
                    new SqlParameter("@correo", model.correo),
                    new SqlParameter("@peso", model.peso),
                    new SqlParameter("@WH", model.WH),
                    new SqlParameter("@status_code", model.status_code),
                    new SqlParameter("@Consignado", model.consignado),
                    new SqlParameter("@Contenido", model.contenido),
                    new SqlParameter("@Tienda", model.tienda),
                    new SqlParameter("@Origen", model.origen),
                    new SqlParameter("@workflowId",model.workflowid),
                    new SqlParameter("@manejo",model.manejo),
                    new SqlParameter("@costoXLibra",model.costoXLibra),
                    new SqlParameter("@packageStatus",model.packageStatus)

            };
            var result = Get(@"RETURN_PACKAGES @usersId,@courierid, @tracking_code,@correo,@peso,@WH,@status_code,@Consignado,@Contenido,@Tienda,@Origen,@workflowId,@manejo, @costoXLibra,@packageStatus", parameters);

            if (result != null && result.Count > 0 && result[0].ErrorMessage != "")
            {
                Error_Message = result[0].ErrorMessage;
            }
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
        public void Set(Packages model)
        {
               var parameters = new SqlParameter[]{
                   new SqlParameter("@usersId", model.usersId),
                   new SqlParameter("@courierid", model.courierId),
                    new SqlParameter("@tracking_code", model.tracking_code),
                    new SqlParameter("@correo", model.correo),
                    new SqlParameter("@peso", model.peso),
                    new SqlParameter("@WH", model.WH),
                    new SqlParameter("@status_code", model.status_code),
                    new SqlParameter("@Consignado", model.consignado),
                    new SqlParameter("@Contenido", model.contenido),
                    new SqlParameter("@Tienda", model.tienda),
                    new SqlParameter("@Origen", model.origen),
                    new SqlParameter("@workflowId",model.workflowid),
                    new SqlParameter("@manejo",model.manejo),
                    new SqlParameter("@costoXLibra",model.costoXLibra),
                    new SqlParameter("@packageStatus",model.packageStatus),
                    new SqlParameter("@packageStatusDescription",model.packageStatusFromSourceDesc),
                    new SqlParameter("@valorMercancia",model.ValorMercancia),
                    new SqlParameter("@SeguroMonto",model.SeguroMonto),
                    new SqlParameter("@Moneda",model.Moneda),
                    
            };
            var result = Get(@"INSERT_PACKAGES @usersId,@courierid,@tracking_code,@correo,@peso,@WH,@status_code,@Consignado,@Contenido,@Tienda,@Origen,@workflowId,@manejo, @costoXLibra,@packageStatus,@packageStatusDescription,@valorMercancia,@SeguroMonto,@Moneda", parameters);

            if (result != null && result.Count > 0 && result[0].ErrorMessage != "")
            {
                Error_Message = result[0].ErrorMessage;
            }
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
        public void UpdateFinalCostumer(Packages model)
        {
            var parameters = new SqlParameter[]{
                   new SqlParameter("@usersId", model.usersId),
                   new SqlParameter("@courierid", model.courierId),
                    new SqlParameter("@tracking_code", model.tracking_code),
                    new SqlParameter("@WH", model.WH),
                    new SqlParameter("@total", model.total),
                new SqlParameter("@Comments", model.Comments)
            };
            var result = Get(@"UPDATE_PACKAGES_COSTUMER @usersId,@courierid,@tracking_code,@WH,@total, @Comments", parameters);

            if (result != null && result.Count > 0 && result[0].ErrorMessage != "")
            {
                Error_Message = result[0].ErrorMessage;
            }
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
        public void UpdateInvoice(Packages model)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@tracking_code", model.tracking_code),
                    new SqlParameter("@WH", model.WH),
                    new SqlParameter("@factura",model.Factura)
            };
            var result = Get(@"UPDATE_PACKAGES_Invoice @tracking_code,@WH,@factura", parameters);

            if (result != null && result.Count > 0 && result[0].ErrorMessage != "")
            {
                Error_Message = result[0].ErrorMessage;
            }
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
        public void UpdateRetained(Packages model)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@tracking_code", model.tracking_code),
                    new SqlParameter("@peso", model.peso),
                    new SqlParameter("@WH", model.WH),
                    new SqlParameter("@manejo",model.manejo),
                    new SqlParameter("@costoXLibra",model.costoXLibra),
                    new SqlParameter("@valorMercancia",model.ValorMercancia),
                    new SqlParameter("@SeguroMonto",model.SeguroMonto),
                    new SqlParameter("@CostoTotal",model.CostoTotal),
                    new SqlParameter("@itbis_pagado",model.itbis_pagado)     ,
                    new SqlParameter("@precioXLibraCliente",model.precioXLibraCliente)
            };
            var result = Get(@"UPDATE_PACKAGES_RETAINED @tracking_code,@peso,@WH,@manejo,@costoXLibra,@valorMercancia,@SeguroMonto, @CostoTotal,@itbis_pagado,@precioXLibraCliente", parameters);

            if (result != null && result.Count > 0 && result[0].ErrorMessage != "")
            {
                Error_Message = result[0].ErrorMessage;
            }
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
        public void Update(Packages model)
        {
               var parameters = new SqlParameter[]{
                    new SqlParameter("@tracking_code", model.tracking_code),
                    new SqlParameter("@peso", model.peso),
                    new SqlParameter("@WH", model.WH),
                    new SqlParameter("@manejo",model.manejo),
                    new SqlParameter("@costoXLibra",model.costoXLibra),
                    new SqlParameter("@valorMercancia",model.ValorMercancia),
                    new SqlParameter("@SeguroMonto",model.SeguroMonto),
                    new SqlParameter("@CostoTotal",model.CostoTotal),
                    new SqlParameter("@itbis_pagado",model.itbis_pagado)     ,
                    new SqlParameter("@precioXLibraCliente",model.precioXLibraCliente)               
            };
            var result = Get(@"UPDATE_PACKAGES @tracking_code,@peso,@WH,@manejo,@costoXLibra,@valorMercancia,@SeguroMonto, @CostoTotal,@itbis_pagado,@precioXLibraCliente", parameters);

            if (result != null && result.Count > 0 && result[0].ErrorMessage != "")
            {
                Error_Message = result[0].ErrorMessage;
            }
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
    }
}
