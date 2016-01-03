using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.data
{
    public class dNomina
    {
        public Int32 setGrabarNomina(NOMINA nomina) {
            try
            {
                using (var db = new DISEntities()) 
                {
                    db.NOMINAs.Add(nomina);
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}
