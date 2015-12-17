using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.data
{
    public class contrato_sis_detalle
    {
        public List<CONTRATO_DETALLE> getlistContratoDetalle(){
            try
            {
                using (var db = new DISEntities())
                {
                    return db.CONTRATO_DETALLEs.ToList();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
