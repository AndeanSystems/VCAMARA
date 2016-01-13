using System;
using System.Collections.Generic;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.Negocio
{
    public class nAprobacionCarga
    {
        public List<EAprobacionCarga> listApruebaCarga(CONTRATO_SYS contrato, int jtStartIndex, int jtPageSize,object[] filters, out int total)
        {
            return new dAprobacionCarga().listApruebaCarga(contrato,jtStartIndex,jtPageSize, filters, out total);
        }

        public void actualizarEstado(HistorialCargaArchivo_LinCab historialCargaArchivo_LinCab)
        {
            new dAprobacionCarga().actualizarEstado(historialCargaArchivo_LinCab);
        }

        public void actulaizarArchivoIdNomina(Archivo archivo)
        {
            new dAprobacionCarga().actualizarArchivoIdNomina(archivo);
        }

        public void eliminarPagoYNomina(HistorialCargaArchivo_LinCab historialCargaArchivo_LinCab)
        {
            new dAprobacionCarga().eliminarPagoYNomina(historialCargaArchivo_LinCab);
        }

        //public void actualizaEstadoArchivo(Archivo archivo)
        //{
        //    new dAprobacionCarga().actualizaEstadoArchivo(archivo);
        //}

        public List<eAprobacionCargaDetalle> listApruebaCargaDetalle(HistorialCargaArchivo_LinCab linCab)
        {
            return new dAprobacionCarga().listApruebaCargaDetalle(linCab);
        }
    }
}
