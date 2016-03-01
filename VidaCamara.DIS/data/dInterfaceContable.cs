using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.data
{
    public class dInterfaceContable
    {
        public void createInterfaceContableDetalle(NOMINA nomina,EXACTUS_CABECERA_SIS cabecera, int asiento)
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

        public EXACTUS_CABECERA_SIS createInterfaceContableCabecera(NOMINA nomina)
        {
            try
            {
                var cabecera = new EXACTUS_CABECERA_SIS()
                {
                    IDE_CONTRATO = nomina.IDE_CONTRATO,
                    ArchivoId = (int)nomina.ArchivoId,
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
                                 join ta in db.TipoArchivos on a.TipoArchivoId equals ta.TipoArchivoId
                                 join n in db.NOMINAs on a.ArchivoId equals n.ArchivoId
                                 where x.IDE_CONTRATO == cabecera.IDE_CONTRATO &&
                                       x.FECHA >= cabecera.FECHA &&
                                       x.FECHA < fechaHasta &&
                                       //(ta.NombreTipoArchivo == archivo.NombreTipoArchivo || archivo.NombreTipoArchivo == "0") &&
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
                                CONTABILIDAD = item.x.CONTABILIDAD
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
    }
}
