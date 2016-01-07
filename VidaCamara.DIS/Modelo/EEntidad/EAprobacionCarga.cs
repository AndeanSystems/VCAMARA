using System;

namespace VidaCamara.DIS.Modelo.EEntidad
{
    public partial class EAprobacionCarga
    {
        public string NombreArchivo { get; set; }
        public DateTime FechaCarga { get; set; }
        public string moneda { get; set; }
        public long TotalRegistros { get; set; }
        public string TotalImporte { get; set; }
        public string PagoVc { get; set; }
        public DateTime FechaInfo { get; set; }
        public string UsuReg { get; set; }
        public string Aprobar { get; set; }
        public string Eliminar { get; set; }
    }
}
