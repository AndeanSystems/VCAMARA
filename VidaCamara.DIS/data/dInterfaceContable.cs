using System;
using System.Linq;
using VidaCamara.DIS.Modelo;

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
                    db.pa_create_cuenta_42_26_sis(cabecera.IDE_EXACTUS_CABECERA_SIS, nomina.ArchivoId, asiento);
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
                    ASIENTO = "SIN-"+DateTime.Now.ToString("yyyyMMdd"),
                    PAQUETE = "SIN",
                    TIPO_ASIENTO = "RS",
                    FECHA = DateTime.Now,
                    CONTABILIDAD = "C",
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
            catch (System.Exception ex)
            {
                throw(new System.Exception(ex.Message));
            }
        }
    }
}
