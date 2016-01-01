﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Negocio;
using VidaCamara.SBS.Negocio;
using VidaCamara.DIS.Helpers;

namespace VidaCamara.Web.WebPage.ModuloDIS.Consultas
{
    public partial class frmSegConsulta : System.Web.UI.Page
    {
        #region VARIABLES
        static int total;
        static bValidarAcceso accesso = new bValidarAcceso();
        static HistorialCargaArchivo_LinCab cabecera = new HistorialCargaArchivo_LinCab();
        static HistorialCargaArchivo_LinDet historiaLinDet = new HistorialCargaArchivo_LinDet();
        static object[] filterParam = new object[3];
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
            }
        }
        //LISTAR HISTORIA CARGA DETALLE
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listHistoriaDetalle(int jtStartIndex, int jtPageSize,string jtSorting)
        {
            var listCargaDetalle = new nArchivoCargado().listArchivoCargado(cabecera,historiaLinDet, filterParam, jtStartIndex, jtPageSize, out total);
            return new { Result = "OK", Records = listCargaDetalle, TotalRecordCount = total };
        }
        protected void btn_consultar_Click1(object sender, ImageClickEventArgs e)
        {
            setLlenarEntiddes();
            var action = "/WebPage/ModuloDIS/Consultas/frmSegConsulta.aspx/listHistoriaDetalle";
            var regla = new ReglaArchivo() { Archivo = ddl_tipo_archivo.SelectedItem.Value, TipoLinea = "D" };
            var fields = new nReglaArchivo().getColumnGridByArchivo(regla).ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "Fields", fields, true);
            var grid = new gridCreator().getGrid("frmSeqConsulta", "4800", action, "TIP_REGI ASC").ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "Grid", grid, true);
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
            //HISTORIALINQCAB
            cabecera.IDE_CONTRATO = Convert.ToInt32(ddl_contrato.SelectedItem.Value);
            //HISTORIALINQDET
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
        }
        private void MessageBox(String text)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "$('<div style=\"font-size:14px;text-align:center;\">" + text + "</div>').dialog({title:'Confirmación',modal:true,width:400,height:240,buttons: [{id: 'aceptar',text: 'Aceptar',icons: { primary: 'ui-icon-circle-check' },click: function () {$(this).dialog('close');}}]})", true);
        }
        #endregion METODOS
    }
}