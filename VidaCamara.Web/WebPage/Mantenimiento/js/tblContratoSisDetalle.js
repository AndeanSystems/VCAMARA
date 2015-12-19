var descripcion_contrato = "Contrato SYS ";
var meses = new Array("-", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
$(document).ready(function () {
    //LIMPIAR DATOS DEL FORMULARIO
    $("section").delegate("#ctl00_ContentPlaceHolder1_btnNuevo", "click", function (ev) {
        ev.preventDefault();
        $("#ctl00_ContentPlaceHolder1_ddl_contrato_sis").val("0");
        $("#ctl00_ContentPlaceHolder1_ddl_compania_seg_vida").val("0");
        $("#ctl00_ContentPlaceHolder1_txt_participacion_sis").val("");
        $("#ctl00_ContentPlaceHolder1_txt_orden_empresa_sis").val("");
        $("#ctl00_ContentPlaceHolder1_txt_ide_contrato_sis_det").val("0");
    });
    //CARGAR GRILLA POR CONTRATO SELECIONADO
    $("section").delegate("#ctl00_ContentPlaceHolder1_ddl_contrato_sis", "change", function (ev) {
        getlistContratoSisDetalle($(this).val());
    });
    //CARGAR GRILLA POR SI EL CONTRATO TIENE ITEM SELECCIONADO
    if (parseInt($("#ctl00_ContentPlaceHolder1_ddl_contrato_sis").val()) != 0) {
        getlistContratoSisDetalle($("#ctl00_ContentPlaceHolder1_ddl_contrato_sis").val());
    };
    //FUNCION QUE CONSTRUYE LA GRILLA JTABLE
    function getlistContratoSisDetalle(ide_contrato) {
        $('#tblContratoViewSySDetalle').jtable({
            tableId: 'Contratos_SYS',
            paging: true,
            sorting: true,
            pageSize: 5,
            defaultSorting: 'IDE_CONTRATO ASC',
            selecting: true,
            saveUserPreferences: true,
            actions: {
                listAction: '/WebPage/Mantenimiento/frmGeneral.aspx/ContratoSisDetalleList',
            },
            fields: {
                IDE_CONTRATO_DET: { key: true, list: false },
                IDE_CONTRATO: {
                    title: 'N° Contrato', display: function (data) {
                        return data.record.CONTRATO_SYS.NRO_CONTRATO
                    }
                },
                COD_CSV: { title: 'Cia. Seguros Vida' },
                PRC_PARTICIACION: { title: '% Participación' },
                NRO_ORDEN: { title: 'Orden (*)' },
                ESTADO: { title: 'Estado' },
                USU_REG: { title: 'Usu. Registro' },
                FEC_REG: { title: 'Fec. Registro', displayFormat: 'dd/mm/yy', type: 'date' },
            },
            selectionChanged: function () {
                //Get all selected rows
                var $selectedRows = $('#tblContratoViewSySDetalle').jtable('selectedRows');

                $('#SelectedRowList').empty();
                if ($selectedRows.length > 0) {
                    //Show selected rows
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        $("#ctl00_ContentPlaceHolder1_ddl_contrato_sis").val(record.IDE_CONTRATO);
                        $("#ctl00_ContentPlaceHolder1_ddl_compania_seg_vida").val(record.COD_CSV);
                        $("#ctl00_ContentPlaceHolder1_txt_participacion_sis").val(record.PRC_PARTICIACION);
                        $("#ctl00_ContentPlaceHolder1_txt_orden_empresa_sis").val(record.NRO_ORDEN);
                        $("#ctl00_ContentPlaceHolder1_txt_ide_contrato_sis_det").val(record.IDE_CONTRATO_DET);
                        $('#ctl00_hdf_control').val(999);

                    });
                } else {
                    $('#SelectedRowList').append('No row selected! Select rows to see here...');
                }
            }
        });
        $('#tblContratoViewSySDetalle.jtable-main-container').css({ "width": "4800px" });
        $('#tblContratoViewSySDetalle').jtable('load', { WhereBy: ide_contrato });
    }

    //funcion muestra alertas mesagw box
    function MessageBox(texto) {
        $("<div style='font-size:14px;text-align:center;'>" + texto + "</div>").dialog({ title: 'Alerta', modal: true, width: 400, height: 160, buttons: [{ id: 'aceptar', text: 'Aceptar', icons: { primary: 'ui-icon-circle-check' }, click: function () { $(this).dialog('close'); } }] })
    }
});
