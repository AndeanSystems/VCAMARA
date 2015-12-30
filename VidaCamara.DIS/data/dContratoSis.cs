using System.Collections.Generic;
using VidaCamara.DIS.Modelo;
using System.Linq;

namespace VidaCamara.DIS.data
{
    public class dContratoSis
    {
        public CONTRATO_SYS listContratoByID(CONTRATO_SYS contrato) {
            try
            {
                using (var db = new DISEntities())
                {
                    return db.CONTRATO_SYSs.OrderBy(a => a.IDE_CONTRATO).Where(a => a.IDE_CONTRATO == contrato.IDE_CONTRATO).FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                
                throw;
            }
        }
    }
}
