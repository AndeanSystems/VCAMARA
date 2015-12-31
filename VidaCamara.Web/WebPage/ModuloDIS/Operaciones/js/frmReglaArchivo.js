$(document).ready(function () {
    var action = "/WebPage/ModuloDIS/Operaciones/frmCargaDatos.aspx/listReglaArchivo";
    var fields = {
        NombreCampo: { title: 'NombreCampo'},
        InformacionCampo: { title: 'InformacionCampo' },
        TipoLinea: { title: 'TipoLinea' },
        CaracterInicial: { title: 'CaracterInicial' },
        LargoCampo: { title: 'LargoCampo' },
        TipoCampo: { title: 'TipoCampo' },
        FormatoContenido: { title: 'FormatoContenido' }
    }
    var reglaArchivo = function () {
        this.TipoLinea = "0",
        this.Archivo = "LIQPPP"
    };
    console.log(fields);
    listReglaArchivo(new reglaArchivo());
    function listReglaArchivo(regla) {
        $('#tblReglaArchivo').jtable({
            tableId: 'reglaArchivoID',
            paging: true,
            sorting: true,
            pageSize: 12,
            defaultSorting: 'CaracterInicial ASC',
            selecting: true,
            actions: {
                listAction: action,
            },
            recordsLoaded: function (event, data) {
                //GetTipoTabla($("#ctl00_ContentPlaceHolder1_ddl_tabla_t").val());
            },
            fields: fields
        });

        $('.jtable-main-container').css({ "width": "1420px" });
        $('#tblReglaArchivo').jtable('load', {regla : regla});
    }
})