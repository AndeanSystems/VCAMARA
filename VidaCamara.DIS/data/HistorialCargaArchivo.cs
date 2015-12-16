using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Negocio;

namespace VidaCamara.DIS.data
{
    public class HistorialCargaArchivo
    {
        public virtual int pa_file_InsertaHistorialCarga(Nullable<int> aux_IdArchivo, Nullable<int> aux_IdReglaArchivo, string aux_TipoLinea, Nullable<int> aux_NumeroLinea, Nullable<int> aux_CampoInicial, Nullable<int> aux_LargoCampo, string aux_ValorCampo, Nullable<int> aux_CumpleValidacion)
        {
            if (aux_TipoLinea.Equals("c"))
            {
                setGrabarCabecera(aux_IdArchivo, aux_IdReglaArchivo, aux_TipoLinea, aux_NumeroLinea, aux_ValorCampo,aux_CumpleValidacion);
            }
            var columna = aux_ValorCampo;
            return 1;
            /*var aux_IdArchivoParameter = aux_IdArchivo.HasValue ?
                new ObjectParameter("Aux_IdArchivo", aux_IdArchivo) :
                new ObjectParameter("Aux_IdArchivo", typeof(int));
    
            var aux_IdReglaArchivoParameter = aux_IdReglaArchivo.HasValue ?
                new ObjectParameter("Aux_IdReglaArchivo", aux_IdReglaArchivo) :
                new ObjectParameter("Aux_IdReglaArchivo", typeof(int));
    
            var aux_TipoLineaParameter = aux_TipoLinea != null ?
                new ObjectParameter("Aux_TipoLinea", aux_TipoLinea) :
                new ObjectParameter("Aux_TipoLinea", typeof(string));
    
            var aux_NumeroLineaParameter = aux_NumeroLinea.HasValue ?
                new ObjectParameter("Aux_NumeroLinea", aux_NumeroLinea) :
                new ObjectParameter("Aux_NumeroLinea", typeof(int));
    
            var aux_CampoInicialParameter = aux_CampoInicial.HasValue ?
                new ObjectParameter("Aux_CampoInicial", aux_CampoInicial) :
                new ObjectParameter("Aux_CampoInicial", typeof(int));
    
            var aux_LargoCampoParameter = aux_LargoCampo.HasValue ?
                new ObjectParameter("Aux_LargoCampo", aux_LargoCampo) :
                new ObjectParameter("Aux_LargoCampo", typeof(int));
    
            var aux_ValorCampoParameter = aux_ValorCampo != null ?
                new ObjectParameter("Aux_ValorCampo", aux_ValorCampo) :
                new ObjectParameter("Aux_ValorCampo", typeof(string));
    
            var aux_CumpleValidacionParameter = aux_CumpleValidacion.HasValue ?
                new ObjectParameter("Aux_CumpleValidacion", aux_CumpleValidacion) :
                new ObjectParameter("Aux_CumpleValidacion", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pa_file_InsertaHistorialCarga", aux_IdArchivoParameter, aux_IdReglaArchivoParameter, aux_TipoLineaParameter, aux_NumeroLineaParameter, aux_CampoInicialParameter, aux_LargoCampoParameter, aux_ValorCampoParameter, aux_CumpleValidacionParameter);*/
        }

        private void setGrabarCabecera(int? aux_IdArchivo, int? aux_IdReglaArchivo, string aux_TipoLinea, int? aux_NumeroLinea, string aux_ValorCampo, int? aux_CumpleValidacion)
        {
            var cargaArchivoCab = new HistorialCargaArchivo_LinCab()
            {
                IdEmpresa = 1,
                NumeroContrato = "201401",
                TIP_REGI = "PRE",
                FEC_ENVI_ARC = new DateTime(),
                COD_CSV = 2,
                NUM_CONT_LIC = 2,
                MONEDA = 1,
                PER_CONT = "123",
                TIP_PAGO = "SEMESTRAL",
                FEC_PAGO = new DateTime()

            };
        }

        internal long guardarCabecera(Dictionary<string, string> distionaryCabecera, HistorialCargaArchivo_LinCab cabecera)
        {
            long resp = 0;
            try
            {
                cabecera.FEC_ENVI_ARC = DateTime.Now;
                cabecera.COD_CSV = Convert.ToInt32(distionaryCabecera["10"].ToString());
                cabecera.NUM_CONT_LIC = Convert.ToInt32(distionaryCabecera["14"].ToString());
                cabecera.PER_CONT = distionaryCabecera["20"].ToString();
                cabecera.TIP_PAGO = distionaryCabecera["26"].ToString();
                cabecera.MONEDA = Convert.ToInt32(distionaryCabecera["29"].ToString());
                cabecera.FEC_PAGO = DateTime.Now;
                cabecera.CumpleValidacion = 1;
                cabecera.ESTADO = "A";
                cabecera.FEC_REG = DateTime.Now;

                using (var db = new DISEntities())
                {
                    db.HistorialCargaArchivo_LinCabs.Add(cabecera);
                    db.SaveChanges();
                    resp = cabecera.IdHistorialCargaArchivoLinCab;
                }
                return resp;
            }
            catch (Exception ex)
            {

                throw;
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
                    Tabladestino = i.TablaDestino
                };
                reglaLinea.Add(regla);
            }
            return reglaLinea;
        }
    }
}
