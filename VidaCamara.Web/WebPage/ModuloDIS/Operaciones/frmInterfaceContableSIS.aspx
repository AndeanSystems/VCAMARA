<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/Inicio/mpFEPCMAC.Master" AutoEventWireup="true" CodeBehind="frmInterfaceContableSIS.aspx.cs" Inherits="VidaCamara.Web.WebPage.ModuloDIS.Operaciones.frmInterfaceContableSIS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/WebPage/ModuloDIS/operaciones/js/frmInterfaceContable.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content_header">
         <!--Botones de CRUD-->
        <div class="btn_crud">
            <asp:HyperLink ID="HyperLink1" CssClass="btn_crud_button"  ToolTip="Inicio" runat="server" ImageUrl="~/Resources/Imagenes/u158_normal.png" NavigateUrl="~/Inicio"></asp:HyperLink>
            <asp:ImageButton  CssClass="btn_crud_button" ID="btn_exportar" runat="server" ToolTip="Exportar" ImageUrl="~/Resources/Imagenes/u123_normal.png" OnClick="btn_exportar_Click"/>
            <asp:ImageButton  CssClass="btn_crud_button" ID="btn_buscar" runat="server" ToolTip="Buscar" ImageUrl="~/Resources/Imagenes/u154_normal.png" />
        </div>
    </div>
        <div class="tabBody" id="frmComprobante">
        <asp:MultiView id="multiTabs" ActiveViewIndex="0" Runat="server">
            <!--VISTA OPERACIONES-->
            <asp:View ID="view1" runat="server">

                <label class="label_to" for="ddl_contrato_c">Contrato </label>
                <asp:DropDownList CssClass="input_to" ID="ddl_contrato" runat="server" Height="25px" Width="77%"></asp:DropDownList>

                <label class="label_to" for="ddl_tipcom_c">Tipo de Archivo </label>
                <asp:DropDownList CssClass="input_to" ID="ddl_tipo_archivo" runat="server" Height="25px" Width="15.8%"></asp:DropDownList>

                <label class="input_right_L" for="ddl_ramo_c">Moneda </label>
                <asp:DropDownList ID="ddl_moneda" runat="server" CssClass="input_right" Height="25px" Width="15.5%"></asp:DropDownList>

                <label class="label_to" for="ddl_ramo_c">Desde </label>
                <asp:TextBox runat="server"  CssClass="input_to datetime"  Height="25px" Width="15.5%" ID="txt_desde"/>

                <label class="input_right_L" for="ddl_ramo_c">Hasta </label>
                <asp:TextBox runat="server"  CssClass="input_right datetime"  Height="25px" Width="15.5%" ID="txt_hasta"/>

                <label class="input_right_L" for="ddl_ramo_c">Estado </label>
                <asp:DropDownList ID="ddl_estado" runat="server" CssClass="input_right" Height="25px" Width="12.5%"></asp:DropDownList>

                <div class="iframe" id="tblInterfaceSIS">
                    <div id="tblInterfaceContableSIS"></div>
                </div>  

            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
