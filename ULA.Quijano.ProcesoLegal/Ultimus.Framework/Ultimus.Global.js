var server = "http://" + location.host + "/ULA.Quijano.ProcesoLegal/";
var Ultimus = {};

var ULT = {};
var tableBitacora = {};

//#region MOMENT CONFIGURATION (DATE AND TIME CONFIGURATION)

moment.lang('es-PA', {
    months: [
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio",
        "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
    ]
});

moment.lang('es-PA');

//#endregion

//#region GENERIC MESSAGE

var Message_Success = "PROCESO COMPLETADO";
var Message_Warning = "ADVERTENCIA DEL SISTEMA";
var Message_Info = "INFORMACIÓN DEL SISTEMA";
var Message_Error = "ERROR INESPERADO";

var MessageFull_Error = "Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde";

//#endregion

//#region JQUERY.VALIDATE CONFIGURATION

jQuery.extend(jQuery.validator.messages, {
    required: "",
    email: "CORREO INCORRECTO: Por favor introducir un correo electrónico valido. Asegúrese de no tener espacios en blanco ni caracteres especiales",
    number: "NUMERO INCORRECTO: Por favor introducir un numero valido. Asegúrese de no tener espacios en blanco ni caracteres especiales"
});

jQuery.validator.addMethod("lettersonly", function (value, element) {
    return this.optional(element) || /^[a-z\s]+$/i.test(value);
}, "SOLO LETRAS");




//#endregion

//#region Obsolete methods

