using System;
using System.Collections.Generic;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.Negocio
{
    public class nTelebanking
    {
        public List<EGeneraTelebankig> listTelebanking(NOMINA nomina, int jtStartIndex, int jtPageSize, out int total)
        {
            return new dTelebanking().listTelebanking(nomina, jtStartIndex, jtPageSize,out total);
        }

        public List<EGeneraTelebankig> listTelebankingByArchivoId(NOMINA nomina)
        {
            return new dTelebanking().listTelebankingByArchivoId(nomina);
        }
    }
}
