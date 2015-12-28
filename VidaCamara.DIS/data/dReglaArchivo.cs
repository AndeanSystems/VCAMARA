using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.data
{
    public class dReglaArchivo
    {
        public List<ReglaArchivo> getListReglaArchivo(ReglaArchivo regla)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    return db.ReglaArchivos.Where(a=>a.Archivo.Equals(regla.Archivo) && (a.TipoLinea.Equals(regla.TipoLinea) || regla.TipoLinea.Equals("0"))).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
