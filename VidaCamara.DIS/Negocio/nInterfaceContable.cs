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
    public class nInterfaceContable
    {
        public void createInterfaceContable(NOMINA nomina)
        {
            nomina = new nNomina().getNominaByArchivoId(nomina);
            var nombreLiqByNomina = new nArchivo().getArchivoByNomina(new Archivo() { NombreArchivo = nomina.Archivo.NombreArchivo });
            var cabecera = new dInterfaceContable().createInterfaceContableCabecera(nomina, nombreLiqByNomina);

            var asiento = new List<int>(){ 42, 26 };
            for (int i = 0; i < asiento.Count; i++)
            {
                new dInterfaceContable().createInterfaceContableDetalle(nomina, cabecera, asiento[i]);
            }
        }
        public List<HEXACTUS_DETALLE_SIS> listInterfaceContable(EXACTUS_CABECERA_SIS cabecera,TipoArchivo archivo, int index, int size, out int total)
        {
            return new dInterfaceContable().listInterfaceContable(cabecera,archivo,index,size,out total);
        }
        public string descargarExcel(EXACTUS_CABECERA_SIS cabecera,TipoArchivo archivo)
        {
            var helperStyle = new Helpers.excelStyle();
            try
            {
                int total;
                var listInterface = new dInterfaceContable().listInterfaceContable(cabecera, archivo,0, 100000, out total);
                //atributos del file
                var nombreArchivo = string.Format("Interface {0}_{1}",cabecera.IDE_CONTRATO,DateTime.Now.ToString("yyyyMMdd"));
                var rutaTemporal = @HttpContext.Current.Server.MapPath(string.Format("~/Temp/Descargas/{0}.xlsx", nombreArchivo));
                var book = new XSSFWorkbook();
                string[] columns = { "PAQUETE", "ASIENTO", "FECHA_REGISTRO", "TIPO_ASIENTO", "CONTABILIDAD", "FUENTE", "REFERENCIA", "CONTRIBUYENTE",
                                   "CENTRO_COSTO","CUENTA_CONTABLE","DebitoSoles","CreditoSoles","DebitoDolar","CreditoDolar","MONTO_UNIDADES"};
                var sheet = book.CreateSheet(nombreArchivo);
                var rowBook = sheet.CreateRow(1);
                var headerStyle = helperStyle.setFontText(12, true, book);
                var bodyStyle = helperStyle.setFontText(11, false, book);
                ICell cellBook;
                for (int i = 0; i < columns.Length; i++)
                {
                    cellBook = rowBook.CreateCell(i + 1);
                    cellBook.SetCellValue(columns[i]);
                    cellBook.CellStyle = headerStyle;
                }
                for (int i = 0; i < listInterface.Count; i++)
                {
                    var rowBody = sheet.CreateRow(2 + i);

                    ICell cellPaquete = rowBody.CreateCell(1);
                    cellPaquete.SetCellValue(listInterface[i].EXACTUS_CABECERA_SIS.PAQUETE);
                    cellPaquete.CellStyle = bodyStyle;

                    ICell cellAsiento = rowBody.CreateCell(2);
                    cellAsiento.SetCellValue(listInterface[i].EXACTUS_CABECERA_SIS.ASIENTO);
                    cellAsiento.CellStyle = bodyStyle;

                    ICell cellFechaReg = rowBody.CreateCell(3);
                    cellFechaReg.SetCellValue(listInterface[i].EXACTUS_CABECERA_SIS.FECHA.ToString());
                    cellFechaReg.CellStyle = bodyStyle;

                    ICell cellTipoAsiento = rowBody.CreateCell(4);
                    cellTipoAsiento.SetCellValue(listInterface[i].EXACTUS_CABECERA_SIS.TIPO_ASIENTO);
                    cellTipoAsiento.CellStyle = bodyStyle;

                    ICell cellContabilidad = rowBody.CreateCell(5);
                    cellContabilidad.SetCellValue(listInterface[i].EXACTUS_CABECERA_SIS.CONTABILIDAD);
                    cellContabilidad.CellStyle = bodyStyle;

                    ICell cellFuente = rowBody.CreateCell(6);
                    cellFuente.SetCellValue(listInterface[i].FUENTE);
                    cellFuente.CellStyle = bodyStyle;

                    ICell cellReferencia = rowBody.CreateCell(7);
                    cellReferencia.SetCellValue(listInterface[i].REFERENCIA);
                    cellReferencia.CellStyle = bodyStyle;

                    ICell cellContribuyente = rowBody.CreateCell(8);
                    cellContribuyente.SetCellValue(listInterface[i].CONTRIBUYENTE);
                    cellContribuyente.CellStyle = bodyStyle;

                    ICell cellCentroCosto = rowBody.CreateCell(9);
                    cellCentroCosto.SetCellValue(listInterface[i].CENTRO_COSTO);
                    cellCentroCosto.CellStyle = bodyStyle;

                    ICell cellCuentaCont = rowBody.CreateCell(10);
                    cellCuentaCont.SetCellValue(listInterface[i].CUENTA_CONTABLE);
                    cellCuentaCont.CellStyle = bodyStyle;

                    ICell cellDebitoSol = rowBody.CreateCell(11);
                    cellDebitoSol.SetCellValue(listInterface[i].DebitoSoles);
                    cellDebitoSol.CellStyle = bodyStyle;

                    ICell cellCreditoSol = rowBody.CreateCell(12);
                    cellCreditoSol.SetCellValue(listInterface[i].CreditoSoles);
                    cellCreditoSol.CellStyle = bodyStyle;

                    ICell cellDebitoDol = rowBody.CreateCell(13);
                    cellDebitoDol.SetCellValue(listInterface[i].DebitoDolar);
                    cellDebitoDol.CellStyle = bodyStyle;

                    ICell cellCreditoDol = rowBody.CreateCell(14);
                    cellCreditoDol.SetCellValue(listInterface[i].CreditoDolar);
                    cellCreditoDol.CellStyle = bodyStyle;

                    ICell cellMontoUnid = rowBody.CreateCell(15);
                    cellMontoUnid.SetCellValue(listInterface[i].MONTO_UNIDADES.ToString());
                    cellMontoUnid.CellStyle = bodyStyle;
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

        public bool transferirInterfaceContable(EXACTUS_CABECERA_SIS contrato, TipoArchivo tipoArchivo)
        {
            var exactusCabecera = new dInterfaceContable().getExactusCabecera(contrato,tipoArchivo);
            var interfaceContable = new dInterfaceContable();
            var response = true;
            try
            {
                foreach (var item in exactusCabecera)
                {
                    if (interfaceContable.createCabeceraInRemoteExactus(item))
                    {
                        interfaceContable.createDetalleInRemote(item);
                        interfaceContable.actualizarEstadoTransferido(item);
                    }
                    else
                    {
                        response = false;
                        break;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw(new Exception(ex.Message));
            }
        }
    }
}
