using System.Collections.Generic;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.data;

namespace VidaCamara.DIS.Negocio
{
    public class nReglaArchivo
    {
        public List<ReglaArchivo> getListReglaArchivo(ReglaArchivo regla)
        {
            return new dReglaArchivo().getListReglaArchivo(regla);
        }
    }
}
