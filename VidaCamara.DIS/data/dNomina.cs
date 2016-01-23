using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.data
{
    public class dNomina
    {
        public Int32 setGrabarNomina(NOMINA nomina) {
            try
            {
                using (var db = new DISEntities()) 
                {
                    db.NOMINAs.Add(nomina);
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public List<NOMINA> listNominaByArchivo(NOMINA nomina, object[] filters, int jtStartIndex, int jtPageSize, out int total)
        {
            var listNomina = new List<NOMINA>();
            try
            {
                using (var db = new DISEntities())
                {
                    var archivoId = filters[3].ToString();
                    var query = db.pa_sel_nominaXArchivo(nomina.IDE_CONTRATO, archivoId).ToList();
                    total = query.Count();
                    foreach (var item in query.Skip(jtStartIndex).Take(jtPageSize))
                    {
                        var entity = new NOMINA()
                        {
                            Id_Nomina = item.Id_Nomina,
                            Id_Empresa = item.Id_Empresa,
                            ArchivoId = item.ArchivoId,
                            IDE_CONTRATO = item.IDE_CONTRATO,
                            RUC_ORDE = item.RUC_ORDE,
                            CTA_ORDE =  item.CTA_ORDE,
                            COD_TRAN = item.COD_TRAN,
                            TIP_MONE = item.TIP_MONE,
                            MON_TRAN = item.MON_TRAN,
                            FEC_TRAN = item.FEC_TRAN,
                            RUC_BENE = item.RUC_BENE,
                            NOM_BENE = item.NOM_BENE,
                            TIP_CTA = item.TIP_CTA,
                            CTA_BENE = item.CTA_BENE,
                            DET_TRAN = item.DET_TRAN,
                            ReglaObservada = item.ReglaObservada

                        };
                        listNomina.Add(entity);
                    }
                }
                return listNomina;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void actualizarEstadoFallido(int idArchivo, int contratoId)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    db.pa_upd_cambiaEstadoNomina(idArchivo, contratoId);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<HNOMINA> listNominaConsulta(NOMINA nomina, object[] filters, int jtStartIndex, int jtPageSize, out int total)
        {
            var listNomina = new List<HNOMINA>();
            try
            {
                using (var db = new DISEntities())
                {
                    var nombreTipoArchivo = filters[0].ToString();
                    var query = (from a in db.NOMINAs
                                 join b in db.Archivos on
                                 a.ArchivoId equals b.ArchivoId
                                 join c in db.TipoArchivos on
                                 b.TipoArchivoId equals c.TipoArchivoId
                                 where
                                    c.NombreTipoArchivo == nombreTipoArchivo
                                &&  a.IDE_CONTRATO == nomina.IDE_CONTRATO
                                select new { a, b }).ToList();
                    total = query.Count();
                    foreach (var item in query.Skip(jtStartIndex).Take(jtPageSize))
                    {
                        var eNomina = new HNOMINA() {
                            FechaReg = item.a.FechaReg,
                            NombreArchivo = item.b.NombreArchivo,
                            Id_Nomina = item.a.Id_Nomina,
                            ArchivoId = item.a.ArchivoId,
                            IDE_CONTRATO = item.a.IDE_CONTRATO,
                            RUC_ORDE = item.a.RUC_ORDE,
                            CTA_ORDE = item.a.CTA_ORDE,
                            COD_TRAN = item.a.COD_TRAN,
                            TIP_MONE = item.a.TIP_MONE,
                            MON_TRAN = item.a.MON_TRAN,
                            FEC_TRAN = item.a.FEC_TRAN,
                            RUC_BENE = item.a.RUC_BENE,
                            NOM_BENE = item.a.NOM_BENE,
                            TIP_CTA = item.a.TIP_CTA,
                            CTA_BENE = item.a.CTA_BENE,
                            DET_TRAN = item.a.DET_TRAN
                        };
                        listNomina.Add(eNomina);
                    }
                }
                return listNomina;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
