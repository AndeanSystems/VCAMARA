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

        public List<LogOperacion> getListLogOperacion(LogOperacion log, int jtStartIndex, int jtPageSize, out int total)
        {
            var listLogOperacion = new List<LogOperacion>();
            try 
	        {	        
		        using (var db = new DISEntities())
                {
                    //db.pa_sel_LogOperacion();
                    var query = (from a in db.LogOperacions
                                 where a.TipoOper.Equals(log.TipoOper) || log.TipoOper == "0"
                                 select a).ToList();
                    total = query.Count;
                    //var query = db.LogOperacions.OrderBy(a => a.CodiLogOper)
                    //    .Where(a=>a.TipoOper.Equals(log.TipoOper) || log.TipoOper == "0")
                    //    .ToList();
                    //total = query.Count();

                    foreach (var item in query.Skip(jtStartIndex).Take(jtPageSize))
                    {

                        //var log1 = new HLogOperacion()
                        //{

                        //};
                        var logOperacion = new LogOperacion()
                        {
                            CodiLogOper = item.CodiLogOper,
                            FechEven   = item.FechEven,
                            TipoOper   = item.TipoOper,
                            CodiOper   = item.CodiOper,
                            CodiEven   = item.CodiEven,
                            CodiUsu    = item.CodiUsu,
                            CodiCnx    = item.CodiCnx
                        };
                        listLogOperacion.Add(logOperacion);
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
