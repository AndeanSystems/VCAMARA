using System;
using System.Collections.Generic;
using System.IO;
using VidaCamara.DIS.data;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.Negocio
{
    public class nInterfaceContable
    {
        public void createInterfaceCotable(NOMINA nomina)
        {
            var cabecera = new dInterfaceContable().createInterfaceContableCabecera(nomina);

            var asiento = new List<int>(){ 42, 26 };
            for (int i = 0; i < asiento.Count; i++)
            {
                new dInterfaceContable().createInterfaceContableDetalle(nomina, cabecera, asiento[i]);
            }
        }
    }
}
