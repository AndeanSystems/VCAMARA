using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VidaCamara.DIS.Modelo;
using VidaCamara.SBS.Negocio;
using VidaCamara.DIS.Negocio;
using System.IO;

namespace VidaCamara.Web.WebPage.ModuloDIS.Operaciones
{
    public partial class frmInterfaceContableSIS : System.Web.UI.Page
    {
        #region VARIABLES
        static bValidarAcceso _accesso = new bValidarAcceso();
        static int total;
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
                concepto.SetEstablecerDataSourceConcepto(ddl_tipo_archivo, "17");
                concepto.SetEstablecerDataSourceConcepto(ddl_moneda, "20");
                txt_desde.Text = DateTime.Now.ToShortDateString();
                txt_hasta.Text = DateTime.Now.ToShortDateString();
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listInterfaceContable(int jtStartIndex, int jtPageSize, EXACTUS_CABECERA_SIS cabecera,object[] filter)
        {
            cabecera.FECHA = Convert.ToDateTime(filter[0].ToString());
            cabecera.FECHA_CREACION = Convert.ToDateTime(filter[1].ToString());
            var negocio = new nInterfaceContable();
            return new { Result = "OK", Records = negocio.listInterfaceContable(cabecera,new TipoArchivo(){NombreTipoArchivo = filter[2].ToString() },jtStartIndex, jtPageSize, out total), TotalRecordCount = total };
        }

        protected void btn_exportar_Click(object sender, ImageClickEventArgs e)
        {
            var contrato = new EXACTUS_CABECERA_SIS()
            {
                IDE_CONTRATO = Convert.ToInt32(ddl_contrato.SelectedItem.Value),
                FECHA = Convert.ToDateTime(txt_desde.Text),
                FECHA_CREACION = Convert.ToDateTime(txt_hasta.Text),
                ESTADO_TRANSFERENCIA = ddl_estado.SelectedItem.Value,
                IDE_MONEDA = Convert.ToInt32(ddl_moneda.SelectedItem.Value)
            };
            var pathArchivo = new nInterfaceContable().descargarExcel(contrato, new TipoArchivo() { NombreTipoArchivo = ddl_tipo_archivo.SelectedItem.Value});
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}",Path.GetFileName(pathArchivo)));
            Response.TransmitFile(pathArchivo);
            Response.End();
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

        protected void btn_transfer_Click(object sender, ImageClickEventArgs e)
        {
            var contrato = new EXACTUS_CABECERA_SIS()
            {
                IDE_CONTRATO = Convert.ToInt32(ddl_contrato.SelectedItem.Value),
                FECHA = Convert.ToDateTime(txt_desde.Text),
                FECHA_CREACION = Convert.ToDateTime(txt_hasta.Text),
                ESTADO_TRANSFERENCIA = ddl_estado.SelectedItem.Value,
                IDE_MONEDA = Convert.ToInt32(ddl_moneda.SelectedItem.Value)
            };
            try
            {
                var respuesta = new nInterfaceContable().transferirInterfaceContable(contrato, new TipoArchivo() { NombreTipoArchivo = ddl_tipo_archivo.SelectedItem.Value });
                MessageBox("Asiento (s) transferido (s) correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox(string.Format("ERROR =>{0}",ex.Message.Replace(Environment.NewLine,"").ToString()));
            }
            
        }

        private void MessageBox(string text)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "$('<div style=\"font-size:14px;text-align:center;\">" + text + "</div>').dialog({title:'Confirmación',modal:true,width:400,height:240,buttons: [{id: 'aceptar',text: 'Aceptar',icons: { primary: 'ui-icon-circle-check' },click: function () {$(this).dialog('close');}}]});", true);
        }
    }
}