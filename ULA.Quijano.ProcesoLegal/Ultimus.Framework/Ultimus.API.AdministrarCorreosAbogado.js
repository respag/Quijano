
/*------- VARIABLES GLOBALES -------*/
var UI = new Ultimus.UI();

jQuery(document).ready(function () {

    $("#btnEnviar").css("display", "inline");

    /*### REGION INICIALIZAR CONTROLES ###*/    
    $("#formRegistrarCorreo").EnableValidationToolTip();    
    Datepicker("#txtModificarFecha");
    Datepicker("#txtModificarFechaResp");   

    UI.DataTable_Spanish('#tbClientesEncontrados', 0, "asc", true, true, true);

    switch ($("#hiddenStep").val())
    {
        case "GestionaCorreo": 
            if ($("#hiddenIsAbogado").val() == "") {

                $("#txtaContenido").val(firma);
                nicEditors.findEditor('txtaContenido').setContent(firma);
            }

            break;
        case "RespondeCorreo":
            GetComunicacionByID($("#hiddenIncident").val(), $("#hiddenStep").val());
            DisabledFormRegistrarCorreo();
            $("#adjuntosRO").attr("src", "/AttachmentPage/?" + $("#hiddenAttachmentParamRO").val());

            $("#txtaRespuesta").val(firma);
            nicEditors.findEditor('txtaRespuesta').setContent(firma);
            break;
        case "NotificarCliente":
            ObtenerComunicacion("Entrega");
            break;
        case "PendDocCliente":
        case "Entrega":
            ObtenerDatosCliente();
            break;
        default: break;
    }

    if ($("#hiddenProcess").val() == "ServiciosLegales") {
        $("#txtaContenido").val(firma);
        nicEditors.findEditor('txtaContenido').setContent(firma);
    }

    $("#adjuntos").attr("src", "/AttachmentPage/?" + $("#hiddenAttachmentParam").val());
        
    $(function () {

        var ul = $('#upload ul');

        $('#drop a').click(function () {
            // Simulate a click on the file input button
            // to show the file browser dialog
            $(this).parent().find('input').click();
        });

        // Initialize the jQuery File Upload plugin
        $('#upload').fileupload({

            // This element will accept file drag/drop uploading
            dropZone: $('#drop'),

            // This function is called when a file is added to the queue;
            // either via the browse button, or via drag/drop:
            add: function (e, data) {

                var tpl = $('<li class="working"><input type="text" value="0" data-width="48" data-height="48"' +
                    ' data-fgColor="#0788a5" data-readOnly="1" data-bgColor="#3e4043" /><p></p><span>'+
                    '<button class="btn btn-info btn-xs" type="button">Subir</button>' +
                    '</span></li>');

                // Append the file name and file size
                tpl.find('p').text(data.files[0].name)
                             .append('<i>' + formatFileSize(data.files[0].size) + '</i>');

                // Add the HTML to the UL element
                data.context = tpl.appendTo(ul);

                // Initialize the knob plugin
                tpl.find('input').knob();

                // Listen for clicks on the cancel icon
                tpl.find('span').click(function () {

                    if (tpl.hasClass('working')) {
                        jqXHR.abort();
                    }

                    tpl.fadeOut(function () {
                        tpl.remove();
                    });

                });

                // Automatically upload the file once it is added to the queue
                var jqXHR = data.submit();
            },

            progress: function (e, data) {

                // Calculate the completion percentage of the upload
                var progress = parseInt(data.loaded / data.total * 100, 10);

                // Update the hidden input field and trigger a change
                // so that the jQuery knob plugin knows to update the dial
                data.context.find('input').val(progress).change();

                if (progress == 100) {
                    data.context.removeClass('working');
                }
            },

            fail: function (e, data) {
                // Something has gone wrong!
                data.context.addClass('error');
            }

        });


        // Prevent the default action when a file is dropped on the window
        $(document).on('drop dragover', function (e) {
            e.preventDefault();
        });

        // Helper function that formats the file sizes
        function formatFileSize(bytes) {
            if (typeof bytes !== 'number') {
                return '';
            }

            if (bytes >= 1000000000) {
                return (bytes / 1000000000).toFixed(2) + ' GB';
            }

            if (bytes >= 1000000) {
                return (bytes / 1000000).toFixed(2) + ' MB';
            }

            return (bytes / 1000).toFixed(2) + ' KB';
        }

    });       

    $("#btnEnviar").click(function () {
        RegistrarCorreoServiciosLegales();
    });

    $("#btnEnviarRespuesta").click(function () {

        if ($("#chkbDevuelta").is(':checked')) {

            DevolverCorreo();
            return;
        }

        $("#formRegistrarCorreo").valid();

        if ($("#formRegistrarCorreo").validate().invalidElements().length == 1 &&
            $("#formRegistrarCorreo").validate().invalidElements()[0].id == 'txtaContenido' &&
            String(nicEditors.findEditor('txtaContenido').getContent()).replace("<br>", "").trim() == "")
        {
            toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
            return false;
        }

        if ($("#formRegistrarCorreo").validate().invalidElements().length == 1 &&
            $("#formRegistrarCorreo").validate().invalidElements()[0].id == 'txtaRespuesta' &&
            String(nicEditors.findEditor('txtaRespuesta').getContent()).replace("<br>", "").trim() == "")
        {
            toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
            return false;
        }

        //if (!$("#chkbDevuelta").is(':checked'))
        //{
        //    toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
        //    return false;
        //}

        if ($("#hiddenStep").val() == "GestionaCorreo") {
            RegistrarCorreo();
        }
        else if ($("#hiddenStep").val() == "RespondeCorreo") {
            RespondeCorreo();
        }
    });

    $("#btnAdminCorreoBuscarCliente").click(function () {
        $('#modalBuscarCliente').modal('show');
    });    

    $("#btnCloseBuscarCliente").click(function () {
        $('#modalBuscarCliente').modal('hide');
    });

    $("#btnCloseClientesEncontrados").click(function () {
        $('#modalClientesEncontrados').modal('hide');
    });

    $("#modalClientesEncontrados").draggable({
        handle: ".modal-header-popup"
    });

    $("#modalBuscarCliente").draggable({
        handle: ".modal-header-popup"
    });    

    $("#chkbClienteNuevo").click(function () {
        if (this.checked) {
            $('#divIncidentesActivos').hide(300);            
        }
        else {
            $('#divIncidentesActivos').show(500);            
        }        
    }); 
    
    $("#btnModalBuscarCliente").click(function () {
        if ($('#txtModalBuscarCliente').val() != "")
        GetClientesPorPalabras();
    });

    $("#tbClientesEncontrados tbody td").live("dblclick", "td", function (event) {        
        var data = $('#tbClientesEncontrados').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            $('#hiddenTxtNombreCliente').val(data.ID);
            $('#hiddenCodCliente').val(data.ID);
            document.getElementById("txtModalBuscarCliente").value = data.Nombre;
            $('#hiddenEmailCliente').val(data.Email);
            $('#modalClientesEncontrados').modal('hide');
        }
        
    });

    $("#btnModalBuscarClienteGuardar").click(function () {        
        $('#txtNombreCliente').val($('#txtModalBuscarCliente').val());
        $('#txtCorreoOrigen').val($('#hiddenEmailCliente').val());
        $('#modalBuscarCliente').modal('hide');        
    });
    
    $('#btnFileUpload').fileupload({
        url: 'FileUploadHandler.ashx?upload=start',
        add: function (e, data) {
            console.log('add', data);
            $('#progressbar').show();
            data.submit();
        },
        progress: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progressbar div').css('width', progress + '%');
        },
        success: function (response, status) {
            $('#progressbar').hide();
            $('#progressbar div').css('width', '0%');
            console.log('success', response);
        },
        error: function (error) {
            $('#progressbar').hide();
            $('#progressbar div').css('width', '0%');
            console.log('error', error);
        }
    });
    
    /*### END REGION EVENTOS ###*/
    
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(this).attr('href');

        $(target).css('left', '-' + $(window).width() + 'px');
        var left = $(target).offset().left;
        $(target).css({ left: left }).animate({ "left": "0px" }, "10");
    });

    $("#chkbDevuelta").change(function () {

        if ($("#chkbDevuelta").is(':checked') == false)
            return;

        var justificacion = prompt("Ingrese la justificación de la devolución", "");

        if (justificacion == null) {

            $("#chkbDevuelta").prop("checked", false);
            return;
        }

        if (justificacion.trim() == '') {

            alert("Debe ingresar la justificación de la devolución");
            $("#chkbDevuelta").prop("checked", false);
            return;
        }

        $("#chkbDevuelta").attr("data-justificacion", justificacion);
        $("body :input").attr("disabled", true);
        $("#btnEnviarRespuesta").removeAttr("disabled");
    });


});

