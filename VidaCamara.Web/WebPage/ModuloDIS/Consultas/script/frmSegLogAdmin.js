$(document).ready(function () {
    var mifuncion = function () {
        alert("funciona mi prueba");
    }
    //variables
    //$("tblLogOperacion").listLogOperacion(log);
    var action = "/WebPage/ModuloDIS/Consultas/frmSegLogAdmin.aspx/listLogOperacion";
    var fields = {
        CodiLogOper: { title: 'CodiLogOper'},
        FechEven: { title: 'FechEven', type: 'date', displayFormat: 'dd/mm/yy' },
        TipoOper: { title: 'TipoOper' },
        CodiOper: { title: 'CodiOper' },
        CodiEven: { title: 'CodiEven' },
        CodiUsu: { title: 'CodiUsu' },
        CodiCnx: { title: 'CodiCnx' }
    }
    var logOperacion = function () {
        this.TipoOper = $("#ctl00_ContentPlaceHolder1_ddl_operacion").val(),
        this.CodiEven = $("#ctl00_ContentPlaceHolder1_ddl_tipo_evento").val()

    };
    //
    listLogOperacion(new logOperacion());
    //ejecutar recarga de la grilla por tipo de archivo
    //$(".tabBody").delegate("#ctl00_ContentPlaceHolder1_ddl_tipo_linea", "change", function () {
    //    if (existePestanaRegla == 1 && parseInt(tipoArchivo) != 0)
    //        listLogOperacion(new logOperacion());
    //});
    function listLogOperacion(log) {
        $('#tblLogOperacion').jtable({
            tableId: 'CodiLogOper',
            paging: true,
            sorting: true,
            pageSize: 12,
            defaultSorting: 'CodiLogOper ASC',
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
        $('#tblLogOperacion').jtable('load', {log : log});
    }
})