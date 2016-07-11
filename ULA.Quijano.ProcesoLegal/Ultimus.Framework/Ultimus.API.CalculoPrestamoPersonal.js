//#region Variables globales

var DataTable;
var txtName;
var stringFounded;

var UI = new Ultimus.UI();
var listaFerias = {};
var listaSegmento = {};
var listaTipoPrestamo = {};
var PrestamoPersonalJson = {};

//#endregion

$("#formDatosPrestamo").EnableValidationToolTip();

jQuery(document).ready(function () {

    Datepicker("#txtFechaPrimerDescuento");
    Datepicker("#txtFechaEfectivaPago");

    //#region Campos con autoNumeric

    $('#txtConSeguroVida').autoNumeric('init', { aSign: 'B./ ' });

    $('#txtMontoPrestamo').autoNumeric('init', { aSign: 'B./ ' });

    $('#txtCapitalIntereses').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtCapitalIntereses").autoNumeric('set', '0');

    $('#txtSeguroVida').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtSeguroVida").autoNumeric('set', '0');

    $('#txtSeguroFraude').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtSeguroFraude").autoNumeric('set', '0');

    $('#txtFECI').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtFECI").autoNumeric('set', '0');

    $('#txtMensualidadTotal').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtMensualidadTotal").autoNumeric('set', '0');

    $('#txtCapacidadPago').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtCapacidadPago").autoNumeric('set', '0');

    $('#txtEndeudamientoTotal').autoNumeric('init', { aSign: ' %', pSign: 's' });
    $("#txtEndeudamientoTotal").autoNumeric('set', '0');

    $('#txtEndeudamientoPP').autoNumeric('init', { aSign: ' %', pSign: 's' });
    $("#txtEndeudamientoPP").autoNumeric('set', '0');

    $('#txtCostoVida').autoNumeric('init', { aSign: ' %', pSign: 's' });
    $("#txtCostoVida").autoNumeric('set', '0');

    $('#txtComisionManejo').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtComisionManejo").autoNumeric('set', '0');

    $('#txtComision').autoNumeric('init', { aSign: ' %', pSign: 's' });
    $("#txtComision").autoNumeric('set', '0');

    $('#txtServicioDescuento').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtServicioDescuento").autoNumeric('set', '0');

    $('#txtProvisionSeguroVida').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtProvisionSeguroVida").autoNumeric('set', '0');

    $('#txtProvisionSeguroFraude').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtProvisionSeguroFraude").autoNumeric('set', '0');

    $('#txtTimbresFiscales').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtTimbresFiscales").autoNumeric('set', '0');

    $('#txtNotaria').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtNotaria").autoNumeric('set', '0');

    $('#txtTotalGastosCierre').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtTotalGastosCierre").autoNumeric('set', '0');

    $('#txtRecargoSeguroVida').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtRecargoSeguroVida").autoNumeric('set', '0');

    $('#txtSalarioMensual').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtSalarioMensual").autoNumeric('set', '0');

    $('#txtOtrosIngresos').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtOtrosIngresos").autoNumeric('set', '0');

    $('#txtTotalSaldosCancelar').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtTotalSaldosCancelar").autoNumeric('set', '0');

    $('#txtTotalSaldosNoCancela').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtTotalSaldosNoCancela").autoNumeric('set', '0');

    $('#txtTasa').autoNumeric('init', { aSign: ' %', pSign: 's' });
    $("#txtTasa").autoNumeric('set', '0');

    $('#txtMontoMaximo').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtMontoMaximo").autoNumeric('set', '0');

    $('#txtTasaEfectiva').autoNumeric('init', { aSign: 'B./ ' });
    $("#txtTasaEfectiva").autoNumeric('set', '0');

    $('#txtMontoBrutoRecibir').autoNumeric('init', { aSign: 'B./ ' });
    $('#txtMontoTotal').autoNumeric('init', { aSign: 'B./ ' });
    $('#txtLetraMaxima').autoNumeric('init', { aSign: 'B./ ' });


    //#endregion

    TableFeria_OnLoad();
    TableSegmento_OnLoad();
    TableTipoPrestamo_OnLoad();
    PrestamoPersonal_OnLoad();
});

//#region DEFINICION DE CATALOGOS

