
/*------- VARIABLES GLOBALES -------*/
var UI = new Ultimus.UI();

jQuery(document).ready(function () {

    /*### REGION INICIALIZAR CONTROLES ###*/    
    $("#formRegistrarCorreo").EnableValidationToolTip();    
    Datepicker("#txtModificarFecha");
    Datepicker("#txtModificarFechaResp");   

    UI.DataTable_Spanish('#tbClientesEncontrados', 0, "asc", true, true, true);
    UI.DataTable_Spanish('#tblIncidentesActivos', 0, "asc", true, true, true);

    //Carga de listbox
    GetComunicacion();
    GetIdiomas();
    GetProcedencia();
    GetArea();
    GetJurisdiccion();
    //

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

    $("#tbAdminCorreos tbody td").live("dblclick", "td", function (event) {
        var data = $('#tbAdminCorreos').DataTable().row($(this).parents('tr')).data();
        GetComunicacionByID(data.Incidente, $("#hiddenStep").val());
        DisabledFormRegistrarCorreo();
        $('#modalAdministrarCorreos').modal('show');
        //document.getElementById("titleRegistrarCorreo").innerHTML = document.getElementById("titleRegistrarCorreo").innerHTML ;        
    });

    $("#btnAgregar").click(function () {
        EnabledFormRegistrarCorreo();
        $("#txtAdmCorrFecha").val(moment(Date.now()).format("DD-MMMM-YYYY hh:mm a"));
        $("#adjuntos").attr("src", "/AttachmentPage/?" + $("#hiddenAttachmentParam").val());
        $('#modalAdministrarCorreos').modal('show');
    });    

    $("#btnClosePlantillaRegistro").click(function () {
        ResetFormRegistrarCorreo();
        $("#divCorreoInfractor").hide(800);

        if ($("#hiddenStep").val() == "GestionaCorreo" && $("#hiddenIncident").val() == "0") {
            EliminarAdjuntos();
        }
    });

    $("#txtNombreCliente").focus(function () {
        $("#tblIncidentesActivos").DataTable().clear().draw();
        $("#txtModalBuscarCliente").val('');
        $("#chkbClienteNuevo").attr('checked', false);
        $('#modalBuscarCliente').modal('show');

        setTimeout(function () {
            $("#txtModalBuscarCliente").focus();
        }, 500);
    });

    $("#btnAdminCorreoBuscarCliente").click(function () {
        $("#tblIncidentesActivos").DataTable().clear().draw();
        $("#txtModalBuscarCliente").val('');
        $("#chkbClienteNuevo").attr('checked', false);
        $('#modalBuscarCliente').modal('show');

        setTimeout(function () {
            $("#txtModalBuscarCliente").focus();
        }, 500);
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

    $("#chkbAbogado").click(function () {
        var ddlAbogado = document.getElementById("ddlAbogado");
        var txt = document.getElementById("txtCorreoOrigen");
        if (this.checked) {
            $("#hiddenchkbAbogado").val(1);
            txt.value = ddlAbogado.options[ddlAbogado.selectedIndex].innerHTML;
            txt.disabled = true;
        }
        else {
            txt.value = '';
            txt.disabled = false;
        }        
    });

    $("#chkbClienteNuevo").click(function () {
        if (this.checked) {
            $('#divIncidentesActivos').hide(300);            
        }
        else {
            $('#divIncidentesActivos').show(500);            
        }        
    });    

    $("#ddlProcedencia").on("change", function () {
        var opcion = this.value;

        if (opcion != 5) {            
            $("#divCorreoInfractor").hide(800);
            document.getElementById("txtEmailSinAsignacion").disabled = true;
        }
        else {            
            $("#divCorreoInfractor").show(800);
            document.getElementById("txtEmailSinAsignacion").disabled = true;
        }
        
    });
    
    $("#ddlArea").on("change", function () {
        $('#ddlTramite option').remove();
        $('#ddlJurisdiccion option').remove();

        /*if ($("#ddlArea").val() != 1) {
            GetJurisdiccion();
        }
        else {
            GetJurisdiccionPanama();
        }*/
        GetJurisdiccion();
        GetTramite();
    });

    $("#ddlJurisdiccion").change(function () {
        //if ($("#ddlJurisdiccion option:selected").filter(":contains('PANAMA')").length != 0) {
            
        //}       
    });

    /*$("#chkbDevuelta").change(function () {

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
        $("#btnActualizarCorreo").removeAttr("disabled");
    });*/

    $("#btnEnviarCorreo").click(function() {
        $("#formRegistrarCorreo").valid();
        if (!((($("#formRegistrarCorreo").validate().invalidElements().length == 1 && $("#formRegistrarCorreo").validate().invalidElements()[0].id == 'txtaContenido') || $("#formRegistrarCorreo").validate().invalidElements().length == 0) && String(nicEditors.findEditor('txtaContenido').getContent()).replace("<br>", "").trim() != "")) {

            toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
        else {
            RegistrarCorreo();
            $("#divCorreoInfractor").hide(800);
            $('#modalAdministrarCorreos').modal('hide');            
        }
    });

    $("#btnActualizarCorreo").click(function () {

        /*if ($("#chkbDevuelta").is(':checked')) {

            DevolverCorreo();
            return;
        }*/

        $("#formRegistrarCorreo").valid();
        if (!((($("#formRegistrarCorreo").validate().invalidElements().length == 1 && $("#formRegistrarCorreo").validate().invalidElements()[0].id == 'txtaContenido') || $("#formRegistrarCorreo").validate().invalidElements().length == 0) && String(nicEditors.findEditor('txtaContenido').getContent()).replace("<br>", "").trim() != "")) {
            toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
        else {
            ActualizarCorreo();
        }
    });

    $("#btnModalBuscarCliente").click(function () {
        if ($("#txtModalBuscarCliente").val() != "")
            GetClientesPorPalabras();
    });

    $("#tbClientesEncontrados tbody td").live("dblclick", "td", function (event) {        
        var data = $('#tbClientesEncontrados').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            $('#hiddenTxtNombreCliente').val(data.ID);
            document.getElementById("txtModalBuscarCliente").value = data.Nombre;
            $('#hiddenEmailCliente').val(data.Email);
            $('#modalClientesEncontrados').modal('hide');

            //Cargar la grilla de Incidente Activo Relacionado
            LoadIncidentesActivos(data);
        }
    });

    $("#btnModalBuscarClienteGuardar").click(function () {
        if ($('#txtModalBuscarCliente').val() == '') {
            toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
            return;
        }

        if (!$("#chkbClienteNuevo").is(':checked')) {
            if ($("#tblIncidentesActivos tbody tr td").text() != "Ningún dato disponible") {
                if ($("input[name=rbSelectIncident]:checked").length > 0) {
                    CompletarPlantillaRegistro($("input[name=rbSelectIncident]:checked").val());
                    return;
                }
            }
        }

        $('#txtNombreCliente').val($('#txtModalBuscarCliente').val());
        $('#txtCorreoOrigen').val($('#hiddenEmailCliente').val());
        $('#modalBuscarCliente').modal('hide');
    });

    $("#btnCloseRegistarCorreo").click(function () {
        ResetFormRegistrarCorreo();
        $("#divCorreoInfractor").hide(800);

        if ($("#hiddenStep").val() == "GestionaCorreo" && $("#hiddenIncident").val() == "0") {
            EliminarAdjuntos();
        }
    });

    $("#ddlIdioma").on("change", function () {
        GetAbogados();
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
    })    

    if ($("#hiddenIncident").val() != "0")
    {
        GetComunicacionByID($("#hiddenIncident").val(), $("#hiddenStep").val());
    }

    if ($("#hiddenStep").val() == "Revision") {
        $("#btnCorreoBasura").click(function () {
            if (confirm("¿Está seguro de querer ignorar esta comunicación?"))
                AbortarComunicacion();
        });
    }

});

/*### REGION FUNCIONES ###*/

this.GetIdiomas = function () {

    var uri = "api/SolicitudAPI/GetIdioma/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            idiomas = data
            $("#ddlIdioma").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlIdioma").append('<option value=' + item.CodIdioma + '>' + item.Descripcion + '</option>');
            })
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

this.GetProcedencia = function () {

    var uri = "api/SolicitudAPI/GetProcedencia/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            idiomas = data
            $("#ddlProcedencia").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlProcedencia").append('<option value=' + item.CodProcedencia + '>' + item.Descripcion + '</option>');
            })
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

this.GetArea = function () {

    var uri = "api/SolicitudAPI/GetArea/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            areas = data
            $("#ddlArea").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlArea").append("<option value=\"" + item.CodArea + "\">" + item.Descripcion + "</option>");
            })
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

