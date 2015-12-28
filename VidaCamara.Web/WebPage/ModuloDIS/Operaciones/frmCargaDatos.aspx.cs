using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Negocio;
using VidaCamara.SBS.Negocio;

namespace VidaCamara.Web.WebPage.ModuloDIS.Operaciones
{
    public partial class CargaDatos : System.Web.UI.Page
    {
        #region variables
        static int total;
        readonly bValidarAcceso _accesso = new bValidarAcceso();
        static string nombreArchivo = string.Empty;
        #endregion variables

        #region metodos control
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
                bTablaVC concepto = new bTablaVC();
                SetLLenadoContrato();
                concepto.SetEstablecerDataSourceConcepto(ddl_tipo_archivo, "17");
                concepto.SetEstablecerDataSourceConcepto(ddl_tipo_linea,"18");
            }
        }
        protected void btnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(5000);
                if (!fileUpload.HasFile) return;


                //david choque 27 12 2015
                //aki se verificara si el archivo ya fue cargado con el mismo nombre
                var archivo = new Archivo() {NombreArchivo = fileUpload.FileName.ToString()};
                var existe = new nArchivo().listExisteArchivo(archivo);
                //if (existe.Count > 0)
                //    MessageBox("Este archivo "+archivo.NombreArchivo +" ya fue cargado anteriormente, ");
                //else
                //fin david choque 27 12 2015
                var fileName = Server.MapPath(("~/Temp/Archivos/")) + fileUpload.FileName;
                fileUpload.SaveAs(fileName);
                var cargaLogica = new CargaLogica(fileName) { UsuarioModificacion = /*Session["usernameId"].ToString() */   "2"};
                cargaLogica.CargarArchivo(Convert.ToInt32(ddl_conrato1.SelectedValue));
                //david choque 27 12 2015
                nombreArchivo = fileUpload.FileName.ToString().ToUpper();
                setCargarReglaArchivo();
                //fin david choque 27 12 2015

                if ((cargaLogica.MensajeExcepcion != ""))
                {
                    var mensaje = "Se produjo un error al cargar el archivo. Se terminó la carga. " + cargaLogica.MensajeExcepcion;
                    MessageBox(mensaje.Replace(Environment.NewLine,""));
                }
                else if ((cargaLogica.ContadorErrores > 0))
                {
                    txt_registro_observado.Text = cargaLogica.ContadorErrores.ToString();
                    if ((cargaLogica.MensageError != String.Empty))
                    {
                        MessageBox(cargaLogica.MensageError.Replace(Environment.NewLine,""));
                    }
                    else if ((cargaLogica.Observacion != String.Empty))
                    {
                        MessageBox(cargaLogica.Observacion.Replace(Environment.NewLine, ""));
                    }
                    else
                    {
                        this.gvCargaExito.DataSource = cargaLogica.Resultado;
                        this.gvCargaExito.DataBind();
                    }
                }
                else
                {
                    string nombre;
                    if ((cargaLogica.NombreArchivo.Split('_')[0] == "NOMINA"))
                    {
                        nombre = "Nomina procesada Ok.";
                        if ((cargaLogica.MensageError != String.Empty))
                        {
                            nombre = (nombre + (", " + cargaLogica.MensageError));
                            MessageBox(nombre.Replace(Environment.NewLine,""));
                        }
                        else
                        {
                            MessageBox(nombre.Replace(Environment.NewLine,""));
                        }
                    }
                    else if ((cargaLogica.NombreArchivo.Split('_')[0] == "INOMINA"))
                    {
                        nombre = "Nomina procesada Ok.";
                        if ((cargaLogica.MensageError != String.Empty))
                        {
                            nombre = (nombre + (", " + cargaLogica.MensageError));
                            MessageBox(nombre.Replace(Environment.NewLine, ""));
                        }
                        else
                        {
                            MessageBox(nombre.Replace(Environment.NewLine, ""));
                        }
                    }
                    else
                    {
                        txt_registro_procesado.Text = cargaLogica.ContadorExito.ToString();
                        nombre = "Archivo procesado Ok.";
                        if ((cargaLogica.Observacion != String.Empty))
                        {
                            //david choque 27 12 2015
                            setMostrarRegistroCargados(cargaLogica.getInformacionCargada());
                            //fin david choque
                            nombre = (nombre + (", " + cargaLogica.Observacion));
                            MessageBox(nombre.ToString().Replace(Environment.NewLine,""));
                            if (cargaLogica.Observacion.Contains("alto"))
                            {
                                string respuesta = setEnviarCorreo(cargaLogica);
                            }
                        }
                        else if ((cargaLogica.MensageError != String.Empty))
                        {
                            nombre = (nombre + (", " + cargaLogica.MensageError));
                            MessageBox(nombre.Replace(Environment.NewLine, ""));
                        }
                        else
                        {
                            MessageBox(nombre.Replace(Environment.NewLine, ""));
                        }
                    }
                }
            }
            catch (Exception s)
            {
                MessageBox("ERROR =>" + s.Message.Replace("'", "-"));
            }
        }
        protected void ddl_tipo_linea_SelectedIndexChanged(object sender, EventArgs e)
        {
            setCargarReglaArchivo();
        }
        #endregion metodos control

        #region metodos usuario
        private string setEnviarCorreo(CargaLogica cargaLogica)
        {
            string FormatoCuerpo = "";
            Correo mail = new Correo()
            {
                Para = cargaLogica.Correo,
                CC = "",
                CCO = "",
                Asunto = ("Monto alto a reembolsar: " + cargaLogica.NombreArchivo),
                Cuerpo = ("Estimado Usuario (a),</BR> liquidacion cargada contiene monto superior al establecido, el dia " + DateTime.Now) + "</BR> " + cargaLogica.Observacion,
                Archivo = ""
            };
            return cargaLogica.EnviarCorreo(mail, FormatoCuerpo);
        }

        private void setCargarReglaArchivo()
        {
            txt_nombre_archivo_inf.Text = nombreArchivo;
            txt_tipo_archivo_inf.Text = ddl_tipo_archivo.SelectedItem.Text;
            var regla = new ReglaArchivo()
            {
                Archivo = ddl_tipo_archivo.SelectedItem.Value,
                TipoLinea = ddl_tipo_linea.SelectedItem.Value,
            };
            var negocio = new nReglaArchivo();
            gvReglaArchivo.DataSource = negocio.getListReglaArchivo(regla);
            gvReglaArchivo.DataBind();
        }

        private void setMostrarRegistroCargados(DataTable dataTable)
        {
            txt_nombre_archivo_det.Text = nombreArchivo;
            txt_tipo_informacion_det.Text = ddl_tipo_archivo.SelectedItem.Text;
            gvCargaExito.DataSource = dataTable;
            gvCargaExito.DataBind();
            multiTabs.ActiveViewIndex = 1;
        }

        private void SetLLenadoContrato()
        {
            var list = new VidaCamara.SBS.Utils.Utility().getContratoSys(out total);
            ddl_conrato1.DataSource = list;
            ddl_conrato1.DataTextField = "_des_Contrato";
            ddl_conrato1.DataValueField = "_ide_Contrato";
            ddl_conrato1.DataBind();
            ddl_conrato1.Items.Insert(0, new ListItem("Seleccione ----", "0"));
        }

        private void MessageBox(string text)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "$('<div style=\"font-size:14px;text-align:center;\">"+ text +"</div>').dialog({title:'Confirmación',modal:true,width:400,height:240,buttons: [{id: 'aceptar',text: 'Aceptar',icons: { primary: 'ui-icon-circle-check' },click: function () {$(this).dialog('close');}}]})", true);
        }
        #endregion metodos usuario
    }
}