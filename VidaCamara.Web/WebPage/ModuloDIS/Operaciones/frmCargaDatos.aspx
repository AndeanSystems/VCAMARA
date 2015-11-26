<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/Inicio/mpFEPCMAC.Master" AutoEventWireup="true" CodeBehind="frmCargaDatos.aspx.cs" Inherits="VidaCamara.Web.WebPage.ModuloDIS.Operaciones.CargaDatos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="/Resources/CSS/Progressbar.css" rel="stylesheet" />
<script src="/WebPage/ModuloDIS/Operaciones/js/CargaDatos.js"></script>
<script src="/WebPage/ModuloDIS/Operaciones/js/CargaRsp.js"></script>

<script type="text/javascript">

    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            loading.css({ top: '40%', left: '45%' });
        }, 200);
    }
    $('form').live("submit", function () {
        var excelerror = $("#ctl00_ContentPlaceHolder1_hdf_excel_error").val();
        if (excelerror == 0) {
            ShowProgress();
        }
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!--Comienzo de los Tabs-->
   <script runat="server">
        protected void menuTabs_MenuItemClick(object sender, MenuEventArgs e)
        {
            multiTabs.ActiveViewIndex = Int32.Parse(menuTabs.SelectedValue);
        }
    </script>

        <div class="btn_crud">
            <asp:HyperLink CssClass="btn_crud_button"  ToolTip="Inicio" runat="server" ImageUrl="~/Resources/Imagenes/u158_normal.png" NavigateUrl="~/Inicio"></asp:HyperLink>
            <asp:ImageButton  CssClass="btn_crud_button" ID="btnGuardar" runat="server" ToolTip="Guardar" ImageUrl="~/Resources/Imagenes/upload.png" OnClick="btnGuardar_Click" />
        </div>
        <asp:Menu id="menuTabs" CssClass="menuTabs" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                    Orientation="Horizontal" OnMenuItemClick="menuTabs_MenuItemClick" Runat="server">
                <Items >
                    <asp:MenuItem  Text="Carga" Value="0" Selected="true" />
                    <asp:MenuItem  Text="Detalle" Value="1"/>
                    <asp:MenuItem  Text="Información Arhivo" Value="2"/>
                </Items>
            <StaticMenuItemStyle CssClass="tab"></StaticMenuItemStyle>
            <StaticSelectedStyle CssClass="selectedTab" BackColor="#006666"></StaticSelectedStyle>
        </asp:Menu>
      
    <!--Cuerpo de los tabs-->
    <div class="tabBody">
        <asp:MultiView id="multiTabs" ActiveViewIndex="0" Runat="server">
            <!--VISTA CARGA DE DATOS-->
            <asp:View ID="view1" runat="server">                
                <label class="label_to" for="dbl_contrato_d">Contrato (*)</label>
                <asp:DropDownList CssClass="input_to" ID="ddl_conrato1" runat="server" Height="25px" Width="77%"></asp:DropDownList>

                <label class="label_to" for="fileUpload">Nombre del Archivo (*)</label>
                <asp:FileUpload CssClass="input_to" ID="fileUpload" ToolTip="Selecione el archivo a subir" runat="server" Height="25px" Width="48.4%" />

                <label class="label_to">Tipo del Archivo (*)</label>
                <asp:DropDownList runat="server" ID="ddl_tipo_archivo" CssClass="input_to" Height="25px" Width="40%">
                    <asp:ListItem Text="Liquidación Aporte Adicional" />
                    <asp:ListItem Text="Liquidación Pago Aporte Adicional" />
                </asp:DropDownList>

                <label class="label_to" for="fileUpload">Moneda</label>
                <asp:Label runat="server" CssClass="input_to" Text="Soles"></asp:Label>

                <label class="label_to" for="fileUpload">Tipo del importe</label>
                <asp:Label runat="server" CssClass="input_to" Text="20,000"></asp:Label>

                <label class="label_to" for="fileUpload">Registros Procesados</label>
                <asp:Label runat="server" CssClass="input_to" Text="300"></asp:Label>

                <label class="label_to" for="fileUpload">Registros Observadas</label>
                <asp:Label runat="server" CssClass="input_to" Text="40"></asp:Label>
            </asp:View>
            <!--seccion de RSP-->
            <asp:View ID="view2" runat="server">               
                <label class="label_to" for="dbl_contrato_d">Nombre del Archivo (*)</label>
                <asp:Label Text="LIQPSEP_20150905_1_1_086.CAM" runat="server"  CssClass="input_to"/>

                <label class="label_to" for="dbl_contrato_d">Tipo del Archivo (*)</label>
                <asp:Label Text="Liquidaciones de Pago de Sepelio" runat="server"  CssClass="input_to"/>

                <div class="iframe" id="Cargada">
                      <asp:GridView ID="GridView1" runat="server" Width="100%" HorizontalAlign="center"
                            CssClass="CSSTableGenerator">
                      </asp:GridView>
                </div>
                
                <div class="iframe" id="Observada">
                    <asp:GridView ID="GridView2" runat="server" Width="100%" HorizontalAlign="center"
                            CssClass="CSSTableGenerator">
                      </asp:GridView>
                </div>               
               
            </asp:View>
            <asp:View ID="view3" runat="server">
                <label class="label_to" for="dbl_contrato_d">Nombre del Archivo (*)</label>
                <asp:Label Text="LIQPSEP_20150905_1_1_086.CAM" runat="server"  CssClass="input_to"/>

                <label class="label_to" for="dbl_contrato_d">Tipo del Archivo (*)</label>
                <asp:Label Text="Liquidaciones de Pago de Sepelio" runat="server"  CssClass="input_to"/>

                <label class="input_right_L" for="ddl_tipinfo_d">Tipo de Linia (*)</label>
                <asp:DropDownList CssClass="input_to" ID="ddl_tipinfo_d" runat="server" Height="25px" Width="15%">
                    <asp:ListItem Text="Todos" />
                    <asp:ListItem Text="Cabecera - Título" />
                    <asp:ListItem Text="Detalle - Contenido" />
                </asp:DropDownList>
                <div class="iframe" id="informacion">
                    <asp:GridView runat="server"></asp:GridView>
                </div>
            </asp:View>
        </asp:MultiView>
        <asp:HiddenField ID="hdf_excel_error" runat="server" Value="0"/>
        <asp:HiddenField ID="hdf_estado_borrar" runat="server" Value="A"/>

        <div class="loading">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Resources/Imagenes/loading19.gif" />
        </div>
    </div>
</asp:Content>
