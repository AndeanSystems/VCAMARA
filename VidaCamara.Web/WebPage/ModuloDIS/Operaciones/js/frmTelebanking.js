$(document).ready(function () {
    const urlListTelebanking = "/WebPage/ModuloDIS/Operaciones/frmTelebankig.aspx/listTelebanking";
    var nomina = function (ArchivoId) {
        this.ArchivoId = ArchivoId,
        this.IDE_CONTRATO = parseInt($("#ctl00_ContentPlaceHolder1_ddl_contrato").val())
    }

    $("body").delegate("#ctl00_ContentPlaceHolder1_btn_buscar", "click", function (ev) {
        ev.preventDefault();
        var fecha = $("#ctl00_ContentPlaceHolder1_txt_fecha").val();
        listApruebaCarga(new nomina(0), fecha);
    });
    //$("body #tblTelebanking").delegate("#lnk_descarga", "click", function () {
    //    var rutaDescarga = $(this).attr('class');
    //    document.getElementById("fileDonwload").src = rutaDescarga;
    //});
    var fields = {
        ArchivoId: {
            title: 'Detalle', sorting: false, display: function (data) {
                var $icon = $("<a href='#'>Detalle</a>");
                $icon.click(function () {
                    console.log($icon, data);
                    $("#tblTelebanking").jtable('openChildTable',
                        $icon.closest('tr'),
                        {
                            actions: { listAction: "/WebPage/ModuloDIS/Operaciones/frmTelebankig.aspx/listTelebankingByArchivoId" },
                            fields: {
                                RUC_BENE: { title: 'RUC_BENE' },
                                NOM_BENE: { title: 'NOM_BENE' },
                                TIP_CTA: { title: 'TIP_CTA' },
                                CTA_BENE: { title: 'CTA_BENE' },
                                Importe: { title: 'Importe' }
                            }
                        }, function (dataDetail) {
                            dataDetail.childTable.jtable('load', { ArchivoId: data.record.ArchivoId });
                        });
                });
                return $icon;
            }
        },
        NombreArchivo: { title: 'NombreArchivo' },
        FechaOperacion: {
            title: 'FechaOperación', display: function (data) {
                return ConvertNumberToDateTime(data.record.FechaOperacion);
            }
        },
        Moneda: { title: 'Moneda' },
        Importe: { title: 'Importe' },
        RutaNomina: {
            title: 'Descargar', display: function (data) {
                return "<a id='lnk_descarga' class='" + data.record.RutaNomina + "' href='" + data.record.RutaNomina + "' target='_blank'>Descargar</a>";
            }
        }
    }
    function listApruebaCarga(nomina,fecha) {
        $('#tblTelebanking').jtable({
            tableId: 'Telebanking',
            paging: true,
            sorting: true,
            pageSize: 12,
            defaultSorting: 'FechaOperacion ASC',
            selecting: false,
            openChildAsAccordion: true,
            actions: {
                listAction: urlListTelebanking,
            },
            recordsLoaded: function (event, data) {
                //GetTipoTabla($("#ctl00_ContentPlaceHolder1_ddl_tabla_t").val());
            },
            fields: fields
        });

        $("#tblTelebanking").css({ "text-align": "center" });
        $('#tblTelebanking.jtable-main-container').css({ "width": "1500px" });
        $('#tblTelebanking').jtable('load', { nomina: nomina, fecha: fecha });
    }
})