using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.data
{
    
    public class dLogOperacion
    {
        public long setGuardarLogOperacion(LogOperacion log)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    db.LogOperacions.Add(log);
                    db.SaveChanges();
                    return log.CodiLogOper;
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }

        public List<HLogOperacion> getListLogOperacion(HLogOperacion log, int jtStartIndex, int jtPageSize, out int total)
        {
            var listLogOperacion = new List<HLogOperacion>();
            try 
	        {	        
		        using (var db = new DISEntities())
                {
                    var query = db.pa_sel_LogOperacion(log.IDE_CONTRATO, log.TipoOper, log.FechEven, log.FechEven, log.Evento).ToList();
                    total = query.Count();

                    foreach (var item in query.Skip(jtStartIndex).Take(jtPageSize))
                    {

                        var log1 = new HLogOperacion()
                        {
                            IDE_CONTRATO = item.IDE_CONTRATO,
                            TipoOper     = item.TipoOper,
                            FechEven     = item.FechEven,
                            Evento       = item.Evento
                        };

                        listLogOperacion.Add(log1);
                    }

                }
                return listLogOperacion;
	        }
	        catch (Exception ex)
	        {
		
		        throw;
	        }
        }
    }
}
