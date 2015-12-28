using System.Collections.Generic;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.Negocio
{
    public class nArchivo
    {
        public List<Archivo> listExisteArchivo(Archivo archivo)
        {
            return new dArchivo().listExisteArchivo(archivo);
        }
    }
}
