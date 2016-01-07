using System;
using System.Collections.Generic;
using System.Linq;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.data
{
    public class dAprobacionCarga
    {
        public List<EAprobacionCarga> listApruebaCarga(CONTRATO_SYS contrato)
        {
            var listAprueba = new List<EAprobacionCarga>();
            try
            {
                using (var db = new DISEntities())
                {
                    var query = db.pa_sel_pagoAprueba(contrato.IDE_CONTRATO, "", DateTime.Now).ToList();
                    foreach (var item in query)
                    {
                        var eApruebaCarga = new EAprobacionCarga()
                        {
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
    }
}
