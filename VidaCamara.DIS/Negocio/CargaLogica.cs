using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.Negocio
{
    public partial class CargaLogica
    {
        public string FullNombreArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public ObjectResult Resultado { get; set; }
        public bool ValidaNombre { get; set; }
        public StringCollection TipoLinea { get; set; }
        public int IdArchivo { get; set; }
        public int ContadorErrores { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool Vigente { get; set; }
        public string ExtensionArchivo { get; set; }
        public List<int> LargoLinea { get; set; }
        public string MensageError { get; set; }
        public string MensajeExcepcion { get; set; }
        public string Observacion { get; set; }
        public int Estado { get; set; }
        public string Correo { get; set; }
        private string CampoActual { get; set; }
        private long CodigoCabecera { get; set; }

        private HistorialCargaArchivo_LinCab _lineaCabecera = new HistorialCargaArchivo_LinCab();
        private List<HistorialCargaArchivo_LinDet> _lineaDetalles = new List<HistorialCargaArchivo_LinDet>();
        private Dictionary<string, List<Regla>> _reglasLineaPorTipo = new Dictionary<string, List<Regla>>();

        public CargaLogica(string archivo)
        {
            FullNombreArchivo = archivo;
            NombreArchivo = Path.GetFileName(archivo);
            string nombreArchivo = null;
            string extensionArchivo = null;
            nombreArchivo = Path.GetFileNameWithoutExtension(NombreArchivo);
            extensionArchivo = Path.GetExtension(NombreArchivo);

            if (extensionArchivo.Contains("CAM"))
            {
                if (NombreArchivo.Split('_')[0] == "NOMINA" | NombreArchivo.Split('_')[0] == "INOMINA")
                {
                    extensionArchivo = ".CSV";
                    NombreArchivo = nombreArchivo + extensionArchivo;
                }
                ValidaNombre = ValidaNombreArchivo(NombreArchivo);
            }
            else
            {
                ValidaNombre = false;
            }
            MensajeExcepcion = "";
        }

        public void CargarArchivo(int contratoId)
        {
            if (!NombreArchivo.Distinct().Any()) return;

            if (ValidaNombre)
            {
                ObtieneTipoLinea(NombreArchivo.Split('_')[0]);
                var idestado = 0;
                idestado = LeeArchivo(NombreArchivo.Split('_')[0], TipoLinea, contratoId);
                if (idestado > 2)
                {
                    MensageError = "No se puede procesar archivo por estar aprobado/pagado";
                    ContadorErrores = ContadorErrores + 1;
                }
                else
                {
                    if (idestado == 2)
                    {
                        MensageError = "No se puede procesar archivo por tener Checklists";
                        ContadorErrores = ContadorErrores + 1;
                    }
                }
            }
            else
            {
                MensageError = "Nombre de archivo no cumple formato";
                ContadorErrores = ContadorErrores + 1;

                NombreArchivo = string.Empty;
            }
        }

        public bool ValidaNombreArchivo(string nombreArchivo)
        {
            StringCollection archivo;
            string tipoArchivo = null;
            using (var context = new DISEntities())
            {
                var resultado = (object)context.pa_file_ObtieneTipoArchivos();
                archivo = ObtieneColeccion(resultado);
            }
            tipoArchivo = nombreArchivo.Split('_')[0];

            return archivo.Contains(tipoArchivo);
        }

        public void ObtieneTipoLinea(string tipoArchivo)
        {
            using (var context = new DISEntities())
            {
                var resultado = (object)context.pa_file_ObtienePrimerCaracterLineaPorTipoArchivo(tipoArchivo);
                TipoLinea = ObtieneColeccion(resultado);
            }
        }

        public string[] LineaArchivo()
        {
            ContadorErrores = 0;
            var sr = new StreamReader(FullNombreArchivo,
                System.Text.Encoding.GetEncoding(437));

            var texto = sr.ReadToEnd();
            var text = texto.Split('\n');
            sr.Close();
            return text;
        }

        public int LeeArchivo(string tipoArchivo, StringCollection tipoLinea, int contratoId)
        {
            var text = LineaArchivo();

            //Consultar en tabla el estado del archivo
            //entregara 0 si no existe
            using (var context = new DISEntities())
            {
                var estadoArchivo = context.pa_file_ConsultaEstadoArchivo(NombreArchivo).FirstOrDefault();
                if (estadoArchivo != null) Estado = estadoArchivo.Value;
            }
            InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "Consulta estado archivo",
                "pa_file_ConsultaEstadoArchivo '" + NombreArchivo + "'", 0);

            //valor anterior = 3
            //Si el valor es menor que 2, significa 2 cosas:
            //1.- que la nomina no esta aprobada y puede ser cargado nuevos archivos.
            //2.- que no hay checklist sobre el o los cuspp de la liquidacion
            if (Estado < 2)
            {
                //Insertar en tabla
                if (Estado == 1)
                {
                    MensageError = "Archivo ya cargado previamente";
                }
                using (var context = new DISEntities())
                {
                    var archivo = context.pa_file_InsertaReferenciaArchivo(NombreArchivo, UsuarioModificacion).FirstOrDefault();
                    if (archivo != null) IdArchivo = archivo.Value;
                }
                //InsertaAuditoria(Me.UsuarioModificacion, "Inserta Referencia Archivo", "pa_file_InsertaReferenciaArchivo '" + Me.NombreArchivo + "'", Me.idArchivo)

                for (var indexLinea = 0; indexLinea <= text.Length - 1; indexLinea++)
                {
                    var caracterInicial = Mid(text[indexLinea].Trim(), 0, 1);
                    var carterInicialNumer = 0;
                    if (int.TryParse(caracterInicial, out carterInicialNumer))
                    {
                        caracterInicial = "*";
                    }
                    if (tipoLinea.Contains(caracterInicial))
                    {
                        using (var context = new DISEntities())
                        {
                            if (!_reglasLineaPorTipo.ContainsKey(caracterInicial))
                            {
                                _reglasLineaPorTipo.Add(caracterInicial,
                                    ObtieneReglaLinea(context.pa_file_ObtieneReglasArchivoPorLinea(tipoArchivo,
                                        caracterInicial)));
                            }
                        }
                        //InsertaAuditoria(Me.UsuarioModificacion, "Obtiene Regla de archivo por línea", "pa_file_ObtieneReglasArchivoPorLinea '" + tipoLinea + "', " + CaracterInicial, Me.idArchivo)
                        try
                        {
                            var propertyValues = new Dictionary<string, object>();
                            var exitoLinea = 1;

                            foreach (var regla in _reglasLineaPorTipo[caracterInicial])
                            {
                                try
                                {
                                    exitoLinea &= EvaluarRegla(tipoArchivo, regla, text, indexLinea, propertyValues);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }

                            GrabarFilaArchivo(caracterInicial, IdArchivo, indexLinea + 1, propertyValues, contratoId, exitoLinea);
                        }
                        catch (Exception ex)
                        {
                            MensajeExcepcion = ex.Message;
                            return 0;
                        }
                    }
                    else
                    {
                        if (text[indexLinea].Trim().Any())
                        {
                            using (var context = new DISEntities())
                            {
                                context.pa_file_InsertaHistorialCarga(IdArchivo, 451, "#", indexLinea + 1, 1,
                                    text[indexLinea].Trim().Count(), text[indexLinea], 0);
                            }
                            //InsertaAuditoria(Me.UsuarioModificacion, "Inserta Historial de CargaLogica", "pa_file_InsertaHistorialCarga 451" + ", " + "'#'" + ", " + (x + 1).ToString() + ", " + "1" + ", " + text[x].Trim()().Count().ToString() + ", '" + Me.campoActual + "', " + "0", Me.idArchivo)
                            ContadorErrores = ContadorErrores + 1;
                        }
                    }
                }

                //aca se debe realizar el bolcado de archivo sin errores
                try
                {
                    TraspasaArchivo(tipoArchivo);

                    ProcesarErrores(tipoArchivo);
                }
                catch (Exception ex)
                {
                    Observacion = ex.Message + "// TraspasaArchivo...!";
                }
            }
            else
            {
                Observacion = "ya está aprobado";
                return Estado;
            }
            return 0;
        }

        private void GrabarFilaArchivo(string tipoLinea, int archivoId, int nroLinea, Dictionary<string, object> propertyValues, int contratoId, int exitoLinea)
        {
            if (tipoLinea == "C")
            {
                PopulateType(_lineaCabecera, propertyValues);
                _lineaCabecera.ArchivoId = archivoId;
                _lineaCabecera.USU_REG = System.Web.HttpContext.Current.Session["username"].ToString();
                _lineaCabecera.FEC_REG = DateTime.Now;
                _lineaCabecera.ESTADO = "A";
                _lineaCabecera.CumpleValidacion = exitoLinea;
                _lineaCabecera.IDE_CONTRATO = contratoId;

                GrabarLineaCabecera();
            }

            if (tipoLinea == "D")
            {
                var detalle = new HistorialCargaArchivo_LinDet();
                PopulateType(detalle, propertyValues);
                detalle.IdHistorialCargaArchivoLinCab = CodigoCabecera;
                detalle.FechaInsert = DateTime.Now;
                detalle.CumpleValidacion = exitoLinea;
                detalle.TipoLinea = tipoLinea;
                detalle.NumeroLinea = nroLinea;
                
                PopulateType(detalle, propertyValues);
                _lineaDetalles.Add(detalle);
            }

            if (tipoLinea == "T")
            {
                GrabarLineaDetalles();
                _lineaDetalles = new List<HistorialCargaArchivo_LinDet>();
            }
        }

        private void GrabarLineaCabecera()
        {
            try
            {
                using (var db = new DISEntities())
                {
                    db.HistorialCargaArchivo_LinCabs.Add(_lineaCabecera);
                    db.SaveChanges();
                    CodigoCabecera = _lineaCabecera.IdHistorialCargaArchivoLinCab;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine(
                        "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        private void GrabarLineaDetalles()
        {
            try
            {
                using (var db = new DISEntities())
                {
                    db.HistorialCargaArchivo_LinDets.AddRange(_lineaDetalles);
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine(
                        "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        private void ProcesarErrores(string tipoArchivo)
        {
            if (ValidacionesArchivo(tipoArchivo, 2) == false)
            {
                using (var context = new DISEntities())
                {
                    Resultado = context.pa_file_ObtieneErrorArchivo(IdArchivo);
                    var result = context.pa_file_ObtieneErrorArchivo(IdArchivo);

                    var nombre = "";
                    var largo = 0;
                    foreach (var datoLoopVariable in result)
                    {
                        var dato = datoLoopVariable;
                        if (dato.NumeroLinea.Value > 0)
                        {
                            nombre = dato.NombreArchivo;
                            largo = dato.LargoCampo.Value;
                        }
                    }
                    if (nombre != string.Empty & largo != null)
                    {
                        //If largo = 25 Then
                        var valor1 = context.pa_valida_CodigoTransferenciaNomina(nombre, IdArchivo, largo);
                        var resultado = 0;
                        resultado = valor1.FirstOrDefault().Value;
                        if (resultado == 0)
                        {
                            Resultado = null;
                            Observacion =
                                "No existe liquidación, debe cargar liquidación y despúes la nómina";
                            //End If
                        }
                    }
                }
            }

            if (ContadorErrores == 0)
            {
                using (var context = new DISEntities())
                {
                    var cantidad = context.pa_file_CantidadRegistroArchivo(IdArchivo);
                    var cant = 0;
                    cant = cantidad.FirstOrDefault().Value;
                    Observacion = "cantidad de registros cargados: " + cant;
                    InsertaAuditoria(Convert.ToInt32(UsuarioModificacion),
                        "Archivo cargado correctamente, cantidad de registros cargados: " + cant,
                        NombreArchivo, IdArchivo);
                }
            }

            //esto válida que los montos por cuspp no sean mayor a lo establecido
            //en la entidad: negocio.MontoAlto
            if (NombreArchivo.Substring(0, 3).ToLower() == "liq")
            {
                using (var context = new DISEntities())
                {
                    var monto = context.pa_valida_MontoAlto(IdArchivo,
                        Convert.ToInt32(UsuarioModificacion));
                    string montoAlto = null;
                    montoAlto = monto.ToString();
                    if (montoAlto == "1")
                    {
                        dynamic monto1 = context.pa_devuelveresultado(IdArchivo);
                        var correo = "";
                        foreach (var registroLoopVariable in monto1)
                        {
                            var registro = registroLoopVariable;
                            correo = registro.correo;
                            Observacion = Observacion + "\\n Monto alto cargado al CUSPP: " +
                                          registro.Cuspp + ", por valor = " + registro.Valor.ToString;
                            InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), Observacion,
                                NombreArchivo, IdArchivo);
                        }
                        Correo = correo;
                    }
                }
            }
        }

        public string EnviarCorreo(string para, string cc, string cco, string asunto, string cuerpo, string formatoCuerpo, string archivos)
        {
            using (var repositorio = new DISEntities())
            {
                return repositorio.pa_envioCorreo_Procesos(para, cc, cco, asunto, cuerpo, formatoCuerpo, archivos).ToString();
            }
        }

        private static string Mid(string text, int startIndex, int length)
        {
            return text.Substring(startIndex, Math.Min(text.Length - startIndex, length));
        }

        private void InsertaAuditoria(int idUsuario, string descripcion, string comando, int idarchivo)
        {
            using (var context = new DISEntities())
            {
                context.pa_audit_InsertaBitacora(idUsuario, descripcion, comando, idarchivo);
            }
        }

        private void TraspasaArchivo(string tipoArchivo)
        {
            try
            {
                var directorioArchivo = System.Configuration.ConfigurationManager.AppSettings["RutaArchivos"] +
                                        tipoArchivo + "\\";
                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), directorioArchivo, NombreArchivo,
                    IdArchivo);

                if (!Directory.Exists(directorioArchivo))
                {
                    Directory.CreateDirectory(directorioArchivo);
                }
                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "despues de validar directorio",
                    NombreArchivo, IdArchivo);

                var rutaArchivos = directorioArchivo + NombreArchivo;

                if (File.Exists(rutaArchivos))
                {
                    File.Delete(rutaArchivos);
                }
                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion),
                    "despues de validar si el archivo existe en el directorio: " + rutaArchivos, NombreArchivo,
                    IdArchivo);

                File.Copy(FullNombreArchivo, rutaArchivos);
                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "despues de copiar archivo en directorio",
                    NombreArchivo, IdArchivo);

                if (tipoArchivo == "PRIMAPAG")
                {
                    var nombre = FullNombreArchivo.Replace("PRIMAPAG", "PRIMPAGA");
                    dynamic nombreArch = Path.GetFileName(nombre);

                    directorioArchivo = System.Configuration.ConfigurationManager.AppSettings["RutaArchivos"] +
                                        "PRIMPAGA\\";

                    if (!Directory.Exists(directorioArchivo))
                    {
                        Directory.CreateDirectory(directorioArchivo);
                    }

                    rutaArchivos = directorioArchivo + nombreArch;

                    if (File.Exists(rutaArchivos))
                    {
                        File.Delete(rutaArchivos);
                    }

                    ContadorErrores = 0;

                    using (var writer = new StreamWriter(rutaArchivos))
                    {
                        using (var sr = new StreamReader(FullNombreArchivo))
                        {
                            var texto = sr.ReadLine() + "\r";

                            while ((sr.Peek() >= 0))
                            {
                                if (texto.Contains("PAP"))
                                {
                                    var linea = texto.Substring(0, 1) + "PRE" + texto.Substring(4);
                                    texto = linea;
                                }
                                writer.Write(texto);
                                texto = sr.ReadLine() + "\r";
                            }
                            if (texto != string.Empty)
                            {
                                writer.Write(texto);
                            }
                        }
                    }
                    //Insertar en tabla
                    using (var context = new DISEntities())
                    {
                        var m = context.pa_file_InsertaReferenciaArchivo(nombreArch, UsuarioModificacion);
                        IdArchivo = m;

                        InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "Se genero archivo(PrimPaga) en Servidor",
                            nombreArch, IdArchivo);
                    }
                }

                File.Delete(FullNombreArchivo);

                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "Se guarda archivo en Servidor",
                    NombreArchivo, IdArchivo);

                return;
            }
            catch (Exception ex)
            {
                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), ex.Message, NombreArchivo, IdArchivo);
                Observacion = ex.Message;
                return;
            }
        }

        public List<Regla> ObtieneReglaLinea(ObjectResult<pa_file_ObtieneReglasArchivoPorLinea_Result> dt)
        {
            var reglaLinea = new List<Regla>();
            foreach (var iLoopVariable in dt)
            {
                var i = iLoopVariable;
                var regla = new Regla
                {
                    idRegla = i.IdReglaArchivo,
                    CaracterInicial = i.CaracterInicial.Value,
                    LargoCampo = i.LargoCampo.Value,
                    TipoCampo = i.TipoCampo,
                    TipoValidacion = i.TipoValidacion,
                    ReglaValidacion = i.ReglaValidacion,
                    Tabladestino = i.TablaDestino,
                    NombreCampo = i.NombreCampo
                };
                reglaLinea.Add(regla);
            }
            return reglaLinea;
        }

        private StringCollection ObtieneColeccion(object dt)
        {
            var coleccion = new StringCollection();

            foreach (var iLoopVariable in (IEnumerable)dt)
            {
                var i = iLoopVariable;
                coleccion.Add(i == null ? "" : i.ToString());
            }
            return coleccion;
        }
        
        private void PopulateType(object obj, Dictionary<String, Object> defaultValues)
        {
            foreach (var defaultValue in defaultValues)
            {
                var prop = obj.GetType().GetProperty(defaultValue.Key.ToString().Trim(), BindingFlags.Public | BindingFlags.Instance);
                if(null != prop && prop.CanWrite)
                {
                    var type = prop.PropertyType;
                    var underlyingType = Nullable.GetUnderlyingType(type);
                    var returnType = underlyingType ?? type;
                    var typeCode = Type.GetTypeCode(returnType);

                    var value = defaultValue.Value;

                    try
                    {
                        switch (typeCode)
                        {
                            case TypeCode.Int32:
                                prop.SetValue(obj, Convert.ToInt32(value), null);
                                break;
                            case TypeCode.DateTime:
                                if ((string) value == "00000000")
                                {
                                    prop.SetValue(obj, null, null);
                                    break;
                                }

                                var dateTimeValue = DateTime.ParseExact(value.ToString(), "yyyyMMdd",
                                    System.Globalization.CultureInfo.InvariantCulture);
                                prop.SetValue(obj, dateTimeValue, null);
                                break;
                            default:
                                prop.SetValue(obj, value, null);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
