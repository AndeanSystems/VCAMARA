using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VidaCamara.Web.WebPage.ModuloDIS.Consultas
{
    public partial class frmSegDescarga : System.Web.UI.Page
    {
        static int total;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetLLenadoContrato();
            }
        }
        private void SetLLenadoContrato()
        {
            var list = new VidaCamara.SBS.Utils.Utility().getContrato(out total);
            ddl_contrato.DataSource = list;
            ddl_contrato.DataTextField = "_des_Contrato";
            ddl_contrato.DataValueField = "_nro_Contrato";
            ddl_contrato.DataBind();
            ddl_contrato.Items.Insert(0, new ListItem("Seleccione ----", "0"));
        }
    }
}