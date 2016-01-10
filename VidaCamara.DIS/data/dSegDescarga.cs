using System;
using System.Collections.Generic;
using VidaCamara.DIS.Modelo;
using VidaCamara.DIS.Modelo.EEntidad;

namespace VidaCamara.DIS.data
{
    public class dSegDescarga
    {
        public List<ESegDescarga> listSegDescarga()
        {
            try
            {
                using (var db = new DISEntities())
                {
                    return new List<ESegDescarga>();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
