$(document).ready(function () {
    //validacion detablas para mostrar data
    //validacion de campos para grabar
    $("section").delegate("#ctl00_ContentPlaceHolder1_btnGuardar", 'click', function () {
        var nro_contrato = $('#ctl00_ContentPlaceHolder1_ddl_conrato1').val();
        var tipo_archivo = $("#ctl00_ContentPlaceHolder1_ddl_tipo_archivo").val();
        var tipo_archivo_des = $("#ctl00_ContentPlaceHolder1_ddl_tipo_archivo option:selected").text();
        if (parseInt(nro_contrato) == 0) {
            MessageBox("Seleccione el contrato"); return false;
        } else if (parseInt(tipo_archivo) == 0) {
            MessageBox("Seleccione el tipo de archivo"); return false;
        } else {
            return confirm("¿ Esta seguro de Guardar el tipo de archivo " + tipo_archivo_des + " ?");
        }
    });

    function MessageBox(texto) {
        $("<div style='font-size:14px;text-align:center;'>" + texto + "</div>").dialog({ title: 'Alerta', modal: true, width: 400, height: 160, buttons: [{ id: 'aceptar', text: 'Aceptar', icons: { primary: 'ui-icon-circle-check' }, click: function () { $(this).dialog('close'); } }] })
    }
    //asigncion de funciones jquery 
    //$("#ctl00_ContentPlaceHolder1_txt_mesvig_d").datepicker();
});
