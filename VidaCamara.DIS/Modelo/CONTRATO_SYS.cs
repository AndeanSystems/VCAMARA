
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
    
public partial class CONTRATO_SYS
{

    public CONTRATO_SYS()
    {

        this.CONTRATO_SIS_DET = new HashSet<CONTRATO_SIS_DET>();

        this.EXACTUS_CABECERA_SIS = new HashSet<EXACTUS_CABECERA_SIS>();

        this.HistorialCargaArchivo_LinCab = new HashSet<HistorialCargaArchivo_LinCab>();

        this.LogOperacions = new HashSet<LogOperacion>();

        this.NOMINAs = new HashSet<NOMINA>();

    }


    public int IDE_CONTRATO { get; set; }

    public Nullable<int> ID_EMPRESA { get; set; }

    public string NRO_CONTRATO { get; set; }

    public string CLA_CONTRATO { get; set; }

    public System.DateTime FEC_INI_VIG { get; set; }

    public System.DateTime FEC_FIN_VIG { get; set; }

    public string CENTRO_COSTO { get; set; }

    public string DES_CONTRATO { get; set; }

    public string ESTADO { get; set; }

    public System.DateTime FEC_REG { get; set; }

    public string USU_REG { get; set; }

    public Nullable<System.DateTime> FEC_MOD { get; set; }

    public string USU_MOD { get; set; }

    public Nullable<int> NRO_EMPRESAS { get; set; }



    public virtual ICollection<CONTRATO_SIS_DET> CONTRATO_SIS_DET { get; set; }

    public virtual ICollection<EXACTUS_CABECERA_SIS> EXACTUS_CABECERA_SIS { get; set; }

    public virtual ICollection<HistorialCargaArchivo_LinCab> HistorialCargaArchivo_LinCab { get; set; }

    public virtual ICollection<LogOperacion> LogOperacions { get; set; }

    public virtual ICollection<NOMINA> NOMINAs { get; set; }

}

}
