using VidaCamara.DIS.Modelo;
using System.Linq;

namespace VidaCamara.DIS.data
{
    public class dContratoSis
    {
        //INVOCAR STORE PROCEDURE
        //var date1 = DateTime.Now;
        //var date2 = DateTime.Now;
        //db.pa_Valida_RangoFechaXContrato(date1, date2);
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
