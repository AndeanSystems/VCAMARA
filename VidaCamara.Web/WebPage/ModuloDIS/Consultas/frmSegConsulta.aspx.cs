using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Negocio;
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
                SetLLenadoContratoSIS();
                concepto.SetEstablecerDataSourceConcepto(ddl_tipo_archivo, "17");
                concepto.SetEstablecerDataSourceConcepto(ddl_afp, "23");
                //concepto.SetEstablecerDataSourceConcepto(ddl_afp, "14");
                concepto.SetEstablecerDataSourceConcepto(ddl_moneda, "20");
                concepto.SetEstablecerDataSourceConcepto(ddl_moneda, "10");
            }
        }
        private void SetLLenadoContratoSIS()
        {
            var list = new VidaCamara.SBS.Utils.Utility().getContratoSys(out total);
            ddl_contrato.DataSource = list;
            ddl_contrato.DataTextField = "_des_Contrato";
            ddl_contrato.DataValueField = "_ide_Contrato";
            ddl_contrato.DataBind();
            ddl_contrato.Items.Insert(0, new ListItem("Seleccione ----", "0"));
        }
        private void getLlistarArchivoCargada()
        {
            var filterParam = new object[1] { ddl_tipo_archivo.SelectedItem.Value};
            var historiaLinCab = new HistorialCargaArchivo_LinCab()
            {
                IDE_CONTRATO = Convert.ToInt32(ddl_contrato.SelectedItem.Value),
            };
            var listCargaDetalle = new nArchivoCargado().listArchivoCargado(historiaLinCab, filterParam);

            if (listCargaDetalle == null)
            {
                MessageBox("No existen registros para la consulta seleccionada");
            }
            else
            {
                gv_archivo_cargado.DataSource = listCargaDetalle;
                gv_archivo_cargado.DataBind();
            }
        }

        protected void btn_consultar_Click(object sender, ImageClickEventArgs e)
        {
            getLlistarArchivoCargada();
        }

        private void MessageBox(String text)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "$('<div style=\"font-size:14px;text-align:center;\">" + text + "</div>').dialog({title:'Confirmación',modal:true,width:400,height:240,buttons: [{id: 'aceptar',text: 'Aceptar',icons: { primary: 'ui-icon-circle-check' },click: function () {$(this).dialog('close');}}]})", true);
        }
    }
}