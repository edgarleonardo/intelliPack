using IntelliPack.DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliPack.DataAccessLayer.Base
{
    public class BaseManager<T> where T : class
    {
        protected DbContext _db;
        public string Error_Message = "";
        public BaseManager()
        {
            _db = new IntelliPackContextDb();
        }

        /// <summary>
        /// Return All Clients
        /// </summary>
        /// <returns></returns>
        protected List<T> Get(string script)
        {
            try
            {
                var Result = _db.Database.SqlQuery<T>(script);

                return Result.ToList<T>();
            }
            catch (Exception ex)
            {
                Error_Message = ex.Message;
            }
            return null;
        }
        protected SqlParameter Param_With_Type(string param_name, SqlDbType type, object value)
        {
            SqlParameter param = new SqlParameter(param_name, type);
            param.Value = value;
            return param;
        }
        /// <summary>
        /// Save Clients
        /// </summary>
        /// <returns></returns>
        protected List<T> Get(string script, SqlParameter[] parameters)
        {
            try
            {
                var Result = _db.Database.SqlQuery<T>(script,
                                                    parameters);

                return Result.ToList<T>();
            }
            catch (Exception ex)
            {
                Error_Message = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// Save Clients
        /// </summary>
        /// <returns></returns>
        protected void Execute(string script, SqlParameter[] parameters)
        {
            try
            {
                var Result = _db.Database.ExecuteSqlCommand(script,
                                                    parameters);
            }
            catch (Exception ex)
            {
                Error_Message = ex.Message;
            }
        }


    }
}
