using System.Collections.Generic;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.data;
using System.Text;

namespace VidaCamara.DIS.Negocio
{
    public class nReglaArchivo
    {
        /// <summary>
        /// Retorna el listado de regla por nombre paginado
        /// </summary>
        /// <param name="regla"></param>
        /// <param name="jtStartIndex"></param>
        /// <param name="jtPageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ReglaArchivo> getListReglaArchivo(ReglaArchivo regla, int jtStartIndex, int jtPageSize, out int total)
        {
            return new dReglaArchivo().getListReglaArchivo(regla, jtStartIndex, jtPageSize, out total);
        }
        /// <summary>
        /// Devuelve la las colunas para una grilla en formato de json acuerdo a un tipo de regla
        /// </summary>
        /// <param name="regla"></param>
        /// <returns></returns>
        public StringBuilder getColumnGridByArchivo(ReglaArchivo regla)
        {
            var total = 0;
            var listRegla = new dReglaArchivo().getListReglaArchivo(regla, 0, 200, out total);
            var sb = new StringBuilder();
            sb.Append("var fields = {");
            for (int i = 1; i <= listRegla.Count; i++)
            {
                sb.Append(listRegla[i - 1].NombreCampo + ":{");
                sb.Append("title:" + "'" + listRegla[i - 1].TituloColumna + "'}" + (i == listRegla.Count ? "" : ","));
            }
            sb.Append("};");
            return sb;
        }
    }
}
