using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VidaCamara.DIS.Negocio;
using VidaCamara.DIS.Modelo;
using VidaCamara.SBS.Negocio;

namespace VidaCamara.Web.WebPage.Mantenimiento
{
    public partial class frmReglaArchivo : System.Web.UI.Page
    {
        #region VARIBALES
        static int total;
        static bTablaVC concepto = new bTablaVC();
        #endregion VARIABLES
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetLLenadoContrato();
                concepto.SetEstablecerDataSourceConcepto(ddl_Archivo, "17");
                concepto.SetEstablecerDataSourceConcepto(ddl_tipo_linea, "18");
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listReglaArchivo(int jtStartIndex, int jtPageSize, string jtSorting, ReglaArchivo regla)
        {
            var listRegla = new nReglaArchivo().getListReglaArchivo(regla, jtStartIndex, jtPageSize, jtSorting, out total);
            return new { Result = "OK", Records = listRegla, TotalRecordCount = total };
        }

        protected void btn_nuevo_Click(object sender, ImageClickEventArgs e)
        {
            setGrabarActualizarReglaArchivo();
        }

        private void setGrabarActualizarReglaArchivo()
        {
            try
            {
                var regla = new ReglaArchivo()
                {
                    IdReglaArchivo = Convert.ToInt32(hdf_idRegla.Value),
                    Archivo = ddl_Archivo.SelectedItem.Value,
                    TipoLinea = ddl_tipo_linea.SelectedItem.Value,
                    CaracterInicial = Convert.ToInt32(txt_caracter_inicial.Text),
                    LargoCampo = Convert.ToInt32(txt_largo_Campo.Text),
                    TipoCampo = txt_tipo_Campo.Text,
                    InformacionCampo = txt_informacion.Text,
                    FormatoContenido = txt_formato.Text,
                    TipoValidacion = ddl_tipo_validacion.SelectedItem.Value,
                    ReglaValidacion = txt_regla_validacion.Text,
                    vigente = Convert.ToInt16(ddl_vigente.SelectedItem.Value),
                    NombreCampo = txt_nombre_Campo.Text,
                    TituloColumna = txt_titulo.Text,
                    NUM_CONT_LIC = Convert.ToInt32(ddl_contrato.SelectedItem.Value),
                };
                if (regla.IdReglaArchivo == 0)
                    new nReglaArchivo().grabarReglaArchivo(regla);
                else
                    new nReglaArchivo().actualizarReglaArchivo(regla);
                  
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void SetLLenadoContrato()
        {
            var list = new VidaCamara.SBS.Utils.Utility().getContratoSys(out total);
            ddl_contrato.DataSource = list;
            ddl_contrato.DataTextField = "_des_Contrato";
            ddl_contrato.DataValueField = "_nro_contrato";
            ddl_contrato.DataBind();
            ddl_contrato.Items.Insert(0, new ListItem("Seleccione ----", "0"));
        }
    }
}
