using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.data
{
    public class dPagoCargado
    {
        public List<HistorialCargaArchivo_LinDet> listArchivoCargado(HistorialCargaArchivo_LinCab historiaLinCab,object[] filterParam)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    var nombreTipoArchivo = filterParam[0].ToString();
                    var tipoArchivo = db.TipoArchivos.Where(t => t.NombreTipoArchivo.Equals(nombreTipoArchivo)).FirstOrDefault();
                    if (tipoArchivo.Archivoes.Count == 0)
                        return null;
                    else
                    {
                        var archivoId = tipoArchivo.Archivoes.Where(a => a.TipoArchivoId == tipoArchivo.TipoArchivoId).FirstOrDefault();
                        return db.HistorialCargaArchivo_LinDets.Where(a => a.HistorialCargaArchivo_LinCab.IDE_CONTRATO == historiaLinCab.IDE_CONTRATO && a.HistorialCargaArchivo_LinCab.ArchivoId == archivoId.ArchivoId).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
