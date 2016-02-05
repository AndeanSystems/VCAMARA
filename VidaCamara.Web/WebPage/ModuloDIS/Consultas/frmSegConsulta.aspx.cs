using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Negocio;
using VidaCamara.SBS.Negocio;
using VidaCamara.DIS.Helpers;
using System.Configuration;

namespace VidaCamara.Web.WebPage.ModuloDIS.Consultas
{
    public partial class frmSegConsulta : System.Web.UI.Page
    {
        #region VARIABLES
        static int total;
        static bValidarAcceso accesso = new bValidarAcceso();
        static HistorialCargaArchivo_LinCab cabecera = new HistorialCargaArchivo_LinCab();
        static HistorialCargaArchivo_LinDet historiaLinDet = new HistorialCargaArchivo_LinDet();
        static object[] filterParam = new object[4];//[0]Tipo de archivo [1] fecha inicio [2] fecha fin [3] fomato meneda 
        static NOMINA nomina = new NOMINA();
        static string formatoMoneda = string.Empty;

        #endregion VARIABLES
        #region EVENTOS
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
                concepto.SetEstablecerDataSourceConcepto(ddl_moneda, "20");
                formatoMoneda = ConfigurationManager.AppSettings.Get("Float").ToString();
                txt_fec_ini_o.Text = DateTime.Now.ToShortDateString();
                txt_fec_hasta_o.Text = DateTime.Now.ToShortDateString();
            }
        }
        //LISTAR HISTORIA CARGA DETALLE
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listHistoriaDetalle(int jtStartIndex, int jtPageSize,string jtSorting)
        {
            filterParam[3] = formatoMoneda;
            var listCargaDetalle = new nArchivoCargado().listArchivoCargado(cabecera,historiaLinDet, filterParam, jtStartIndex, jtPageSize, out total);
            return new { Result = "OK", Records = listCargaDetalle, TotalRecordCount = total };
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listNominaConsulta(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var negocio = new nNomina();
            return new { Result = "OK", Records = negocio.listNominaConsulta(nomina, filterParam, jtStartIndex, jtPageSize, out total), TotalRecordCount = total };
        }
        protected void btn_consultar_Click1(object sender, ImageClickEventArgs e)
        {
            var nombreTipoArchivo = ddl_tipo_archivo.SelectedItem.Value;
            //AQUI SE AGREGAN DATOS FIJOS A LA GRILLA DE CONSULTA
            var columns = nombreTipoArchivo != "NOMINA" ? ",FechaInsert:{title:'Fecha_Registro',display: function (data) {return ConvertNumberToDateTime(data.record.FechaInsert);}},NombreArchivo: { title: 'NombreArchivo'}" :
                                                         ",FechaReg:{title:'Fecha_Registro',display: function (data) {return ConvertNumberToDateTime(data.record.FechaReg);}},NombreArchivo: { title: 'NombreArchivo'}";
            setLlenarEntiddes();
            var action = nombreTipoArchivo == "NOMINA"? "/WebPage/ModuloDIS/Consultas/frmSegConsulta.aspx/listNominaConsulta" : "/WebPage/ModuloDIS/Consultas/frmSegConsulta.aspx/listHistoriaDetalle";
            var tipoLinea = nombreTipoArchivo == "NOMINA"?"*":"D";
            var sorter = nombreTipoArchivo == "NOMINA" ? "RUC_ORDE ASC" : "TIP_REGI ASC";
            var contratoSis = new nContratoSis().listContratoByID(new CONTRATO_SYS() { IDE_CONTRATO = Convert.ToInt32(ddl_contrato.SelectedItem.Value) });
            var regla = new ReglaArchivo() { Archivo = ddl_tipo_archivo.SelectedItem.Value, TipoLinea = tipoLinea,NUM_CONT_LIC = Convert.ToInt32(contratoSis.NRO_CONTRATO) };
            var fields = new nReglaArchivo().getColumnGridByArchivo(regla, columns).ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "Fields", fields, true);
            var grid = new gridCreator().getGrid("frmSeqConsulta", "5000", action, sorter).ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "Grid", grid, true);
        }
        protected void btn_exportar_Click(object sender, ImageClickEventArgs e)
        {
            setLlenarEntiddes();
            var nombreArchivo = new nArchivoCargado().getDescargarConsulta(cabecera,nomina, historiaLinDet, filterParam);
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename="+ nombreArchivo);
            Response.TransmitFile(nombreArchivo);
            Response.End();
        }
        #endregion EVENTOS
        #region METODOS
        private void SetLLenadoContratoSIS()
        {
            var list = new VidaCamara.SBS.Utils.Utility().getContratoSys(out total);
            ddl_contrato.DataSource = list;
            ddl_contrato.DataTextField = "_des_Contrato";
            ddl_contrato.DataValueField = "_ide_Contrato";
            ddl_contrato.DataBind();
            ddl_contrato.Items.Insert(0, new ListItem("Seleccione ----", "0"));
        }
        private void setLlenarEntiddes()
        {
            //HISTORIALINCAB
            cabecera.IDE_CONTRATO = Convert.ToInt32(ddl_contrato.SelectedItem.Value);
            cabecera.ESTADO = ddl_estado.SelectedItem.Value;
            //HISTORIALINDET
            historiaLinDet.COD_AFP = ddl_afp.SelectedItem.Value;
            historiaLinDet.TIP_MONE = ddl_moneda.SelectedItem.Value;
            historiaLinDet.COD_CUSP = txt_cod_cusp.Text;
            historiaLinDet.APE_MATE_PEN = txt_apellido.Text;
            historiaLinDet.PRI_NOMB_PEN = txt_nombre.Text;
            historiaLinDet.NUM_DOCU_PEN = txt_dni.Text;
            historiaLinDet.NUM_SOLI_PEN = txt_nro_solicitud.Text;
            //PARAMETROS DE FECHA Y TIPO DE ARCHIVO
            filterParam[0] = ddl_tipo_archivo.SelectedItem.Value;
            filterParam[1] = txt_fec_ini_o.Text;
            filterParam[2] = txt_fec_hasta_o.Text;

            if(ddl_tipo_archivo.SelectedItem.Value.Equals("NOMINA"))
                nomina.IDE_CONTRATO = Convert.ToInt32(ddl_contrato.SelectedItem.Value);
        }
        private void MessageBox(String text)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "$('<div style=\"font-size:14px;text-align:center;\">" + text + "</div>').dialog({title:'Confirmación',modal:true,width:400,height:240,buttons: [{id: 'aceptar',text: 'Aceptar',icons: { primary: 'ui-icon-circle-check' },click: function () {$(this).dialog('close');}}]})", true);
        }
        #endregion METODOS
    }
}