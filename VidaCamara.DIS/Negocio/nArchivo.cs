using System;
using System.Collections.Generic;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.Negocio
{
    public class nArchivo
    {
        /// <summary>
        /// Devuelve la validacion de un archivo si ya fue cargado anteriormente
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public List<Archivo> listExisteArchivo(Archivo archivo)
        {
            string[] collectionArchivo = archivo.NombreArchivo.Split('_');
            //archivo.NombreArchivo = collectionArchivo[0] + "_" + collectionArchivo[1] + "_" + collectionArchivo[2] + "_" + collectionArchivo[3];
            var tamanoNombre = archivo.NombreArchivo.Length;
            return new dArchivo().listExisteArchivo(archivo, tamanoNombre);
        }

        public Int32 listExistePagoNomina(Archivo archivo)
        {
            var nombreNomina = archivo.NombreArchivo.Split('_');
            archivo.NombreArchivo = "LIQ" + nombreNomina[1] + archivo.NombreArchivo.Substring(nombreNomina[0].Length+nombreNomina[1].Length+1);
            return new dArchivo().listExistePagoNomina(archivo);
        }
    }
}