// Metodo exclusivamente para redireccionar la lista de opciones de datos generales
this.RedireccionandoListaDatosPersonales = function (parametroEncriptar, participantes, controladorAction) {

    var uri = 'api/SolicitudAPI/EncryptarParametro?cedulaParticipante=' + parametroEncriptar;

    var UI = new Ultimus.UI();

    $.getJSON(server + uri).done(function (data) {

        var url;
        if (data != "") {

            UI.ENDREQUEST();

            if (participantes != "") {

                url = server + controladorAction + $("#hiddenQueryEncripted").val() + '&ParticipanteID=' + participantes + '&identificacion=' + data;
                window.location = url;
            }
            else {
                url = server + controladorAction + $("#hiddenQueryEncripted").val() + '&identificacion=' + data;
                window.location = url;
            }
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

this.OpcionesDatosGenerales = function (id, identificacion, participantes) {

    switch (id) {
        case "listDatosGenerales":
            RedireccionandoListaDatosPersonales(identificacion, participantes, 'Solicitantes/DatosGenerales');
            break;
        case "listInfoLaboral":
            RedireccionandoListaDatosPersonales(identificacion, participantes, 'Solicitantes/InfoLaboral');
            break;
        case "listCompromisos":
            RedireccionandoListaDatosPersonales(identificacion, participantes, 'Solicitantes/Compromisos');
            break;
        case "listReferenciasPersonales":
            RedireccionandoListaDatosPersonales(identificacion, participantes, 'Solicitantes/ReferenciasPersonales');
            break;
    }
}

$("#btnBitacoraComentarios").click(function () {
    ObtenerBitacoraComentarios();
});

this.ObtenerBitacoraComentarios = function () {

    var uri = 'api/Generales/ObtenerBitacoraComentarios?proceso=' + $('#hiddenProcess').val() + '&incidente=' +
              ($('#hiddenIncident').val() != '' && $('#hiddenIncident').val() != '0' ? $('#hiddenIncident').val() : $('#hiddenTempIncident').val())
              + '&solicitud=' + $('#hiddensol_co_solicitud').val();

    $.getJSON(server + uri).done(function (data) {

        tableBitacora.clear().draw();

        // On success, 'data' contains a list of products.
        $.each(data, function (key, item) {
            //tableBitacora.row.add([item.Usuario, DateFormatNoTime(item.Fecha), item.Texto]).draw();
            tableBitacora.row.add([item.Usuario, item.strFecha, item.Fecha, item.Texto]).draw();
        });

        $('#modalBitacora').modal('show');

        $('#tableBitacora tr td:nth-child(3)').css({'text-align': 'left'});
 
    }).fail(function (jqxhr, textStatus, error) {

        toastr.error("Ha ocurrido un error inesperado. Vuela a intentarlo mas tarde");
        console.log("Request Failed: " + textStatus + ', ' + error);

    }).then(function (value) {


        ULT.ENDREQUEST();

    });

};

this.GuardarBitacoraComentarios = function () {

    var uri = 'api/Generales/GuardarBitacoraComentarios?proceso=' + $('#hiddenProcess').val() + '&incidente=' +
           ($('#hiddenIncident').val() != '' && $('#hiddenIncident').val() != '0' ? $('#hiddenIncident').val() : $('#hiddenTempIncident').val()) +
           '&usuario=' + $('#hiddenUserID').val() + '&comentario=' + encodeURIComponent($('#txtComentarioBitacora').val()) +
           '&solicitud=' + $('#hiddensol_co_solicitud').val();

    $.getJSON(server + uri).done(function (data) {

        if (data == true) {
            $('#txtComentarioBitacora').val("");
            toastr.success("Se ha guardado su comentario exitosamente.", "Comentario Guardado");
            ObtenerBitacoraComentarios();
        }
        else {
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", "Error");
        }

    }).fail(function (jqxhr, textStatus, error) {

        toastr.error("Ha ocurrido un error inesperado. Vuela a intentarlo mas tarde");
        console.log("Request Failed: " + textStatus + ', ' + error);

    }).then(function (value) {

        ULT.ENDREQUEST();

    });

};

$("#btnSaveComment").click(function () {  
     if ($("#hiddensol_co_solicitud").val() == "0") {
            var url = "api/ProcesosLegalesApi/DevuelveCodSolicitud?codigo=0"
            $.getJSON(server + url, function (data) {                             
                $("#hiddensol_co_solicitud").val(data);
               
            }).done(function () {
               GuardarBitacoraComentarios();
            });
     }
     else {
             GuardarBitacoraComentarios();
     }
   
});

this.DisabledToRequired = function (controlId) {

    /// <summary>Put a control with Required atribute and remove Disabled atribute.</summary>
    /// <param name="ControlId" type="text">Control ID of text input control</param>;



    $(controlId).attr("required", true);
    $(controlId).removeAttr("disabled");

};

this.RequiredToDisabled = function (controlId) {

    /// <summary>Put a control with Disabled atribute and remove Required atribute.</summary>
    /// <param name="ControlId" type="text">Control ID of text input control</param>;

    $(controlId).attr("disabled", true);
    $(controlId).removeAttr("required");

};

this.DatepickerDateBirth = function (ControlId) {

    /// <summary>Method for asing Datepciker that start from 18 year ago to a text input html control.</summary>
    /// <param name="ControlId" type="text">Control ID of text input control</param>

    var _dateObject = new Date();
    var _yearInit = _dateObject.getFullYear() - 18;

    _dateObject.setFullYear(_yearInit);

    $(ControlId).datepicker({ yearRange: '1945:' + ((new Date).getFullYear() - 18), defaultDate: _dateObject }).datepicker('widget').wrap('<div class="ll-skin-cangas"/>');

};

this.Datepicker = function (ControlId) {

    /// <summary>Method for asing Datepciker to a text input html control.</summary>
    /// <param name="ControlId" type="text">Control ID of text input control</param>;


    var _dateObject = new Date();
    var _yearInit = _dateObject.getFullYear();

    _dateObject.setFullYear(_yearInit);

    $(ControlId).datepicker({ yearRange: '1945:' + ((new Date).getFullYear() + 50), defaultDate: _dateObject }).datepicker('widget').wrap('<div class="ll-skin-cangas"/>');


};

this.SendForm = function (form) {

    if (!$(form).valid()) {
        return false;
    }
    else {

        LoadMessage();
        form.submit();
    }
};

this.CleanValidationMessage = function (value) {

    $(value + " input, " + value + " select").each(function (key, value) {



        var $element = $("#" + value.id);

        $element.removeClass("selectWarning");
        $element.removeClass("inputWarning");
        $element.removeClass("inputSuccess");


        $element.data("title", "").removeClass("error").tooltip("destroy");
    });

};


this.EnableErrorToolTip = function (value) {


    $(value).validate({

        ignore: "not:hidden",

        showErrors: function (errorMap, errorList) {

            // Clean up any tooltips for valid elements
            $.each(this.validElements(), function (index, element) {
                var $element = $(element);

                if ($element.prop("type") == "select-one") {

                    if ($element.attr('required')) {

                        $element.removeClass("selectWarning").addClass("inputSuccess");

                    }

                }
                else {

                    if ($element.attr('required')) {

                        $element.removeClass("inputWarning").addClass("inputSuccess");
                    }

                }



                if ($element.data("role") != undefined) {

                    $("#" + $element.data("role")).removeClass("error").tooltip("destroy");

                } else {

                    $element.data("title", "").removeClass("error").tooltip("destroy");

                }
            });

            // Create new tooltips for invalid elements
            $.each(errorList, function (index, error) {
                var $element = $(error.element);


                if ($element.prop("type") == "select-one") {

                    $element.removeClass("inputSuccess").addClass("selectWarning");

                }
                else {

                    $element.removeClass("inputSuccess").addClass("inputWarning");

                }

                if ($element.data("role") != undefined) {



                    $("#" + $element.data("role")).tooltip("destroy").data("title", error.message).addClass("error")
.tooltip({ placement: 'right', trigger: 'manual', container: 'body' }).tooltip('show');

                } else {

                    $element.tooltip("destroy").data("title", error.message).addClass("error")
      .tooltip({ placement: 'right', trigger: 'manual', container: 'body' }).tooltip('show');

                }


            });
        }




    });

};


this.IsEnableTab = function (tab, IsEnable) {

    if (IsEnable) {

        $("#li" + tab).removeAttr("style");
        $("#a" + tab).removeAttr("style");

    }
    else {

        $("#li" + tab).attr("style", "cursor:not-allowed");
        $("#a" + tab).attr("style", "pointer-events:none;");

    }



};



this.DecimalParse = function (value) {

    try {

        return Number(value.replace(/[^0-9\.]+/g, ""));
    }
    catch (ex) {

        return "0";
    }

};

this.IsValidValue = function (value) {

    var isValid = true;

    if (value == null)
        isValid = false;

    if (value == 0)
        isValid = false;

    if (value == -1)
        isValid = false;

    if (value == "")
        isValid = false;


    return isValid;

};

this.DataEnBlanco = function (value) {

    if (value != null && value != "0" && value != "-1")
        return value;
    else
        return "";

};

this.EnviarIncidente = function () {

    var UI = new Ultimus.UI();
    UI.ENDREQUEST();


    //var uri = 'api/Generales/ActualizarDatosGenerales';

    //var DTO = JSON.stringify(ObligacionDetalle);

    //$.ajax({
    //    type: 'POST',
    //    url: server + uri,
    //    cache: false,
    //    data: DTO,
    //    contentType: 'application/json; charset=utf-8',
    //    dataType: 'json',
    //    success: function (data) {

    //        if (data == "EXITOSO") {
    //            toastr.success('Informacion ha sido guardada exitosamente');
    //        }
    //        else {
    //            toastr.error("Ha ocurrido un error inesperado al guardar los datos generales del solicitante. Por favor contactar al administrador.");
    //            console.log("Request Failed: " + data);
    //        }

    //    }
    //}).fail(function (jqxhr, textStatus, error) {

    //    toastr.error("Ha ocurrido un error inesperado. Vuela a intentarlo mas tarde");

    //    console.log("Request Failed: " + textStatus + ', ' + error);

    //}).then(function (value) {



    //});
};

this.DateFormat = function (value) {

    if (value != null && value != "") {
        var fecha = new Date(value);
        return moment(fecha.toGMTString()).format("DD-MMMM-YYYY, h:hh A");
    }
    else
        return "";

};


this.DateFormatShort = function (value) {

    if (value != null && value != "") {
        var fecha = new Date(value);
        return moment(fecha.toGMTString()).format("DD-MMMM-YYYY");
    }
    else
        return "";

};



var string_empty = "";

// Cambiar texto titulo H (tag)
this.ChangeModalHeadingText = function (Htag, className, text) {
    $(Htag + '.' + className).text(text);
}

// Verificar formato moneda
this.VerifyCurrency = function (id) {

    var valor = $("#" + id).val();

    if (valor != "") {
        var dotIndex;

        if (valor.indexOf(",") != -1) {
            valor = valor.replace(",", "");
        }

        if (valor.indexOf(".") != -1) {
            dotIndex = valor.indexOf(".");
        }

        var regEx = null;

        switch (dotIndex) {
            case 4:
                regEx = /^\$?([0-9]{1})([0-9]{3}|[0-9]{1,3})(\.[0-9]{2})?$/;
                break;
            case 5:
                regEx = /^\$?([0-9]{2})([0-9]{3}|[0-9]{1,3})(\.[0-9]{2})?$/;
                break;
            case 6:
                regEx = /^\$?([0-9]{2})([0-9]{3}|[0-9]{1,3})(\.[0-9]{2})?$/;
                break;
        }

        if (regEx != null) {
            $("#" + id).val(valor.replace(regEx, "$1,$2$3"));
        }
        else if (dotIndex >= 4)
            toastr.warning("Verifique que haya introducido una cifra válida", "Salario Mensual");
        $("#" + id).focus();

    }
}

// Verificar tamano de cadena
this.VerificarStringLength = function (inputID, btnID) {

    if ($("#" + inputID).val().length >= 2) {

        $("#" + btnID).removeAttr('disabled');
        return true;
    }
    else {
        $("#" + btnID).attr('disabled', 'disabled');
        return false;
    }
}

// Permitir Letras y numeros
this.LettersNumbersOnly = function (evt) {
    evt = (evt) ? evt : event;

    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
      ((evt.which) ? evt.which : 0));

    if ((charCode >= 48 && charCode <= 57) ||
        (charCode >= 65 && charCode <= 90) ||
        (charCode >= 97 && charCode <= 122) ||
         charCode == 8 || charCode == 32 || charCode == 180) { // 8 = backspace, 32 = space, 180 = apostrofe 
        return true;
    }
    return false;
};

// Permitir solo Letras
this.LettersOnly = function (evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
      ((evt.which) ? evt.which : 0));
    if (charCode > 32 && (charCode < 65 || charCode > 90) &&
      (charCode < 97 || charCode > 122)) {
        return false;
    }
    else
        return true;
};

// formato numero telefonico residencial
this.VerifyPhoneNumber = function (id) {

    if ($("#" + id).val() != "") {

        var formatoCorrectoNumeroTelefono = ($("#" + id).val().match(/\b\d{3}[-]\d{4}\b/)) ? true : false;
        //alert(formatoCorrecto);

        if (formatoCorrectoNumeroTelefono == false) {
            var numeroTelefono = $("#" + id).val();

            if (numeroTelefono.indexOf("-") != -1) {
                numeroTelefono = numeroTelefono.replace("-", "");
            }

            if (numeroTelefono.length == 7) {

                //reformat  phone number
                $("#" + id).val(numeroTelefono.replace(/(\d{3})(\d{4})/, "$1-$2"));

            }
            else {
                toastr.warning("Verifique haya introducido 7 números", "Ingresar Datos Requeridos");
                $("#" + id).val("");
                $("#" + id).focus();
            }
        }
    }
}

// formato numero de celular
this.VerifyMobileNumber = function (id) {

    if ($("#" + id).val() != "") {

        var formatoCorrectoNumeroCelular = ($("#" + id).val().match(/\b\d{4}[-]\d{4}\b/)) ? true : false;
        //alert(formatoCorrecto);

        if (formatoCorrectoNumeroCelular == false) {
            var numeroCelular = $("#" + id).val();

            if (numeroCelular.indexOf("-") != -1) {
                numeroCelular = numeroCelular.replace("-", "");
            }

            if (numeroCelular.length == 8) {

                //reformat  phone number
                $("#" + id).val(numeroCelular.replace(/(\d{4})(\d{4})/, "$1-$2"));

            }
            else {

                toastr.warning("Verifique haya introducido 8 números", "Ingresar Datos Requeridos");
                $("#" + id).val("");
                $("#" + id).focus();
            }
        }
    }

}

// Solo permite numeros -- Segun Caracteres
this.OnlyNumbers = function (event) {
    var charCode = (event.which) ? event.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

// Solo permite numeros -- Segun Caracteres
this.OnlyNumbersAndDot = function (event, id) {
    var charCode = (event.which) ? event.which : event.keyCode

    if (charCode == 46) {

        var valor = $("#" + id).val();

        var permitirPunto = (valor.indexOf('.') != -1) ? false : true;

        return permitirPunto;

    }

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

this.replaceAll = function (find, replace, str) {
    return str.replace(new RegExp(find.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g'), replace);
};

this.FormReadOnly = function () {
    $('input,select').attr('disabled', true);
    $('input,select').removeAttr('required', true);
};

//#endregion

$(document).ajaxStart(function () {

    LoadMessage();

});

jQuery(document).ready(function () {

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-right",
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "10000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "swing",
        "showMethod": "slideDown",
        "hideMethod": "slideUp"
    }

    ULT = new Ultimus.UI();
    ULT.DataTable_Spanish("#tableBitacora", 2, "desc", false, false, false);
    tableBitacora = $('#tableBitacora').DataTable();

    $("form").submit(function () {
        if (!$(this).valid()) {
            return false;
        }
        else {

            LoadMessage();

            return true;
        }
    });

    $("[data-toggle=tooltip]").tooltip({ placement: 'right' });

    $('[data-toggle="popover"]').popover({ trigger: 'hover', 'placement': 'top' });

    jQuery('[data-confirm]').click(function (e) {
        if (!confirm(jQuery(this).attr("data-confirm"))) {
            e.preventDefault();
        }
        else {

            LoadMessage();

        }
    });

    jQuery('[data-toggle="tab"]').click(function (e) {

        jQuery('[class="nav-active"]').attr("class", "");
        jQuery(this).attr("class", "nav-active");
        //window.scrollTo(0, 0);

    });

});

this.LoadToastContainer = function (form) {

    if ($("#toast-container").length > 0) {
        var $div = $('<div />').appendTo('body');
        $div.attr('id', 'toast-container');
        $div.attr('class', 'toast-top-right');
    }

};

this.LoadMessage = function (Message) {

    if (Message == null) {

        Message = "Cargando Datos, Espere por favor!";
    }

    if ($("#toast-container").length > 0) {
        var $div = $('<div />').appendTo('body');
        $div.attr('id', 'toast-container');
        $div.attr('class', 'toast-top-right');
    }

    $("#toast-container").empty();

    var stringHtml = "";

    stringHtml += "<div id=\"loadingGif\" class=\"toast toast-default\"  style=\"font-size:14px;\">";
    stringHtml += "<table>";
    stringHtml += "<tr>";
    stringHtml += "<td><img src=\"../../../../ULA.Quijano.ProcesoLegal/Images/ajax-loader.gif\" alt=\"loading\" height=\"50\" width=\"50\"></td>";
    //stringHtml += "<td><i class=\"fa fa-spinner fa-spin fa-4x\"></i></td>";
    stringHtml += "<td>&nbsp;&nbsp;</td>";
    stringHtml += "<td>";
    stringHtml += "<div class=\"toast-title\">" + Message + "</div>";
    stringHtml += "</td>";
    stringHtml += "</tr>";
    stringHtml += "</table>";
    stringHtml += "</div>";

    $("#toast-container").append(stringHtml);

};

//#region ULTIMUS DATEPIKER CONFIGURATION

jQuery(function ($) {
    $.datepicker.regional['es'] = {
        closeText: 'Cerrar',
        prevText: '&#x3c;Ant',
        nextText: 'Sig&#x3e;',
        currentText: 'Hoy',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
        'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
        weekHeader: 'Sm',
        dateFormat: 'dd-MM-yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: '',
        yearRange: '1945:' + (new Date).getFullYear(),
        changeMonth: true,
        changeYear: true
    };
    $.datepicker.setDefaults($.datepicker.regional['es']);

});



//#endregion

//#region Manejo de Catálogos

this.SeleccionarOpcion = function (id, description, controlID, controlDescription, modal, form) {

    $(controlID).val(id);
    $(controlDescription).val(description);
    $(modal).modal('hide');


}

//#endregion

//#region ULTIMUS PROPERTIES AND EXTENTION

//#region General Control extentions

$.fn.Lista = function () {

    this.append("<div class=\"input-group padding-bottom\">" +
                    "<span class=\"input-group-addon\"> <input type=\"checkbox\" onchange=\"if(this.checked) $(this).ChequearElementoLista(); else $(this).EliminarElementoLista(); \" /> &nbsp;&nbsp; <strong></strong> </span>" +
                    "<input type=\"text\" class=\"form-control\" disabled />" +
                "</div>");

};

$.fn.ChequearElementoLista = function () {

    var txt = this.parent().next();
    var div = this.parent().parent().parent();

    txt.removeAttr("disabled");
    txt.focus();
    div.Lista();
    div.EnumerarElementosLista();
    div.scrollTop(div[0].scrollHeight);
};

$.fn.EliminarElementoLista = function () {

    var div = this.parent().parent().parent();
    this.parent().parent().remove();
    div.EnumerarElementosLista();
};

$.fn.EnumerarElementosLista = function () {

    var l = this.children("div").length;
    for (i = 0; i < l - 1; i++)
        this.children().eq(i).find("strong").text(i + 1);
};

$.fn.ListaInstrucciones = function (datos) {

    for (i = 0; i < datos.length; i++) {
        this.append(
            "<div class=\"row\">" +
                "<div class=\"col-xs-5\">" +
                    "<div class=\"input-group padding-bottom\">" +
                        "<span class=\"input-group-addon span-input-white\">" +
                            "<img src=\"" + (datos[i].respuesta != null ? "/ULA.Quijano.ProcesoLegal/Images/check-circle-20.png" : "/ULA.Quijano.ProcesoLegal/Images/warning-20.png") + "\" class=\"center-block\" />" +
                        "</span>" +
                        "<textarea class=\"form-control\" rows=\"1\" readonly>" + (i + 1) + ") " + datos[i].instruccion + "</textarea>" +
                    "</div>" +
                "</div>" +
                "<div class=\"col-xs-5\">" +
                    "<textarea class=\"form-control\" rows=\"1\" onchange=\"RespuestaInstruccion(this);\" " + (datos[i].respuesta != null ? "readonly" : "") + ">" + (datos[i].respuesta != null ? datos[i].respuesta : "") + "</textarea>" +
                "</div>" +
                "<div class=\"col-xs-2\">" +
                    "<span class=\"usuario\">" + (datos[i].usuario != null ? datos[i].usuario : "") + "</span>" +
                "</div>" +
            "</div>" +
            "<hr />");
    }

};

this.RespuestaInstruccion = function (txt) {
    $(txt).parent().parent().find("img").attr("src", ($(txt).val().trim() != "" ? "/ULA.Quijano.ProcesoLegal/Images/check-circle-20.png" : "/ULA.Quijano.ProcesoLegal/Images/warning-20.png"));
    $(txt).parent().parent().find(".usuario").text($(txt).val().trim() != "" ? $("#hiddenUserID").val() : "");
};

$.fn.disable = function () {

    this.attr('disabled', true);

};

$.fn.enable = function () {

    this.attr('disabled', false);

};

$.fn.required = function () {

    this.attr('required', true);

};

$.fn.noRequired = function () {

    this.attr('required', false);

};

$.fn.requiredContainer = function () {

    this.find(".form-control").attr('required', true);

};

$.fn.noRequiredContainer = function () {

    this.find(".form-control").attr('required', false);

};

$.fn.EnableValidationToolTip = function () {

    this.validate({

        ignore: "not:hidden",

        showErrors: function (errorMap, errorList) {

            $.each(this.validElements(), function (index, element) {
                var $element = $(element);

                if ($element.prop("type") == "select-one") {
                    if ($element.attr('required')) {
                        $element.removeClass("selectWarning").addClass("inputSuccess");
                    }
                    else {
                        $element.removeClass("selectWarning");
                    }
                }
                else {

                    if ($element.attr('required')) {
                        $element.removeClass("inputWarning").addClass("inputSuccess");
                    }
                    else {
                        $element.removeClass("inputWarning");
                    }
                }

                if ($element.data("role") != undefined) {
                    $("#" + $element.data("role")).removeClass("error").tooltip("destroy");
                } else {
                    $element.data("title", "").removeClass("error").tooltip("destroy");
                }
            });

            $.each(errorList, function (index, error) {
                var $element = $(error.element);

                if ($element.prop("type") == "select-one") {
                    $element.removeClass("inputSuccess").addClass("selectWarning");
                }
                else {
                    $element.removeClass("inputSuccess").addClass("inputWarning");
                }

                if ($element.data("role") != undefined) {
                    $("#" + $element.data("role")).tooltip("destroy").data("title", error.message).addClass("error").tooltip({ placement: 'right', trigger: 'manual', container: 'body' }).tooltip('show');
                } else {
                    $element.tooltip("destroy").data("title", error.message).addClass("error").tooltip({ placement: 'right', trigger: 'manual', container: 'body' }).tooltip('show');
                }
            });
        }
    });

};

$.fn.TableInit = function (columnOrder, order, bPaginate, bFilter, bInfo, columnDefinition, data) {
    this.dataTable({
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        "order": [[columnOrder, order]],
        "destroy": true,
        "bPaginate": bPaginate,
        "bFilter": bFilter,
        "bInfo": bInfo,
        "data": data,
        "columns": columnDefinition
    });
};

$.fn.TableInitPag = function (columnOrder, order, bPaginate, bDisplayLength, bFilter, bInfo, columnDefinition, data) {
    this.dataTable({
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        "order": [[columnOrder, order]],
        "destroy": true,
        "bPaginate": bPaginate,
        "iDisplayLength": bDisplayLength,
        "aLengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        "bFilter": bFilter,
        "bInfo": bInfo,
        "data": data,
        "columns": columnDefinition
    });
};

$.fn.validateElement = function (controlId) {

    var validator = this.validate();
    validator.element(controlId);

};

$.fn.reset = function () {
    $(this).each(function () { this.reset(); });
}

$.fn.setByText = function (value) {

    if (value != '')
        $(this.selector + " option:contains(" + value + ")").attr('selected', 'selected');

};

//#endregion

//#region Date time extentions

$.fn.dateFormatNoTime = function (value) {

    if (value != null && value != '') {

        try { value = value.substring(0, 19); }
        catch (ex) { }

        var fecha = new Date(value);
        this.val(moment(fecha.toUTCString()).format("DD-MMMM-YYYY, h:mm:ss a"));
    }
    else
        return '';
};

$.fn.CalculateAge = function (controlYear, controlMonth) {

    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    var mayoriaEdad = 18;

    var fechaActual = new Date();
    var mesActual = fechaActual.getUTCMonth() + 1;
    var diaActual = fechaActual.getDate();
    var añoActual = fechaActual.getUTCFullYear();

    var momentfecha = moment(this.val(), "DD-MMMM-YYYY");
    var DayOfBirthString = monthNames[momentfecha._a[1]] + " " + momentfecha._a[2] + ", " + momentfecha._a[0];

    var fecha = new Date(DayOfBirthString);
    var mesFechaNacimiento = fecha.getUTCMonth() + 1;
    var diaFechaNacimiento = fecha.getDate();
    var añoFechaNacimiento = fecha.getUTCFullYear();

    var edad = añoActual - añoFechaNacimiento;

    if (edad >= mayoriaEdad) {

        var mesesCumplidos = 0;
        var mesVerificar;

        var verificarAñoSiguiente = (mesFechaNacimiento >= mesActual) ? true : false;

        if (verificarAñoSiguiente) {
            mesActual = 12; // refleja un año entero a verificar
        }

        mesVerificar = mesFechaNacimiento + 1;

        while (mesVerificar <= mesActual) {

            if (mesVerificar == mesActual) {
                if ((diaActual >= diaFechaNacimiento) ? true : false) {

                    if (verificarAñoSiguiente) {
                        mesActual = fechaActual.getUTCMonth() + 1;
                        mesVerificar = 1;
                        verificarAñoSiguiente = false;
                        mesesCumplidos++;
                        if (mesesCumplidos != 12) // Año cumplido
                            edad--;
                        else if (mesesCumplidos == 12)
                            mesesCumplidos = 0;
                    }
                    else {
                        mesVerificar++;
                        mesesCumplidos++;
                    }
                }
                else {

                    if (verificarAñoSiguiente) {
                        mesActual = fechaActual.getUTCMonth() + 1;
                        mesVerificar = 1;

                        verificarAñoSiguiente = false;
                        mesesCumplidos++;
                        if (mesesCumplidos != 12) // Año cumplido
                            edad--;
                        else if (mesesCumplidos == 12)
                            mesesCumplidos = 0;
                    }
                    else {
                        mesVerificar++;
                    }
                }
            }
            else {
                mesesCumplidos++;
                mesVerificar++;
            }
        }

        $(controlYear).val(edad);
        $(controlMonth).val(mesesCumplidos);
    }
    else {
        $(controlYear).val('');
        $(controlMonth).val('');
    }
}

$.fn.CalculateLaboralAgeActual = function (controlYear, controlMonth) {

    var txtFechaInicioEmpleo = moment(this.val(), "DD-MMMM-YYYY");

    var years = moment().diff(txtFechaInicioEmpleo, 'years');
    var months = moment().diff(txtFechaInicioEmpleo, 'months');

    months = months - (years * 12);

    $(controlYear).val(years);
    $(controlMonth).val(months);
}

$.fn.dotNetFormat = function () {

    if (this.val() != null && this.val() != '') {

        var momentDate = moment(this.val(), "DD-MMMM-YYYY");
        var dotNetFormatDate = momentDate.format("MM-DD-YYYY");

        return dotNetFormatDate;
    }
    else
        return '';
};

$.fn.CalculateLaboralAge = function (controlYear, controlMonth, controlEndDate) {

    var txtFechaInicioEmpleo = moment(this.val(), "DD-MMMM-YYYY");
    var txtFechaFinEmpleo = moment($(controlEndDate).val(), "DD-MMMM-YYYY");

    var years = txtFechaFinEmpleo.diff(txtFechaInicioEmpleo, 'years');
    var months = txtFechaFinEmpleo.diff(txtFechaInicioEmpleo, 'months');

    months = months - (years * 12);

    $(controlYear).val(years);
    $(controlMonth).val(months);
}

$.fn.DatepickerPast = function (ControlId) {

    /// <summary>Method for asing Datepciker that start from 18 year ago to a text input html control.</summary>
    /// <param name="ControlId" type="text">Control ID of text input control</param>

    var _dateObject = new Date();
    var _yearInit = _dateObject.getFullYear();

    _dateObject.setFullYear(_yearInit);

    this.datepicker({ yearRange: '1945:' + (new Date).getFullYear(), defaultDate: _dateObject }).datepicker('widget').wrap('<div class="ll-skin-cangas"/>');

};

//#endregion

//#endregion

this.DateFormatNoTime = function (value) {

    if (value != null && value != "") {


        try {

            value = value.substring(0, 19);

        }
        catch (ex) {
        }

        var fecha = new Date(value);
        return moment(fecha.toUTCString()).format("DD-MMMM-YYYY, h:mm:ss a");
    }
    else
        return "";

};

this.DateFormatNoTimeGrid = function (data, type, full, meta) {

    //var is_checked = data == true ? "checked" : "";

    if (data != null && data != "") {


        try {

            data = data.substring(0, 19);

        }
        catch (ex) {
        }

        var fecha = new Date(data);
        return moment(fecha.toUTCString()).utc().format("DD-MMMM-YYYY");
    }
    else
        return "";
}


this.DateFormatSimple = function (value) {

    if (value != null && value != "") {
        var fecha = new Date(value);
        return moment(fecha.toGMTString()).format("DD-MM-YYYY");
    }
    else
        return "";

};

this.ToMoney = function (value) {

    return "B/. " + value.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');

};

this.ToMoneyText = function (value) {

    try {
        return value.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    }
    catch (ex) {

        return "0.00";
    }

};

$('input.money-bank').on('keydown', function (e) {
    // tab, esc, enter
    if ($.inArray(e.keyCode, [9, 27, 13]) !== -1 ||
        // Ctrl+A
        (e.keyCode == 65 && e.ctrlKey === true) ||
        // home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
        return;
    }

    e.preventDefault();

    // backspace & del
    if ($.inArray(e.keyCode, [8, 46]) !== -1) {
        $(this).val('');
        return;
    }

    var a = ["a", "b", "c", "d", "e", "f", "g", "h", "i", "`"];
    var n = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "0"];

    var value = $(this).val();
    var clean = value.replace(/\./g, '').replace(/,/g, '').replace(/^0+/, '');
    var p = $.inArray(e.key, n);

    if (p !== -1) {
        value = clean + n[p];

        if (value.length == 2) value = '0' + value;
        if (value.length == 1) value = '00' + value;

        var formatted = '';
        for (var i = 0; i < value.length; i++) {
            var sep = '';
            if (i == 2) sep = '.';
            if (i > 3 && (i + 1) % 3 == 0) sep = ',';
            formatted = value.substring(value.length - 1 - i, value.length - i) + sep + formatted;
        }

        $(this).val(formatted);
    }

    return;

});

this.UpdateRequestParam = function (param, value, url) {

    if (url == null) { url = window.location.href; };

    var re = new RegExp("([?&])" + param + "=.*?(&|$)", "i");

    var separator = (url.indexOf('?') != -1 ? "&" : "?");

    if (url.match(re))
        return url.replace(re, '$1' + param + "=" + value + '$2');
    else
        return url + separator + param + "=" + value;

}
