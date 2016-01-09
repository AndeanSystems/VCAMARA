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

        public Int32 listExistePagoNomina(Archivo archivo)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    return (from a in db.Archivos join b in db.HistorialCargaArchivo_LinCabs on
                                 a.ArchivoId equals b.ArchivoId
                                 where
                                    a.NombreArchivo == archivo.NombreArchivo
                                 && b.ESTADO == "C"
                                 select a).Count();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Archivo getArchivoByNombre(Archivo archivo)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    return db.Archivos.Include("HistorialCargaArchivo_LinCab").Where(a => a.NombreArchivo == archivo.NombreArchivo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}
