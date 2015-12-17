using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VidaCamara.SBS.Negocio;

namespace VidaCamara.Web.WebPage.ModuloDIS.Consultas
{
    public partial class frmSegConsulta : System.Web.UI.Page
    {
        static int total;
        bValidarAcceso accesso = new bValidarAcceso();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["pagina"] = "OTROS";
            if (Session["username"] == null)
                Response.Redirect("Login?go=0");
            else
            {
                if (!accesso.GetValidarAcceso(Request.QueryString["go"]))
                {
                    Response.Redirect("Error");
                }
            }

            if (!IsPostBack)
            {
                var concepto = new bTablaVC();
                SetLLenadoContrato();
                //concepto.SetEstablecerDataSourceConcepto(ddl_tipo_tramite,"22");
                concepto.SetEstablecerDataSourceConcepto(ddl_afp, "23");
                //concepto.SetEstablecerDataSourceConcepto(ddl_afp, "14");
                concepto.SetEstablecerDataSourceConcepto(ddl_moneda, "20");
                concepto.SetEstablecerDataSourceConcepto(ddl_moneda, "10");
            }
        }
        private void SetLLenadoContrato()
        {
            var list = new VidaCamara.SBS.Utils.Utility().getContratoSys(out total);
            ddl_contrato.DataSource = list;
            ddl_contrato.DataTextField = "_des_Contrato";
            ddl_contrato.DataValueField = "_nro_Contrato";
            ddl_contrato.DataBind();
            ddl_contrato.Items.Insert(0, new ListItem("Seleccione ----", "0"));
        }
        private void SetLLenadoContratoSIS()
        {
            var list = new VidaCamara.SBS.Utils.Utility().getContratoSys(out total);
            ddl_contrato.DataSource = list;
            ddl_contrato.DataTextField = "_des_Contrato";
            ddl_contrato.DataValueField = "_nro_Contrato";
            ddl_contrato.DataBind();
            ddl_contrato.Items.Insert(0, new ListItem("Seleccione ----", "0"));
        }
    }
}