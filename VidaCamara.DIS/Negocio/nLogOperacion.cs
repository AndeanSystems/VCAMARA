using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.Negocio
{
    public class nLogOperacion
    {
        private DateTime FechEven = DateTime.Now;
        private String CodiCnx = String.Empty;
        //este el el metodo generico para llamar desde cualquier parte de la aplicacion
        //ejemplo
        //llena la entidad
        //var operacion = LogOperacion(){agregar sus propiedades menos (FechEven,CodiCnx)}
        //luego instacia la clase nLogOperacion
        //var negocio = nLogOperacion().setGuardarLogOperacion(operacion);
        public long setGuardarLogOperacion(LogOperacion log)
        { 
            log.FechEven = FechEven;
            log.CodiCnx = CodiCnx;
            return new dLogOperacion().setGuardarLogOperacion(log);
        }

        public LogOperacion setLLenarEntidad(int IdeContrato,string TipoOper,string CodiEven, string CodiOper,string CodiUsu)
        {
            return new LogOperacion()
            {
                IDE_CONTRATO = IdeContrato,
                TipoOper = TipoOper,
                CodiEven = CodiEven,
                CodiOper = CodiOper,
                CodiUsu = CodiUsu
            };
        }

        public List<HLogOperacion> getListLogOperacion(HLogOperacion log, int jtStartIndex, int jtPageSize,object[] filters, out int total) 
        {
            return new dLogOperacion().getListLogOperacion(log, jtStartIndex, jtPageSize, filters, out total);
        }
    }
}
