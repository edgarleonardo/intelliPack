using IntelliPack.DataAccessLayer.Base;
using IntelliPack.DataAccessLayer.Context;
using IntelliPack.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliPack.DataAccessLayer.DataManagers
{
    public class PackagesFromSourceManagers : BaseManager<PackagesFromSource>
    {
        public PackagesFromSourceManagers() : base(new SourcePaqContextDb())
        {

        }
        public List<PackagesFromSource> GetPackagesFromSource(string Id)
        {
            var result = Get(@"select IFNULL(fpkuno, '') as fpkuno, 
                                        IFNULL(tpeso, '') as tpeso,
                                        IFNULL(fpkdes, '') as fpkdes,
                                        IFNULL(fpkcon, '') as fpkcon,
                                        IFNULL(fpkcmm, '') as fpkcmm,
                                        IFNULL(fpksup, '') as fpksup,
                                        IFNULL(fpklb, 0) as fpklb,
                                        IFNULL(fpkbul, 0) as fpkbul,
                                        IFNULL(fpkval, 0) as fpkval,
                                        IFNULL(fpkarr, CURDATE()) as fpkarr,
                                        IFNULL(fpkh, 0) as fpkh,
                                        IFNULL(fpkl, 0) as fpkl,
                                        IFNULL(fpkz, 0) as fpkz,
                                        IFNULL(fpktck, '') as fpktck,
                                        IFNULL(pksdesc, '') as pksdesc,
                                        IFNULL(cstas, 0) as cstas,
                                        IFNULL(tcgDesc, '') as tcgDesc,
                                        IFNULL(nac_desc, '') as nac_desc,
                                        IFNULL(mon_desc, 'DOLLAR') as mon_desc,
                                        IFNULL(clicuenta, '') as clicuenta,
                                        IFNULL(cnombrec, '') as cnombrec,
                                        IFNULL(sucdesc, '') as sucdesc,
                                        IFNULL(pktot, 0) as pktot,
                                        IFNULL(cedula, '') as cedula from vwfpk where sucdesc = 'INTELLIPAQ' and cedula ='"+ Id + "'");
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
