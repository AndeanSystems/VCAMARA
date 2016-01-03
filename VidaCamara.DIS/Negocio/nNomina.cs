using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.Negocio
{
    public class nNomina
    {
        public Int32 setGrabarNomina(NOMINA nomina) 
        {
            return new dNomina().setGrabarNomina(nomina);
        }
    }
}
