using System;
using System.Collections.Generic;
using System.Linq;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.data
{
    public class dAprobacionCarga
    {
        public List<EAprobacionCarga> listApruebaCarga(CONTRATO_SYS contrato, int jtStartIndex, int jtPageSize,object[] filters, out int total)
        {
            var listAprueba = new List<EAprobacionCarga>();
            try
            {
                using (var db = new DISEntities())
                {
                    var fecha_inicio = Convert.ToDateTime(filters[1]);
                    var query = db.pa_sel_pagoNominaAprueba(contrato.IDE_CONTRATO,filters[0].ToString(), fecha_inicio).ToList();
                    total = query.Count();
                    foreach (var item in query.Skip(jtStartIndex).Take(jtPageSize))
                    {
                        var eApruebaCarga = new EAprobacionCarga()
                        {
                            IdLinCab = item.IdLinCab,
                            NombreArchivo = item.NombreArchivo,
                            FechaCarga = item.FechaCarga,
                            moneda = item.Moneda,
                            TotalRegistros = Convert.ToInt64(item.TotalRegistros),
                            TotalImporte = item.ImporteTotal.ToString(),
                            PagoVc = item.PagoVC.ToString(),
                            FechaInfo = item.FechaInfo,
                            UsuReg = item.UsuReg,
                            Aprobar = item.Aprobar,
                            Eliminar = item.Eliminar
                        };
                        listAprueba.Add(eApruebaCarga);
                    }
                }
                return listAprueba;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public void eliminarPagoYNomina(HistorialCargaArchivo_LinCab historialCargaArchivo_LinCab)
        {
            try
            {
                using (var db = new DISEntities())
                {
                   
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void actualizarEstado(HistorialCargaArchivo_LinCab historialCargaArchivo_LinCab)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    db.pa_upd_ApruebaLinCabNomina(historialCargaArchivo_LinCab.IDE_CONTRATO, Convert.ToInt32(historialCargaArchivo_LinCab.IdHistorialCargaArchivoLinCab));
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public void actualizarArchivoIdNomina(Archivo archivo)
        {
            try
            {
                var historiaLinCab  = archivo.HistorialCargaArchivo_LinCab.FirstOrDefault();
                using (var db = new DISEntities())
                {
                    var query = db.HistorialCargaArchivo_LinCabs.Find(historiaLinCab.IdHistorialCargaArchivoLinCab);
                    query.ArchivoIdNomina = archivo.ArchivoId;
                    db.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
    }
}