this.GetTramite = function (CodTramite) {

    var uri = "api/SolicitudAPI/GetTramite?idArea=" + $("#ddlArea").val();    

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            tramite = data
            $("#ddlTramite").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlTramite").append("<option value=\"" + item.CodTramite + "\">" + item.Descripcion + "</option>");
            })

            if (CodTramite != undefined && CodTramite > 0) {
                $("#ddlTramite").val(CodTramite);
            }
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

this.GetJurisdiccion = function () {

    var uri = "api/SolicitudAPI/GetJurisdiccion";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            tramite = data
            $("#ddlJurisdiccion").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlJurisdiccion").append("<option value=\"" + item.CodJurisdiccion + "\">" + item.Nombre + "</option>");
            })
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

this.GetJurisdiccionPanama = function () {

    var uri = "api/SolicitudAPI/GetJurisdiccionPanama";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            tramite = data
            $("#ddlJurisdiccion").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlJurisdiccion").append("<option value=\"" + item.CodJurisdiccion + "\">" + item.Nombre + "</option>");
            })
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

            $('#tbClientesEncontrados').TableInitPag(0, true, true, 5, true,true, columnDefinition, clientes);
            //$('#tbClientesEncontrados').TableInit(0, true, true, false, true, columnDefinition, clientes);
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