this.PrestamoPersonal_OnLoad = function () {

    var uri = "api/CalculoPPAPI/ObtenerPrestamoPersonal/?sol_co_solicitud=" + $("#hiddensol_co_solicitud").val() + "&cl_co_cliente=" + $("#hiddencl_co_cliente").val();

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            PrestamoPersonalJson = data;

            $("#hiddenCodFeria").val(PrestamoPersonalJson.DetCalculoPersonal.calp_co_feria);
            $("#txtFeria").val(PrestamoPersonalJson.FeriaDesc);

            $("#hiddenCodSegmento").val(PrestamoPersonalJson.DetCalculoPersonal.calp_co_segmento);
            $("#txtSegmento").val(PrestamoPersonalJson.SegmentoDesc);

            $("#hiddenCodTipoPrestamo").val(PrestamoPersonalJson.DetCalculoPersonal.calp_co_tipo_prestamo);
            $("#txtTipoPrestamo").val(PrestamoPersonalJson.TipoPrestamoDesc);

            $("#txtCargoPosicion").val(PrestamoPersonalJson.CargoPosicionDesc);
            $("#txtProfesion").val(PrestamoPersonalJson.ProfesionDesc);
            $("#txtLugarTrabajo").val(PrestamoPersonalJson.LugarTrabajoDesc);

            $("#txtNombreSolicitante").val(PrestamoPersonalJson.DetClienteDatosIniciales.cl_primer_nombre
                + " " + PrestamoPersonalJson.DetClienteDatosIniciales.cl_primer_apellido);

            $("#txtIdentificacionSolicitante").val(PrestamoPersonalJson.DetClienteDatosIniciales.cl_identificacion);


            $('#txtMontoPrestamo').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_monto_prestamo);
            $('#txtNumeroPagos').val(PrestamoPersonalJson.DetCalculoPersonal.calp_pagos);
            $('#txtFechaPrimerDescuento').dateFormatNoTime(PrestamoPersonalJson.DetCalculoPersonal.calp_fecha_primer_descuento);
            $('#txtFechaEfectivaPago').dateFormatNoTime(PrestamoPersonalJson.DetCalculoPersonal.calp_fecha_efectiva_pago);



            //Esto hay que remplazarlo por los valores que son
            $('#txtSalarioMensual').autoNumeric('set', PrestamoPersonalJson.DetClienteLaboral.cll_salario_mensual);
            $('#txtOtrosIngresos').autoNumeric('set', PrestamoPersonalJson.DetClienteLaboral.cll_otros_ingresos);

            $('#txtTotalSaldosCancelar').autoNumeric('set', PrestamoPersonalJson.SaldoCancelar);
            $('#txtTotalSaldosNoCancela').autoNumeric('set', PrestamoPersonalJson.SaldoNoCancelado);

            $('#txtCantidadCancelar').val(PrestamoPersonalJson.NumSaldoCancelar);
            $('#txtCantidadNoCancela').val(PrestamoPersonalJson.NumSaldoNoCancelado);


            CargaResultadoCalculo();

        }

        else {

            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });

}

this.TableFeria_OnLoad = function () {

    var uri = 'api/CalculoPPAPI/ObtenerFeriaPP/';

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            listaFerias = data;

        }

        else {

            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });

}

$("#btnBuscarFeria").click(function () {

    SelectedCatalog = "FERIA";

    if (listaFerias != null) {

        var Seleccionar = $("#ucBtnSeleccionar").html();

        var columnDefinition = [
            { "data": null, className: "center", defaultContent: Seleccionar },
            { "data": "co_feria" },
            { "data": "de_feria" }
        ];

        $('#tableResultadoCatalogo').TableInit(0, true, true, true, true, columnDefinition, listaFerias);

        $('#tableResultadoCatalogo').DataTable().search($("#txtFeria").val()).draw();

        $('#modalAgregarDescripcion').modal('show');

        UI.ENDREQUEST();
    }

    else {

        UI.ENDREQUEST();

        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
    }


});

this.TableSegmento_OnLoad = function () {

    var uri = 'api/CalculoPPAPI/ObtenerSegmentoPP/';

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            listaSegmento = data;
        }

        else {
            UI.ENDREQUEST();
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();
    });

}

