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
    public class nTelebanking
    {
        public List<EGeneraTelebankig> listTelebanking(NOMINA nomina, int jtStartIndex, int jtPageSize,string formatoMoneda, out int total)
        {
            return new dTelebanking().listTelebanking(nomina, jtStartIndex, jtPageSize,formatoMoneda,out total);
        }

        public List<EGeneraTelebankig> listTelebankingByArchivoId(NOMINA nomina,string formatoMoneda)
        {
            return new dTelebanking().listTelebankingByArchivoId(nomina, formatoMoneda);
        }

        public string descargarExcelTelebankig(NOMINA nomina, string formatoMoneda)
        {
            var helperStyle = new Helpers.excelStyle();
            try
            {
                int total;
                var listDescarga = new dTelebanking().listTelebanking(nomina, 0, 100000, formatoMoneda, out total);
                //atributos del file
                var nombreArchivo = string.Format("Nomina {0}", DateTime.Now.ToString("yyyyMMdd"));
                var rutaTemporal = @HttpContext.Current.Server.MapPath(string.Format("~/Temp/Descargas/{0}.xlsx", nombreArchivo));
                var book = new XSSFWorkbook();
                string[] columns = { "NombreArchivo", "Fecha Operación", "Moneda", "Importe"};
                var sheet = book.CreateSheet(nombreArchivo);
                var rowBook = sheet.CreateRow(1);
                ICell cellBook;
                for (int i = 0; i < columns.Length; i++)
                {
                    cellBook = rowBook.CreateCell(i + 1);
                    cellBook.SetCellValue(columns[i]);
                    cellBook.CellStyle = helperStyle.setFontText(12, true, book);
                }
                for (int i = 0; i < listDescarga.Count; i++)
                {
                    var rowBody = sheet.CreateRow(2 + i);

                    ICell cellArchivo = rowBody.CreateCell(1);
                    cellArchivo.SetCellValue(listDescarga[i].NombreArchivo);
                    cellArchivo.CellStyle = helperStyle.setFontText(11, false, book);

                    ICell cellFechaCarga = rowBody.CreateCell(2);
                    cellFechaCarga.SetCellValue(listDescarga[i].FechaOperacion.ToShortDateString());
                    cellFechaCarga.CellStyle = helperStyle.setFontText(11, false, book);

                    ICell cellMoneda = rowBody.CreateCell(3);
                    cellMoneda.SetCellValue(listDescarga[i].Moneda);
                    cellMoneda.CellStyle = helperStyle.setFontText(11, false, book);

                    ICell cellTotalRegistros = rowBody.CreateCell(4);
                    cellTotalRegistros.SetCellValue(listDescarga[i].Importe);
                    cellTotalRegistros.CellStyle = helperStyle.setFontText(11, false, book);
                }
                if (File.Exists(rutaTemporal))
                    File.Delete(rutaTemporal);
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
                throw(new Exception(ex.Message));
            }
        }

        public void aprobarTelebanking(NOMINA nOMINA)
        {
            //llamada para generar los asientos contables respectivos
            new nInterfaceContable().createInterfaceContable(nOMINA);
            new dTelebanking().aprobarTelebanking(nOMINA);
        }
    }
}
