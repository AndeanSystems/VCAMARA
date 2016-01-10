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
                concepto.SetEstablecerDataSourceConcepto(ddl_tipo_archivo, "17");
            }
        }
        protected void btn_exportar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            var ruta =  descargarConsultaExcel();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ruta);
            Response.TransmitFile(ruta);
            Response.End();
        }

        private string descargarConsultaExcel()
        {
            var filters = new Object[3] {ddl_tipo_archivo.SelectedItem.Value,txt_fec_ini_o.Text,txt_fec_hasta_o.Text };
            return new nSegDescarga().descargarConsultaExcel(new CONTRATO_SYS() { IDE_CONTRATO =Convert.ToInt32(ddl_contrato.SelectedItem.Value)}, filters);
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object listSegDescarga(int jtStartIndex, int jtPageSize, string jtSorting, CONTRATO_SYS contrato,object[] filters)
        {
            var negocio = new nSegDescarga();
            return new { Result = "OK", Records = negocio.listSegDescarga(contrato, filters,jtStartIndex, jtPageSize, out total), TotalRecordCount = total };
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