$("#btnBuscarSegmento").click(function () {

    SelectedCatalog = "SEGMENTO";

    if (listaFerias != null) {

        var Seleccionar = $("#ucBtnSeleccionar").html();

        var columnDefinition = [
            { "data": null, className: "center", defaultContent: Seleccionar },
            { "data": "co_segmento_cliente" },
            { "data": "de_segmento_cliente" }
        ];

        $('#tableResultadoCatalogo').TableInit(0, true, true, true, true, columnDefinition, listaSegmento);

        $('#tableResultadoCatalogo').DataTable().search($("#txtSegmento").val()).draw();

        $('#modalAgregarDescripcion').modal('show');

        UI.ENDREQUEST();
    }

    else {

        UI.ENDREQUEST();

        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
    }


});

this.TableTipoPrestamo_OnLoad = function () {

    var uri = 'api/CalculoPPAPI/ObtenerTipoPP/';

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            listaTipoPrestamo = data;
        }

        else {
   
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        }

    }).fail(function (jqxhr, textStatus, error) {
        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();
    });

}

$("#btnBuscarTipoPrestamo").click(function () {

    SelectedCatalog = "TIPO_PRESTAMO";

    if (listaFerias != null) {

        var Seleccionar = $("#ucBtnSeleccionar").html();

        var columnDefinition = [
            { "data": null, className: "center", defaultContent: Seleccionar },
            { "data": "co_prestamo" },
            { "data": "de_prestamo" }
        ];

        $('#tableResultadoCatalogo').TableInit(0, true, true, true, true, columnDefinition, listaTipoPrestamo);

        $('#tableResultadoCatalogo').DataTable().search($("#txtTipoPrestamo").val()).draw();

        $('#modalAgregarDescripcion').modal('show');

        UI.ENDREQUEST();
    }

    else {

        UI.ENDREQUEST();

        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
    }


});

$("#tableResultadoCatalogo tbody td").live("click", "td", function (event) {

    var data = $('#tableResultadoCatalogo').DataTable().row($(this).parents('tr')).data();

    if (SelectedCatalog == "FERIA") {

        $('#hiddenCodFeria').val(data.co_feria);
        $('#txtFeria').val(data.de_feria);

        $("#formDatosPrestamo").validateElement('#txtFeria');
    }
    else if (SelectedCatalog == "SEGMENTO") {

        $('#hiddenCodSegmento').val(data.co_segmento_cliente);
        $('#txtSegmento').val(data.de_segmento_cliente);

        $("#formDatosPrestamo").validateElement('#txtSegmento');
    }
    else if (SelectedCatalog == "TIPO_PRESTAMO") {

        $('#hiddenCodTipoPrestamo').val(data.co_prestamo);
        $('#txtTipoPrestamo').val(data.de_prestamo);

        $("#formDatosPrestamo").validateElement('#txtTipoPrestamo');
    }


    $('#modalAgregarDescripcion').modal('hide');

});

//#endregion

//#region CONTROLES Y EVENTOS

$("#chkRefinanciamiento").click(function () {
    if ($("#chkRefinanciamiento").prop('checked') == true) {

        $("#btnBuscarRefinanciamiento").removeAttr('disabled', 'disabled');
    }
    else
    {
        $("#btnBuscarRefinanciamiento").attr('disabled', 'disabled');
    }

});

$("#btnFechaPrimerDescuento").click(function () {

    //var UI = new Ultimus.UI();

    //$("#tableFechaDescuento").DataTable().clear().destroy();

    //UI.dataTable_language_NoSearch("#tableFechaDescuento");
    //$('thead th').css({ "background-color": "rgb(186, 231, 137)" });
    //var DataTable = $('#tableFechaDescuento').DataTable();

    ////var DataTable = $('#tablePosicionCliente').DataTable({ "bInfo": false, "bPaginate": false, "bFilter": false });

    //var seleccionar = $("#ucBtnSeleccionarFecha").html();

    //DataTable.row.add([seleccionar, "11/11/2014", "15/12/2014", "16/12/2014", "17/12/2014", "2014"]).draw();
    //DataTable.row.add([seleccionar, "14/11/2014", "18/12/2014", "19/12/2014", "20/12/2014", "2014"]).draw();
    //$('#modalCalendarioFechaDescuento').modal('show');
});

