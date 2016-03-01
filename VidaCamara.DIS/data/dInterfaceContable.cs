using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;
using System.Data;

namespace VidaCamara.DIS.data
{
    public class dInterfaceContable
    {
        internal void createInterfaceContableDetalle(NOMINA nomina, EXACTUS_CABECERA_SIS cabecera, int asiento)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    db.pa_create_cuenta_42_26_sis(cabecera.IDE_EXACTUS_CABECERA_SIS,cabecera.ASIENTO,nomina.ArchivoId, asiento);
                }
            }
            catch (System.Exception ex)
            {
                throw(new System.Exception(ex.Message));
            }
        }

        internal EXACTUS_CABECERA_SIS createInterfaceContableCabecera(NOMINA nomina, Archivo archivo)
        {
            try
            {
                var cabecera = new EXACTUS_CABECERA_SIS()
                {
                    IDE_CONTRATO = nomina.IDE_CONTRATO,
                    ArchivoId = (int)nomina.ArchivoId,
                    TIPO_ARCHIVO = archivo.NombreArchivo.Split('_')[0].ToString(),
                    ASIENTO =  string.Format("SIN{0}{1}",DateTime.Now.ToString("yyMMdd"),nomina.TIP_MONE.ToString()),
                    PAQUETE = "SIN",
                    TIPO_ASIENTO = "RS",
                    FECHA = DateTime.Now,
                    CONTABILIDAD = "A",
                    NOTAS = "CONTABLE SIS",
                    ESTADO = 2,
                    ESTADO_TRANSFERENCIA = "C",
                    PERMITIR_DESCUADRADO = "N",
                    CONSERVAR_NUMERACION = "S",
                    ACTUALIZAR_CONSECUTIVO = "N",
                    FECHA_AUDITORIA = DateTime.Now,
                    FECHA_CREACION = DateTime.Now,
                    USUARIO_REGISTRO = System.Web.HttpContext.Current.Session["username"].ToString()
                };
                using (var db = new DISEntities())
                {
                    db.EXACTUS_CABECERA_SISs.Add(cabecera);
                    db.SaveChanges();
                }
                return cabecera;
            }
            catch (DbEntityValidationException ex)
            {
                var menssageException = string.Empty;
                foreach (var eve in ex.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        menssageException += string.Format("{0} - {1} <br>", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw(new System.Exception(ex.Message));
            }
        }

        internal List<HEXACTUS_DETALLE_SIS> listInterfaceContable(EXACTUS_CABECERA_SIS cabecera, TipoArchivo archivo, int moneda, int index, int size, out int total)
        {
            var listInterface = new List<HEXACTUS_DETALLE_SIS>();
            var formatoMoneda = System.Configuration.ConfigurationManager.AppSettings["Float"].ToString();
            try
            {
                using (var db = new DISEntities())
                {
                    var fechaHasta = cabecera.FECHA_CREACION.AddDays(1);
                    var query = (from xd in db.EXACTUS_DETALLE_SISs
                                 join x in db.EXACTUS_CABECERA_SISs on xd.IDE_EXACTUS_CABECERA_SIS equals x.IDE_EXACTUS_CABECERA_SIS
                                 join a in db.Archivos on x.ArchivoId equals a.ArchivoId
                                 join n in db.NOMINAs on a.ArchivoId equals n.ArchivoId
                                 where x.IDE_CONTRATO == cabecera.IDE_CONTRATO &&
                                       (x.ESTADO_TRANSFERENCIA == cabecera.ESTADO_TRANSFERENCIA || cabecera.ESTADO_TRANSFERENCIA == "0") &&
                                       x.FECHA >= cabecera.FECHA &&
                                       x.FECHA < fechaHasta &&
                                       (x.TIPO_ARCHIVO == archivo.NombreTipoArchivo || archivo.NombreTipoArchivo == "0") &&
                                       (n.TIP_MONE == moneda || moneda == 0)
                                 select new {xd,x}).ToList();
                    total = query.Count;
                    foreach (var item in query.Skip(index).Take(size))
                    {
                        var detalle = new HEXACTUS_DETALLE_SIS(){
                            FUENTE = item.xd.FUENTE,
                            REFERENCIA = item.xd.REFERENCIA,
                            CONTRIBUYENTE = item.xd.CONTRIBUYENTE,
                            CENTRO_COSTO = item.xd.CENTRO_COSTO,
                            CUENTA_CONTABLE = item.xd.CUENTA_CONTABLE,
                            DebitoSoles = string.Format(formatoMoneda,item.xd.MONTO_LOCAL>=0?item.xd.MONTO_LOCAL:0.00M),
                            CreditoSoles = string.Format(formatoMoneda,item.xd.MONTO_LOCAL < 0?item.xd.MONTO_LOCAL:0.00M),
                            DebitoDolar = string.Format(formatoMoneda,item.xd.MONTO_DOLAR>=0?item.xd.MONTO_DOLAR:0.00M),
                            CreditoDolar = string.Format(formatoMoneda,item.xd.MONTO_DOLAR < 0?item.xd.MONTO_DOLAR:0.00M),
                            MONTO_UNIDADES = item.xd.MONTO_UNIDADES,
                            EXACTUS_CABECERA_SIS = new EXACTUS_CABECERA_SIS(){
                                PAQUETE = item.x.PAQUETE,
                                ASIENTO = item.x.ASIENTO,
                                FECHA = item.x.FECHA,
                                TIPO_ASIENTO = item.x.TIPO_ASIENTO,
                                CONTABILIDAD = item.x.CONTABILIDAD,
                                ESTADO_TRANSFERENCIA = item.x.ESTADO_TRANSFERENCIA
                            }
                        };
                        listInterface.Add(detalle);
                    }
                }
                return listInterface;
            }
            catch (Exception ex)
            {
                throw(new Exception(ex.Message));
            }
        }

        internal List<EXACTUS_CABECERA_SIS> getExactusCabecera(EXACTUS_CABECERA_SIS cabecera, TipoArchivo tipoArchivo, int moneda)
        {
            var listCabecera = new List<EXACTUS_CABECERA_SIS>();
            try
            {
                using (var db = new DISEntities())
                {
                    var fechaHasta = cabecera.FECHA_CREACION.AddDays(1);
                    return (from x in db.EXACTUS_CABECERA_SISs 
                                 join a in db.Archivos on x.ArchivoId equals a.ArchivoId
                                 join n in db.NOMINAs on a.ArchivoId equals n.ArchivoId
                                 where x.IDE_CONTRATO == cabecera.IDE_CONTRATO &&
                                       x.ESTADO_TRANSFERENCIA == "C" &&
                                       x.FECHA >= cabecera.FECHA &&
                                       x.FECHA < fechaHasta &&
                                       (x.TIPO_ARCHIVO == tipoArchivo.NombreTipoArchivo || tipoArchivo.NombreTipoArchivo == "0") &&
                                       (n.TIP_MONE == moneda || moneda == 0)
                                 select x).ToList();
                }
            }
            catch (Exception ex)
            {
                throw(new Exception(ex.Message));
            }
        }

        internal bool createCabeceraInRemoteExactus(EXACTUS_CABECERA_SIS item)
        {
            var connectionString = ConfigurationManager.AppSettings.Get("CnnBDEX").ToString(); 
            try
            {
                var queryInsert = @"INSERT INTO VCAMARA.EXACTUS_ASIENTO_DE_DIARIO(ASIENTO,PAQUETE,TIPO_ASIENTO,FECHA,CONTABILIDAD,NOTAS,ESTADO,PERMITIR_DESCUADRADO,CONSERVAR_NUMERACION,ACTUALIZAR_CONSECUTIVO,FECHA_AUDITORIA)
                                    VALUES(@ASIENTO,@PAQUETE,@TIPO_ASIENTO,@FECHA,@CONTABILIDAD,@NOTAS,@ESTADO,@PERMITIR_DESCUADRADO,@CONSERVAR_NUMERACION,@ACTUALIZAR_CONSECUTIVO,@FECHA_AUDITORIA)";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand sqlcmd = connection.CreateCommand();
                    sqlcmd.CommandText = queryInsert;

                    sqlcmd.Parameters.Clear();
                    sqlcmd.Parameters.AddWithValue("@ASIENTO", SqlDbType.VarChar).Value = item.ASIENTO;
                    sqlcmd.Parameters.AddWithValue("@PAQUETE", SqlDbType.VarChar).Value = item.PAQUETE;
                    sqlcmd.Parameters.AddWithValue("@TIPO_ASIENTO", SqlDbType.VarChar).Value = item.TIPO_ASIENTO;
                    sqlcmd.Parameters.AddWithValue("@FECHA", SqlDbType.DateTime).Value = item.FECHA;
                    sqlcmd.Parameters.AddWithValue("@CONTABILIDAD", SqlDbType.VarChar).Value = item.CONTABILIDAD;
                    sqlcmd.Parameters.AddWithValue("@NOTAS", SqlDbType.VarChar).Value = item.NOTAS;
                    sqlcmd.Parameters.AddWithValue("@ESTADO", SqlDbType.Int).Value = item.ESTADO;
                    sqlcmd.Parameters.AddWithValue("@PERMITIR_DESCUADRADO", SqlDbType.Char).Value = item.PERMITIR_DESCUADRADO;
                    sqlcmd.Parameters.AddWithValue("@CONSERVAR_NUMERACION", SqlDbType.Char).Value = item.CONSERVAR_NUMERACION;
                    sqlcmd.Parameters.AddWithValue("@ACTUALIZAR_CONSECUTIVO", SqlDbType.Char).Value = item.ACTUALIZAR_CONSECUTIVO;
                    sqlcmd.Parameters.AddWithValue("@FECHA_AUDITORIA", SqlDbType.DateTime).Value = item.FECHA_AUDITORIA;

                    if (sqlcmd.ExecuteNonQuery() > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw(new Exception(ex.Message));
            }
        }

        internal void createDetalleInRemote(EXACTUS_CABECERA_SIS item)
        {
            var connectionString = ConfigurationManager.AppSettings.Get("CnnBDEX").ToString();
            var queryInsertDet = @"INSERT INTO VCAMARA.EXACTUS_DIARIO(ASIENTO,CONSECUTIVO,CENTRO_COSTO,CUENTA_CONTABLE,FUENTE,REFERENCIA,MONTO_LOCAL,MONTO_DOLAR,MONTO_UNIDADES,NIT,DIMENSION1,DIMENSION2,DIMENSION3,DIMENSION4)
                                                VALUES(@ASIENTO,@CONSECUTIVO,@CENTRO_COSTO,@CUENTA_CONTABLE,@FUENTE,@REFERENCIA,@MONTO_LOCAL,@MONTO_DOLAR,@MONTO_UNIDADES,@NIT,@DIMENSION1,@DIMENSION2,@DIMENSION3,@DIMENSION4)";
            try
            {
                using (var db = new DISEntities())
                {

                    var detalle = db.EXACTUS_DETALLE_SISs.Where(x => x.IDE_EXACTUS_CABECERA_SIS == item.IDE_EXACTUS_CABECERA_SIS).ToList();
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand sqlcmd = connection.CreateCommand();
                        sqlcmd.CommandText = queryInsertDet;
                        var consecutivo = 1;
                        foreach (var det in detalle)
                        {
                            sqlcmd.Parameters.Clear();
                            sqlcmd.Parameters.AddWithValue("@ASIENTO", SqlDbType.VarChar).Value = det.ASIENTO;
                            sqlcmd.Parameters.AddWithValue("@CONSECUTIVO", SqlDbType.Int).Value = consecutivo;
                            sqlcmd.Parameters.AddWithValue("@CENTRO_COSTO", SqlDbType.VarChar).Value = det.CENTRO_COSTO;
                            sqlcmd.Parameters.AddWithValue("@CUENTA_CONTABLE", SqlDbType.VarChar).Value = det.CUENTA_CONTABLE;
                            sqlcmd.Parameters.AddWithValue("@FUENTE", SqlDbType.VarChar).Value = det.FUENTE;
                            sqlcmd.Parameters.AddWithValue("@REFERENCIA", SqlDbType.VarChar).Value = det.REFERENCIA;
                            sqlcmd.Parameters.AddWithValue("@MONTO_LOCAL", SqlDbType.Decimal).Value = det.MONTO_LOCAL;
                            sqlcmd.Parameters.AddWithValue("@MONTO_DOLAR", SqlDbType.Decimal).Value = det.MONTO_DOLAR;
                            sqlcmd.Parameters.AddWithValue("@MONTO_UNIDADES", SqlDbType.Decimal).Value = det.MONTO_UNIDADES;
                            sqlcmd.Parameters.AddWithValue("@NIT", SqlDbType.Char).Value = det.NIT;
                            sqlcmd.Parameters.AddWithValue("@DIMENSION1", SqlDbType.VarChar).Value = det.DIMENSION1;
                            sqlcmd.Parameters.AddWithValue("@DIMENSION2", SqlDbType.VarChar).Value = det.DIMENSION2;
                            sqlcmd.Parameters.AddWithValue("@DIMENSION3", SqlDbType.VarChar).Value = det.DIMENSION3;
                            sqlcmd.Parameters.AddWithValue("@DIMENSION4", SqlDbType.VarChar).Value = det.DIMENSION4;
                            sqlcmd.ExecuteNonQuery();
                            consecutivo++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw(new Exception(ex.Message));
            }
        }

        internal void actualizarEstadoTransferido(EXACTUS_CABECERA_SIS item)
        {
            try
            {
                using (var db = new DISEntities())
                {
                    var query = db.EXACTUS_CABECERA_SISs.Find(item.IDE_EXACTUS_CABECERA_SIS);
                    query.ESTADO_TRANSFERENCIA = "T";//TRANSFERIDO
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw(new Exception(ex.Message));
            }
        }
    }
}
