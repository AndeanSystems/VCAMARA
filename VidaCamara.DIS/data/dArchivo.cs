using System;
using System.Collections.Generic;
using System.Linq;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.data
{
    public class dArchivo
    {
        public List<Archivo> listExisteArchivo(Archivo archivo,int tamanoNombre)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    return db.Archivos.Where(a => a.NombreArchivo.Equals(archivo.NombreArchivo) && a.Vigente == true).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