$("#btnCalcularPersonal").click(function () {

    if (!$("#formDatosPrestamo").valid()) {

        toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
        return false;
    }

    //#region Obtener todos los datos necesarios para el calculo

    PrestamoPersonalJson.DetCalculoPersonal.calp_co_feria = $("#hiddenCodFeria").val();
    PrestamoPersonalJson.DetCalculoPersonal.calp_co_segmento = $("#hiddenCodSegmento").val();
    PrestamoPersonalJson.DetCalculoPersonal.calp_co_tipo_prestamo = $("#hiddenCodTipoPrestamo").val();
    PrestamoPersonalJson.DetCalculoPersonal.calp_monto_prestamo = $("#txtMontoPrestamo").autoNumeric('get');
    PrestamoPersonalJson.DetCalculoPersonal.calp_pagos = $("#txtNumeroPagos").val();

    //PrestamoPersonalJson.DetCalculoPersonal.calp_plazos = $("#txtPlazosMeses").val();
    PrestamoPersonalJson.DetCalculoPersonal.calp_fecha_primer_descuento = $("#txtFechaPrimerDescuento").dotNetFormat();
    PrestamoPersonalJson.DetCalculoPersonal.calp_recargo_seguro_vida = $("#txtRecargoSeguroVida").autoNumeric('get');
    PrestamoPersonalJson.DetCalculoPersonal.calp_fecha_efectiva_pago = $("#txtFechaEfectivaPago").dotNetFormat();

    //#endregion


    var uri = 'api/CalculoPPAPI/CalcularPrestamoPersonal';
    var DTO = JSON.stringify(PrestamoPersonalJson);

    $.ajax({
        type: 'POST',
        url: server + uri,
        cache: false,
        data: DTO,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {

            if (data.CodigoRespuesta == "EXITOSO") {

                PrestamoPersonalJson = data;

                CargaResultadoCalculo();


                toastr.success("Los datos fueron ingresados satisfactoriamente", Message_Warning);


            }
            else if (data.CodigoRespuesta == "ADVERTENCIA") {

                toastr.warning("No se pudo ingresar los datos Iniciales del Cliente. ", Message_Warning);

            }
            else {

                toastr.error("Ha ocurrido un error.", Message_Error);
            }

        }
    }).fail(function (jqxhr, textStatus, error) {

        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        UI.ENDREQUEST();

    }).then(function (value) {

        UI.ENDREQUEST();

    });
    

});

//#endregion


this.CargaResultadoCalculo = function () {

    //#region Obtener datos de petición

    $('#txtCapitalIntereses').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_capital_intereses);

    $('#txtSeguroVida').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_seguro_vida);
    $('#txtSeguroFraude').autoNumeric('set', 0);

    $('#txtFECI').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_feci);
    $('#txtMensualidadTotal').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_mensualidad_total);

    $('#txtCapacidadPago').autoNumeric('set', 0);
    $('#txtEndeudamientoTotal').autoNumeric('set', 0);
    $('#txtEndeudamientoPP').autoNumeric('set', 0);
    $('#txtCostoVida').autoNumeric('set', 0);

    $('#txtComisionManejo').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_comision_manejo);
    $('#txtComision').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_comision);
    $('#txtServicioDescuento').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_servicio_descuento);
    $('#txtProvisionSeguroVida').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_servicio_descuento);

    $('#txtProvisionSeguroFraude').autoNumeric('set', 0);
    $('#txtTimbresFiscales').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_timbres_fiscales);
    $('#txtNotaria').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_notaria);

    $('#txtTotalGastosCierre').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_total_gastos);

    $('#txtTasa').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_tasa);
    $('#txtTasaEfectiva').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_tasa_efectiva);
    $('#txtMesNumeroPago').val(PrestamoPersonalJson.DetCalculoPersonal.calp_mes_no_pago);
    $('#txtPlazoMaximo').val(PrestamoPersonalJson.DetCalculoPersonal.calp_plazo_maximo);
    $('#txtMontoMaximo').autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_monto_maximo);

    $('#txtFechaEfectivaPago').dateFormatNoTime(PrestamoPersonalJson.DetCalculoPersonal.calp_fecha_efectiva_pago);

    $("#txtPlazosMeses").val(PrestamoPersonalJson.DetCalculoPersonal.calp_plazos);

    $("#txtMontoBrutoRecibir").autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_monto_bruto_recibir);

    $("#txtMontoTotal").autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_monto_total);
    $("#txtLetraMaxima").autoNumeric('set', PrestamoPersonalJson.DetCalculoPersonal.calp_letra_maxima_permitida);
    $("#txtPeriodoGracia").val(PrestamoPersonalJson.DetCalculoPersonal.calp_periodo_gracia);

    $("#txtConSeguroVida").val(PrestamoPersonalJson.DetCalculoPersonal.calp_con_seguro_vida);

    //#endregion
}
