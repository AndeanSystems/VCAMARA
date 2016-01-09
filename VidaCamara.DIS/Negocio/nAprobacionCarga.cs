using System.Collections.Generic;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.Negocio
{
    public class nAprobacionCarga
    {
        public List<EAprobacionCarga> listApruebaCarga(CONTRATO_SYS contrato)
        {
            return new dAprobacionCarga().listApruebaCarga(contrato);
        }

        public void actualizarEstado(HistorialCargaArchivo_LinCab historialCargaArchivo_LinCab)
        {
            new dAprobacionCarga().actualizarEstado(historialCargaArchivo_LinCab);
        }

        public void actulaizarArchivoIdNomina(Archivo archivo)
        {
            new dAprobacionCarga().actualizarArchivoIdNomina(archivo);
        }
    }
}
