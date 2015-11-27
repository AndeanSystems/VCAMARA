<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/Inicio/mpFEPCMAC.Master" AutoEventWireup="true" CodeBehind="frmSegLogAdmin.aspx.cs" Inherits="VidaCamara.Web.WebPage.ModuloDIS.Consultas.frmLogAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Comienzo de los Tabs-->
         <!--Botones de CRUD-->
        <div class="btn_crud">
            <asp:HyperLink ID="HyperLink1" CssClass="btn_crud_button"  ToolTip="Inicio" runat="server" ImageUrl="~/Resources/Imagenes/u158_normal.png" NavigateUrl="~/Inicio"></asp:HyperLink>
            <asp:ImageButton  CssClass="btn_crud_button" ID="btn_exportar" runat="server" ToolTip="Exportar" ImageUrl="~/Resources/Imagenes/u123_normal.png"/>
            <asp:ImageButton  CssClass="btn_crud_button" ID="btn_buscar" runat="server" ToolTip="Buscar" ImageUrl="~/Resources/Imagenes/u154_normal.png" />
        </div>
    <!--Cuerpo de los tabs-->
    <div class="tabBody" id="frmOperacion">
        <asp:MultiView id="multiTabs" ActiveViewIndex="0" Runat="server">
            <!--VISTA OPERACIONES-->
            <asp:View ID="view1" runat="server">

                <label class="label_to" for="ddl_contrato_o">Contrato (*)</label>
                <asp:DropDownList CssClass="input_to" ID="ddl_contrato" runat="server" Height="25px" Width="77%"></asp:DropDownList>

                <label class="label_to" for="ddl_tipcom_o">Operación </label>
                <asp:DropDownList CssClass="input_to" ID="list" runat="server" Height="25px" Width="14.8%"></asp:DropDownList>

                <label class="input_right_L" for="txt_fec_ini_o">Desde</label>
                <asp:TextBox CssClass="input_right" ID="txt_fec_ini_o" runat="server" Height="25px" Width="14.7%" TextMode="Date"></asp:TextBox>

                <label class="input_right_T" for="txt_fec_hasta_o">Hasta  </label>
                <asp:TextBox CssClass="input_right" ID="txt_fec_hasta_o" runat="server" Height="25px" Width="14.7%" TextMode="Date"></asp:TextBox>

                <label class="label_to">Tipo de Evento</label>
                <asp:DropDownList runat="server" Height="25px" Width="14.7%" CssClass="input_to">
                    <asp:ListItem Text="Todas" />
                    <asp:ListItem Text="Registro" />
                </asp:DropDownList>

                <div class="iframe" id="tblConsulta1">

                </div>  
            </asp:View>
        </asp:MultiView>    
    </div>
</asp:Content>
