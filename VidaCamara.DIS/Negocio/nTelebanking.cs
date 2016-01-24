using System;
using System.Collections.Generic;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.Negocio
{
    public class nTelebanking
    {
        public List<EGeneraTelebankig> listTelebanking(NOMINA nomina, int jtStartIndex, int jtPageSize,string formatoMoneda, out int total)
        {
            return new dTelebanking().listTelebanking(nomina, jtStartIndex, jtPageSize,formatoMoneda,out total);
        }

        public List<EGeneraTelebankig> listTelebankingByArchivoId(NOMINA nomina,string formatoMoneda)
        {
            return new dTelebanking().listTelebankingByArchivoId(nomina, formatoMoneda);
        }
    }
}
