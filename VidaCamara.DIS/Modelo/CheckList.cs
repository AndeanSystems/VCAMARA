
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
    
public partial class CheckList
{

    public int CheckListId { get; set; }

    public string CUSPP { get; set; }

    public int TipoMovimientoId { get; set; }

    public Nullable<System.DateTime> FechaModificacion { get; set; }

    public string UsuarioModificacion { get; set; }

    public Nullable<bool> Vigente { get; set; }

    public Nullable<int> TipoSolicitudId { get; set; }

    public Nullable<int> Estado { get; set; }

    public string Observaciones { get; set; }

}

}
