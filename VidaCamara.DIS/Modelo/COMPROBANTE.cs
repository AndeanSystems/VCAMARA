
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
    
public partial class COMPROBANTE
{

    public int NRO_COMPROBANTE { get; set; }

    public string TIP_COMPROBANTE { get; set; }

    public string COD_REASEGURADOR { get; set; }

    public System.DateTime FEC_COMPROBANTE { get; set; }

    public int ANO_COMPROBANTE { get; set; }

    public int MES_COMPROBANTE { get; set; }

    public string COD_ASEGURADO { get; set; }

    public string IDE_CONTRATO { get; set; }

    public string COD_RAMO { get; set; }

    public string COD_MONEDA { get; set; }

    public Nullable<decimal> TCA_CAMBIO { get; set; }

    public string DES_REG_TRIMESTRE { get; set; }

    public Nullable<decimal> PRI_XPAG_REA_CED { get; set; }

    public Nullable<decimal> PRI_XCOB_REA_ACE { get; set; }

    public Nullable<decimal> SIN_XCOB_REA_CED { get; set; }

    public Nullable<decimal> SIN_XPAG_REA_ACE { get; set; }

    public Nullable<decimal> OTR_CTA_XCOB_REA_CED { get; set; }

    public Nullable<decimal> OTR_CTA_XPAG_REA_ACE { get; set; }

    public Nullable<decimal> DSCTO_COMIS_REA { get; set; }

    public Nullable<decimal> SALDO_DEUDOR { get; set; }

    public Nullable<decimal> SALDO_ACREEDOR { get; set; }

    public Nullable<decimal> SALDO_DEUDOR_COMP { get; set; }

    public Nullable<decimal> SALDO_ACREEDOR_COMP { get; set; }

    public string ESTADO { get; set; }

    public System.DateTime FEC_REG { get; set; }

    public string USU_REG { get; set; }

    public Nullable<System.DateTime> FEC_MOD { get; set; }

    public string USU_MOD { get; set; }

    public string ORI_REG { get; set; }

}

}
