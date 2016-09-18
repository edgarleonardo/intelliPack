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
    public class EnviosMasivosManager : BaseManager<EnviosMasivos>
    {
        public void GuardarEnvio(EnviosMasivos envio)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@ID", envio.Id),
                    new SqlParameter("@subject", envio.Subject),
                    new SqlParameter("@htmlInfo", envio.HtmlInfo)};
            Execute("set_EnviosMasivos @ID, @subject, @htmlInfo", parameters);
        }
    }
}
