using System;
using System.Web.UI.WebControls;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Negocio;
using VidaCamara.SBS.Negocio;

namespace VidaCamara.Web.WebPage.ModuloDIS.Consultas
{
    public partial class frmSegDescarga : System.Web.UI.Page
    {
        static int total;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var concepto = new bTablaVC();
                SetLLenadoContrato();
                concepto.SetEstablecerDataSourceConcepto(ddl_tipo_tramite, "22");
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listApruebaCarga(int jtStartIndex, int jtPageSize, string jtSorting,CONTRATO_SYS contrato)
        {
            var negocio = new nAprobacionCarga();
            return new { Result = "OK", Records = negocio.listApruebaCarga(contrato), TotalRecordCount = 50 };
        }
        private void SetLLenadoContrato()
        {
            var list = new VidaCamara.SBS.Utils.Utility().getContratoSys(out total);
            ddl_contrato.DataSource = list;
            ddl_contrato.DataTextField = "_des_Contrato";
            ddl_contrato.DataValueField = "_ide_Contrato";
            ddl_contrato.DataBind();
            ddl_contrato.Items.Insert(0, new ListItem("Seleccione ----", "0"));
        }
    }
}