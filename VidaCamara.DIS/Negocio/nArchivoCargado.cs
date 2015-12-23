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
        public List<HistorialCargaArchivo_LinDet> listArchivoCargado(HistorialCargaArchivo_LinCab historiaLinCab, object[] filterParam)
        {
            return new dPagoCargado().listArchivoCargado(historiaLinCab,filterParam);
        }
    }
}
