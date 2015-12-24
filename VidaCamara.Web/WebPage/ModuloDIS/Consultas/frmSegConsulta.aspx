<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/Inicio/mpFEPCMAC.Master" AutoEventWireup="true" CodeBehind="frmSegConsulta.aspx.cs" Inherits="VidaCamara.Web.WebPage.ModuloDIS.Consultas.frmSegConsulta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" href="../../../Resources/CSS/bootstrap.css" />
<script src="/WebPage/ModuloDIS/Consultas/script/frmSeqConsulta.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!--Comienzo de los Tabs-->
         <!--Botones de CRUD-->
        <div class="btn_crud">
            <asp:HyperLink ID="HyperLink1" CssClass="btn_crud_button"  ToolTip="Inicio" runat="server" ImageUrl="~/Resources/Imagenes/u158_normal.png" NavigateUrl="~/Inicio"></asp:HyperLink>
            <asp:ImageButton  CssClass="btn_crud_button" ID="btn_exportar" runat="server" ToolTip="Exportar" ImageUrl="~/Resources/Imagenes/u123_normal.png"/>
            <asp:ImageButton  CssClass="btn_crud_button" ID="btn_consultar" runat="server" ToolTip="Buscar" ImageUrl="~/Resources/Imagenes/u154_normal.png" />
        </div>
    <!--Cuerpo de los tabs-->
    <div class="tabBody" id="frmOperacion">
        <asp:MultiView id="multiTabs" ActiveViewIndex="0" Runat="server">
            <!--VISTA OPERACIONES-->
            <asp:View ID="view1" runat="server">
                <label class="label_to" for="ddl_contrato_o">Contrato (*)</label>
                <asp:DropDownList CssClass="input_to" ID="ddl_contrato" runat="server" Height="25px" Width="77%"></asp:DropDownList>

                <label class="label_to" for="ddl_tipo_archivo">Tipo de Tramite </label>
                <asp:DropDownList CssClass="input_to" ID="ddl_tipo_archivo" runat="server" Height="25px" Width="14.8%"></asp:DropDownList>

                <label class="input_right_L" for="txt_fec_ini_o">Desde</label>
                <asp:TextBox CssClass="input_right datetime" ID="txt_fec_ini_o" runat="server" Height="25px" Width="14.7%" ></asp:TextBox>

                <label class="input_right_T" for="txt_fec_hasta_o">Hasta  </label>
                <asp:TextBox CssClass="input_right datetime" ID="txt_fec_hasta_o" runat="server" Height="25px" Width="14.7%" ></asp:TextBox>

                <label class="label_to" for="ddl_ramo_o">AFP </label>
                <asp:DropDownList CssClass="input_to" ID="ddl_afp" runat="server" Height="25px" Width="14.8%"></asp:DropDownList>

                <label class="input_right_L">Moneda </label>
                <asp:DropDownList runat="server" ID="ddl_moneda" Height="25px" Width="14.8%" CssClass="input_right"></asp:DropDownList>

                <label class="label_to">CUSPP </label>
                <asp:TextBox runat="server" ID="txt_cod_cusp" CssClass="input_to" Height="25px" Width="14.8%"/>

                <label class="input_right_L">Nombres</label>
                <asp:TextBox runat="server" ID="txt_nombre" CssClass="input_right" Height="25px" Width="14.8%"/>

                <label class="input_right_T">Apellidos</label>
                <asp:TextBox runat="server" ID="txt_apellido" CssClass="input_right" Height="25px" Width="14.8%"/>

                <label class="label_to">DNI</label>
                <asp:TextBox runat="server" ID="txt_dni" CssClass="input_to" Height="25px" Width="14.8%"/>

                <label class="input_right_L">Nº solicitud</label>
                <asp:TextBox runat="server" ID="txt_nro_solicitud" CssClass="input_right" Height="25px" Width="14.8%"/>

                <div class="iframe" id="tblConsulta1">
                    <asp:GridView ID="gv_archivo_cargado" runat="server" CssClass="table table-hover" Font-Size="9px"></asp:GridView>
                    <div id="frmSeqConsulta"></div>
                </div>  

            </asp:View>
        </asp:MultiView>    
    </div>
</asp:Content>
