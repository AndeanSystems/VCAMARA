using System;
using System.Collections.Generic;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;
using System.Linq;

namespace VidaCamara.DIS.data
{
    public class dTelebanking
    {
        public List<EGeneraTelebankig> listTelebanking(NOMINA nomina, int jtStartIndex, int jtPageSize, out int total)
        {
            var listTelebankig = new List<EGeneraTelebankig>();
            try
            {
                using (var db = new DISEntities())
                {
                    var query = db.pa_sel_NominaForTelebanking(nomina.IDE_CONTRATO, nomina.FechaReg).ToList();
                    total = query.Count;
                    foreach (var item in query)
                    {
                        var telebankig = new EGeneraTelebankig()
                        {
                            ArchivoId = item.ArchivoId,
                            NombreArchivo = item.NombreArchivo,
                            FechaOperacion = Convert.ToDateTime(item.FechaOperacion),
                            Moneda = item.Moneda,
                            Importe = item.Importe.ToString(),
                            RutaNomina = "Temp/"+item.NombreArchivo
                        };
                        listTelebankig.Add(telebankig);
                    }
                }
                return listTelebankig;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<EGeneraTelebankig> listTelebankingByArchivoId(NOMINA nomina)
        {
            var listTelebankig = new List<EGeneraTelebankig>();
            try
            {
                using (var db = new DISEntities())
                {
                    var query = db.pa_sel_NominaForTelebankinByArchivoId(nomina.ArchivoId).ToList();
                    foreach (var item in query)
                    {
                        var telebankig = new EGeneraTelebankig()
                        {
                            RUC_BENE = item.RUC_BENE,
                            NOM_BENE = item.NOM_BENE,
                            TIP_CTA = item.TIP_CTA,
                            CTA_BENE = item.CTA_BENE,
                            Importe = item.IMPORTE.ToString()
                        };
                        listTelebankig.Add(telebankig);
                    }
                }
                return listTelebankig;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
