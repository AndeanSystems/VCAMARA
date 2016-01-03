using System.Collections.Generic;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.data;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Web;
using System;
using System.Reflection;
using VidaCamara.DIS.Helpers;

namespace VidaCamara.DIS.Negocio
{
    public class nArchivoCargado
    {
        /// <summary>
        /// Devuelve la lista de historia lin det utilizando los parametros endicados por el usuario
        /// </summary>
        /// <param name="historiaLinCab"></param>
        /// <param name="historiaLinDet"></param>
        /// <param name="filterParam"></param>
        /// <param name="jtStartIndex"></param>
        /// <param name="jtPageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<HistorialCargaArchivo_LinDet> listArchivoCargado(HistorialCargaArchivo_LinCab historiaLinCab,HistorialCargaArchivo_LinDet historiaLinDet, object[] filterParam,int jtStartIndex, int jtPageSize,out int total)
        {
            return new dPagoCargado().listArchivoCargado(historiaLinCab,historiaLinDet, filterParam, jtStartIndex, jtPageSize,out total);
        }
        /// <summary>
        /// Crea un archivo excel acuerdo a los filtros especificados
        /// </summary>
        /// <param name="cab"></param>
        /// <param name="det"></param>
        /// <param name="filterParam"></param>
        /// <returns></returns>
        public string getDescargarHistoriaLinCab(HistorialCargaArchivo_LinCab cab, HistorialCargaArchivo_LinDet det, object[] filterParam)
        {
            try
            {
                var excelStyle = new excelStyle();
                var nombreArchivo = "Archivo " + filterParam[0].ToString()+" "+DateTime.Now.ToString("yyyyMMdd");
                var rutaTemporal = @HttpContext.Current.Server.MapPath("~/Temp/Descargas/" + nombreArchivo + ".xlsx");
                int total;
                //new Utils.DeleteFile().deleteFile(HttpContext.Current.Server.MapPath(@"~/Utils/xlsxs/"));
                XSSFWorkbook book = new XSSFWorkbook();
                var reglaArchivo = new ReglaArchivo() { Archivo = filterParam[0].ToString(), TipoLinea = "D" };
                var listReglaArchivo = new nReglaArchivo().getListReglaArchivo(reglaArchivo, 0, 200, out total);

                //crear el libro
                var sheet = book.CreateSheet(nombreArchivo);
                var rowCabecera = sheet.CreateRow(1);
                //construir cabecera
                ICell cellCabecera;
                for (int i = 0; i < listReglaArchivo.Count; i++)
                {
                    cellCabecera = rowCabecera.CreateCell(i+1);
                    cellCabecera.SetCellValue(listReglaArchivo[i].TituloColumna);
                    //cellCabecera.CellStyle = excelStyle.setFontText(12, true, book);
                }
                //consultar datos segun los filtros especificados
                var listHistoriaLinDet = new dPagoCargado().listArchivoCargado(cab, det, filterParam, 0, 100000, out total);
                
                for (int i = 0; i < listHistoriaLinDet.Count; i++)
                {
                    IRow rowBody = sheet.CreateRow(i+2);
                    ICell cellBody;
                    for (int c = 0; c < listReglaArchivo.Count; c++)
                    {
                        cellBody = rowBody.CreateCell(c+1);
                        var property = listHistoriaLinDet[i].GetType().GetProperty(listReglaArchivo[c].NombreCampo.ToString().Trim(), BindingFlags.Public | BindingFlags.Instance);
                        cellBody.SetCellValue(property.GetValue(listHistoriaLinDet[i]) == null?"": property.GetValue(listHistoriaLinDet[i]).ToString());
                        //cellBody.CellStyle = excelStyle.setFontText(11, false, book);
                    }
                }

                //guardar el archivo creado en memoria
                using (var file = new FileStream(rutaTemporal,FileMode.Create,FileAccess.ReadWrite))
                {
                    book.Write(file);
                    file.Close();
                    book.Close();
                }
                return rutaTemporal;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
