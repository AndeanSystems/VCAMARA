using System;
using System.Configuration;
using System.Web.UI.WebControls;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Negocio;
using VidaCamara.SBS.Negocio;

namespace VidaCamara.Web.WebPage.ModuloDIS.Operaciones
{
    public partial class frmTelebankig : System.Web.UI.Page
    {
        #region VARIABLES
        static bValidarAcceso _accesso = new bValidarAcceso();
        static int total;
        static string formatoMoneda = string.Empty;
        #endregion VARIABLES
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["pagina"] = "OTROS";
            if (Session["username"] == null)
                Response.Redirect("Login?go=0");
            else
            {
                if (!_accesso.GetValidarAcceso(Request.QueryString["go"]))
                {
                    Response.Redirect("Error");
                }
            }
            if (!IsPostBack)
            {
                var concepto = new bTablaVC();
                SetLLenadoContrato();
                txt_fecha.Text = DateTime.Now.ToShortDateString();
                //concepto.SetEstablecerDataSourceConcepto(ddl_tipo_archivo,"17");
                formatoMoneda = ConfigurationManager.AppSettings.Get("Float").ToString();
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listTelebanking(int jtStartIndex, int jtPageSize, string jtSorting, NOMINA nomina,string fecha)
        {
            var negocio = new nTelebanking();
            nomina.FechaReg = Convert.ToDateTime(fecha);
            return new { Result = "OK", Records = negocio.listTelebanking(nomina,jtStartIndex, jtPageSize,formatoMoneda, out total), TotalRecordCount = total };
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listTelebankingByArchivoId(int ArchivoId)
        {
            var nomina = new NOMINA() { ArchivoId = ArchivoId };
            var negocio = new nTelebanking();
            return new { Result = "OK", Records = negocio.listTelebankingByArchivoId(nomina, formatoMoneda) };
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