/*### REGION FUNCIONES ###*/

this.GetClientesPorPalabras = function () {

    var uri = "api/SolicitudAPI/GetClientesPorPalabras?palabra=" + $("#txtModalBuscarCliente").val().toUpperCase();

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            clientes = data

            var columnDefinition = [            
            { "data": "ID" },
            { "data": "Nombre" },
            { "data": "Email", className: "email-td" }
            ];

            $('#tbClientesEncontrados').TableInit(0, true, true, false, true, columnDefinition, clientes);
            
            var clientesEncontrados = $('#tbClientesEncontrados').DataTable();            

            $('#modalClientesEncontrados').modal('show');
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

this.RegistrarCorreo = function () {

    $("#btnEnviarRespuesta").prop('disabled', true);

    if($("#txtEmailSinAsignacion").val() == ""){
        var txtEmailSinAsignacion = "test@test.com";
    }
    else{
        var txtEmailSinAsignacion = $("#txtEmailSinAsignacion").val();
    }

    var registrarCorreo = {
        Proceso: $("#hiddenProcess").val(),
        TempIncident: $("#hiddenTempIncident").val(),
        UserId: $("#hiddenUserID").val(),
        TaskId: $("#hiddenTaskId").val(),
        IdCliente: ($("#hiddenTxtNombreCliente").val() == "" ? "0" : $("#hiddenTxtNombreCliente").val()),
        Nombre: $("#txtNombreCliente").val(),
        IncidenteRelacionado: "0",
        CodIdioma: "0",
        CodProcedencia: "0",
        CodArea: "0",
        CodJurisdiccion: "0",
        CodTramite: "0",
        AbogadoAsignado: "",
        UsuarioAbogadoAsignado: "",
        CorreoOrigen: $("#txtCorreoOrigen").val(),
        CorreoPara: "",
        Asunto: encodeURIComponent($("#txtAsunto").val()),
        Contenido: "",
        AsuntoOrigen: "",
        CorreoInfractor: "",
        Respuesta: encodeURIComponent(nicEditors.findEditor('txtaContenido').getContent()),
        AbogadoResponde: $("#hiddenUserFullName").val(),
        Etapa: $("#hiddenStep").val(),
        CorreoCC: $("#txtCC").length == 0 ? "" : $("#txtCC").val()
    };

    var uri = "api/SolicitudAPI/RegistrarCorreo";

    $.ajax({
        url: server + uri,
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(registrarCorreo),
        error: function (jqXHR, textStatus, errorThrown) {
            //jqXHR.responseText
            //jqXHR.status
            UI.ENDREQUEST();
            $("#btnEnviarRespuesta").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        },
        success: function (response, textStatus, jqXHR) {
            UI.ENDREQUEST();

            if (response > 0) {
                toastr.options.onHidden = function () { window.close(); };
                toastr.success("Registro guardado. Incidente " + response, Message_Success);
                ResetFormRegistrarCorreo();
            }
            else {

                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }
        }
    });
}

this.RegistrarCorreoServiciosLegales = function () {

    $("#btnEnviar").prop('disabled', true);

    if($("#txtEmailSinAsignacion").val() == ""){
        var txtEmailSinAsignacion = "test@test.com";
    }
    else{
        var txtEmailSinAsignacion = $("#txtEmailSinAsignacion").val();
    }

    var Comunicacion = {
        Proceso: $("#hiddenProcess").val(),
        Incidente: "0",
        UserId: $("#hiddenUserID").val(),
        TaskId: $("#hiddenTaskId").val(),
        IdCliente: ($("#hiddenTxtNombreCliente").val() == "" ? "0" : $("#hiddenTxtNombreCliente").val()),
        Nombre: $("#txtNombreCliente").val(),
        Incidente_Relacionado: $("#hiddenIncident").val(),
        CodIdioma: $("#ddlIdioma").val(),
        CodProcedencia: $("#ddlProcedencia").val(),
        CodArea: $("#ddlArea").val(),
        CodJurisdiccion: $("#ddlJurisdiccion").val(),
        CodTramite: ($("#ddlTramite").val() == 0 ? "0" : $("#ddlTramite").val()),
        Abogado_Asignado: $("#ddlAbogado option:selected").text(),
        UsuarioAbogadoAsignado: $("#ddlAbogado").val(),
        Correo_Origen: txtEmailSinAsignacion,
        Correo_Para: $("#txtCorreoOrigen").val(),
        Asunto: encodeURIComponent($("#txtAsunto").val()),
        Contenido: encodeURIComponent(nicEditors.findEditor('txtaContenido').getContent()),
        AsuntoOrigen: "",
        CorreoInfractor: "",
        Respuesta: "",
        AbogadoResponde: "",
        EtapaActual: $("#hiddenStep").val(),
        EtapaAnterior: ($("#hiddenStep").val() == "NotificarCliente" ? "Entrega" : $("#hiddenStep").val()),
        CorreoCC: $("#txtCC").length == 0 ? "" : $("#txtCC").val()
    };

    var uri = "api/SolicitudAPI/RegistrarCorreoServiciosLegales";

    $.ajax({
        url: server + uri,
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(Comunicacion),
        error: function (jqXHR, textStatus, errorThrown) {
            UI.ENDREQUEST();
            $("#btnEnviar").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        },
        success: function (data, textStatus, jqXHR) {
            UI.ENDREQUEST();

            CompletarTarea();
        }
    });
}

this.RespondeCorreo = function () {

    $("#btnEnviarRespuesta").prop('disabled', true);

    var respondeCorreo = {
        UserId: $("#hiddenUserID").val(),
        TaskId: $("#hiddenTaskId").val(),
        Nombre: $("#txtNombreCliente").val(),
        CodCliente: $("#hiddenCodCliente").val(),
        Incidente: $("#hiddenIncident").val(),
        Respuesta: encodeURIComponent(nicEditors.findEditor('txtaRespuesta').getContent()),
        AbogadoResponde: $("#hiddenUserFullName").val(),
        Devuelta: ($("#chkbDevuelta").is(':checked') ? "1" : "0"),
        ServicioLegal: ($("#chkbServicioLegal").is(':checked') ? "1" : "0"),
        Etapa: $("#hiddenStep").val(),
        CorreoCC: $("#txtCC").length == 0 ? "" : $("#txtCC").val()
    };

    var uri = "api/SolicitudAPI/RespondeCorreo";

    $.ajax({
        url: server + uri,
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(respondeCorreo),
        error: function (jqXHR, textStatus, errorThrown) {
            //jqXHR.responseText
            //jqXHR.status
            UI.ENDREQUEST();
            $("#btnEnviarRespuesta").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        },
        success: function (data, textStatus, jqXHR) {
            UI.ENDREQUEST();

            if (data > 0) {
                toastr.options.onHidden = function () { window.close(); };
                toastr.success("Registro guardado. </br>Incidente Servicios Legales: " + data, Message_Success);
                ResetFormRegistrarCorreo();
            }
            else if (data == 0) {
                toastr.options.onHidden = function () { window.close(); };
                toastr.success("Registro guardado.", Message_Success);
                ResetFormRegistrarCorreo();
            }
            else {
                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }
        }
    });
}

this.ResetFormRegistrarCorreo = function () {    
    
    $("#txtNombreCliente").val('');
    $("#txtAdmCorrFecha").val('');    
    $("#txtCorreoOrigen").val('');
    $("#txtAsunto").val('');
    $("#txtaContenido").val('');
    nicEditors.findEditor('txtaContenido').setContent('');

}

this.DisabledFormRegistrarCorreo = function () {
    
    $("#txtNombreCliente").prop('disabled', true);
    $("#ddlIdioma").prop('disabled', true);
    $("#ddlProcedencia").prop('disabled', true);
    $("#ddlArea").prop('disabled', true);
    $("#ddlJurisdiccion").prop('disabled', true);
    $("#ddlTramite").prop('disabled', true);
    $("#ddlAbogado").prop('disabled', true);
    $("#txtCorreoOrigen").prop('disabled', true);
    $("#txtCorreoInfractor").prop('disabled', true);
    $("#txtEmailSinAsignacion").prop('disabled', true);
    $("#txtAsunto").prop('disabled', true);
    $("#txtaContenido").prop('disabled', true);
    nicEditors.findEditor('txtaContenido').editorContain.getElementsByClassName('nicEdit-main')[0].setAttribute('contenteditable', 'false');
    $("#txtAdmCorrFecha").prop('disabled', true);
    $("input[name='chkbAsuntoOrigen']").prop('disabled', true);
    $("#chkbAbogado").prop('disabled', true);
    $("#rbSinAsignacion").prop('disabled', true);
    $("#btnAdminCorreoBuscarCliente").prop('disabled', true);
    $("#btnAdminCorreoCargaAbogado").prop('disabled', true);
    $("#btnEnviarCorreo").prop('disabled', true);
    
}

this.EnabledFormRegistrarCorreo = function () {

    $("#txtNombreCliente").prop('disabled', false);
    $("#ddlIdioma").prop('disabled', false);
    $("#ddlProcedencia").prop('disabled', false);
    $("#ddlArea").prop('disabled', false);
    $("#ddlJurisdiccion").prop('disabled', false);
    $("#ddlTramite").prop('disabled', false);
    $("#ddlAbogado").prop('disabled', false);
    $("#txtCorreoOrigen").prop('disabled', false);
    $("#txtCorreoInfractor").prop('disabled', false);
    $("#txtEmailSinAsignacion").prop('disabled', false);
    $("#txtAsunto").prop('disabled', false);
    $("#txtaContenido").prop('disabled', false);
    nicEditors.findEditor('txtaContenido').editorContain.getElementsByClassName('nicEdit-main')[0].setAttribute('contenteditable', 'true');
    $("input[name='chkbAsuntoOrigen']").prop('disabled', false);
    $("#chkbAbogado").prop('disabled', false);
    $("#rbSinAsignacion").prop('disabled', false);
    $("#btnAdminCorreoBuscarCliente").prop('disabled', false);
    $("#btnAdminCorreoCargaAbogado").prop('disabled', false);
    $("#btnEnviarCorreo").prop('disabled', false);

}

this.GetComunicacionByID = function (incidenteID, etapa) {
    var uri = "api/SolicitudAPI/GetComunicacionByID?incidenteID=" + incidenteID + "&etapa=" + etapa;

    $.getJSON(server + uri).done(function (data) {

        if (data != null && data.length > 0) {
            $("#txtNombreCliente").val(data[0].Nombre);
            $("#hiddenTxtNombreCliente").val(data[0].IdCliente);
            $("#txtCorreoOrigen").val(data[0].Correo_Origen);
            $("#hiddenEmailCliente").val(data[0].Correo_Origen);
            $("#txtAsunto").val(decodeURIComponent(data[0].Asunto));
            $("#txtaContenido").val(decodeURIComponent(data[0].Contenido));
            nicEditors.findEditor('txtaContenido').setContent(decodeURIComponent(data[0].Contenido));

            if (data[0].Respuesta != null && data[0].Respuesta.trim() != "") {

                $("#txtaRespuesta").val(decodeURIComponent(data[0].Respuesta));
                nicEditors.findEditor('txtaRespuesta').setContent(decodeURIComponent(data[0].Respuesta));
            }

            if ($("#txtCC").length > 0)
                $("#txtCC").val(data[0].CorreoCC);
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

this.Test = function () {

    var uri = "api/SolicitudAPI/Test/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            testData = data;

            $("#txtNombreCliente").val(testData.Name + " " + testData.LastName);                     

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

this.ObtenerDatosCliente = function () {
    //$("#btnEnviar").attr("disabled", "disabled");
    var uri = "api/SolicitudAPI/ObtenerDatosCliente?incidente=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val())
              + "&solicitud=" + $("#hiddensol_co_solicitud").val();
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {          
            $('#txtNombreCliente').val(data.Nombre);
            $('#hiddenTxtNombreCliente').val(data.Cod);
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.ObtenerComunicacion = function (etapa) {
    var uri = "api/SolicitudAPI/GetComunicacionByIncidenteRelacionadoAndEtapa?incidenteRelacionado=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()) +
        "&etapa=" + etapa + "&solicitud=" + $("#hiddensol_co_solicitud").val();
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            $("#txtNombreCliente").val(data.Nombre);
            $("#hiddenTxtNombreCliente").val(data.IdCliente);
            $("#txtCorreoOrigen").val(data.Correo_Para);
            $("#hiddenEmailCliente").val(data.Correo_Para);
            $("#txtAsunto").val(data.Asunto == null ? "" : decodeURIComponent(data.Asunto))
            $("#txtaContenido").val(data.Contenido == null ? "" : decodeURIComponent(data.Contenido));
            if (data.Contenido != null)
                nicEditors.findEditor('txtaContenido').setContent(decodeURIComponent(data.Contenido));
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.CompletarTarea = function () {
    var uri = "api/ProcesosLegalesApi/CompletarTarea?proceso=" + $("#hiddenProcess").val()
                                            + "&tempIncident=" + $("#hiddenTempIncident").val()
                                            + "&userID=" + $("#hiddenUserID").val()
                                            + "&taskID=" + $("#hiddenTaskId").val();

    $.getJSON(server + uri).done(function (data) {
        if (data > 0) {
            toastr.options.onHidden = function () { window.close(); };
            toastr.success("Registro guardado. Incidente " + data, Message_Success);
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

this.DevolverCorreo = function () {

    $("#btnEnviarRespuesta").prop('disabled', true);

    var devolverCorreo = {
        UserId: $("#hiddenUserID").val(),
        TaskId: $("#hiddenTaskId").val(),
        Nombre: $("#txtNombreCliente").val(),
        Incidente: $("#hiddenIncident").val(),
        Justificacion: $("#chkbDevuelta").attr("data-justificacion")
    };

    var uri = "api/SolicitudAPI/DevolverCorreo";

    $.ajax({
        url: server + uri,
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(devolverCorreo),
        error: function (jqXHR, textStatus, errorThrown) {
            //jqXHR.responseText
            //jqXHR.status
            UI.ENDREQUEST();
            $("#btnEnviarRespuesta").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        },
        success: function (response, textStatus, jqXHR) {
            UI.ENDREQUEST();

            if (response == true) {
                toastr.options.onHidden = function () { window.close(); };
                toastr.success("Registro guardado", Message_Success);
            }
            else {
                $("#btnEnviarRespuesta").prop('disabled', false);
                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }
        }
    });
}


/*### END REGION FUNCIONES ###*/