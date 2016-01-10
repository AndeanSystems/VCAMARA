using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.Negocio
{
    public class nSegDescarga
    {
        public List<ESegDescarga> listSegDescarga(CONTRATO_SYS contrato, object[] filters, int jtStartIndex, int jtPageSize, out int total)
        {
            return new dSegDescarga().listSegDescarga(contrato,filters,jtStartIndex,jtPageSize,out total);
        }

        public string descargarConsultaExcel(CONTRATO_SYS contrato, object[] filters)
        {
            try
            {
                var nombreArchivo = "Descarga " + filters[0].ToString() + " " + DateTime.Now.ToString("yyyyMMdd");
                var rutaTemporal = @HttpContext.Current.Server.MapPath("~/Temp/Descargas/" + nombreArchivo + ".xlsx");
                int total;
                var book = new XSSFWorkbook();
                string[] columns = {"Archivo","Fecha Carga","Usuario","Nro Lineas","Estado","Moneda","Importe" };
                var sheet = book.CreateSheet(nombreArchivo);
                var rowBook = sheet.CreateRow(1);
                ICell cellBook;
                for (int i = 0; i < columns.Length; i++)
                {
                    cellBook = rowBook.CreateCell(i+1);
                    cellBook.SetCellValue(columns[i]);
                    cellBook.CellStyle = setFontText(12, true, book);
                }

                var listSegDescarga = new nSegDescarga().listSegDescarga(contrato, filters, 0, 100000, out total);
                for (int i = 0; i < listSegDescarga.Count; i++)
                {
                    var rowBody = sheet.CreateRow(2+i);

                    ICell cellNombre = rowBody.CreateCell(1);
                    cellNombre.SetCellValue(listSegDescarga[i].nombreArchivo);
                    cellNombre.CellStyle = setFontText(11, false, book);

                    ICell cellFecCarga = rowBody.CreateCell(2);
                    cellFecCarga.SetCellValue(listSegDescarga[i].FechaCarga.ToShortDateString());
                    cellFecCarga.CellStyle = setFontText(11, false, book);

                    ICell cellUsuario = rowBody.CreateCell(3);
                    cellUsuario.SetCellValue(listSegDescarga[i].Usuario);
                    cellUsuario.CellStyle = setFontText(11, false, book);

                    ICell cellNroLinea = rowBody.CreateCell(4);
                    cellNroLinea.SetCellValue(listSegDescarga[i].NroLineas);
                    cellNroLinea.CellStyle = setFontText(11, false, book);

                    ICell cellEstado = rowBody.CreateCell(5);
                    cellEstado.SetCellValue(listSegDescarga[i].Estado);
                    cellEstado.CellStyle = setFontText(11, false, book);

                    ICell cellMoneda = rowBody.CreateCell(6);
                    cellMoneda.SetCellValue(listSegDescarga[i].Moneda);
                    cellMoneda.CellStyle = setFontText(11, false, book);

                    ICell cellImporte = rowBody.CreateCell(7);
                    cellImporte.SetCellValue(listSegDescarga[i].Importe);
                    cellImporte.CellStyle = setFontText(11, false, book);

                }

                using (var file = new FileStream(rutaTemporal, FileMode.Create, FileAccess.ReadWrite))
                {
                    book.Write(file);
                    file.Close();
                    book.Close();
                }

                return rutaTemporal;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private ICellStyle setFontText(short point, bool color, XSSFWorkbook book)
        {
            var font = book.CreateFont();
            font.FontName = "Calibri";
            font.Color = (IndexedColors.Black.Index);
            font.FontHeightInPoints = point;

            var style = book.CreateCellStyle();
            style.SetFont(font);
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            if (color)
            {
                style.FillForegroundColor = HSSFColor.Grey25Percent.Index;
                style.FillPattern = FillPattern.SolidForeground;
            }
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            return style;
        }
    }
}
