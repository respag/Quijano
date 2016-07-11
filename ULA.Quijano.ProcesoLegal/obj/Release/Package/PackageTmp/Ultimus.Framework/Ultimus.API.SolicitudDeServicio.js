/*------- VARIABLES GLOBALES -------*/
var UI = new Ultimus.UI();

jQuery(document).ready(function () {

    /*### REGION INICIALIZAR CONTROLES ###*/
    GetIdiomas();
    GetMonedas();
    GetSectoresEconomicos();
    GetPaises();
    //GetProcedencia();
    //GetArea();
    //GetJurisdiccion();

    setTimeout(function () {
        ObtenerDatosCliente();
    }, 500);    

    //---- tabla Instrucciones ----
    UI.DataTable_Spanish("#tbInstrucciones", 0, "asc", false, false, true);

    GetInstruccion();

    //---- tabla libreta de direcciones ----
    UI.DataTable_Spanish("#tbLibretaDireccion", 0, "asc", false, false, true);
    var tableResultado = $('#tbLibretaDireccion').DataTable();

    $('#btnLibDirAgregar').show(500);
    
    $("#txtFechaSolicitud").dateFormatNoTime(Date.now());
    Datepicker("#txtFechaNacimiento");
    Datepicker("#txtSolNotTraFechaEntrega");
    Datepicker("#txtInstSocFechaApertura");
    Datepicker("#txtInstMigFechaProgramada");
    Datepicker("#txtInstMigFechaRegistro");
    Datepicker("#txtSolTesFecha");

    //inicializa o establece la validacion a los formularios
    $("#formModalAddLibretaDireccion").EnableValidationToolTip();
    $("#formModalModLibretaDireccion").EnableValidationToolTip();
    $("#formNuevoCliente").EnableValidationToolTip();
    $("#formBuscarCliente").EnableValidationToolTip();

    if ($("#hiddenStep").val() == "RegistrarSolicitud")
        $("#btnEnviar").attr("disabled", "disabled");
    
    /*### END REGION INICIALIZAR CONTROLES ###*/
    

    
    /*### EVENTOS ###*/

    $("#ddlAreaXXX").change(function () {
        $('#ddlTramite option').remove();
        $('#ddlJurisdiccion option').remove();

        if ($("#ddlArea").val() != 1) { GetJurisdiccion(); }
        else { GetJurisdiccionPanama(); }

        GetTramite();
    });

    $("#btnAdminCorreoCargaAbogadoXXX").click(function () {
        $('#ddlAbogado option').remove();
        GetAbogados();
    });

    //---- tabla libreta de direcciones ----

    $("#tbLibretaDireccion tbody td").live("click", "td", function (event) {
        var data = $('#tbLibretaDireccion').DataTable().row($(this).parents('tr')).data();
        if (event.target.id == "btnEliminar") {
            alert("eliminar");
        }
        else if (event.target.id == "btnEditar") {
            $('#modalModLibretaDireccion').modal('show');
            document.getElementById("txtModalModLibDirCod").value = data[2];
            document.getElementById("txtModalModLibDirFact").value = data[0];
            document.getElementById("txtModalModLibDirDireccion").value = data[1];
        }
    });

    $("#btnCloseLibDirModificar").click(function () {
        $('#modalModLibretaDireccion').modal('hide');
    });

    $("#modalModLibretaDireccion").draggable({
        handle: ".modal-header-popup"
    });

    $("#btnLibDirAgregar").click(function () {
        $('#modalAddLibretaDireccion').modal('show');        
    });

    $("#btnInstrucAgregar").click(function () {
        $('#modalAddInstrucciones').modal('show');
    });

    $("#tbLibretaDireccion tr td input[type='buttom']").click(function () {
        alert($(this).parent().parent().children().index($(this).parent()));
    });

    $("#btnCloseInstrucAgregar").click(function () {
        $('#modalAddInstrucciones').modal('hide');
    });

    $("#btnCloseInstrucActualizar").click(function () {
        $('#modalUpdateInstrucciones').modal('hide');
    });

    $("#btnCloseInstrucResponder").click(function () {
        $('#modalResponderInstrucciones').modal('hide');
    });
    
    $("#btnModalInstrucAgregar").click(function () {
        var flag = false; //confirma si el codigo es duplicado
        if (!$("#formModalAddInstrucciones").valid()) {
            toastr.warning("Ingresar todos los campos que son requeridos.", Message_Warning);
            return false;
        }
        else {
            if ($("#hiddensol_co_solicitud").val() == "0") {
                var url = "api/ProcesosLegalesApi/DevuelveCodSolicitud?codigo=0"
                $.getJSON(server + url, function (data) {
                    $("#hiddensol_co_solicitud").val(data);

                }).done(function () {
                    RegistrarInstruccion($("#txtModalInstruccion").val());
                });
            }
            else {
                RegistrarInstruccion($("#txtModalInstruccion").val());
            }   
        }
    });

    $("#btnModalInstrucResponder").click(function () {
        if (!$("#formModalResponderInstrucciones").valid()) {
            toastr.warning("Ingresar todos los campos que son requeridos.", Message_Warning);
            return false;
        }
        else {
            ActualizarInstruccionRespuesta($("#txtIdInstruccion").val(), $("#txtModalRespuesta").val(), $("#txtModalEjecutado").val());
        }
    });

    $("#btnModalLibDirAgregar").click(function () {
        var flag = false; //confirma si el codigo es duplicado
        if (!$("#formModalAddLibretaDireccion").valid()) {
            toastr.warning("Ingresar todos los campos que son requeridos.", Message_Warning);
            return false;
        }
        else {
            /*$("#tbLibretaDireccion tr").each(function () {
                var code = $(this).find("td").eq(2).html();
                if (code == $("#txtModalLibDirCod").val()) {
                    flag = true;
                }                
            });*/

            if (flag != true) {
                addGridLibretaDireccion($("#txtModalLibDirFact").val(),
                                        $("#txtModalLibDirCod").val(),
                                        $("#txtModalLibDirDireccion").val());

                document.getElementById("formModalAddLibretaDireccion").reset();

                $("#txtModalLibDirFact").removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
                $("#txtModalLibDirDireccion").removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");
                $("#txtModalLibDirCod").removeClass("inputWarning").removeClass("inputSuccess").removeClass("error").tooltip("destroy");

                $('#modalAddLibretaDireccion').modal('hide');
            }
            else {
                toastr.warning("El código que ha ingresado, ya existe.", Message_Warning);
                return false;
            }
        }        
    });

    $("#btnCloseLibDirAgregar").click(function () {
        $('#modalAddLibretaDireccion').modal('hide');
    });

    $("#modalAddLibretaDireccion").draggable({
        handle: ".modal-header-popup"
    });

    //$("#btnCreacionCliente").click(function () {
    //    if (!$("#formNuevoCliente").valid()) {
    //        toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);            
    //        return false;
    //    }
    //});

    $("#btnEjecutar").click(function () {
        if (!$("#formBuscarCliente").valid()) {
            toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);            
            return false;
        }
        else {
            GetClientesPorPalabras();
        }
    });
                     
    $("#tbClientesEncontrados tbody td").live("dblclick", "td", function (event) {
        var data = $('#tbClientesEncontrados').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            document.getElementById("txtNombreCliente").value = data.Nombre;
            LoadCliente(data.ID);
            $('#modalClientesEncontrados').modal('hide');
        }
    });  

    $("#btnCloseClientesEncontrados").click(function () {
        $('#modalClientesEncontrados').modal('hide');
    });

    $("#rbExiste").click(function () {
        showFormBuscarCliente();
    });

    $("#rbNuevo").click(function () {
        hideFormBuscarCliente();
        ResetForm();
    });

    //SE ENVIA DESDE EL FORMULARIO DATOS_SOLICITUD
    //$("#btnEnviar").click(function () {
    //    GuardarDatosCliente();
    //});

    if ($("#hiddenStep").val() == "InstruccionCumplimiento" || $("#hiddenStep").val() == "PendDocCliente") {
        $("#btnEnviar").click(function () {
            $("#btnEnviar").prop('disabled', true);
            CompletarTarea();
        });
    }

    if ($("#hiddenStep").val() == "AnalisisLegal") {
        $("#btnEnviar").click(function () {
            $("#btnEnviar").prop('disabled', true);
            CompletarTarea2();
        });
    }

    function addGridLibretaDireccion(nombre, direccion, codigo) {
        //agrega fila con datos a grid
        tableResultado.row.add([nombre, direccion, codigo, ""]).draw();        
    }

    function addGridInstruccion(instruccion, respuesta, ejecutado) {
        //agrega fila con datos a grid
        tableInstrucciones.row.add([instruccion, respuesta, ejecutado, ""]).draw();
    }   

    function habilitaFormCliente() {
        elementos = document.getElementsByClassName("nuevo-cliente");
        for (var i = 0; i < elementos.length; i++) {
            elementos[i].disabled = false;
        }
    }

    function deshabilitaFormCliente() {
        elementos = document.getElementsByClassName("nuevo-cliente");
        for (var i = 0; i < elementos.length; i++) {
            elementos[i].disabled = true;
        }
    }

    /*### END REGION FUNCIONES ###*/
    //SOLO en las etapas RegistrarSolicitud y AnalisisLegal
    if ($("#hiddenStep").val() == "RegistrarSolicitud" || $("#hiddenStep").val() == "AnalisisLegal") {
        $('#tbInstrucciones').DataTable().column(0).visible(true);
        $('#tbInstrucciones tbody td:not(:first-child)').live('click', 'td', function (event) {
            var row = $('#tbInstrucciones').DataTable().row($(this).parents('tr')).data();
            $('#modalUpdateInstrucciones').modal('show');
            $("#txtModalUpdateInstruccion").val(row[2]);
            $("#hiddenId").val(row[1]);
            // var nuevaInstr = prompt("Edite la instrucción:", row[1]);   
        });

        $('#tbInstrucciones tbody td:first-child').live('click', 'td', function (event) {
            var row = $('#tbInstrucciones').DataTable().row($(this).parents('tr')).data();
            var inst = { "IdInstruccion": row[1] };

            var r = confirm("¿Está seguro de querer borrar la instrucción " + row[1] + "?");
            if (r == true) {
                var uri = server + "api/ProcesosLegalesAPI/BorraInstruccion";

                $.ajax({
                    method: "POST",
                    url: uri,
                    data: inst
                }).done(function () {
                    toastr.info("Instrucción borrada", "Exito");
                    GetInstruccion();
                });
            }
        });
    } else {
        $('#tbInstrucciones').DataTable().column(0).visible(false);
   }

    $("#btnModalInstrucActualizar").click(function () {
        var inst = { "Instrucciones": $("#txtModalUpdateInstruccion").val(), "Incidente": $("#hiddenIncident").val(), "Proceso": "ServiciosLegales", "IdInstruccion":  $("#hiddenId").val() };
        var uri = server + "api/ProcesosLegalesAPI/EditaInstruccion";
   
        $.ajax({
            method: "POST",
            url: uri,
            data: inst
        }).done(function () {
            toastr.info("Instrucción modificada", "Exito");
            GetInstruccion();
        });
        $('#modalUpdateInstrucciones').modal('hide');
    });

});

