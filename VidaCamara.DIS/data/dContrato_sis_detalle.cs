using System;
using System.Collections.Generic;
using System.Linq;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.data
{
    public class dContrato_sis_detalle
    {
        public List<CONTRATO_SIS_DET> getlistContratoDetalle(CONTRATO_SIS_DET contratoDetalle, object[] filterOptions, out int total)
        {
            var listDetalle = new List<CONTRATO_SIS_DET>();
            try
            {
                using (var db = new DISEntities())
                {
                   total = db.CONTRATO_SIS_DETs.Count();
                   var query = db.CONTRATO_SIS_DETs.Include("CONTRATO_SYS").Where(a => a.IDE_CONTRATO == contratoDetalle.IDE_CONTRATO).ToList();
                    foreach (var item in query)
                    {
                        var entity = new CONTRATO_SIS_DET()
                        {
                            IDE_CONTRATO_DET = item.IDE_CONTRATO_DET,
                            IDE_CONTRATO = item.IDE_CONTRATO,
                            COD_CSV = item.COD_CSV,
                            PRC_PARTICIACION = item.PRC_PARTICIACION,
                            NRO_ORDEN = item.NRO_ORDEN,
                            ESTADO = item.ESTADO,
                            FEC_REG = item.FEC_REG,
                            USU_REG = item.USU_REG,
                            CONTRATO_SYS = new CONTRATO_SYS()
                            {
                                DES_CONTRATO = item.CONTRATO_SYS.DES_CONTRATO,
                                NRO_CONTRATO = item.CONTRATO_SYS.NRO_CONTRATO
                            },
                        };
                        listDetalle.Add(entity);
                    }
                }
                return listDetalle;
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
        public Int32 setEliminarContratoDetalle(int primary_key)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    var entity = db.CONTRATO_SIS_DETs.Find(primary_key);
                    db.CONTRATO_SIS_DETs.Remove(entity);
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