this.GetAbogados = function (abogado) {

    $("#ddlAbogado").empty();

    if ($("#ddlIdioma").val() == null || $("#ddlIdioma").val() == "")
        return;

    var uri = "api/SolicitudAPI/GetAbogado?idIdioma=" + $("#ddlIdioma").val();
    
    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            abogados = data;
            $("#ddlAbogado").empty().append("<option value=''></option>");
            $.each(abogados, function (key, item) {
                $("#ddlAbogado").append("<option value=" + item.Usuario + ">" + item.Nombre + "</option>");
            });

            if(abogado != undefined)
            {
                $("#ddlAbogado option").filter(function () { return $(this).text() == abogado; }).prop('selected', true);
            }
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

    $("#btnEnviarCorreo").prop('disabled', true);

    if ($("#txtEmailSinAsignacion").val() == "") {
        var txtEmailSinAsignacion = "test@test.com";
    }
    else {
        var txtEmailSinAsignacion = $("#txtEmailSinAsignacion").val();
    }

    var chkbQueja = ($("#chkbQueja").is(':checked') ? "1" : "0");
    var chkbDevuelta = ($("#chkbDevuelta").is(':checked') ? "1" : "0");
    var chkbBuzonCorreo = ($("#chkbBuzonCorreo").is(':checked') ? "1" : "0");

    var registrarCorreo = {
        Proceso: $("#hiddenProcess").val(),
        TempIncident: $("#hiddenTempIncident").val(),
        UserId: $("#hiddenUserID").val(),
        TaskId: $("#hiddenTaskId").val(),
        IdCliente: ($("#hiddenTxtNombreCliente").val() == "" ? "0" : $("#hiddenTxtNombreCliente").val()),
        Nombre: $("#txtNombreCliente").val(),
        IncidenteRelacionado: "0",
        CodIdioma: $("#ddlIdioma").val(),
        CodProcedencia: $("#ddlProcedencia").val(),
        CodArea: $("#ddlArea").val(),
        CodJurisdiccion: $("#ddlJurisdiccion").val(),
        CodTramite: ($("#ddlTramite").val() == 0 ? "0" : $("#ddlTramite").val()),
        AbogadoAsignado: $("#ddlAbogado option:selected").text(),
        UsuarioAbogadoAsignado: $("#ddlAbogado").val(),
        CorreoOrigen: $("#txtCorreoOrigen").val(),
        CorreoPara: txtEmailSinAsignacion,
        Asunto: encodeURIComponent($("#txtAsunto").val()),
        Contenido: encodeURIComponent(nicEditors.findEditor('txtaContenido').getContent()),
        AsuntoOrigen: chkbQueja + "-" + chkbDevuelta + "-" + chkbBuzonCorreo,
        CorreoInfractor: $("#txtCorreoInfractor").val(),
        Respuesta: "",
        AbogadoResponde: "",
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
            $("#btnEnviarCorreo").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        },
        success: function (response, textStatus, jqXHR) {
            UI.ENDREQUEST();

            if (response > 0) {
                toastr.success("Registro guardado. Incidente " + response, Message_Success);
                ResetFormRegistrarCorreo();
                GetComunicacion(); // carga el grid de correos
            }
            else {

                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }
        }
    });
}

