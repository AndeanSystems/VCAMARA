using System.Collections.Generic;
using VidaCamara.SBS.Entity;
using VidaCamara.SBS.Negocio;

namespace VidaCamara.SBS.Utils
{
    public class Utility
    {
        public List<eContratoVC> getContrato(out int total)
        {
            try
            {
                var  o = new eContratoVC()
                {
                    _inicio = 0,
                    _fin = 10000,
                    _orderby = "IDE_CONTRATO ASC",
                    _nro_Contrato = "NO",
                    _estado = "A"
                };

                return new bContratoVC().GetSelecionarContrato(o, out total);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
