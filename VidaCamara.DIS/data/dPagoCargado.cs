using System;
using System.Collections.Generic;
using System.Linq;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.data
{
    public class dPagoCargado
    {
        public List<HistorialCargaArchivo_LinDet> listArchivoCargado(HistorialCargaArchivo_LinCab historiaLinCab, HistorialCargaArchivo_LinDet historiaLinDetParam, object[] filterParam, int jtStartIndex, int jtPageSize, out int total)
        {
            var listDetalle = new List<HistorialCargaArchivo_LinDet>();
            try
            {
                using (var db = new DISEntities())
                {
                    var nombreTipoArchivo = filterParam[0].ToString();
                    var tipoArchivo = db.TipoArchivos.Where(t => t.NombreTipoArchivo.Equals(nombreTipoArchivo)).FirstOrDefault();
                    if (tipoArchivo.Archivoes.Count == 0)
                    {
                        total = 0;
                        return null;
                    }
                    else
                    {
                        
                        var archivoId = tipoArchivo.Archivoes.Where(a => a.TipoArchivoId == tipoArchivo.TipoArchivoId).FirstOrDefault();
                        total = db.HistorialCargaArchivo_LinDets.Where(a => a.HistorialCargaArchivo_LinCab.IDE_CONTRATO == historiaLinCab.IDE_CONTRATO && a.HistorialCargaArchivo_LinCab.ArchivoId == archivoId.ArchivoId).Count();
                        var query = db.HistorialCargaArchivo_LinDets.OrderBy(a=>a.IdHistorialCargaArchivoLinDet).Where(a => a.HistorialCargaArchivo_LinCab.IDE_CONTRATO == historiaLinCab.IDE_CONTRATO && a.HistorialCargaArchivo_LinCab.ArchivoId == archivoId.ArchivoId).Skip(jtStartIndex).Take(jtPageSize).ToList();

                        foreach (var item in query)
                        {
                            //llenar toda la entidad  HistorialCargaArchivo_LinDet 
                            var historiaLinDet = new HistorialCargaArchivo_LinDet()
                            {
                                IdHistorialCargaArchivoLinDet = item.IdHistorialCargaArchivoLinDet,
                                HistorialCargaArchivo_LinCab = new HistorialCargaArchivo_LinCab()
                                {
                                    CONTRATO_SYS = new CONTRATO_SYS()
                                    {
                                        NRO_CONTRATO = item.HistorialCargaArchivo_LinCab.CONTRATO_SYS.NRO_CONTRATO
                                    }
                                }
                            };
                            listDetalle.Add(historiaLinDet);
                        }
                    }
                }
                return listDetalle;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