this.ActualizarCorreo = function () {

    $("#btnActualizarCorreo").prop('disabled', true);

    if ($("#txtEmailSinAsignacion").val() == "") {
        var txtEmailSinAsignacion = "test@test.com";
    }
    else {
        var txtEmailSinAsignacion = $("#txtEmailSinAsignacion").val();
    }

    var chkbQueja = ($("#chkbQueja").is(':checked') ? "1" : "0");
    var chkbDevuelta = ($("#chkbDevuelta").is(':checked') ? "1" : "0");
    var chkbBuzonCorreo = ($("#chkbBuzonCorreo").is(':checked') ? "1" : "0");

    var actualizarCorreo = {
        Proceso: $("#hiddenProcess").val(),
        Incidente: $("#hiddenIncident").val(),
        UserId: $("#hiddenUserID").val(),
        TaskId: $("#hiddenTaskId").val(),
        IdCliente: ($("#hiddenTxtNombreCliente").val() == "" ? "0" : $("#hiddenTxtNombreCliente").val()),
        Nombre: $("#txtNombreCliente").val(),
        IncidenteRelacionado: "0",
        CodIdioma: $("#ddlIdioma").val(),
        CodProcedencia: $("#ddlProcedencia").val(),
        CodArea: $("#ddlArea").val(),
        CodJurisdiccion: $("#ddlJurisdiccion").val(),
        CodTramite: ($("#ddlTramite").val() == 0 ? "0" : $("#ddlTramite").val()),
        AbogadoAsignado: $("#ddlAbogado option:selected").text(),
        UsuarioAbogadoAsignado: $("#ddlAbogado").val(),
        CorreoOrigen: $("#txtCorreoOrigen").val(),
        CorreoPara: txtEmailSinAsignacion,
        Asunto: encodeURIComponent($("#txtAsunto").val()),
        Contenido: encodeURIComponent(nicEditors.findEditor('txtaContenido').getContent()),
        AsuntoOrigen: chkbQueja + "-" + chkbDevuelta + "-" + chkbBuzonCorreo,
        CorreoInfractor: $("#txtCorreoInfractor").val(),
        Respuesta: "",
        AbogadoResponde: "",
        Etapa: $("#hiddenStep").val(),
        CorreoCC: $("#txtCC").length == 0 ? "" : $("#txtCC").val()
    };

    var uri = "api/SolicitudAPI/ActualizarCorreo";

    $.ajax({
        url: server + uri,
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(actualizarCorreo),
        error: function (jqXHR, textStatus, errorThrown) {
            //jqXHR.responseText
            //jqXHR.status
            UI.ENDREQUEST();
            $("#btnActualizarCorreo").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        },
        success: function (response, textStatus, jqXHR) {
            UI.ENDREQUEST();

            if (response == true) {
                toastr.options.onHidden = function () { window.close(); };
                toastr.success("Registro guardado", Message_Success);
            }
            else {
                $("#btnActualizarCorreo").prop('disabled', false);
                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }
        }
    });
}

