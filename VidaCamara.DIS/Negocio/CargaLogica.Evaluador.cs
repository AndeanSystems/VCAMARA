using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.Negocio
{
    public partial class CargaLogica
    {
        private bool ValidacionesArchivo(string tipoArchivo, int tipoRegla)
        {
            var exitoValidacion = 0;

            try
            {
                string regla = null;
                using (var context = new DISEntities())
                {
                    if (tipoRegla == 2 & ContadorErrores == 0)
                    {
                        var valorRetorno = context.pa_file_ArchivoValido(IdArchivo);
                        InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "Se valida archivo",
                            "pa_file_ArchivoValido", IdArchivo);
                        exitoValidacion = int.Parse(valorRetorno.ToString());
                        if (exitoValidacion == 0)
                        {
                            ContadorErrores = ContadorErrores + 1;
                            MensageError = "Error en Cuadratura de primas";
                            InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), MensageError, NombreArchivo,
                                IdArchivo);
                            return false;
                        }
                    }
                    var resultado = context.pa_file_ObtieneReglavalidacionArchivo(tipoArchivo, tipoRegla);
                    regla = resultado.FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(regla))
                {
                    string[] conditionString = null;
                    regla = regla.Replace("@NombreArchivo", NombreArchivo);
                    regla = regla.Replace("@IdArchivo", IdArchivo.ToString());
                    conditionString = regla.Split(';');

                    switch (conditionString[0].Substring(0, 3))
                    {
                        case "SP#":
                            if (ExecSpBool(conditionString[0].Substring(3)))
                            {
                                exitoValidacion = 1;
                            }
                            else
                            {
                                exitoValidacion = 0;
                            }
                            InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), conditionString[0].Substring(3),
                                conditionString[0].Substring(3), IdArchivo);
                            break;
                    }

                    if (exitoValidacion == 1)
                    {
                        switch (conditionString[1].Substring(0, 3))
                        {
                            case "SP#":
                                if (ExecSpBool(conditionString[1].Substring(3)))
                                {
                                    exitoValidacion = 1;
                                }
                                else
                                {
                                    exitoValidacion = 0;
                                }
                                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), conditionString[1].Substring(3),
                                    conditionString[1].Substring(3), IdArchivo);
                                break;
                        }
                    }
                    else
                    {
                        switch (conditionString[2].Substring(0, 3))
                        {
                            case "SP#":
                                if (ExecSpBool(conditionString[2].Substring(3)))
                                {
                                    exitoValidacion = 1;
                                }
                                else
                                {
                                    exitoValidacion = 0;
                                }
                                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), conditionString[2].Substring(3),
                                    conditionString[2].Substring(3), IdArchivo);
                                break;
                        }
                    }

                    return exitoValidacion == 1;
                }
                return true;
            }
            catch (Exception ex)
            {
                Observacion = ex.Message;
                return false;
            }
        }

        private int EvaluarRegla(string tipoArchivo, Regla regla, string[] text, int indexLinea,
            Dictionary<string, object> propertyValues)
        {
            CampoActual = Mid(text[indexLinea].Trim(), regla.CaracterInicial - 1, regla.LargoCampo);
            var exitoLinea = 0;
            string valor = null;
            string[] inString = null;
            var j = 0;

            if (regla.NombreCampo != null) propertyValues.Add(regla.NombreCampo, CampoActual);

            switch (regla.TipoValidacion)
            {
                case "EQUAL":
                    exitoLinea = EvaluarEqual(regla, exitoLinea);
                    break;

                case "BOOL_SP":
                    exitoLinea = EvaluarBoolSp(tipoArchivo, regla, indexLinea, exitoLinea);
                    break;

                case "BOOL_IF_SP":
                    exitoLinea = EvaluarBoolIfSp(regla, indexLinea, exitoLinea);
                    break;

                case "IN_QUERY":
                    exitoLinea = EvaluarInQuery(regla, exitoLinea);
                    break;

                case "IN":
                    exitoLinea = EvaluarIn(regla, text, indexLinea, exitoLinea);
                    break;

                case "FILLER":
                    exitoLinea = EvaluarFiller(regla, exitoLinea);
                    break;

                case "":
                    exitoLinea = 1;
                    break;
            }

            if (exitoLinea == 0)
            {
                var fail = true;
            }

            return exitoLinea;
        }


        private int EvaluarFiller(Regla r, int exitoLinea)
        {
            if (CampoActual == "".PadLeft(r.LargoCampo))
            {
                exitoLinea = 1;
            }
            else
            {
                ContadorErrores = ContadorErrores + 1;
            }
            return exitoLinea;
        }

        private int EvaluarIn(Regla r, string[] text, int x, int exitoLinea)
        {
            string[] inString;
            int j;
            inString = r.ReglaValidacion.Split(',');
            var existe = 0;
            for (j = 0; j <= inString.Count() - 1; j++)
            {
                if (text[x].Trim().Substring(r.CaracterInicial - 1, r.LargoCampo) == inString[j])
                {
                    existe = 1;
                }
            }

            if (existe == 1)
            {
                exitoLinea = 1;
            }
            else
            {
                ContadorErrores = ContadorErrores + 1;
            }
            return exitoLinea;
        }

        private int EvaluarInQuery(Regla r, int exitoLinea)
        {
            StringCollection resultadoValor;
            resultadoValor = ExecQuery(r.ReglaValidacion);
            var index = (r.ReglaValidacion.IndexOf("FROM ")+5);
            var tableValidacion = r.ReglaValidacion.Substring(index,(r.ReglaValidacion.Length-index)).ToString();
            if (resultadoValor.Contains(CampoActual))
            {
                exitoLinea = 1;
            }
            else
            {
                MensageError = "No se encontró el valor: " + CampoActual + " en tabla " + tableValidacion;
                ContadorErrores = ContadorErrores + 1;
            }
            return exitoLinea;
        }

        private int EvaluarBoolIfSp(Regla r, int x, int exitoLinea)
        {
            string valor;
            int res1;
            int res2;
            StringCollection resultadoValor;
            string[] inString;
            int j;
            string[] conditionString = null;
            valor = r.ReglaValidacion;
            valor = valor.Replace("@valor", "'" + CampoActual + "'");
            valor = valor.Replace("@IdArchivo", IdArchivo.ToString());
            valor = valor.Replace("@NumeroLinea", (x + 1).ToString());
            valor = valor.Replace("@CampoInicial", r.CaracterInicial.ToString());
            valor = valor.Replace("@LargoCampo", r.LargoCampo.ToString());
            conditionString = valor.Split(';');

            //InsertaAuditoria(Me.UsuarioModificacion, "Validación BOOL_IF_SP", valor, Me.idArchivo)

            switch (conditionString[0])
            {
                case "AND":
                case "OR":
                    res1 = 0;
                    res2 = 0;
                    switch (conditionString[1].Substring(0, 3))
                    {
                        case "SP#":
                            if (ExecSpBool(conditionString[1].Substring(3)))
                            {
                                res1 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP SP#", ConditionString[1].Substring(3), Me.idArchivo)
                        case "EQ#":
                            if (CampoActual == conditionString[1].Substring(3))
                            {
                                res1 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP EQ#", Me.campoActual + "=" + ConditionString[1].Substring(3), Me.idArchivo)
                        case "IQ#":
                            resultadoValor = ExecQuery(conditionString[1].Substring(3));
                            if (resultadoValor.Contains(CampoActual))
                            {
                                res1 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IQ#", Me.campoActual + "IN(" + ConditionString[1].Substring(3) + ")", Me.idArchivo)
                        case "IN#":
                            inString = conditionString[1].Substring(3).Split(',');
                            for (j = 0; j <= inString.Count() - 1; j++)
                            {
                                if (CampoActual == inString[j])
                                {
                                    res1 = 1;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }

                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IN#", Me.campoActual + "IN(" + ConditionString[1].Substring(3) + ")", Me.idArchivo)
                    }
                    switch (conditionString[2].Substring(0, 3))
                    {
                        case "SP#":
                            if (ExecSpBool(conditionString[2].Substring(3)))
                            {
                                res2 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP SP#", ConditionString[2].Substring(3), Me.idArchivo)
                        case "EQ#":
                            if (CampoActual == conditionString[2].Substring(3))
                            {
                                res2 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP EQ#", Me.campoActual + "=" + ConditionString[2].Substring(3), Me.idArchivo)
                        case "IQ#":
                            resultadoValor = ExecQuery(conditionString[2].Substring(3));
                            if (resultadoValor.Contains(CampoActual))
                            {
                                res2 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IQ#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                        case "IN#":
                            inString = conditionString[1].Substring(3).Split(',');
                            for (j = 0; j <= inString.Count() - 1; j++)
                            {
                                if (CampoActual == inString[j])
                                {
                                    res2 = 1;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }

                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IN#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                    }

                    if (conditionString[0] == "AND")
                    {
                        if (res1 == 1 & res2 == 1)
                        {
                            exitoLinea = 1;
                        }
                        else
                        {
                            ContadorErrores = ContadorErrores + 1;
                        }
                    }
                    else
                    {
                        if (res1 == 1 | res2 == 1)
                        {
                            exitoLinea = 1;
                        }
                        else
                        {
                            ContadorErrores = ContadorErrores + 1;
                        }
                    }
                    break;
                case "IF":
                    switch (conditionString[1].Substring(0, 3))
                    {
                        case "SP#":
                            if (ExecSpBool(conditionString[1].Substring(3)))
                            {
                                switch (conditionString[2].Substring(0, 3))
                                {
                                    case "SP#":
                                        if (ExecSpBool(conditionString[2].Substring(3)))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            ContadorErrores = ContadorErrores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP SP#", ConditionString[2].Substring(3), Me.idArchivo)
                                    case "EQ#":
                                        if (
                                            CampoActual.Equals(
                                                conditionString[2].Substring(3)))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            ContadorErrores = ContadorErrores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP EQ#", Me.campoActual + "=" + ConditionString[2].Substring(3), Me.idArchivo)
                                    case "IQ#":
                                        resultadoValor =
                                            ExecQuery(conditionString[2].Substring(3));
                                        if (resultadoValor.Contains(CampoActual))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            ContadorErrores = ContadorErrores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IQ#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                                    case "IN#":
                                        inString = conditionString[2].Substring(3)
                                            .Split(',');
                                        for (j = 0; j <= inString.Count() - 1; j++)
                                        {
                                            if (CampoActual == inString[j])
                                            {
                                                exitoLinea = 1;
                                                break;
                                                // TODO: might not be correct. Was : Exit For
                                            }
                                        }

                                        if (exitoLinea == 0)
                                        {
                                            ContadorErrores = ContadorErrores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IN#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                                }
                            }
                            else
                            {
                                switch (conditionString[3].Substring(0, 3))
                                {
                                    case "SP#":
                                        if (ExecSpBool(conditionString[3].Substring(3)))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            ContadorErrores = ContadorErrores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP SP#", ConditionString[2].Substring(3), Me.idArchivo)
                                    case "EQ#":
                                        if (CampoActual ==
                                            conditionString[3].Substring(3))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            ContadorErrores = ContadorErrores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP EQ#", Me.campoActual + "=" + ConditionString[2].Substring(3), Me.idArchivo)
                                    case "IQ#":
                                        resultadoValor =
                                            ExecQuery(conditionString[3].Substring(3));
                                        if (resultadoValor.Contains(CampoActual))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            ContadorErrores = ContadorErrores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IQ#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                                    case "IN#":
                                        inString = conditionString[3].Substring(3)
                                            .Split(',');
                                        for (j = 0; j <= inString.Count() - 1; j++)
                                        {
                                            if (CampoActual == inString[j])
                                            {
                                                exitoLinea = 1;
                                                break;
                                                // TODO: might not be correct. Was : Exit For
                                            }
                                        }

                                        if (exitoLinea == 0)
                                        {
                                            ContadorErrores = ContadorErrores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IN#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                                }
                            }
                            break;
                        case "EQ#":
                            if (CampoActual.Equals(conditionString[2].Substring(3)))
                            {
                                exitoLinea = 1;
                            }
                            else
                            {
                                ContadorErrores = ContadorErrores + 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP EQ#", Me.campoActual + "=" + ConditionString[2].Substring(3), Me.idArchivo)
                    }
                    break;
            }
            return exitoLinea;
        }

        private int EvaluarBoolSp(string tipoArchivo, Regla regla, int x,int exitoLinea)
        {
            StringCollection resultadoValor;
            var valor = regla.ReglaValidacion;

            if (valor.Contains("pa_valida_SumaDetalleEnTotal"))
                return EvaluarBoolSumaDetalleEnTotal(regla);
            
            valor = valor.Replace("@valor", "'" + CampoActual + "'");
            valor = valor.Replace("@IdArchivo", IdArchivo.ToString());
            valor = valor.Replace("@NumeroLinea", (x + 1).ToString());
            valor = valor.Replace("@CampoInicial", regla.CaracterInicial.ToString());
            valor = valor.Replace("@LargoCampo", regla.LargoCampo.ToString());

            using (var context = new DISEntities())
            {
                var resultado = context.pa_valida_EjecutaProcedimientoAlmacenado(valor);
                resultadoValor = ObtieneColeccion(resultado);
            }

            if (resultadoValor[0] == "1")
            {
                exitoLinea = 1;
                //start:
                //1.- valida que exista una PRIMAPAG
                if (tipoArchivo == "PRIMDCUA")
                if (tipoArchivo == "PRIMDCUA")
                {
                    exitoLinea = EvaluarBoolSpPrimaPag(regla, exitoLinea);
                }
                //end
            }
            else
            {
                ContadorErrores = ContadorErrores + 1;
                exitoLinea = 0;
            }
            return exitoLinea;
        }

        private int EvaluarBoolSumaDetalleEnTotal(Regla regla)
        {
            var reglaDetalle =
                _reglasLineaPorTipo["D"].FirstOrDefault(
                    x => x.CaracterInicial == regla.CaracterInicial && x.LargoCampo == regla.LargoCampo);

            var sumaDetalle =
                _lineaDetalles.Sum(
                    x => Convert.ToDecimal(x.GetType().GetProperty(reglaDetalle.NombreCampo).GetValue(x, null)));

            return sumaDetalle == Convert.ToDecimal(CampoActual) ? 1 : 0;
        }


        private int EvaluarBoolSpPrimaPag(Regla r, int exitoLinea)
        {
            if (r.Tabladestino != string.Empty)
            {
                var valor = r.Tabladestino;
                valor = valor.Replace("@valor", "'" + CampoActual + "'");
                StringCollection resultadoValor;
                using (var context = new DISEntities())
                {
                    ObjectResult resultado =
                        context.pa_valida_EjecutaProcedimientoAlmacenado(valor);
                    resultadoValor = ObtieneColeccion(resultado);
                }
                if (resultadoValor[0] == "1")
                {
                    exitoLinea = 1;
                }
                else
                {
                    ContadorErrores = ContadorErrores + 1;
                    exitoLinea = 0;
                }
            }
            return exitoLinea;
        }

        private int EvaluarEqual(Regla r, int exitoLinea)
        {
            if (CampoActual == r.ReglaValidacion)
            {
                exitoLinea = 1;
            }
            else
            {
                ContadorErrores = ContadorErrores + 1;
            }
            return exitoLinea;
        }

        private bool ExecSpBool(string procedure)
        {
            var resultadovalor = default(StringCollection);
            using (var context = new DISEntities())
            {
                ObjectResult resultado = context.pa_valida_EjecutaProcedimientoAlmacenado(procedure);
                resultadovalor = ObtieneColeccion(resultado);
            }
            return resultadovalor[0] == "1";
        }

        private StringCollection ExecQuery(string query)
        {
            var resultadovalor = default(StringCollection);
            using (var context = new DISEntities())
            {
                var resultado = context.pa_valida_EjecutaQuery(query);
                resultadovalor = ObtieneColeccion(resultado);
            }
            return resultadovalor;
        }
    }
}
