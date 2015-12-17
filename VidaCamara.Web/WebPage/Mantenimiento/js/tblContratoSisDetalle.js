var descripcion_contrato = "Contrato SYS ";
var meses = new Array("-", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");

$(document).ready(function () {
    var tablacontrato = $("#tblContratoViewSyS").length;
    $("#ctl00_ContentPlaceHolder1_ddl_estado_sys").val("R");

    $("section").delegate("#ctl00_ContentPlaceHolder1_btnNuevo", "click", function (ev) {
        ev.preventDefault();
        if (tablacontrato == 1) {
            $("#ctl00_ContentPlaceHolder1_txtdescripcion_sys").val("");
            $("#ctl00_ContentPlaceHolder1_txtFechaInicio_sys").val("");
            $("#ctl00_ContentPlaceHolder1_txt_nrocont_sys").val("");
            $("#ctl00_ContentPlaceHolder1_txtFechaFin_sys").val("");
            $("#ctl00_ContentPlaceHolder1_txt_idContrato_sys").val(0);
        }
    });

    $("section").delegate("#ctl00_ContentPlaceHolder1_btnGuardar", "click", function (ev) {
        var tablaidcont = $("#tblContratoViewSyS").length;
        if (tablaidcont == 0) {
            var idgeneral = $("ctl00_ContentPlaceHolder1_txt_idempresa").val();
            if (idgeneral == 0) {
                return confirm("¿Esta seguro de Grabar?");
            } else {
                return confirm("¿ Está Seguro de Actualizar el Registro ?");
            }
        } else if (tablaidcont == 1) {
            var idcontrato = $("#ctl00_ContentPlaceHolder1_txt_idContrato_sys").val();
            if (idcontrato == 0) {
                var clacont = $("#ctl00_ContentPlaceHolder1_ddl_clase_contrato_sys").val();
                var nrocont = $("#ctl00_ContentPlaceHolder1_txt_nrocont_sys").val();
                var fecini = $("#ctl00_ContentPlaceHolder1_txtFechaInicio_sys").val();
                var fecfin = $("#ctl00_ContentPlaceHolder1_txtFechaFin_sys").val();
                if (clacont == 0 || nrocont == "" || fecini == "" || fecfin == "") {
                    MessageBox("Ingrese y Selecione los Campos Requiridos"); return false;
                } else if (porret == 0 || porcec == 0 || porcia == 0) {
                    return confirm("Hay Algunos Campos con valor cero. ¿Esta seguro de Grabar?");
                } else {
                    return confirm("¿Esta seguro de Grabar?");
                }
            } else {
                return confirm("¿ Está Seguro de Actualizar el Registro ?");
            }
        }
    });

    $('#tblContratoViewSySDetalle').jtable({
        tableId: 'Contratos_SYS',
        paging: true,
        sorting: true,
        pageSize: 5,
        defaultSorting: '_estado ASC',
        selecting: true,
        saveUserPreferences: true,
        actions: {
            listAction: '/WebPage/Mantenimiento/frmGeneral.aspx/ContratoSisDetalle',
        },
        fields: {
            _ide_Contrato: { key: true, list: false },
            _ide_Contrato_Det: { key: true, list: false },
            _nro_Contrato: { title: 'N° Contrato' },
            _ide_Reasegurador: { title: 'Cia. Seguros Vida' },
            _cod_Reasegurador: { list: false },
            _cal_Crediticia: { title: '% Participación' },
            _cod_Empresa_Califica: { title: 'Orden (*)' },
            _estado: { title: 'Estado' },
            _usu_reg: { title: 'Usu. Registro' },
            _fec_reg: { title: 'Fec. Registro', displayFormat: 'dd/mm/yy', type: 'date' },
        },
        selectionChanged: function () {
            //Get all selected rows
            var $selectedRows = $('#tblContratoViewSySDetalle').jtable('selectedRows');

            $('#SelectedRowList').empty();
            if ($selectedRows.length > 0) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');

                    $('#ctl00_ContentPlaceHolder1_txt_idContrato_sys').val(record._ide_Contrato);
                    $('#ctl00_ContentPlaceHolder1_txt_nrocont_sys').val(record._nro_Contrato);
                    $('#ctl00_ContentPlaceHolder1_ddl_clase_contrato_sys').val(record._cla_Contrato);
                    $('#ctl00_ContentPlaceHolder1_txtFechaInicio_sys').val(ConvertNumberToDate(record._fec_Ini_Vig));
                    $('#ctl00_ContentPlaceHolder1_txtFechaFin_sys').val(ConvertNumberToDate(record._fec_Fin_Vig));
                    $("#ctl00_ContentPlaceHolder1_txtdescripcion_sys").val(record._des_Contrato);
                    $("#ctl00_ContentPlaceHolder1_ddlEstado_sys").val(record._estado);
                    $("#ctl00_ContentPlaceHolder1_txt_numero_empresa").val(record._nro_empresa);
                    $('#ctl00_hdf_control').val(999);

                });
            } else {
                $('#SelectedRowList').append('No row selected! Select rows to see here...');
            }
        }
    });
    $('#tblContratoViewSySDetalle.jtable-main-container').css({ "width": "4800px" });
    $('#tblContratoViewSySDetalle').jtable('load', { WhereBy: "NO" });
    //asignar valor Inicial de (0) a los textbox
    console.log(tablacontrato);

    //funcion mesagw box
    function MessageBox(texto) {
        $("<div style='font-size:14px;text-align:center;'>" + texto + "</div>").dialog({ title: 'Alerta', modal: true, width: 400, height: 160, buttons: [{ id: 'aceptar', text: 'Aceptar', icons: { primary: 'ui-icon-circle-check' }, click: function () { $(this).dialog('close'); } }] })
    }
});
