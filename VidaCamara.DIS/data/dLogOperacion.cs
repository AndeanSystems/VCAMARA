using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.Modelo;

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
    }
}
