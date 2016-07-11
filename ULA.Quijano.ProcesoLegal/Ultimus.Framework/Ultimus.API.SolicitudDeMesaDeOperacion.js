/*------- VARIABLES GLOBALES -------*/
var UI = new Ultimus.UI();

jQuery(document).ready(function () {       

    /*### REGION INICIALIZAR CONTROLES ###*/
    $("#formSolMesaOperacion").EnableValidationToolTip();
    GetAcciones();
    GetTipoSolicitud();
    if (($("#hiddenProcess").val() == "Mensajeria" && $("#hiddenStep").val() == "SolicitarServicio") || ($("#hiddenProcess").val() == "ServiciosLegales"))
        GetEntidades();


    //---- tabla libreta de solicitudes internas ----
    UI.DataTable_Spanish("#tbSolicitudesInternas", 0, "asc", true, true, true);
    var tableResultado = $('#tbSolicitudesInternas').DataTable();    
    tableResultado.row.add(['001', '08-Abril-2015', 'Mensajeria', 'Jhon Doe', 'En proceso']).draw();
    tableResultado.row.add(['002', '28-Diciembre-2015', 'Mensajeria', 'Jhon Doe', 'Terminado']).draw();
    tableResultado.row.add(['003', '01-Enero-2015', 'Pasante', 'Jhon Doe', 'En proceso']).draw();

    /*### END REGION INICIALIZAR CONTROLES ###*/
    
    /*### EVENTOS ###*/
    $("#ddlSolicita").change(function () {

	    var e = document.getElementById("ddlSolicita");
	    var solicitaOpcion = e.options[e.selectedIndex].value;

	    if (solicitaOpcion == '1') {
	        $('#divSolicitudPasante').show(500);
	        $('#divSolicitudMensajero').hide();
	        $('#divSolicitudPasante').requiredContainer();
	        $('#divSolicitudMensajero').noRequiredContainer();
	    }
	    else if (solicitaOpcion == '2') {
	        $('#divSolicitudPasante').hide();
	        $('#divSolicitudMensajero').show(500);
	        $('#divSolicitudPasante').noRequiredContainer();
	        $('#divSolicitudMensajero').requiredContainer();
        }
	    else if (solicitaOpcion == '0') {
	        $('#divSolicitudMensajero').hide();
	        $('#divSolicitudPasante').hide();
	    }
	    
	});

    $("#ddlListaEntidades").change(function () {

        $('#txtDetallarEntidad').val('');
        if ($("#ddlListaEntidades").val() == '-1') {
            $('#divDetallarEntidad').show(500);
            $("#txtDetallarEntidad").attr("required", true);
        }
        else {
            $('#divDetallarEntidad').hide(300);
            $("#txtDetallarEntidad").removeAttr("required");
        }

    });

    $("#btnEnviar").click(function () {

        if (!$("#formSolMesaOperacion").valid()) {//if (false/*!$("#formSolMesaOperacion").valid()*/) {
            toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
        else {
            $("#btnEnviar").prop('disabled', true);
            var e = document.getElementById("ddlSolicita");
            var solicitaOpcion = e.options[e.selectedIndex].value;

            if ($("#hiddenProcess").val() == "Mensajeria" && $("#hiddenStep").val() == "SolicitarServicio")
            {
                if (solicitaOpcion == '1') {
                    RegistrarServicioPasante();
                }
                else if (solicitaOpcion == '2') {
                    RegistrarServicioMensajeria();
                }
            }
            else if ($("#hiddenProcess").val() == "Mensajeria" && $("#hiddenStep").val() == "MesaOperaciones")
            {
                RegistrarServicioMesaOperaciones();
            }
            else if ($("#hiddenProcess").val() == "Mensajeria" && $("#hiddenStep").val() == "RecibeSolicitud") {
                CompletarTarea();
            }
        }
    });

    $("#btnEnviarSolicitudMesaOperacion").click(function () {

        if (!$("#formSolMesaOperacion").valid()) {//if (false/*!$("#formSolMesaOperacion").valid()*/) {
            toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
        else {
            $("#btnEnviarSolicitudMesaOperacion").prop('disabled', true);
            var e = document.getElementById("ddlSolicita");
            var solicitaOpcion = e.options[e.selectedIndex].value;

            if (solicitaOpcion == '1') {
                RegistrarServicioPasante2();
            }
            else if (solicitaOpcion == '2') {
                RegistrarServicioMensajeria2();
            }
        }
    });

    function CompletarTarea() {
        var uri = "api/ProcesosLegalesApi/CompletarTarea?proceso=" + $("#hiddenProcess").val()
                                                + "&tempIncident=" + $("#hiddenTempIncident").val()
                                                + "&userID=" + $("#hiddenUserID").val()
                                                + "&taskID=" + $("#hiddenTaskId").val();

        $.getJSON(server + uri).done(function (data) {

            if (data > 0) {
                toastr.options.onHidden = function () { window.close(); };
                toastr.success("Registro guardado", Message_Success);
            }
            else {
                $("#btnEnviar").prop('disabled', false);
                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }

        }).fail(function (jqxhr, textStatus, error) {
            $("#btnEnviar").prop('disabled', false);
            UI.ENDREQUEST();
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        }).then(function (value) {
            UI.ENDREQUEST();
        });
    }

    /*### END EVENTOS ###*/

    if ($("#hiddenProcess").val() == "Mensajeria" && ($("#hiddenStep").val() == "MesaOperaciones" || $("#hiddenStep").val() == "RecibeSolicitud" || ($("#hiddenStep").val() == "SolicitarServicio" && $("#hiddenTaskStatus").val() == "3")))
    {
        GetServicio($("#hiddenIncident").val());
        GetEstadoSolicitud();
        DisabledFormServicio();
    }

    if ($("#hiddenProcess").val() == "Mensajeria" && $("#hiddenStep").val() == "RecibeSolicitud") {
        $("#ddlResponsable").prop('disabled', true);
        $("#txtComentario").prop('disabled', true);
    }

});

/*### REGION FUNCIONES ###*/
this.GetTipoSolicitud = function () {

    var uri = "api/MesaOperacion/GetTipoSolicitud";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            solicita = data
            $.each(solicita, function (key, item) {
                $("#ddlSolicita").append("<option value=\"" + item.Codigo + "\">" + item.Descripcion + "</option>");
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

this.GetAcciones = function () {

    var uri = "api/MesaOperacion/GetAcciones";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            acciones = data
            $.each(acciones, function (key, item) {
                $("#ddlAcciones").append("<option value=\"" + item.Codigo + "\">" + item.Descripcion + "</option>");
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

this.GetEntidades = function (IdEntidad) {

    var uri = "api/MesaOperacion/GetEntidades";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            $("#ddlListaEntidades").append("<option value=\"-1\">OTRA (Detallar Entidad)</option>");
            $.each(data, function (key, item) {
                $("#ddlListaEntidades").append("<option value=\"" + item.Codigo + "\">" + item.Entidad + "</option>");
            })

            if (IdEntidad != undefined && IdEntidad > 0) {
                $("#ddlListaEntidades").val(IdEntidad);
                $('#divDetallarEntidad').hide();
            }
            else if (IdEntidad != undefined && IdEntidad == "-1") {
                $("#ddlListaEntidades").val(IdEntidad);
                $('#divDetallarEntidad').show();
            }
            else {
                $('#divDetallarEntidad').hide();
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

this.RegistrarServicioPasante = function () {

    var uri = "api/MesaOperacion/RegistrarServicio?proceso=" + $("#hiddenProcess").val()
                                                    + "&tempIncident=" + $("#hiddenTempIncident").val()
                                                    + "&userID=" + $("#hiddenUserID").val()
                                                    + "&taskID=" + $("#hiddenTaskId").val()
                                                    + "&solicitud=1"
                                                    + "&idEntidad=" + $("#ddlListaEntidades").val()
                                                    + "&socEntEmp=" + ($("#ddlListaEntidades").val() == "-1" ? encodeURIComponent($("#txtDetallarEntidad").val()) : encodeURIComponent($("#ddlListaEntidades option:selected").text()))
                                                    + "&nomPara="
                                                    + "&nomDe="
                                                    + "&tramite=" + encodeURIComponent($("#txtTramite").val())
                                                    + "&detalle=" + encodeURIComponent($("#txtaDetalles").val())
                                                    + "&direccion=" + encodeURIComponent($("#txtDireccion").val())
                                                    + "&piso="
                                                    + "&calle="
                                                    + "&referencia="
                                                    + "&horario="
                                                    + "&telefono="
                                                    + "&asunto="
                                                    + "&idAccion="
                                                    + "&responsable="
                                                    + "&estado="
                                                    + "&comentarios="
                                                    + "&correoIniciador=" + $("#hiddenUserEmail").val()
                                                    + "&solicitadoPor=" + $("#hiddenUserFullName").val();

    $.getJSON(server + uri).done(function (data) {

        if (data > 0) {
            toastr.options.onHidden = function () { window.close(); };
            rutinaEnviadoraDeCorreo(data);
            toastr.success("Registro guardado. Incidente " + data, Message_Success);
            //ResetFormRegistrarCorreo();            
        }
        else {
            $("#btnEnviar").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        }

    }).fail(function (jqxhr, textStatus, error) {
        $("#btnEnviar").prop('disabled', false);
        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });

}

this.rutinaEnviadoraDeCorreo = function (inc) {
    // Primero obtengo las direcciones a los que se enviará el correo
    var uri = "api/MesaOperacion/ObtieneCorreoAdminMesaOp";
    var enviarA = "";
    $.getJSON(server + uri).done(function (data) {
        enviarA = data;
        enviarA += ";" + $("#hiddenUserEmail").val();
        var arrMailTo = enviarA.split(";");
        if ($("#ddlSolicita option:selected").text() == "PASANTE") {
            var instrucciones = $("#txtaDetalles").val().replace(/\n\r?/g, '<br />');
            // Armo el contenido del correo para pasantes
            var summary = "Solicitud de Pasante (" + $("#ddlListaEntidades option:selected").text() + ")";
            var asunto = "Mensajeria - " + inc + " - " + summary;
            var body = "Proceso: <b>Mensajería</b><br>Incidente: <b>" + inc + "</b><br>Summary: <b>" + summary + 
                        "</b><br>Trámite: <b>" + $("#txtTramite").val() + "</b><br>Instrucciones: <br><b>" +
                        instrucciones + "</b>";
        } else {
            // Armo el contenido del correo para Mensajeros
            var summary = "Solicitud de Mensajero (" + $("#txtEntidad").val() + ")";
            var asunto = "Mensajeria - " + inc + " - " + summary;
            var body = "Proceso: <b>Mensajería</b><br>Incidente: <b>" + inc + "</b><br>Summary: <b>" + summary +
                        "</b><br>Sociedad/Entidad/Empresa: <b>" + $("#txtEntidad").val() + "</b><br>Para: <b>" +
                        $("#txtPara").val() + "</b><br>Asunto: <b>" + $("#txtAsunto").val() + "</b><br>Acciones: <b>" +
                        $("#ddlAcciones option:selected").text() + "</b>";
        }
       
        $.ajax({
            url: "http://localhost/ULA.Quijano.SendMails/WsComunicacion.asmx/SendMail",
            data: JSON.stringify({
                "subject": asunto,
                "body": body,
                "isbodyhtml": true,
                "mailto": arrMailTo,
                "cc": null,
                "process": "Mensajeria",
                "incident": inc,
                "enlaceServicio": false
            }), 
            contentType: "application/json; charset=utf-8",
            type: "POST",
            success: function (data) {
                toastr.info(JSON.stringify(data.d),"Se ha enviado el correo?");
            },
            error: function (x, y, z) {
                alert(x.responseText + "  " + x.status);
            }
        });
    });
    
}

this.RegistrarServicioMensajeria = function () {

    var uri = "api/MesaOperacion/RegistrarServicio?proceso=" + $("#hiddenProcess").val()
                                                    + "&tempIncident=" + $("#hiddenTempIncident").val()
                                                    + "&userID=" + $("#hiddenUserID").val()
                                                    + "&taskID=" + $("#hiddenTaskId").val()
                                                    + "&solicitud=2"
                                                    + "&idEntidad="
                                                    + "&socEntEmp=" + encodeURIComponent($("#txtEntidad").val())
                                                    + "&nomPara=" + encodeURIComponent($("#txtPara").val())
                                                    + "&nomDe=" + encodeURIComponent($("#txtDe").val())
                                                    + "&tramite="
                                                    + "&detalle="
                                                    + "&direccion=" + encodeURIComponent($("#txtDireccion2").val())
                                                    + "&piso=" + encodeURIComponent($("#txtPiso").val())
                                                    + "&calle=" + encodeURIComponent($("#txtCalle").val())
                                                    + "&referencia=" + encodeURIComponent($("#txtReferenciaLugarEnvio").val())
                                                    + "&horario=" + encodeURIComponent($("#txtHorarios").val())
                                                    + "&telefono=" + encodeURIComponent($("#txtTelefonos").val())
                                                    + "&asunto=" + encodeURIComponent($("#txtAsunto").val())
                                                    + "&idAccion=" + $("#ddlAcciones").val()
                                                    + "&responsable="
                                                    + "&estado="
                                                    + "&comentarios="
                                                    + "&correoIniciador=" + $("#hiddenUserEmail").val()
                                                    + "&solicitadoPor=" + $("#hiddenUserFullName").val();

    $.getJSON(server + uri).done(function (data) {

        if (data > 0) {
            toastr.options.onHidden = function () { window.close(); };
            rutinaEnviadoraDeCorreo(data);
            toastr.success("Registro guardado. Incidente " + data, Message_Success);
        }
        else {
            $("#btnEnviar").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        }

    }).fail(function (jqxhr, textStatus, error) {
        $("#btnEnviar").prop('disabled', false);
        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });

}


this.RegistrarServicioPasante2 = function () {

    var uri = "api/MesaOperacion/RegistrarServicio?proceso=Mensajeria"
                                                    + "&tempIncident=" + $("#hiddenTempIncident").val()
                                                    + "&userID=" + $("#hiddenUserID").val()
                                                    + "&taskID="
                                                    + "&solicitud=1"
                                                    + "&idEntidad=" + $("#ddlListaEntidades").val()
                                                    + "&socEntEmp=" + ($("#ddlListaEntidades").val() == "-1" ? encodeURIComponent($("#txtDetallarEntidad").val()) : encodeURIComponent($("#ddlListaEntidades option:selected").text()))
                                                    + "&nomPara="
                                                    + "&nomDe="
                                                    + "&tramite=" + encodeURIComponent($("#txtTramite").val())
                                                    + "&detalle=" + encodeURIComponent($("#txtaDetalles").val())
                                                    + "&direccion=" + encodeURIComponent($("#txtDireccion").val())
                                                    + "&piso="
                                                    + "&calle="
                                                    + "&referencia="
                                                    + "&horario="
                                                    + "&telefono="
                                                    + "&asunto="
                                                    + "&idAccion="
                                                    + "&responsable="
                                                    + "&estado="
                                                    + "&comentarios="
                                                    + "&correoIniciador=" + $("#hiddenUserEmail").val()
                                                    + "&solicitadoPor=" + $("#hiddenUserFullName").val();

    $.getJSON(server + uri).done(function (data) {
        $("#btnEnviarSolicitudMesaOperacion").prop('disabled', false);
        UI.ENDREQUEST();
        if (data > 0) {
            toastr.success("Registro guardado. Incidente " + data, Message_Success);
            ResetFormRegistrarCorreo();            
        }
        else {
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        }

    }).fail(function (jqxhr, textStatus, error) {
        $("#btnEnviarSolicitudMesaOperacion").prop('disabled', false);
        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {
        $("#btnEnviarSolicitudMesaOperacion").prop('disabled', false);
        UI.ENDREQUEST();
    });

}

this.RegistrarServicioMensajeria2 = function () {

    var uri = "api/MesaOperacion/RegistrarServicio?proceso=Mensajeria"
                                                    + "&tempIncident=" + $("#hiddenTempIncident").val()
                                                    + "&userID=" + $("#hiddenUserID").val()
                                                    + "&taskID="
                                                    + "&solicitud=2"
                                                    + "&idEntidad="
                                                    + "&socEntEmp=" + encodeURIComponent($("#txtEntidad").val())
                                                    + "&nomPara=" + encodeURIComponent($("#txtPara").val())
                                                    + "&nomDe=" + encodeURIComponent($("#txtDe").val())
                                                    + "&tramite="
                                                    + "&detalle="
                                                    + "&direccion=" + encodeURIComponent($("#txtDireccion2").val())
                                                    + "&piso=" + encodeURIComponent($("#txtPiso").val())
                                                    + "&calle=" + encodeURIComponent($("#txtCalle").val())
                                                    + "&referencia=" + encodeURIComponent($("#txtReferenciaLugarEnvio").val())
                                                    + "&horario=" + encodeURIComponent($("#txtHorarios").val())
                                                    + "&telefono=" + encodeURIComponent($("#txtTelefonos").val())
                                                    + "&asunto=" + encodeURIComponent($("#txtAsunto").val())
                                                    + "&idAccion=" + $("#ddlAcciones").val()
                                                    + "&responsable="
                                                    + "&estado="
                                                    + "&comentarios="
                                                    + "&correoIniciador=" + $("#hiddenUserEmail").val()
                                                    + "&solicitadoPor=" + $("#hiddenUserFullName").val();

    $.getJSON(server + uri).done(function (data) {
        $("#btnEnviarSolicitudMesaOperacion").prop('disabled', false);
        UI.ENDREQUEST();
        if (data > 0) {
            toastr.success("Registro guardado. Incidente " + data, Message_Success);
            ResetFormRegistrarCorreo();
        }
        else {
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        }

    }).fail(function (jqxhr, textStatus, error) {
        $("#btnEnviarSolicitudMesaOperacion").prop('disabled', false);
        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {
        $("#btnEnviarSolicitudMesaOperacion").prop('disabled', false);
        UI.ENDREQUEST();
    });

}

this.ResetFormRegistrarCorreo = function () {
    $("#ddlSolicita").val("");
    $("#ddlListaEntidades").val("");
    $("#txtDetallarEntidad").val("");
    $("#txtDireccion").val("");
    $("#txtTramite").val("");
    $("#txtaDetalles").val("");
    $("#txtEntidad").val("");
    $("#txtPara").val("");
    $("#txtDe").val("");
    $("#txtDireccion2").val("");
    $("#txtPiso").val("");
    $("#txtCalle").val("");
    $("#txtReferenciaLugarEnvio").val("");
    $("#txtHorarios").val("");
    $("#txtTelefonos").val("");
    $("#txtAsunto").val("");
    $("#ddlAcciones").val("");

    $('#divSolicitudMensajero').hide();
    $('#divSolicitudPasante').hide();

    document.getElementById('adjuntos').src = document.getElementById('adjuntos').src;
    document.getElementById('adjuntos2').src = document.getElementById('adjuntos2').src;
}

this.RegistrarServicioMesaOperaciones = function () {

    var entidad = $("#ddlSolicita").val() == "1" ? ($("#ddlListaEntidades").val() == "-1" ? $("#txtDetallarEntidad").val() : $("#ddlListaEntidades option:selected").text()) : ($("#ddlSolicita").val() == "2" ? $("#txtEntidad").val() : "");
    var uri = "api/MesaOperacion/RegistrarServicio?userID=" + $("#hiddenUserID").val()
                                                    + "&taskID=" + $("#hiddenTaskId").val()
                                                    + "&incidente=" + $("#hiddenIncident").val()
                                                    + "&solicitud=" + $("#ddlSolicita").val()
                                                    + "&socEntEmp=" + encodeURIComponent(entidad)
                                                    + "&idResponsable=" + $("#ddlResponsable").val()
                                                    + "&usuarioResponsable=" + encodeURIComponent($("#ddlResponsable option:selected").attr("data.usuario"))
                                                    + "&correoResponsable=" + encodeURIComponent($("#ddlResponsable option:selected").attr("data.correo"))
                                                    + "&comentarios=" + encodeURIComponent($("#txtComentario").val());

    $.getJSON(server + uri).done(function (data) {

        if (data == true) {
            toastr.options.onHidden = function () { window.close(); };
            toastr.success("Registro guardado", Message_Success);
        }
        else {
            $("#btnEnviar").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        }

    }).fail(function (jqxhr, textStatus, error) {
        $("#btnEnviar").prop('disabled', false);
        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });

}

this.GetServicio = function (incidente) {

    var uri = "api/MesaOperacion/GetServicioByID?incidenteID=" + incidente;

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {
            $("#ddlSolicita").val(data[0].Solicitud);
            $("#txtSolicitadoPor").val(data[0].SolicitadoPor);
            GetPersonalServicio(data[0].Responsable);
            if (data[0].Solicitud == "1")
            {
                $('#divSolicitudPasante').show();
                $('#divSolicitudMensajero').hide();
                $('#divSolicitudPasante').requiredContainer();
                $('#divSolicitudMensajero').noRequiredContainer();
                GetEntidades(data[0].IdEntidad == 0 ? "-1" : data[0].IdEntidad);
                $("#txtDetallarEntidad").val(data[0].SociedadEntidadEmpresa);
                $("#txtDireccion").val(data[0].Direccion);
                $("#txtTramite").val(data[0].Tramite);
                $("#txtaDetalles").val(data[0].Detalle);
            }
            else if (data[0].Solicitud == "2") {
                $('#divSolicitudPasante').hide();
                $('#divSolicitudMensajero').show();
                $('#divSolicitudPasante').noRequiredContainer();
                $('#divSolicitudMensajero').requiredContainer();

                $("#txtEntidad").val(data[0].SociedadEntidadEmpresa);
                $("#txtPara").val(data[0].NombrePara);
                $("#txtDe").val(data[0].NombreDe);
                $("#txtDireccion2").val(data[0].Direccion);
                $("#txtPiso").val(data[0].Piso);
                $("#txtCalle").val(data[0].Calle);
                $("#txtReferenciaLugarEnvio").val(data[0].Referencias);
                $("#txtHorarios").val(data[0].Horario);
                $("#txtTelefonos").val(data[0].Telefono);
                $("#txtAsunto").val(data[0].Asunto);
                $("#ddlAcciones").val(data[0].IdAccion);
            }
            else
            {
                $('#divSolicitudPasante').hide();
                $('#divSolicitudMensajero').hide();
            }

            $("#txtComentario").val(data[0].Comentarios);
            $("#txtEstado").val(data[0].NombreEstado);
            $("#txtComentariosResolucion").val(data[0].ComentariosResolucion);
            $("#txtFechaSeguimiento").val(data[0].FechaSeguimiento);

            if (data[0].Estado == "4")
                $('#divFechaSeguimiento').show();
            else
                $('#divFechaSeguimiento').hide();

            if ($("#hiddenStep").val() == "RecibeSolicitud" || ($("#hiddenStep").val() == "MesaOperaciones" && (data[0].Estado == "3" || data[0].Estado == "4")))
                $('#divInformacionResuelveSolicitud').show();
            else
                $('#divInformacionResuelveSolicitud').hide();
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

this.GetPersonalServicio = function (IdResponsable) {

    if ($("#ddlResponsable") == undefined)
        return;

    var uri = "api/MesaOperacion/GetPersonalServicio?tipo=" + $("#ddlSolicita").val();

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {
            $.each(data, function (key, item) {
                $("#ddlResponsable").append("<option value=\"" + item.Codigo + "\" data.usuario=\"" + item.Usuario + "\" data.correo=\"" + item.Correo + "\">" + item.Nombre + "</option>");
            })

            if (IdResponsable != undefined && IdResponsable > 0) {
                $("#ddlResponsable").val(IdResponsable);
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

this.GetEstadoSolicitud = function () {

    if ($("#ddlEstado") == undefined)
        return;

    var uri = "api/MesaOperacion/GetEstadoSolicitud";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {
            $.each(data, function (key, item) {
                $("#ddlEstado").append("<option value=\"" + item.Codigo + "\">" + item.Nombre + "</option>");
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


this.ResetFormSolMesaOperacion = function () {

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
    $("#chkbQueja").prop('checked', false);
    $("#chkbDevuelta").prop('checked', false);
    $("#chkbBuzonCorreo").prop('checked', false);
}

this.DisabledFormServicio = function () {
    $("#ddlSolicita").prop('disabled', true);
    $("#ddlListaEntidades").prop('disabled', true);
    $("#txtDetallarEntidad").prop('disabled', true);
    $("#txtDireccion").prop('disabled', true);
    $("#txtTramite").prop('disabled', true);
    $("#txtaDetalles").prop('disabled', true);
    $("#txtEntidad").prop('disabled', true);
    $("#txtPara").prop('disabled', true);
    $("#txtDe").prop('disabled', true);
    $("#txtDireccion2").prop('disabled', true);
    $("#txtPiso").prop('disabled', true);
    $("#txtCalle").prop('disabled', true);
    $("#txtReferenciaLugarEnvio").prop('disabled', true);
    $("#txtHorarios").prop('disabled', true);
    $("#txtTelefonos").prop('disabled', true);
    $("#txtAsunto").prop('disabled', true);
    $("#ddlAcciones").prop('disabled', true);
}
