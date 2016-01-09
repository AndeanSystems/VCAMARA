$(document).ready(function () {
    const url = "/WebPage/ModuloDIS/Consultas/frmSegDescarga.aspx/setAprobar";
    //eventos
    $("body #tblApruebaCarga").delegate("#link_aprobar", "click", function () {
        if(confirm("Esta seguro de aprobar este registro")){
            var linCab = {linCabId:parseInt($(this).attr('class')),IdeContrato:parseInt($("#ctl00_ContentPlaceHolder1_ddl_contrato").val())};
            //programar llamada ajax
            llamarAjax(linCab,url).success(function(res){
                console.log(res);
                if(res.d.Result == true)
                    mostrarMensajeAlert("Transacción existosa.");
                else
                    mostrarMensajeAlert(res.d.Result);   
            });
        }
    });
    var contrato_sis = function () {
        this.IDE_CONTRATO = $("#ctl00_ContentPlaceHolder1_ddl_contrato").val()
    };
    //eventos
    $("section").delegate("#ctl00_ContentPlaceHolder1_btn_buscar", "click", function (ev) {
        ev.preventDefault();
        var filters = [$("#ctl00_ContentPlaceHolder1_txt_fec_ini_o").val(), $("#ctl00_ContentPlaceHolder1_txt_fec_hasta_o").val()];
        listApruebaCarga(new contrato_sis(), filters);
    });
    var action = "/WebPage/ModuloDIS/Consultas/frmSegDescarga.aspx/listApruebaCarga";
    var fields = {
        NombreArchivo: { title: 'NombreArchivo' },
        FechaCarga: { title: 'FechaCarga', type: 'date', displayFormat: 'dd/mm/yy' },
        moneda: { title: 'moneda' },
        TotalRegistros: { title: 'TotalRegistros' },
        TotalImporte: { title: 'TotalImporte' },
        PagoVc: { title: 'PagoVc' },
        FechaInfo: { title: 'FechaInfo', type: 'date', displayFormat: 'dd/mm/yy' },
        UsuReg: { title: 'UsuReg' },
        Aprobar: {
            title: 'Aprobar', display: function (data) {
                return "<a id='link_aprobar' class='"+data.record.IdLinCab+"' href='#'>Aprobar</a>";
            }
        },
        Eliminar: {
            title: 'Eliminar', display: function (data) {
                return "<a id='link_aprobar' href='#'>Eliminar</a>";
            }
        }
    }
    function listApruebaCarga(contrato, filters) {
        $('#tblApruebaCarga').jtable({
            tableId: 'ApruebaCarga',
            paging: true,
            sorting: true,
            pageSize: 12,
            defaultSorting: 'NombreArchivo ASC',
            selecting: false,
            actions: {
                listAction: action,
            },
            recordsLoaded: function (event, data) {
                //GetTipoTabla($("#ctl00_ContentPlaceHolder1_ddl_tabla_t").val());
            },
            fields: fields
        });

        $('#tblApruebaCarga.jtable-main-container').css({ "width": "1500px" });
        $('#tblApruebaCarga').jtable('load', { contrato: contrato, filters: filters });
    }
})