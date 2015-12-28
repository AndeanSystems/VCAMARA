using System;
using System.Collections.Generic;
using System.Linq;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.data
{
    public class dArchivo
    {
        public List<Archivo> listExisteArchivo(Archivo archivo)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    return db.Archivos.Where(a => a.NombreArchivo.Equals(archivo.NombreArchivo)).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
