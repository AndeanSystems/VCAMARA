$(document).ready(function () {
    $("section").delegate("#ctl00_ContentPlaceHolder1_btn_consultar", "click", function (ev) {
        var contrato = $("#ctl00_ContentPlaceHolder1_ddl_contrato").val();

        if (parseInt(contrato) == 0) {
            mostrarMensajeAlert("Seleccione Contrato"); return false;
        }
    });
})