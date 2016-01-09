using System;
using System.Web.UI.WebControls;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Negocio;
using VidaCamara.SBS.Negocio;

namespace VidaCamara.Web.WebPage.ModuloDIS.Operaciones
{
    public partial class CargaAprobacion : System.Web.UI.Page
    {
        static int total;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var concepto = new bTablaVC();
                SetLLenadoContrato();
                concepto.SetEstablecerDataSourceConcepto(ddl_tipo_archivo, "17");
                txt_fecha_inicio.Text = DateTime.Now.ToShortDateString();
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listApruebaCarga(int jtStartIndex, int jtPageSize, string jtSorting, CONTRATO_SYS contrato,object[] filters)
        {
            var negocio = new nAprobacionCarga();
            return new { Result = "OK", Records = negocio.listApruebaCarga(contrato, jtStartIndex, jtPageSize, filters, out total), TotalRecordCount = total };
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object setAprobar(int linCabId, int IdeContrato)
        {
            try
            {
                new nAprobacionCarga().actualizarEstado(new HistorialCargaArchivo_LinCab() { IDE_CONTRATO = IdeContrato, IdHistorialCargaArchivoLinCab = linCabId });
                return new { Result = true };
            }
            catch (Exception ex)
            {
                return new { Result = ex.Message };
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object setEliminar(int linCabId, int IdeContrato)
        {
            try
            {
                new nAprobacionCarga().eliminarPagoYNomina(new HistorialCargaArchivo_LinCab() { IDE_CONTRATO = IdeContrato, IdHistorialCargaArchivoLinCab = linCabId });
                return new { Result = true };
            }
            catch (Exception ex)
            {
                return new { Result = ex.Message };
            }
        }

        private void SetLLenadoContrato()
        {
            var list = new VidaCamara.SBS.Utils.Utility().getContratoSys(out total);
            ddl_contrato.DataSource = list;
            ddl_contrato.DataTextField = "_des_Contrato";
            ddl_contrato.DataValueField = "_ide_contrato";
            ddl_contrato.DataBind();
            ddl_contrato.Items.Insert(0, new ListItem("Seleccione ----", "0"));
        }
    }
}