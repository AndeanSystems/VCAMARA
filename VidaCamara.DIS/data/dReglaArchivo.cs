using System;
using System.Collections.Generic;
using System.Linq;
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
                    var query = db.ReglaArchivos.OrderBy(a=>a.CaracterInicial).Where(a=>a.Archivo.Equals(regla.Archivo) && (a.TipoLinea.Equals(regla.TipoLinea) || regla.TipoLinea.Equals("0")) && a.vigente == 1 && a.NUM_CONT_LIC == regla.NUM_CONT_LIC).ToList();
                    total = query.Count();
                    foreach (var item in query.Skip(jtStartIndex).Take(jtPageSize))
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
                            TituloColumna = item.TituloColumna
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
