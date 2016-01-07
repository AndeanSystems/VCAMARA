using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaCamara.DIS.Modelo.EEntidad
{
    public class HLogOperacion:LogOperacion
    {
        public string TipoEvento { get; set; }
        public string Evento { get; set; }
    }
}
