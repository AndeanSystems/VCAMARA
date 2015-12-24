using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.data;

namespace VidaCamara.DIS.Negocio
{
    public class nArchivoCargado
    {
        public List<HistorialCargaArchivo_LinDet> listArchivoCargado(HistorialCargaArchivo_LinCab historiaLinCab,HistorialCargaArchivo_LinDet historiaLinDet, object[] filterParam,int jtStartIndex, int jtPageSize,out int total)
        {
            return new dPagoCargado().listArchivoCargado(historiaLinCab,historiaLinDet, filterParam, jtStartIndex, jtPageSize,out total);
        }
    }
}
