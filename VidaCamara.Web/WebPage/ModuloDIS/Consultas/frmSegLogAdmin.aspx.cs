using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;
using VidaCamara.DIS.Negocio;
using VidaCamara.SBS.Negocio;

namespace VidaCamara.Web.WebPage.ModuloDIS.Consultas
{
    public partial class frmLogAdmin : System.Web.UI.Page
    {
        static int total;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var concepto = new bTablaVC();
                SetLLenadoContrato();
                //concepto.SetEstablecerDataSourceConcepto(ddl_operacion, "26");
                concepto.SetEstablecerDataSourceConcepto(ddl_tipo_evento,"25");
            }
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

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listLogOperacion(int jtStartIndex, int jtPageSize,string jtSorting,HLogOperacion log,object[] filters)
        {
            //var listLogOperacion = new nLogOperacion().getListLogOperacion(log, jtStartIndex, jtPageSize, out total);
            //return new { Result = "OK", Records = listLogOperacion, TotalRecordCount = total };
            var listLogOperacion = new nLogOperacion().getListLogOperacion(log, jtStartIndex, jtPageSize, filters, out total);
            return new { Result = "OK", Records = listLogOperacion, TotalRecordCount = total };
        }

    }
}