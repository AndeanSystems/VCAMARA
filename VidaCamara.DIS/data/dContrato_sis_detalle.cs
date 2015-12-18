using System;
using System.Collections.Generic;
using System.Linq;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.data
{
    public class dContrato_sis_detalle
    {
        public List<CONTRATO_SIS_DET> getlistContratoDetalle(){
            try
            {
                using (var db = new DISEntities())
                {
                    return db.CONTRATO_SIS_DETs.ToList();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public Int32 setGuardarContratoDetalle(CONTRATO_SIS_DET det)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    db.CONTRATO_SIS_DETs.Add(det);
                    db.SaveChanges();
                    return det.IDE_CONTRATO_DET;
                }
            }
            catch (Exception ex)
            {
                throw(new Exception(ex.Message));
            }
        }
        public Int32 setActualizarContratoDetalle(CONTRATO_SIS_DET det)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    var entity = db.CONTRATO_SIS_DETs.Find(det.IDE_CONTRATO_DET);
                    entity.IDE_CONTRATO = det.IDE_CONTRATO;
                    entity.NRO_ORDEN = det.NRO_ORDEN;
                    entity.PRC_PARTICIACION = det.PRC_PARTICIACION;
                    entity.USU_MOD = det.USU_MOD;
                    entity.FEC_MOD = det.FEC_MOD;

                    return db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
