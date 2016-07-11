/*------- VARIABLES GLOBALES -------*/
var UI = new Ultimus.UI();

jQuery(document).ready(function () {

    /*### REGION INICIALIZAR CONTROLES ###*/
    GetTesoreria();

    //inicializa o establece la validacion a los formularios
    $("#formUrgente").EnableValidationToolTip();
    $("#formTraduccion").EnableValidationToolTip();
    $("#formNotaria").EnableValidationToolTip();

    Datepicker("#txtFechaLimite");

    ObtenerInstrucOtrosDeptos();

    /*### EVENTOS ###*/
    $("#btnEnviar").click(function () {
        GuardarInstruccionesOtrosDepartamentos(true);
    });

    /*---------------------- Tab Migracion -----------------------*/
    $("#rbTieneDependiente").click(function () {
        $('#divRequisitosDependientes').show(500);
    });

    $("#rbNoTieneDependiente").click(function () {
        $('#divRequisitosDependientes').hide(300);
    });

    $("#btnGuardarDatosFacturacionMig").click(function () {
        if (!$("#formInstMigDatosFactEntrega").valid()) {
            toastr.warning("Hay campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
        else {
            toastr.success("Campos de Facturacion guardados", Message_Success);
        }
    });

    $("#chkbInstMigCurrier").click(function () {
        var txtarea = document.getElementById("txtInstMigDireccionEnvio");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#btnSolTraMig").click(function () {
        var chk = document.getElementById("chkNotariaMig");
        if (chk.checked) {
            $('#solicitudNotariaTraduccionMig').slideToggle(500);
        }
    });

    $("#chkNotariaMig").click(function () {
        var txtarea = document.getElementById("txtaNotariaMig");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#btnSolTraMig2").click(function () {
        var chk = document.getElementById("chkTraduccionMig");
        if (chk.checked) {
            $('#solicitudNotariaTraduccion2Mig').slideToggle(500);
        }
    });

    $("#chkTraduccionMig").click(function () {
        var txtarea = document.getElementById("txtaTraduccionMig");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#btnTesoreriaMig").click(function () {
        var chk = document.getElementById("chkTesoreriaMig");
        if (chk.checked) {
            $('#solicitudDeTesoreriaMig').slideToggle(500);
        }
    });

    $("#chkTesoreriaMig").click(function () {
        var txtarea = document.getElementById("txtaTesoreriaMig");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    /*---------------------- END Migracion -----------------------*/

    /*---------------------- Tab Sociedades -----------------------*/
    $("#btnGuardarDatosFacturacion").click(function () {
        if (!$("#formInstDatosFactEntrega").valid()) {
            toastr.warning("Hay campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
        else {
            toastr.success("Campos de Facturacion guardados", Message_Success);
        }
    });

    $("#chkbInstCurrier").click(function () {
        var txtarea = document.getElementById("txtInstDireccionEnvio");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#btnSolTra").click(function () {
        var chk = document.getElementById("chkNotaria");
        if (chk.checked) {
            $('#solicitudNotariaTraduccion').slideToggle(500);
            $('#solicitudNotariaTraduccion').find('#TituloSolcitudNotariaTraduccion').html('Solicitud Notaría');
        }
    });

    $("#chkNotaria").click(function () {
        if (this.checked == true) {
            
            $("#txtProtocoliza").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $("#ddlNotaria").val('').removeClass("selectWarning").removeClass("error").tooltip("destroy");
            $("#txtSeccion").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $("#ddlModelo").val('').removeClass("selectWarning").removeClass("error").tooltip("destroy");
            $("#txtComparece").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $("#txtRefrenda").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $("#txtOtros").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $('#divNotaria').show(500);
        }
        else {
            $('#divNotaria').hide(300);
        }
    });

    $("#btnSolTra2").click(function () {
        var chk = document.getElementById("chkTraduccion");
        if (chk.checked) {
            $('#solicitudNotariaTraduccion2').slideToggle(500);
            $('#solicitudNotariaTraduccion2').find('#TituloSolcitudNotariaTraduccion').html('Solicitud Traduccíon');
        }
    });

    $("#chkTraduccion").click(function () {
        if (this.checked == true) {
            $("#txtaTraduccion").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $('#divTraduccion').show(500);
        }
        else {
            $("#txtaTraduccion").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $('#divTraduccion').hide(300);
        }
    });

    $("#chkUrgente").click(function () {
        if (this.checked == true) {
            $("#txtUrgencia").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $("#txtFechaLimite").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $('#divUrgente').show(500);
        }
        else {
            $("#txtUrgencia").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $("#txtFechaLimite").val('').removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
            $('#divUrgente').hide(300);
        }
    });

    $("#btnTesoreria").click(function () {
        var chk = document.getElementById("chkTesoreria");
        if (chk.checked) {
            $('#solicitudDeTesoreria').slideToggle(500);
        }
    });

    $("#chkTesoreria").click(function () {
        var txtarea = document.getElementById("txtaTesoreria");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    /*---------------------- END Sociedades -----------------------*/

    /*---------------------- Tab Fundacion -----------------------*/
    $("#btnDatFunCrearActualizar").click(function () {
        if (!$("#formInstFunDatosFundacion").valid()) {
            toastr.warning("Hay campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
    });

    $("#btnGuardarDatosFacturacionFun").click(function () {
        if (!$("#formInstFunDatosFactEntrega").valid()) {
            toastr.warning("Hay campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
        else {
            toastr.success("Campos de Facturacion guardados", Message_Success);
        }
    });

    $("#chkbInstFunCurrier").click(function () {
        var txtarea = document.getElementById("txtInstFunDireccionEnvio");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#btnSolTraFun").click(function () {
        var chk = document.getElementById("chkNotariaFun");
        if (chk.checked) {
            $('#solicitudNotariaTraduccionFun').slideToggle(500);
        }
    });

    $("#chkNotariaFun").click(function () {
        var txtarea = document.getElementById("txtaNotariaFun");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#btnSolTraFun2").click(function () {
        var chk = document.getElementById("chkTraduccionFun");
        if (chk.checked) {
            $('#solicitudNotariaTraduccion2Fun').slideToggle(500);
        }
    });

    $("#chkTraduccionFun").click(function () {
        var txtarea = document.getElementById("txtaTraduccionFun");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#btnTesoreriaFun").click(function () {
        var chk = document.getElementById("chkTesoreriaFun");
        if (chk.checked) {
            $('#solicitudDeTesoreriaFun').slideToggle(500);
        }
    });

    $("#chkTesoreriaFun").click(function () {
        var txtarea = document.getElementById("txtaTesoreriaFun");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    /*---------------------- END Fundacion -----------------------*/

    /*---------------------- Tab Naves -----------------------*/
    $("#btnGuardarDatosFacturacionNav").click(function () {
        if (!$("#formInstNavDatosFactEntrega").valid()) {
            toastr.warning("Hay campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
        else {
            toastr.success("Campos de Facturacion guardados", Message_Success);
        }
    });

    $("#chkbInstNavCurrier").click(function () {
        var txtarea = document.getElementById("txtInstNavDireccionEnvio");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#chkbDatNavLegApos").click(function () {
        var txtarea = document.getElementById("txtaDatNavLegApos");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#chkbDatNavLegConsult").click(function () {
        var txtarea = document.getElementById("txtaDatNavLegConsult");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#chkbDatNavLegNota").click(function () {
        var txtarea = document.getElementById("txtaDatNavLegNota");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#chkbDatNavTradu").click(function () {
        var txtarea = document.getElementById("txtaDatNavTradu");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#btnSolTraNav").click(function () {
        var chk = document.getElementById("chkNotariaNav");
        if (chk.checked) {
            $('#solicitudNotariaTraduccionNav').slideToggle(500);
        }
    });

    $("#chkNotariaNav").click(function () {
        var txtarea = document.getElementById("txtaNotariaNav");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#btnSolTraNav2").click(function () {
        var chk = document.getElementById("chkTraduccionNav");
        if (chk.checked) {
            $('#solicitudNotariaTraduccion2Nav').slideToggle(500);
        }
    });

    $("#chkTraduccionNav").click(function () {
        var txtarea = document.getElementById("txtaTraduccionNav");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    $("#btnTesoreriaNav").click(function () {
        var chk = document.getElementById("chkTesoreriaNav");
        if (chk.checked) {
            $('#solicitudDeTesoreriaNav').slideToggle(500);
        }
    });

    $("#chkTesoreriaNav").click(function () {
        var txtarea = document.getElementById("txtaTesoreriaNav");
        if (this.checked == true) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
            txtarea.value = "";
        }
    });

    /*---------------------- END Naves -----------------------*/
});

this.GetTesoreria = function () {
    var uri = "api/InstruccionOtrosDepartamentosApi/GetTesoreria?incidente=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()) +
        "&solicitud=" + $('#hiddensol_co_solicitud').val();

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            tesorerias = data

            var columnDefinition = [
                { "data": "Tipo" },
                { "data": "Beneficiario" },
                { "data": "CedRuc" },
                { "data": "Monto" }
            ];

            $('#tbTesoreria').TableInit(0, true, true, false, true, columnDefinition, tesorerias);

            var tablaTesorerias = $('#tbTesoreria').DataTable();

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

function GuardarTab() {
    var ret = GuardarInstruccionesOtrosDepartamentos(false);
    return ret;
}

this.GuardarInstruccionesOtrosDepartamentos = function (enviar) {
    var formUrgenteValid = true;
    var formTraduccionValid = true;
    var formNotariaValid = true;


    if ($("#chkUrgente").is(':checked')) {
        var formUrgente = $("#formUrgente");
        formUrgente.validate();
        formUrgenteValid = formUrgente.valid();
    }

    if ($("#chkTraduccion").is(':checked')) {
        var formTraduccion = $("#formTraduccion");
        formTraduccion.validate();
        formTraduccionValid = formTraduccion.valid();
    }

    if ($("#chkNotaria").is(':checked')) {
        var formNotaria = $("#formNotaria");
        formNotaria.validate();
        formNotariaValid = formNotaria.valid();
    }

    if (!(formUrgenteValid && formTraduccionValid && formNotariaValid)) {
        toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
        return false;
    }
    
    return SaveInstruccionesOtrosDepartamentos(enviar);
}

this.SaveInstruccionesOtrosDepartamentos = function (enviar) {

    $("#btnEnviar").prop('disabled', true);

    var instruccionOtrosDepartamentos = {}
    instruccionOtrosDepartamentos.Incidente = $('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val();
    instruccionOtrosDepartamentos.Instrucciones = ($('#chkTraduccion').is(':checked') ? '1' : '0') + ($('#chkAutenticacion').is(':checked') ? '1' : '0') + ($('#chkNotaria').is(':checked') ? '1' : '0') + ($('#chkApostilla').is(':checked') ? '1' : '0') + ($('#chkMinrexGobJustici').is(':checked') ? '1' : '0') + ($('#chkConsulado').is(':checked') ? '1' : '0') + ($('#chkSeInscribe').is(':checked') ? '1' : '0') + ($('#chkAlteracionTurno').is(':checked') ? '1' : '0') + ($('#chkAperturaCuenta').is(':checked') ? '1' : '0'),
    instruccionOtrosDepartamentos.Urgente = $("#chkUrgente").is(':checked');
    instruccionOtrosDepartamentos.UrgenteComentario = $("#chkUrgente").is(':checked') ? $("#txtUrgencia").val() : null;
    instruccionOtrosDepartamentos.FechaLimiteEntrega = $("#chkUrgente").is(':checked') ? $("#txtFechaLimite").dotNetFormat() : null;
    instruccionOtrosDepartamentos.TraduccionDescripcion = $("#chkTraduccion").is(':checked') ? $("#txtaTraduccion").val() : null;
    instruccionOtrosDepartamentos.NotariaProtocoliza = $("#chkNotaria").is(':checked') ? $("#txtProtocoliza").val() : null;
    instruccionOtrosDepartamentos.NotariaNotaria = $("#chkNotaria").is(':checked') ? $("#ddlNotaria").val() : null;
    instruccionOtrosDepartamentos.NotariaSeccion = $("#chkNotaria").is(':checked') ? $("#txtSeccion").val() : null;
    instruccionOtrosDepartamentos.NotariaModelo = $("#chkNotaria").is(':checked') ? $("#ddlModelo").val() : null;
    instruccionOtrosDepartamentos.NotariaComparece = $("#chkNotaria").is(':checked') ? $("#txtComparece").val() : null;
    instruccionOtrosDepartamentos.NotariaRefrenda = $("#chkNotaria").is(':checked') ? $("#txtRefrenda").val() : null;
    instruccionOtrosDepartamentos.NotariaOtros = $("#chkNotaria").is(':checked') ? $("#txtOtros").val() : null;
    instruccionOtrosDepartamentos.Solicitud = $('#hiddensol_co_solicitud').val();

    var uri = "api/InstruccionOtrosDepartamentosApi/RegistrarInstrucOtrosDeptos";

    $.ajax({
        url: server + uri,
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(instruccionOtrosDepartamentos),
        error: function (jqXHR, textStatus, errorThrown) {
            //jqXHR.responseText
            UI.ENDREQUEST();
            $("#btnEnviar").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

            return false;
        },
        success: function (response, textStatus, jqXHR) {
            UI.ENDREQUEST();

            if (enviar) {
                CompletarTarea();
            }                
            else {
                return true;
            }                
        }
    });
}

this.ObtenerInstrucOtrosDeptos = function () {
    var uri = "api/InstruccionOtrosDepartamentosApi/ObtenerInstrucOtrosDeptos?incidente=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val())
         + "&solicitud=" + $('#hiddensol_co_solicitud').val() ;
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            SetData(data);
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.SetData = function (data) {
    var instruccionOtrosDepartamentos = data;

    $("#chkTraduccion").attr('checked', instruccionOtrosDepartamentos.Traduccion);
    $("#chkAutenticacion").attr('checked', instruccionOtrosDepartamentos.Autenticacion);
    $("#chkNotaria").attr('checked', instruccionOtrosDepartamentos.Notaria);
    $("#chkApostilla").attr('checked', instruccionOtrosDepartamentos.Apostilla);
    $("#chkMinrexGobJustici").attr('checked', instruccionOtrosDepartamentos.MinrexGobJustici);
    $("#chkConsulado").attr('checked', instruccionOtrosDepartamentos.Consulado);
    $("#chkSeInscribe").attr('checked', instruccionOtrosDepartamentos.SeInscribe);
    $("#chkAlteracionTurno").attr('checked', instruccionOtrosDepartamentos.AlteracionTurno);
    $("#chkAperturaCuenta").attr('checked', instruccionOtrosDepartamentos.AperturaCuenta);
    $("#chkUrgente").attr('checked', instruccionOtrosDepartamentos.Urgente);

    if ($("#chkUrgente").is(':checked')) {
        $("#txtUrgencia").val(instruccionOtrosDepartamentos.UrgenteComentario);
        $("#txtFechaLimite").dateFormatNoTime(instruccionOtrosDepartamentos.FechaLimiteEntrega);
        $('#divUrgente').show(500);
    }

    if ($("#chkTraduccion").is(':checked')) {
        $("#txtaTraduccion").val(instruccionOtrosDepartamentos.TraduccionDescripcion);
        $('#divTraduccion').show(500);
    }

    if ($("#chkNotaria").is(':checked')) {
        $("#txtProtocoliza").val(instruccionOtrosDepartamentos.NotariaProtocoliza);
        $("#ddlNotaria").val(instruccionOtrosDepartamentos.NotariaNotaria);
        $("#txtSeccion").val(instruccionOtrosDepartamentos.NotariaSeccion);
        $("#ddlModelo").val(instruccionOtrosDepartamentos.NotariaModelo);
        $("#txtComparece").val(instruccionOtrosDepartamentos.NotariaComparece);
        $("#txtRefrenda").val(instruccionOtrosDepartamentos.NotariaRefrenda);
        $("#txtOtros").val(instruccionOtrosDepartamentos.NotariaOtros);
        $('#divNotaria').show(500);
    }
}

this.CompletarTarea = function () {
    var uri = "api/ProcesosLegalesApi/CompletarTarea?proceso=" + $("#hiddenProcess").val()
                                            + "&tempIncident=" + $('#hiddenIncident').val()
                                            + "&userID=" + $("#hiddenUserID").val()
                                            + "&taskID=" + $("#hiddenTaskId").val();

    $.getJSON(server + uri).done(function (data) {
        if (data > 0) {
            toastr.success("Registro guardado. Incidente " + data, Message_Success);
            setTimeout(function () {
                location.href = "Completado";
            }, 3000);
        }
        else {
            $("#btnEnviar").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        }
    }).fail(function (jqxhr, textStatus, error) {
        UI.ENDREQUEST();
        $("#btnEnviar").prop('disabled', false);
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
    }).then(function (value) {
        UI.ENDREQUEST();
    });
}