
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace VidaCamara.DIS.Modelo
{

using System;
    using System.Collections.Generic;
    
public partial class TipoSolicitude
{

    public TipoSolicitude()
    {

        this.Siniestros = new HashSet<Siniestro>();

    }


    public int TipoSolicitudId { get; set; }

    public string Nombre { get; set; }

    public string Clasificacion { get; set; }



    public virtual ICollection<Siniestro> Siniestros { get; set; }

}

}
