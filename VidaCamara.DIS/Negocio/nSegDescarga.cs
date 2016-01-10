using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.Negocio
{
    public class nSegDescarga
    {
        public List<ESegDescarga> listSegDescarga()
        {
            return new dSegDescarga().listSegDescarga();
        }
    }
}