function hideFormBuscarCliente() {
    $('#formBuscarCliente').hide(300);
    //$("#divDatosCliente").hide(300);
    $('#UsuarioBloqueado').hide(300);
    $('#btnLibDirAgregar').show(500);
}

function showFormBuscarCliente() {
    $('#formBuscarCliente').show(500);
    $('#btnLibDirAgregar').hide(300);
    CleanToolTipError();
}

this.ResetForm = function () {
    $("#txtNombreCliente").val('');
    //$("#dllEstadoCumplimiento").removeAttr('disabled').val('');
    //$("#dllNivelRiesgo").removeAttr('disabled').val('');
    //$("#dllPuntajeRiesgo").removeAttr('disabled').val('');
    $("#txtCod").val('');
    $("#txtNombre").removeAttr('disabled').val('');
    $("#txtTelCasa").removeAttr('disabled').val('');
    $("#txtCelular").removeAttr('disabled').val('');
    $("#ddlSectoresEconomicos").removeAttr('disabled').val('');
    $("#PEP_1").removeAttr('checked');
    $("#PEP_2").removeAttr('checked');
    $(".radio-inline").removeAttr('disabled');
    $("#txtIdPasaporte").removeAttr('disabled').val('');
    //$("#ddlMonedas").removeAttr('disabled').val('');
    $("#ddlMonedas input").removeAttr("disabled").prop('checked', false);
    $("#ddlIdioma").removeAttr('disabled').val('');
    $("#ddlPaises").removeAttr('disabled').val('');
    $("#txtOcupacion").removeAttr('disabled').val('');
    $("#txtEmail").removeAttr('disabled').val('');
    $("#cbFormulario").removeAttr('checked').removeAttr('disabled');
    $("#cbPasaporte").removeAttr('checked').removeAttr('disabled');
    $("#cbReferencia").removeAttr('checked').removeAttr('disabled');
    $("#cbProfesional").removeAttr('checked').removeAttr('disabled');
    $("#cbDomicilio").removeAttr('checked').removeAttr('disabled');
    $("#cbIncorp").removeAttr('checked').removeAttr('disabled');
    $("#cbPromocionales").removeAttr('checked').removeAttr('disabled');
    $("#cbMembresia").removeAttr('checked').removeAttr('disabled');
    $("#dllTipoPersona").removeAttr('disabled').val('');
    $("#dllClasificacionCliente").removeAttr('disabled').val('');
    $("#tbLibretaDireccion").DataTable().clear().draw();
}

