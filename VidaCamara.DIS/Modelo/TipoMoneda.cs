
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
    
public partial class TipoMoneda
{

    public int TipoMonedaId { get; set; }

    public string NombreMoneda { get; set; }

    public int MonedaId { get; set; }



    public virtual Moneda Moneda { get; set; }

}

}
