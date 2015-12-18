using System;
using System.Collections.Generic;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.Negocio
{
    public class nContratoSisDetalle
    {
        public List<CONTRATO_SIS_DET> getlistContratoDetalle()
        {
            return new dContrato_sis_detalle().getlistContratoDetalle();
        }
        public Int32 setGuardarContratoDetalle(CONTRATO_SIS_DET det)
        {
            return new dContrato_sis_detalle().setGuardarContratoDetalle(det);
        }
        public Int32 setActualizarContratoDetalle(CONTRATO_SIS_DET det)
        {
            return new dContrato_sis_detalle().setActualizarContratoDetalle(det);
        }
    }
}
