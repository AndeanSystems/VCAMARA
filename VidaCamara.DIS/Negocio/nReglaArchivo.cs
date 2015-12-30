using System.Collections.Generic;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.data;

namespace VidaCamara.DIS.Negocio
{
    public class nReglaArchivo
    {
        public List<ReglaArchivo> getListReglaArchivo(ReglaArchivo regla, int jtStartIndex, int jtPageSize, out int total)
        {
            return new dReglaArchivo().getListReglaArchivo(regla, jtStartIndex, jtPageSize, out total);
        }
    }
}
