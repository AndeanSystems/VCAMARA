$(document).ready(function () {
    //entidades y variables
    var cabecera = function (ide_contrato) {
        this.IDE_CONTRATO = ide_contrato,
        this.ESTADO_TRANSFERENCIA = $("#ctl00_ContentPlaceHolder1_ddl_estado").val(),
        this.IDE_MONEDA = parseInt($("#ctl00_ContentPlaceHolder1_ddl_moneda").val())
    }
    //eventos
    $("section").delegate("#ctl00_ContentPlaceHolder1_btn_buscar", "click", function (ev) {
        ev.preventDefault();
        var contrato = parseInt($("#ctl00_ContentPlaceHolder1_ddl_contrato").val());
        if (contrato == 0) {
            mostrarMensajeAlert("Seleccione el contrato");
        } else {
            var filter = [$("#ctl00_ContentPlaceHolder1_txt_desde").val(), $("#ctl00_ContentPlaceHolder1_txt_hasta").val(),
                         $("#ctl00_ContentPlaceHolder1_ddl_tipo_archivo").val()];
            listInterfaceContable(new cabecera(contrato), filter);
        }
    });
    //transferir
    $("section").delegate("#ctl00_ContentPlaceHolder1_btn_transfer", "click", function (ev) {
        var contrato = parseInt($("#ctl00_ContentPlaceHolder1_ddl_contrato").val());
        if (contrato == 0) {
            mostrarMensajeAlert("Seleccione el contrato");
            return false;
        } else{
            return confirm("¿Está seguro de transferir la información?");
        }
    });
    
    var action = "/WebPage/ModuloDIS/Operaciones/frmInterfaceContableSIS.aspx/listInterfaceContable";
    var fields = {
        PAQUETE: {
            title: 'PAQUETE', display: function (data) {
                return data.record.EXACTUS_CABECERA_SIS.PAQUETE;
            }
        },
        ASIENTO: {
            title: 'ASIENTO', display: function (data) {
                return data.record.EXACTUS_CABECERA_SIS.ASIENTO;
            }
        },
        FECHA: {
            title: 'FECHA_REGISTRO', display: function (data) {
                return ConvertNumberToDateTime(data.record.EXACTUS_CABECERA_SIS.FECHA);
            }
        },
        TIPO_ASIENTO: {
            title: 'TIPO_ASIENTO', display: function (data) {
                return data.record.EXACTUS_CABECERA_SIS.TIPO_ASIENTO;
            }
        },
        CONTABILIDAD: {
            title: 'CONTABILIDAD', display: function (data) {
                return data.record.EXACTUS_CABECERA_SIS.CONTABILIDAD;
            }
        },
        FUENTE: { title: 'FUENTE' },
        REFERENCIA: { title: 'REFERENCIA' },
        CONTRIBUYENTE: { title: 'CONTRIBUYENTE' },
        CENTRO_COSTO: { title: 'CENTRO_COSTO' },
        CUENTA_CONTABLE: { title: 'CUENTA_CONTABLE' },
        DebitoSoles: { title: 'DebitoSoles' },
        CreditoSoles: { title: 'CreditoSoles' },
        DebitoDolar: { title: 'DebitoDolar' },
        CreditoDolar: { title: 'CreditoDolar' },
        MONTO_UNIDADES: { title: 'MONTO_UNIDADES' },
        ESTADO_TRANSFERENCIA: {
            title: 'EstadoTransferencia', display: function (data) {
                return data.record.EXACTUS_CABECERA_SIS.ESTADO_TRANSFERENCIA == "C" ? "CREADO" : "TRANSFERIDO";
            }
        }
    }
    function listInterfaceContable(cabecera, filter) {
        $('#tblInterfaceContableSIS').jtable({
            tableId: 'interfaceContableID',
            paging: true,
            sorting: false,
            pageSize: 10,
            //defaultSorting: 'PAQUETE ASC',
            selecting: true,
            actions: {
                listAction: action,
            },
            fields: fields
        });

        $('#tblInterfaceContableSIS.jtable-main-container').css({ "width": "1800px" });
        $('#tblInterfaceContableSIS').jtable('load', { cabecera: cabecera, filter: filter });
    }
})