this.ResetFormRegistrarCorreo = function () {    
    
    $("#txtNombreCliente").val('');
    $("#txtAdmCorrFecha").val('');
    $("#ddlIdioma").val('');
    $("#ddlProcedencia").val('');
    $("#ddlArea").val('');
    $("#ddlJurisdiccion").val('');
    $("#ddlTramite").val('');
    $('#ddlAbogado option').remove();
    $("#txtCorreoOrigen").val('');
    $("#txtCorreoInfractor").val('');
    $("#hiddenchkbAbogado").val('');
    $("#hiddenrbSinAsignacion").val('');
    $("#txtEmailSinAsignacion").val('');
    $("#txtAsunto").val('');
    $("#txtaContenido").val('');
    nicEditors.findEditor('txtaContenido').setContent('');
    $("#chkbQueja").prop('checked', false);
    $("#chkbDevuelta").prop('checked', false);
    $("#chkbBuzonCorreo").prop('checked', false);
}

this.GetComunicacion = function () {

    var uri = "api/SolicitudAPI/GetComunicacion/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            comunicacion = data

            var columnDefinition = [
            { "data": "Incidente" },
            { "data": "Fecha" },
            { "data": "Nombre" },
            {
                "aTargets": [3],
                "data": "Asunto",
                "mRender": function (data, type, full) {
                    return decodeURIComponent(data);
                }
            },
            { "data": "Abogado_Responde" },
            { "data": "Fecha_Resp" }
            ];

            $('#tbAdminCorreos').TableInit(0, true, true, true, true, columnDefinition, comunicacion);
            var correosRegistrados = $('#tbAdminCorreos').DataTable();            
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

this.GetComunicacionByID = function (incidenteID, etapa) {

    var uri = "api/SolicitudAPI/GetComunicacionByID?incidenteID=" + incidenteID + "&etapa=" + etapa;

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            comunicacion = data
            
            $("#txtNombreCliente").val(data[0].Nombre);
            $("#txtAdmCorrFecha").val(data[0].Fecha);
            $("#ddlIdioma").val(data[0].CodIdioma);
            $("#ddlProcedencia").val(data[0].CodProcedencia);
            $("#ddlArea").val(data[0].CodArea);
            $("#ddlJurisdiccion").val(data[0].CodJurisdiccion);
            if (data[0].CodArea > 0)
                GetTramite(data[0].CodTramite); //carga la lista de tramites para luego ser asociado el valor que proviena de la BD
            //$("#ddlTramite").val(data[0].CodTramite);
            GetAbogados(data[0].Abogado_Asignado);
            $("#txtCorreoOrigen").val(data[0].Correo_Origen);

            if (data[0].Correo_Infractor != "")
                $("#divCorreoInfractor").show();
            else
                $("#divCorreoInfractor").hide();
            
            $("#txtCorreoInfractor").val(data[0].Correo_Infractor);
            $("#txtEmailSinAsignacion").val(data[0].Correo_Para);
            $("#txtAsunto").val(decodeURIComponent(data[0].Asunto));
            
            if (data[0].Asunto_Origen_Queja == 1) { $("#chkbQueja").prop('checked', true); } else { $("#chkbQueja").prop('checked', false); }
            if (data[0].Asunto_Origen_Devuelta == 1 || (data[0].JustificacionDevolucion != null && data[0].JustificacionDevolucion != "")) { $("#chkbDevuelta").prop('checked', true); } else { $("#chkbDevuelta").prop('checked', false); }
            if (data[0].Asunto_Origen_CPersonal == 1) { $("#chkbBuzonCorreo").prop('checked', true); } else { $("#chkbBuzonCorreo").prop('checked', false); }

            if (data[0].JustificacionDevolucion != null && data[0].JustificacionDevolucion != "") {
                $("#imgJustificacion").attr("title", data[0].JustificacionDevolucion);
                $("#imgJustificacion").show();
            }
            else {
                $("#imgJustificacion").hide();
            }

            $("#txtaContenido").val(data[0].Contenido != "" ? decodeURIComponent(data[0].Contenido) : decodeURIComponent(data[0].Respuesta));
            nicEditors.findEditor('txtaContenido').setContent(data[0].Contenido != "" ? decodeURIComponent(data[0].Contenido) : decodeURIComponent(data[0].Respuesta));
            //$("#txtAdmCorrFecha").val(data[0].Fecha_Resp);
            $("#adjuntos").attr("src", "/AttachmentPage/?" + data[0].ParametroAdjuntos);

            if(data[0].ListaAdjuntos=="")
            {
                $("#BtnAdjuntos").hide();
            }
            else
            {
                $("#BtnAdjuntos").show();
                var adjuntos = "";
                $.each(String(data[0].ListaAdjuntos).split("|"), function (key, item) {
                    var nombre = item.substring(item.lastIndexOf("/")+1);
                    adjuntos += "<p> <img src='../Images/attachment.png' /> <a href='" + item + "' target='_blank'>" + nombre + "</a> </p>";
                })

                $("#ListaAdjuntos").html(adjuntos);
                $('#VisorAdjuntos').appendTo("body");
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

this.EliminarAdjuntos = function () {
    var uri = "api/SolicitudAPI/EliminarAdjuntos";

    var eliminarAdjunto = {
        Process: $("#hiddenProcess").val(),
        UserId: $("#hiddenUserID").val()
    };

    $.ajax({
        url: server + uri,
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(eliminarAdjunto),
        error: function (jqXHR, textStatus, errorThrown) {
            //jqXHR.responseText
            //jqXHR.status
            UI.ENDREQUEST();

            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        },
        success: function (response, textStatus, jqXHR) {
            UI.ENDREQUEST();
        }
    });
}

this.LoadIncidentesActivos = function (cliente) {
    var uri = "api/SolicitudAPI/GetIncidentesActivosByCliente?IdCliente=" + cliente.ID;

    $.getJSON(server + uri).done(function (data) {
        if (data != null) {

            incidentes = data

            var columnDefinition = [
                { "data": "NroIncidente" },
                { "data": "Sumario" },
                {
                    "aTargets": [ 2 ],
                    "mData": "NroIncidente",
                    "mRender": function ( data, type, full ) {
                        return "<input value='" + data + "' type='radio' name='rbSelectIncident'>";
                    }
                }
            ];

            //$('#tblIncidentesActivos').TableInit(0, true, true, false, true, columnDefinition, incidentes);
            $('#tblIncidentesActivos').TableInitPag(0, true, true, 5, true, true, columnDefinition, incidentes);
            
            $('#tblIncidentesActivos').DataTable();
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

this.CompletarPlantillaRegistro = function (incidente) {
    $('#txtNombreCliente').val($('#txtModalBuscarCliente').val());
    $('#txtCorreoOrigen').val($('#hiddenEmailCliente').val());

    var uri = "api/SolicitudAPI/GetDatosByIncidente?incidente=" + incidente;

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            $("#ddlIdioma").val(data.Idioma);
            $("#ddlProcedencia").val(data.IdProcedencia);
            $("#ddlArea").val(data.IdArea);
            $("#ddlIdioma").trigger("change");
            $("#ddlProcedencia").trigger("change");
            $("#ddlArea").trigger("change");

            setTimeout(function () {
                $("#ddlJurisdiccion").val(data.IdJurisdiccion);
                $("#ddlTramite").val(data.IdTramite);
                $("#ddlAbogado").val(data.Abogado);
                $('#modalBuscarCliente').modal('hide');
            }, 1500);            
        }
        else {

            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        }
    }).fail(function (jqxhr, textStatus, error) {
        UI.ENDREQUEST();
        $('#modalBuscarCliente').modal('hide');
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
    }).then(function (value) {
        UI.ENDREQUEST();
        $('#modalBuscarCliente').modal('hide');
    });
}

this.AbortarComunicacion = function () {
    var uri = "api/SolicitudAPI/AbortarComunicacion?userID=" + $("#hiddenUserID").val() + "&taskID=" + $("#hiddenTaskId").val();
    $.getJSON(server + uri).done(function (data) {
        if (data == 1) {
            window.close();
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

    $("#btnActualizarCorreo").prop('disabled', true);

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
            $("#btnActualizarCorreo").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        },
        success: function (response, textStatus, jqXHR) {
            UI.ENDREQUEST();

            if (response == true) {
                toastr.options.onHidden = function () { window.close(); };
                toastr.success("Registro guardado", Message_Success);
            }
            else {
                $("#btnActualizarCorreo").prop('disabled', false);
                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }
        }
    });
}

/*### END REGION FUNCIONES ###*/