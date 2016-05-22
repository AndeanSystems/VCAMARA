using System;
using System.Collections.Generic;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;
using System.Linq;
using System.Configuration;

namespace VidaCamara.DIS.data
{
    public class dSegDescarga
    {
        public List<ESegDescarga> listSegDescarga(CONTRATO_SYS contrato,object[] filters, int jtStartIndex, int jtPageSize, out int total)
        {
            var listDescarag = new List<ESegDescarga>();
            try
            {
                var fechaInicioCreacion = string.IsNullOrEmpty(filters[1].ToString())?DateTime.MinValue: Convert.ToDateTime(filters[1]);
                var fechaFinCreacion = string.IsNullOrEmpty(filters[2].ToString()) ?DateTime.Now: Convert.ToDateTime(filters[2]);
                var fechaInicioAprobacion = string.IsNullOrEmpty(filters[3].ToString()) ? DateTime.MinValue : Convert.ToDateTime(filters[3]);
                var fechaFinAprobacion = string.IsNullOrEmpty(filters[3].ToString()) ? DateTime.Now:Convert.ToDateTime(filters[4]);
                using (var db = new DISEntities())
                {
                    var query = db.pa_sel_SegDescarga(contrato.IDE_CONTRATO, filters[0].ToString(), fechaInicioCreacion, fechaFinCreacion,fechaInicioAprobacion,fechaFinAprobacion).ToList();
                    total = query.Count;
                    foreach (var item in query)
                    {
                        var eSegDescarga = new ESegDescarga() {
                            Estado = item.Estado,
                            FechaCarga = item.FecReg,
                            Importe = string.Format(ConfigurationManager.AppSettings.Get("Float"),item.ImporteTotalSoles),
                            nombreArchivo = item.NombreArchivo,
                            NroLineas = Convert.ToInt32(item.TotalRegistros),
                            Moneda = item.Moneda,
                            Usuario = item.UsuReg
                        };
                        listDescarag.Add(eSegDescarga);
                    }
                }
                return listDescarag;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