this.CleanToolTipError = function() {
    $(".selectWarning").removeClass("selectWarning");
    $(".inputWarning").removeClass("inputWarning");
    $(".inputSuccess").removeClass("inputSuccess");
    $(".selectSuccess").removeClass("selectSuccess");
    $(".error").removeClass("error").tooltip("destroy");
}

this.GetIdiomas = function () {
    var uri = "api/SolicitudAPI/GetIdioma/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            idiomas = data;
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

this.GetMonedas = function () {
    /*
    var uri = "api/SolicitudAPI/GetMonedas/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            monedas = data;
            $("#ddlMonedas").empty().append('<option value=""></option>');
            $.each(data, function (key, item) {
                $("#ddlMonedas").append('<option value=' + item.CodMoneda + '>' + item.DesMoneda + '</option>');
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
    */
}

this.GetSectoresEconomicos = function () {
    var uri = "api/SolicitudAPI/GetSectoresEconomicos/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {
            se = data;
            $.each(data, function (key, item) {
                $("#ddlSectoresEconomicos").append('<option value=' + item.CodSector + '>' + item.Descripcion + '</option>');
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

this.GetPaises = function () {
    var uri = "api/SolicitudAPI/GetPaises/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {
            paises = data;
            $.each(data, function (key, item) {
                $("#ddlPaises").append('<option value=' + item.CodPais + '>' + item.Nombre + '</option>');
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
            //$("#ddlIdioma").append("<option value=\"" + k + "\">" + v + "</option>");
            $("#ddlArea").append("<option>" + "[Seleccione el Area]</option>");
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

this.GetTramite = function () {

    var uri = "api/SolicitudAPI/GetTramite?idArea=" + $("#ddlArea").val();

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            tramite = data
            //$("#ddlTramite").append("<option></option>");
            $.each(data, function (key, item) {
                $("#ddlTramite").append("<option value=\"" + item.CodTramite + "\">" + item.Descripcion + "</option>");
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

this.GetJurisdiccion = function () {

    var uri = "api/SolicitudAPI/GetJurisdiccion";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            tramite = data
            //$("#ddlIdioma").append("<option value=\"" + k + "\">" + v + "</option>");
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
            //$("#ddlIdioma").append("<option value=\"" + k + "\">" + v + "</option>");
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

this.GetAbogados = function () {

    var uri = "api/SolicitudAPI/GetAbogado?idIdioma=" + $("#ddlIdioma").val() + "&idProcedencia=" + $("#ddlProcedencia").val() + "&idArea=" + $("#ddlArea").val() + "&jurisdiccion=" + $("#ddlJurisdiccion option:selected").text();

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            abogados = data
            $.each(abogados, function (key, item) {
                $("#ddlAbogado").append("<option>" + item.Nombre + "</option>");
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


this.GetInstruccion = function () {

    var uri = "api/ProcesosLegalesApi/GetInstruccion?proceso=" + $("#hiddenProcess").val() + "&incidente=" + ($("#hiddenIncident").val() != 0 ? $("#hiddenIncident").val() : $("#hiddenTempIncident").val()) + "&tipo=1&solicitud=" + $("#hiddensol_co_solicitud").val();
    var responder = $("#hiddenProcess").val() == "ServiciosLegales" && $("#hiddenStep").val() == "InstruccionCumplimiento" ? $("#ucBtnResponder").html() : "";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            var tableInstrucciones = $('#tbInstrucciones').DataTable();
            tableInstrucciones.clear().draw();

            $.each(data, function (key, item) {

                tableInstrucciones.row.add(["<a class='btn btn-success btn-xs'>Borrar</a>",  item.IdInstruccion , decodeURIComponent(item.Instrucciones), decodeURIComponent(item.Respuesta), item.EjecutadoPor, responder.replace("_ID", item.IdInstruccion)]).draw();

            })
            tableInstrucciones.column(1).visible(false);
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

this.RegistrarInstruccion = function (instrucciones) {

    var Instruccion = {
        Proceso: $("#hiddenProcess").val(),
        Incidente: ($("#hiddenIncident").val() != 0 ? $("#hiddenIncident").val() : $("#hiddenTempIncident").val()),
        Solicitud: ($("#hiddensol_co_solicitud").val()),
        Tipo: 1,
        Instrucciones: encodeURIComponent(instrucciones)
    }

    var uri = "api/ProcesosLegalesApi/RegistrarInstruccion";

    $.ajax({
        type: "POST",
        data: JSON.stringify(Instruccion),
        url: server + uri,
        contentType: "application/json",
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            UI.ENDREQUEST();
        },
        success: function (data, textStatus, jqXHR) {
            UI.ENDREQUEST();

            if (data == true) {
                var tableInstrucciones = $('#tbInstrucciones').DataTable();
                tableInstrucciones.row.add([instrucciones, null, null, ""]).draw();
                document.getElementById("formModalAddInstrucciones").reset();
                GetInstruccion();
                $('#modalAddInstrucciones').modal('hide');
            }
            else {

                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }
        }
    });
}

this.CargarInstruccion = function (id, instrucciones) {

    document.getElementById("formModalResponderInstrucciones").reset();
    $('#modalResponderInstrucciones').modal('show');

    $("#txtIdInstruccion").val(id);
    $("#txtModalInstruccion2").val(instrucciones);
    $("#txtModalEjecutado").val($("#hiddenUserID").val());

}

this.ActualizarInstruccionRespuesta = function (id, respuesta, ejecutado) {

    var Instruccion = {
        IdInstruccion: id,
        EjecutadoPor: ejecutado,
        Respuesta: encodeURIComponent(respuesta)
    }

    var uri = "api/ProcesosLegalesApi/ActualizarInstruccionRespuesta";

    $.ajax({
        type: "POST",
        data: JSON.stringify(Instruccion),
        url: server + uri,
        contentType: "application/json",
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            UI.ENDREQUEST();
        },
        success: function (data, textStatus, jqXHR) {
            UI.ENDREQUEST();

            if (data == true) {
                GetInstruccion();
                document.getElementById("formModalResponderInstrucciones").reset();
                $('#modalResponderInstrucciones').modal('hide');
            }
            else {
                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }
        }
    });
}

this.GetClientesPorPalabras = function () {

    var uri = "api/SolicitudAPI/GetClientesPorPalabras?palabra=" + $("#txtNombreCliente").val().toUpperCase();

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

this.GetComunicacionByIncidenteRelacionado = function (incidente) {

    var uri = "api/SolicitudAPI/GetComunicacionByIncidenteRelacionado?incidenteRelacionado=" + incidente;

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            if (data.IdCliente == 0) {
                $("#rbNuevo").prop('checked', true);
                hideFormBuscarCliente();
            }
            else {
                $("#rbExiste").prop('checked', true);
                showFormBuscarCliente();
            }

            $("#txtNombreCliente").val(data.Nombre);
            $("#txtCod").val(data.IdCliente);
            $("#txtNombre").val(data.Nombre);
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

this.LoadCliente = function (id) {
    var uri = "api/SolicitudAPI/GetClienteById?id=" + id;

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {
            SetData(data);
            BlockData();
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

this.BlockData = function () {
    //Bloquear controles de la vista
    //$("#dllEstadoCumplimiento").attr("disabled", "disabled");
    //$("#dllNivelRiesgo").attr("disabled", "disabled");
    //$("#dllPuntajeRiesgo").attr("disabled", "disabled");
    $("#txtCod").attr("disabled", "disabled");
    $("#txtNombre").attr("disabled", "disabled");
    $("#txtTelCasa").attr("disabled", "disabled");
    $("#txtCelular").attr("disabled", "disabled");
    $("#ddlSectoresEconomicos").attr("disabled", "disabled");
    $(".radio-inline").attr("disabled", "disabled");
    $("#txtIdPasaporte").attr("disabled", "disabled");
    $("#ddlIdioma").attr("disabled", "disabled");
    $("#ddlPaises").attr("disabled", "disabled");
    $("#txtOcupacion").attr("disabled", "disabled");
    $("#txtEmail").attr("disabled", "disabled");
    $("#tbLibretaDireccion").attr("disabled", "disabled");
    $("#cbFormulario").attr("disabled", "disabled");
    $("#cbPasaporte").attr("disabled", "disabled");
    $("#cbReferencia").attr("disabled", "disabled");
    $("#cbProfesional").attr("disabled", "disabled");
    $("#cbDomicilio").attr("disabled", "disabled");
    $("#cbIncorp").attr("disabled", "disabled");
    $("#cbPromocionales").attr("disabled", "disabled");
    $("#cbMembresia").attr("disabled", "disabled");
    $("#dllTipoPersona").attr("disabled", "disabled");
    $("#dllClasificacionCliente").attr("disabled", "disabled");
}

this.SaveCliente = function (clienteExistente,obj) {

    var monedas = "";
    $("#ddlMonedas input:checked").each(function () {
        monedas += "," + $(this).attr("value");
    });


    if (clienteExistente)
    {
        var datosCliente = {
            Incidente: ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()),
            ClienteExistente: clienteExistente ? 'S' : 'N',
            Cod: $("#txtCod").val(),
            Nombre: $("#txtNombre").val(),
            Moneda: (monedas.substring(1)),
            LibretaDirecciones: {},
            Monedas: {},
            Solicitud:$("#hiddensol_co_solicitud").val()
        };
    }
    else
    {
        var libretaDirecciones = [];

        $('#tbLibretaDireccion >tbody >tr').each(function () {
            libretaDirecciones.push({
                CodCliente: $("#txtCod").val(),
                CodDireccion: this.cells[1].innerHTML,
                Direccion: this.cells[2].innerHTML,
                NombreFactura: this.cells[0].innerHTML
            });
        });

        var datosCliente = {
            Incidente: ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()),
            ClienteExistente: clienteExistente ? 'S' : 'N',
            Cod: $("#txtCod").val(),
            Nombre: $("#txtNombre").val(),
            EstadoCumplimiento: $("#dllEstadoCumplimiento").val(),
            NivelRiesgo: $("#dllNivelRiesgo").val(),
            PuntajeRiesgo: $("#dllPuntajeRiesgo").val(),
            Pasaporte: $("#txtIdPasaporte").val(),
            Idioma: $("#ddlIdioma").val(),
            TelefonoCasa: $("#txtTelCasa").val(),
            Pais: $("#ddlPaises").val(),
            Moneda: (monedas.substring(1)),
            Ocupacion: $("#txtOcupacion").val(),
            Celular: $("#txtCelular").val(),
            Email: $("#txtEmail").val(),
            SectorEconomico: $("#ddlSectoresEconomicos").val(),
            Pep: $("#PEP_1").is(':checked') ? 'S' : 'N',
            TipoPersona: $("#dllTipoPersona").val(),
            ClasificacionCliente: $("#dllClasificacionCliente").val(),
            ReqFormulario: $("#cbFormulario").is(':checked'),
            ReqPasaporte: $("#cbPasaporte").is(':checked'),
            ReqReferencia: $("#cbReferencia").is(':checked'),
            ReqProfesional: $("#cbProfesional").is(':checked'),
            ReqDomicilio: $("#cbDomicilio").is(':checked'),
            ReqIncorp: $("#cbIncorp").is(':checked'),
            ReqPromocionales: $("#cbPromocionales").is(':checked'),
            ReqMembresia: $("#cbMembresia").is(':checked'),
            LibretaDirecciones: libretaDirecciones,
            Monedas: {},
            Solicitud:$("#hiddensol_co_solicitud").val()
        };
    }    

    var uri = "api/SolicitudAPI/RegistrarDatosCliente";


    $.ajax({
        url: server + uri,
        async: false,
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(datosCliente),
        error: function (jqXHR, textStatus, errorThrown) {           
            UI.ENDREQUEST();

            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        },
        success: function (response, textStatus, jqXHR) {                      
           window.location.href = UpdateRequestParam("sol_co_solicitud", $("#hiddensol_co_solicitud").val(),$(obj).attr("href"));            
        }
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
            ResetForm();
            //ResetFormRegistrarCorreo();
            //GetComunicacion(); // carga el grid de correos
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

this.CompletarTarea2 = function () {
    var uri = "api/ProcesosLegalesApi/CompletarTarea2?process=" + $("#hiddenProcess").val()
                                            + "&step=" + $("#hiddenStep").val()
                                            + "&incident=" + $("#hiddenIncident").val()
                                            + "&userID=" + $("#hiddenUserID").val()
                                            + "&taskID=" + $("#hiddenTaskId").val()
                                            + "&codJurisdiccion=" + ($("#ddlJurisdiccion").length == 0 ? "" : $("#ddlJurisdiccion").val())
                                            + "&codProcedencia=" + ($("#ddlProcedencia").length == 0 ? "" : $("#ddlProcedencia").val());

    $.getJSON(server + uri).done(function (data) {
        if (data > 0) {
            toastr.options.onHidden = function () { window.close(); };
            toastr.success("Registro guardado. Incidente " + data, Message_Success);
            ResetForm();
            //ResetFormRegistrarCorreo();
            //GetComunicacion(); // carga el grid de correos
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

this.ObtenerDatosCliente = function () {
    var uri = "api/SolicitudAPI/ObtenerDatosCliente?incidente=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val())
          + "&solicitud=" + $("#hiddensol_co_solicitud").val();
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            SetData(data);
            if (data.ClienteExistente == "S") {
                $("#rbExiste").attr('checked', true);
                $('#btnLibDirAgregar').hide(300);
                $("#txtNombreCliente").val(data.Nombre);
                showFormBuscarCliente();
                BlockData();
            }
            else {
                $("#rbNuevo").attr('checked', true);
                $('#btnLibDirAgregar').show(500);
                hideFormBuscarCliente();
            }
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.SetData = function (data) {
    var datosCliente = data;

    if (datosCliente.EstadoCumplimiento == "N")
        $('#UsuarioBloqueado').show(500);
    else
        $('#UsuarioBloqueado').hide(300);

    if (datosCliente.TipoPersona == "")
        datosCliente.TipoPersona = " ";

    $("#dllEstadoCumplimiento option").each(function () {
        if ($(this).val() == datosCliente.EstadoCumplimiento) {
            $(this).attr('selected', '"selected"');
        }
    });
    $("#dllNivelRiesgo option").each(function () {
        if ($(this).val() == datosCliente.NivelRiesgo) {
            $(this).attr('selected', '"selected"');
        }
    });
    $("#dllPuntajeRiesgo option").each(function () {
        if ($(this).val() == datosCliente.PuntajeRiesgo) {
            $(this).attr('selected', '"selected"');
        }
    });
    $("#ddlSectoresEconomicos option").each(function () {
        if ($(this).val() == datosCliente.SectorEconomico) {
            $(this).attr('selected', '"selected"');
        }
    });
    //if (datosCliente.Monedas != null && datosCliente.Monedas.length > 0) {

        $("#ddlMonedas input").attr("disabled", "disabled").prop('checked', false);

        //$.each(datosCliente.Monedas, function (key, item) {
        //    $("#ddlMonedas input[value='" + item.CodMoneda + "'").prop("checked", true)
        //});

        if (datosCliente.Moneda != null) {
            $.each(datosCliente.Moneda.split(","), function (key, item) {
                $("#ddlMonedas input[value='" + item + "'").prop("checked", true)
            });
        }
    //}
    /*else {
        if (datosCliente.ExistBD) {
            //$("#ddlMonedas").empty().append("<option value='-1'></option>");
        }
        else {
            GetMonedas();
        }            
    }*/

        if (datosCliente.Monedas != null) {
            $.each(datosCliente.Monedas, function (i, v) {
                $("[type='checkbox'][value='" + v.CodMoneda + "']").prop("checked", true);
            })
        }

    $("#ddlIdioma option").each(function () {
        if ($(this).val() == datosCliente.Idioma) {
            $(this).attr('selected', '"selected"');
        }
    });
    $("#ddlPaises option").each(function () {
        if ($(this).val() == datosCliente.Pais) {
            $(this).attr('selected', '"selected"');
        }
    });
    $("#dllTipoPersona option").each(function () {
        if ($(this).val() == datosCliente.TipoPersona) {
            $(this).attr('selected', '"selected"');
        }
    });
    $("#dllClasificacionCliente option").each(function () {
        if ($(this).val() == datosCliente.ClasificacionCliente) {
            $(this).attr('selected', '"selected"');
        }
    });
    $("#txtCod").val(datosCliente.Cod);
    $("#txtNombre").val(datosCliente.Nombre);
    $("#txtTelCasa").val(datosCliente.TelefonoCasa);
    $("#txtCelular").val(datosCliente.Celular);
    $("#txtIdPasaporte").val(datosCliente.Pasaporte);
    $("#txtOcupacion").val(datosCliente.Ocupacion);
    $("#txtEmail").val(datosCliente.Email);
    $("#cbFormulario").prop('checked', datosCliente.ReqFormulario);
    $("#cbPasaporte").prop('checked', datosCliente.ReqPasaporte);
    $("#cbReferencia").prop('checked', datosCliente.ReqReferencia);
    $("#cbProfesional").prop('checked', datosCliente.ReqProfesional);
    $("#cbDomicilio").prop('checked', datosCliente.ReqDomicilio);
    $("#cbIncorp").prop('checked', datosCliente.ReqIncorp);
    $("#cbPromocionales").prop('checked', datosCliente.ReqPromocionales);
    $("#cbMembresia").prop('checked', datosCliente.ReqMembresia);

    $("#tbLibretaDireccion").DataTable().clear().draw();
    if (datosCliente.LibretaDirecciones != null) {
        var tablaLD = $('#tbLibretaDireccion').DataTable();
        var Editar = $("#ucBtnEditar").html();
        $.each(datosCliente.LibretaDirecciones, function (key, item) {
            tablaLD.row.add([item.NombreFactura, item.CodDireccion, item.Direccion, item.CodCliente, Editar]).draw();
        })
    }

    if (datosCliente.Pep == "S")
        $("#PEP_1").prop('checked', true);
    else
        $("#PEP_2").prop('checked', true);
}

this.GuardarDatosCliente = function (obj) {

    if ($('#rbExiste').is(':checked')) {

        /*if ($('#ddlMonedas').children('option').length <= 1) {
            $('#ddlMonedas').children('option').first().attr('selected', 'selected');
        }*/

        var form = $("#formNuevoCliente");
        form.validate();

        if (!form.valid()) {
            toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
            return false;
        }

        if ($("#txtNombre").val() == '') {
            toastr.warning("El campo Nombre es un dato requerido", Message_Warning);
            return false;
        }
        if ($("#txtCod").val() == '') {
            toastr.warning("El campo Cod es un dato requerido", Message_Warning);
            return false;
        }
       
        if ($("#hiddensol_co_solicitud").val() == "0") {
            var url = "api/ProcesosLegalesApi/DevuelveCodSolicitud?codigo=0"
            $.getJSON(server + url, function (data) {
                $("#hiddensol_co_solicitud").val(data);
                SaveCliente(true,obj);
            });
        }
        else {
             SaveCliente(true,obj);             
        }
        
        return false;
    }
    else if ($('#rbNuevo').is(':checked')) {

        var form = $("#formNuevoCliente");
        form.validate();

        if (!form.valid()) {
            toastr.warning("Ingresar todos los campos que son requeridos por el formulario", Message_Warning);
            return false;
        }

        if ($('#tbLibretaDireccion >tbody >tr >td').hasClass('dataTables_empty')) {
            toastr.warning("Ingresar al menos un registro a la libreta de direcciones", Message_Warning);
            return false;
        }

        if ($("#ddlMonedas input:checked").length == 0) {
            toastr.warning("Seleccione al menos una moneda", Message_Warning);
            return false;
        }
        
         if ($("#hiddensol_co_solicitud").val() == "0") {
            var url = "api/ProcesosLegalesApi/DevuelveCodSolicitud?codigo=0"
            $.getJSON(server + url, function (data) {
                $("#hiddensol_co_solicitud").val(data);
                SaveCliente(false,obj);
            });
        }
        else {
             SaveCliente(false,obj);             
        }

        return true;
    }
}

function GuardarTab(obj) {
    var ret = GuardarDatosCliente(obj);
    return ret;
}
