using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.data
{
    public class dReglaArchivo
    {
        public List<ReglaArchivo> getListReglaArchivo(ReglaArchivo regla, int jtStartIndex, int jtPageSize, out int total)
        {
            var listReglaArchivo = new List<ReglaArchivo>();
            try
            {
                using (var db = new DISEntities())
                {
                    //asignar valor al parametro total
                    //doc eso es todod - que tan t
                    total = db.ReglaArchivos.Where(a => a.Archivo.Equals(regla.Archivo) && (a.TipoLinea.Equals(regla.TipoLinea) || regla.TipoLinea.Equals("0"))).Count();
                    var query = db.ReglaArchivos.OrderBy(a=>a.CaracterInicial).Where(a=>a.Archivo.Equals(regla.Archivo) && (a.TipoLinea.Equals(regla.TipoLinea) || regla.TipoLinea.Equals("0"))).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    foreach (var item in query)
                    {
                        var reglaArchivo = new ReglaArchivo()
                        {
                            Archivo = item.Archivo,
                            NombreCampo = item.NombreCampo,
                            InformacionCampo = item.InformacionCampo,
                            TipoLinea = item.TipoLinea,
                            CaracterInicial = item.CaracterInicial,
                            LargoCampo = item.LargoCampo,
                            TipoCampo = item.TipoCampo,
                            FormatoContenido = item.FormatoContenido,

                        };
                        listReglaArchivo.Add(reglaArchivo);
                    }
                }
                return listReglaArchivo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
