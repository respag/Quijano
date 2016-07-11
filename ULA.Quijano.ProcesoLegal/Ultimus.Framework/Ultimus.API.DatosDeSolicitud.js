/*------- VARIABLES GLOBALES -------*/
var UI = new Ultimus.UI();
var enviar = false;
var trSolicitante = null;
Datepicker
jQuery(document).ready(function () {
    if ($("#hiddenStep").val() == 'InstruccionAbogadoGestor')
        $("#divDevolver").removeClass("hidden")
    else
        $("#divDevolver").addClass("hidden")
    enviar = false;
    $("#CambioRl").click(function () {
        if ($("#CambioRl").is(':checked')) {
            $("#divNombreRL").removeClass("hide");
            $("#SomosRl").prop('checked', false);
            $("#CobrarRl").prop('checked', false);
            $("#SomosRl").attr('disabled', 'disabled');
            $("#CobrarRl").attr('disabled', 'disabled');
        }
        else {
            $("#divNombreRL").addClass("hide");
            $("#SomosRl").removeAttr('disabled');
            $("#CobrarRl").removeAttr('disabled');
        }
    });

    if ($("#hiddenStep").val() == 'Control')
        {
            $("#divComentarioControl").removeClass("hidden")
    }
    else {
           $("#divComentarioControl").addClass("hidden")  
    }

    /* INICIO DATOS DE FACTURACION */
    GetTipoFacturacion();
    GetTipoFundacion();
    GetRazones();
    GetPropositos();
    ObtenerDatosSolicitud();

    GetTipoSolicitudMarca();
    GetTipoRegistro();

    Datepicker("#txtInstSocFechaApertura");
    Datepicker("#txtDatFunFechaApertura");
    Datepicker("#txtDatNavFechaApertura");
    Datepicker("#txtFechaProgRegistro");
    Datepicker("#txtFechaProgPresentacion");
    Datepicker("#txtDatFunFecInscrip");
    Datepicker("#txtMigFechaProgRegistro");
    Datepicker("#txtMigFechaProgPresentacion");

    UI.DataTable_Spanish("#tbConsejoFundacional", 0, "asc", false, false, false);
    UI.DataTable_Spanish("#tbLibretaDireccionModal", 0, "asc", false, false, false);
    var tableResultado = $('#tbLibretaDireccionModal').DataTable();

    $('#btnInstSocConsultar').click(function () {        
        $('#modalModLibretaDireccion').modal('show');      
        LoadDireccionCliente($('#txtInstSocCodCliente').val());
    });

    $('#btnInstFunConsultar').click(function () {
        $('#modalModLibretaDireccion').modal('show');
        LoadDireccionCliente($('#txtInstFunCodCliente').val());
    });

    $('#btnInstNavConsultar').click(function () {
        $('#modalModLibretaDireccion').modal('show');
        LoadDireccionCliente($('#txtInstNavCodCliente').val());
    });

    $('#btnInstMigConsultar').click(function () {
        $('#modalModLibretaDireccion').modal('show');
        LoadDireccionCliente($('#txtInstMigCodCliente').val());
    });

    $('#btnInstMarConsultar').click(function () {
        $('#modalModLibretaDireccion').modal('show');
        LoadDireccionCliente($('#txtInstSocCodCliente').val());
    });


    $("#btnCloseLibDireccionPopup").click(function () {
        $('#modalModLibretaDireccion').modal('hide');
    });

    $("#tbLibretaDireccionModal tbody td").live("click", "td", function (event) {
        var data = $('#tbLibretaDireccionModal').DataTable().row($(this).parents('tr')).data();
        if (data !== undefined) {
            $("#txtInst" + GetAreaSelected() + "DireccionFactura").val(data[1]);
        }        
        $('#modalModLibretaDireccion').modal('hide');       
    });
    /* FIN DATOS DE FACTURACION */

    UI.DataTable_Spanish("#tbDatSocEstadoSolicitudes", 0, "asc", false, false, false);
    $('#tbDatSocEstadoSolicitudes').DataTable();
    UI.DataTable_Spanish("#tbDatFunEstadoSolicitudes", 0, "asc", false, false, false);
    $('#tbDatFunEstadoSolicitudes').DataTable();
    UI.DataTable_Spanish("#tbDatNavEstadoSolicitudes", 0, "asc", false, false, false);
    $('#tbDatNavEstadoSolicitudes').DataTable();
    UI.DataTable_Spanish("#tbDatMarEstadoSolicitudes", 0, "asc", false, false, false);
    $('#tbDatMarEstadoSolicitudes').DataTable();

    /* INICIO DATOS ASUNTO */
    $('#btnDatSocCrearActualizar').click(function () {
        if ($('#txtDatSocAsunto').val() == "") {
            toastr.warning("El campo Nro Asunto es requerido para la consulta.", Message_Warning);
            return false;
        }

        BuscarAsuntos($('#txtDatSocAsunto').val());
    });

    $('#btnDatFunCrearActualizar').click(function () {
        if ($('#txtDatFunAsunto').val() == "") {
            toastr.warning("El campo Nro Asunto es requerido para la consulta.", Message_Warning);
            return false;
        }

        BuscarAsuntos($('#txtDatFunAsunto').val());
    });

    $('#btnDatNavCrearActualizar').click(function () {
        if ($('#txtDatNavAsunto').val() == "") {
            toastr.warning("El campo Nro Asunto es requerido para la consulta.", Message_Warning);
            return false;
        }

        BuscarAsuntos($('#txtDatNavAsunto').val());
    });

    $('#btnDatMigCrearActualizar').click(function () {
        if ($('#txtDatMigAsunto').val() == "") {
            toastr.warning("El campo Nro Asunto es requerido para la consulta.", Message_Warning);
            return false;
        }

        BuscarAsuntos($('#txtDatMigAsunto').val());
    });

    $('#btnDatMarCrearActualizar').click(function () {
        if ($('#txtDatMarAsunto').val() == "") {
            toastr.warning("El campo Nro Asunto es requerido para la consulta.", Message_Warning);
            return false;
        }

        BuscarAsuntos($('#txtDatMarAsunto').val());
    });

    $("#modalSocAsuntosEncontrados tbody td").live("dblclick", "td", function (event) {
        var data = $('#tbSocAsuntosEncontrados').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            LoadDatosAsunto(data.NroAsunto);
            $('#modalSocAsuntosEncontrados').modal('hide');
        }
    });

    $("#btnCloseSocAsuntosEncontrados").click(function () {
        $('#modalSocAsuntosEncontrados').modal('hide');
    });

    $("#modalFunAsuntosEncontrados tbody td").live("dblclick", "td", function (event) {
        var data = $('#tbFunAsuntosEncontrados').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            LoadDatosAsunto(data.NroAsunto);
            $('#modalFunAsuntosEncontrados').modal('hide');
        }
    });

    $("#btnCloseFunAsuntosEncontrados").click(function () {
        $('#modalFunAsuntosEncontrados').modal('hide');
    });

    $("#modalNavAsuntosEncontrados tbody td").live("dblclick", "td", function (event) {
        var data = $('#tbNavAsuntosEncontrados').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            LoadDatosAsuntoNaves(data.NroAsunto);
            $('#modalNavAsuntosEncontrados').modal('hide');
        }
    });

    $("#btnCloseNavAsuntosEncontrados").click(function () {
        $('#modalNavAsuntosEncontrados').modal('hide');
    });

    $("#modalMigAsuntosEncontrados tbody td").live("dblclick", "td", function (event) {
        var data = $('#tbMigAsuntosEncontrados').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            LoadDatosAsunto(data.NroAsunto);
            $('#modalMigAsuntosEncontrados').modal('hide');
        }
    });

    $("#btnCloseMigAsuntosEncontrados").click(function () {
        $('#modalMigAsuntosEncontrados').modal('hide');
    });

    $("#modalMarAsuntosEncontrados tbody td").live("dblclick", "td", function (event) {
        var data = $('#tbMarAsuntosEncontrados').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            LoadDatosAsunto(data.NroAsunto);
            $('#modalMarAsuntosEncontrados').modal('hide');
        }
    });

    $("#btnCloseMarAsuntosEncontrados").click(function () {
        $('#modalMarAsuntosEncontrados').modal('hide');
    });


    /* FIN DATOS ASUNTO */

    $("#txtFechaSolicitud").dateFormatNoTime(Date.now());
    
    $("#ddlIdioma").change(function () {
        GetAbogados();
    });

    $("#txtDatSocHonorarios").blur(function () {
        CalcularTotalTramite();
    });

    $("#txtDatSocGastos").blur(function () {
        CalcularTotalTramite();
    });

    $("#txtDatMarHonorarios").blur(function () {
        CalcularTotalMarcas();
    });

    $("#txtDatMarGastos").blur(function () {
        CalcularTotalMarcas();
    });

    $("#txtDatFunHonorarios").blur(function () {
        CalcularTotalTramiteFundacion();
    });

    $("#txtDatFunGastos").blur(function () {
        CalcularTotalTramiteFundacion();
    });

    $("#txtDatMigHonorarios").blur(function () {
        CalcularTotalTramiteMigracion();
    });

    $("#txtDatMigGastos").blur(function () {
        CalcularTotalTramiteMigracion();
    });

    $("#formDatosSolicitud").EnableValidationToolTip();
    $("#formModalAddDistribucion").EnableValidationToolTip();
    $("#formModalModDistribucion").EnableValidationToolTip();
    $("#formModalAddDignatarios").EnableValidationToolTip();
    $("#formModalModDignatarios").EnableValidationToolTip();
    $("#formModalAddConsejos").EnableValidationToolTip();
    $("#modalModTbConsejos").EnableValidationToolTip();
    $("#formModalAddInstrucciones").EnableValidationToolTip();    
    $("#formInstruccionesHeader").EnableValidationToolTip();
    $("#formInstSocDatosSociedad").EnableValidationToolTip();
    $("#formInstDatosFactEntrega").EnableValidationToolTip();
    $("#formInstSocDatosFactEntrega").EnableValidationToolTip();
    $("#formInstFunDatosFactEntrega").EnableValidationToolTip();
    $("#formInstMigDatosFactEntrega").EnableValidationToolTip();
    $("#formInstNavDatosFactEntrega").EnableValidationToolTip();
    $("#formInstMigDatosFactEntrega").EnableValidationToolTip();
    $("#formInstFunDatosFundacion").EnableValidationToolTip();
    $("#formInstNavDatosNaves").EnableValidationToolTip();
    $("#formInstMigDatosMigracion").EnableValidationToolTip();
    $("#formDatSocAsunto").EnableValidationToolTip();
    $("#formDatFunAsunto").EnableValidationToolTip();
    $("#formDatNavAsunto").EnableValidationToolTip();
    $("#formDatMarAsunto").EnableValidationToolTip();
    $("#formCheckListControlFundacion").EnableValidationToolTip();
    $("#formModalAddSolicitantes").EnableValidationToolTip();
    $("#formModalUpdSolicitantes").EnableValidationToolTip();
    $("#formInstMarDatosMarcas").EnableValidationToolTip();
    $("#formInstMarDatosFactEntrega").EnableValidationToolTip();
    $("#formDatMarAsunto").EnableValidationToolTip();


    //---- tabla distribucion ----
    UI.DataTable_Spanish("#tbDistribucion", 0, "asc", true, true, true);
    var tableResultado2 = $('#tbDistribucion').DataTable();
    var ucBtnEditarTableDist = $("#ucBtnEditarTableDist").html();
    tableResultado2.row.add(['0', 'Carlos Turan', '20%', '4', ucBtnEditarTableDist]).draw();
    tableResultado2.row.add(['1', 'Omar Cerrud', '50%', '8', ucBtnEditarTableDist]).draw();
    tableResultado2.row.add(['2', 'Samantha Perez', '67%', '9', ucBtnEditarTableDist]).draw();

    GetInstruccion();

    UI.DataTable_Spanish("#tbInstrucciones", 0, "asc", false, false, true);
    UI.DataTable_Spanish("#tbDignatarios", 0, "asc", false, false, false);
    UI.DataTable_Spanish("#tbAnalisis", 0, "asc", false, false, false);
    UI.DataTable_Spanish("#tbFunAnalisis", 0, "asc", false, false, false);

    $("#btnEnviar").click(function () {
        enviar = true;

        if ($('#hiddenStep').val() == 'Control') {

            $("#btnEnviar").prop('disabled', true);

            if (GetAreaSelected() == 'Soc' || GetAreaSelected() == 'Fun') {
                GuardarCheckListControl();
            } 
            else {
                   var uri = "api/datossociedad/GuardarDatosEtapaControl?" + 
                             "incidente=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()) +
		                     "&solicitud=" + $('#hiddensol_co_solicitud').val() +
		                     "&comentario=" + $('#txtComentarioControl').val();

                    $.getJSON(server + uri).done(function (data) {
                      enviar = false;
                      EnviarIncidente();      
                        }).fail(function (jqxhr, textStatus, error) {
                            UI.ENDREQUEST();
                            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
                        }).then(function (value) {
                            UI.ENDREQUEST();
                        });                  
                }            
        }
        else if($('#hiddenStep').val() =='InstruccionAbogadoGestor')
        {
            if ($("#chkDevolver").is(':checked')) {
                $.ajax({
                    type: "GET",
                    url: server + "api/SolicitudAPI/EnviarTramite?usr=" + $("#hiddenUserID").val() + "&taskId=" + $("#hiddenTaskId").val() + "&retornoAbogado=" + 1,
                    contentType: "application/json"
                }).success(function (value) {
                    //toastr.info("Se ha evaluado la aprobación del reclamo número " + value);
                    window.close();
                }).fail(function (jqxhr, textStatus, error) {
                    toastr.error(MessageFull_Error, Message_Error);
                    //ENDREQUEST();
                }).then(function () {
                    //ENDREQUEST();
                })
            } else {
                $.ajax({
                    type: "GET",
                    url: server + "api/SolicitudAPI/EnviarTramite?usr=" + $("#hiddenUserID").val() + "&taskId=" + $("#hiddenTaskId").val() + "&retornoAbogado=" + 0,
                    contentType: "application/json"
                }).success(function (value) {
                    //toastr.info("Se ha evaluado la aprobación del reclamo número " + value);
                    window.close();
                }).fail(function (jqxhr, textStatus, error) {
                    toastr.error(MessageFull_Error, Message_Error);
                    //ENDREQUEST();
                }).then(function () {
                    //ENDREQUEST();
                })
                GuardarTab();
            }
        }else {
            GuardarTab();
        }        
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
            RegistrarInstruccion($("#txtModalInstruccion").val());
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

    //---- tabla distribucion ----

    $("#tbDistribucion tbody td").live("click", "td", function (event) {
        var data = $('#tbDistribucion').DataTable().row($(this).parents('tr')).data();
        if (event.target.id == "btnEliminarTableDist") {
            //borrar registro en la base de datos y luego recargar grid
        }
        else if (event.target.id == "btnEditarTableDist") {
            $('#modalModTbDistribucion').modal('show');
            document.getElementById("txtModalModDistrCod").value = data[0];
            document.getElementById("txtModalModDistBenef").value = data[1];
            document.getElementById("txtModalModDistPorc").value = data[2];
            document.getElementById("txtModalModDistNAccion").value = data[3];
        }
    });

    $("#btnCloseModificarDist").click(function () {
        $('#modalModTbDistribucion').modal('hide');
    });

    $("#modalModTbDistribucion").draggable({
        handle: ".modal-header-popup"
    });

    $("#btnInstSocDistAgregar").click(function () {
        $('#modalAddTbDistribucion').modal('show');
    });

    $("#btnModalAddDistAgregar").click(function () {
        var flag = false; //confirma si el codigo es duplicado
        if (!$("#formModalAddDistribucion").valid()) {
            toastr.warning("Ingresar todos los campos que son requeridos.", Message_Warning);
            return false;
        }
        else {
            $("#tbDistribucion tr").each(function () {
                var code = $(this).find("td").eq(0).html();
                if (code == $("#txtModalAddDistrCod").val()) {
                    flag = true;
                }
            });

            if (flag != true) {
                addGridDistribucion($("#txtModalAddDistrCod").val(),
                                    $("#txtModalAddDistBenef").val(),
                                    $("#txtModalAddDistPorc").val(),
                                    $("#txtModalAddDistNAccion").val());

                document.getElementById("formModalAddDistribucion").reset();
                $('#modalAddTbDistribucion').modal('hide');
            }
            else {
                toastr.warning("El código que ha ingresado, ya existe.", Message_Warning);
                return false;
            }
        }
    });

    $("#btnCloseAgregarDist").click(function () {
        $('#modalAddTbDistribucion').modal('hide');
    });

    $("#modalAddTbDistribucion").draggable({
        handle: ".modal-header-popup"
    });

    // TABLA DIGNATARIOS
    $("#tbDignatarios tbody td").live("click", "td", function (event) {
        var data = $('#tbDignatarios').DataTable().row($(this).parents('tr')).data();
        if (event.target.id == "btnEliminarTableDign") {
            //borrar registro en la base de datos y luego recargar grid
        }
        else if (event.target.id == "btnEditarTableDign") {
            $('#modalModTbDignatarios').modal('show');
            document.getElementById("txtModalModDignCod").value = data[0];
            document.getElementById("txtModalModDignNombre").value = data[1];
            document.getElementById("txtModalModDignIdent").value = data[2];
            document.getElementById("txtModalModDignDir").value = data[3];
        }
    });

    $("#btnCloseModificarDign").click(function () {
        $('#modalModTbDignatarios').modal('hide');
    });

    $("#modalModTbDignatarios").draggable({
        handle: ".modal-header-popup"
    });

    $("#btnInstSocDignAgregar").click(function () {
        $('#modalAddTbDignatarios').modal('show');
    });

    $("#btnModalAddDignAgregar").click(function () {
        var flag = false; //confirma si el codigo es duplicado
        if (!$("#formModalAddDignatarios").valid()) {
            toastr.warning("Ingresar todos los campos que son requeridos.", Message_Warning);
            return false;
        }
        else {
            var titulo = "";

            if ($("#chkbDig").is(':checked'))
                titulo += " / Dignatario";

            if ($("#chkbApo").is(':checked'))
                titulo += " / Apoderado";

            if ($("#chkbDir").is(':checked'))
                titulo += " / Director";

            if ($("#chkbRep").is(':checked'))
                titulo += " / Representante";

            if ($("#chkbFir").is(':checked'))
                titulo += " / Firmante";

            if ($("#chkbSus").is(':checked'))
                titulo += " / Suscriptor";

            if ($("#chkbAcc").is(':checked'))
                titulo += " / Accionista";

            if ($("#chkbBen").is(':checked'))
                titulo += " / Beneficiado";

            if ($("#chkbPre").is(':checked'))
                titulo += " / Presidente";

            if ($("#chkbSec").is(':checked'))
                titulo += " / Secretario";

            if ($("#chkbTes").is(':checked'))
                titulo += " / Tesorero";

            if (titulo.length > 0)
                titulo = titulo.substr(3);

            addGridDignatarios( $("#txtModalAddDignNombre").val(),
                                $("#txtModalAddDignIdent").val(),
                                titulo
                                );

            document.getElementById("formModalAddDignatarios").reset();
            $('#modalAddTbDignatarios').modal('hide');
        }
    });

    $("#btnInstrucAgregar").click(function () {
        $('#modalAddInstrucciones').modal('show');
    });

    $("#btnDignatarioAgregar").click(function () {
        $('#modalAddTbDignatarios').modal('show');
    });

    $("#btnCloseAgregarDign").click(function () {
        $('#modalAddTbDignatarios').modal('hide');
    });

    $("#modalAddTbDignatarios").draggable({
        handle: ".modal-header-popup"
    });

    // TABLA CONSEJO FUNDACIONAL
    //$("#tbConsejoFundacional tbody td").live("click", "td", function (event) {
    //    var data = $('#tbConsejoFundacional').DataTable().row($(this).parents('tr')).data();
    //    if (event.target.id == "btnEliminarTableDign") {
    //        //borrar registro en la base de datos y luego recargar grid
    //    }
    //    else if (event.target.id == "btnEditarTableDign") {
    //        $('#modalAddTbConsejos').modal('show');
    //        document.getElementById("txtModalModDignCod").value = data[0];
    //        document.getElementById("txtModalModDignNombre").value = data[1];
    //        document.getElementById("txtModalModDignIdent").value = data[2];
    //        document.getElementById("txtModalModDignDir").value = data[3];
    //    }
    //});

    $("#btnCloseModificarConsejo").click(function () {
        $('#modalModTbConsejos').modal('hide');
    });

    $("#modalAddTbConsejos").draggable({
        handle: ".modal-header-popup"
    });

    //$("#btnInstSocDignAgregar").click(function () {
    //    $('#modalAddTbDignatarios').modal('show');
    //});

    $("#btnModalAddConsejoAgregar").click(function () {
        var flag = false; //confirma si el codigo es duplicado
        if (!$("#formModalAddConsejos").valid()) {
            toastr.warning("Ingresar todos los campos que son requeridos.", Message_Warning);
            return false;
        }
        else {
            var titulo = "";

            if ($("#chkbDigFun").is(':checked'))
                titulo += " / Dignatario";

            if ($("#chkbApoFun").is(':checked'))
                titulo += " / Apoderado";

            if ($("#chkbDirFun").is(':checked'))
                titulo += " / Director";

            if ($("#chkbRepFun").is(':checked'))
                titulo += " / Representante";

            if ($("#chkbFirFun").is(':checked'))
                titulo += " / Firmante";

            if ($("#chkbSusFun").is(':checked'))
                titulo += " / Suscriptor";

            if ($("#chkbAccFun").is(':checked'))
                titulo += " / Accionista";

            if ($("#chkbBenFun").is(':checked'))
                titulo += " / Beneficiado";

            if ($("#chkbPreFun").is(':checked'))
                titulo += " / Presidente";

            if ($("#chkbSecFun").is(':checked'))
                titulo += " / Secretario";

            if ($("#chkbTesFun").is(':checked'))
                titulo += " / Tesorero";

            if (titulo.length > 0)
                titulo = titulo.substr(3);

            addGridConsejos($("#txtModalAddConsejoNombre").val(),
                            $("#txtModalAddConsejoIdent").val(),
                            titulo);

            document.getElementById("formModalAddConsejos").reset();
            $('#modalAddTbConsejos').modal('hide');
        }
    });

    $("#btnConsejoFundacionalAgregar").click(function () {
        $('#modalAddTbConsejos').modal('show');
    });

    $("#btnCloseAgregarConsejo").click(function () {
        $('#modalAddTbConsejos').modal('hide');
    });

    $("#modalAddTbConsejos").draggable({
        handle: ".modal-header-popup"
    });

    //----Tabla de Instrucciones
    //$("#btnModalInstrucAgregar").click(function () {
    //    var flag = false; //confirma si el codigo es duplicado
    //    if (!$("#formModalAddInstrucciones").valid()) {
    //        toastr.warning("Ingresar todos los campos que son requeridos.", Message_Warning);
    //        return false;
    //    }
    //    else {

    //        addGridInstruccion($("#txtModalInstruccion").val(),
    //                                       $("#txtModalRespuesta").val(),
    //                                       $("#txtModalEjecutado").val());
    //        //--- agregar a BD ---
    //        document.getElementById("formModalAddInstrucciones").reset();
    //        $('#modalAddInstrucciones').modal('hide');
    //    }
    //});

    //function addGridInstruccion(instruccion, respuesta, ejecutado) {
    //    //agrega fila con datos a grid
    //    $('#tbInstrucciones').DataTable().row.add([instruccion, respuesta, ejecutado, ""]).draw();
    //}

    $("#ddlArea").change(function () {

        var ddlArea = document.getElementById("ddlArea");
        var opcion = ddlArea.options[ddlArea.selectedIndex].value;

        if (opcion == "3")
            $("#formasMigracion").removeClass("hidden");
        else
            $("#formasMigracion").addClass("hidden");

        CargarFormularioPorArea(opcion);

        $('#ddlTramite option').remove();
        $('#ddlJurisdiccion option').remove();

        //if ($("#ddlArea").val() != 1) { GetJurisdiccion(); }
        //else { GetJurisdiccionPanama(); }

        GetJurisdiccion();
        GetTramite();
    });

    $("#btnGuardarDatosNaves").click(function () {
        if (!$("#formInstNavDatosNaves").valid()) {
            toastr.warning("Hay campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
        else {
            var chkb1 = document.getElementById("chkbDatNavLegApos");
            var chkb2 = document.getElementById("chkbDatNavLegConsult");
            var chkb3 = document.getElementById("chkbDatNavLegNota");
            var chkb4 = document.getElementById("chkbDatNavTradu");

            var txtarea1 = document.getElementById("txtaDatNavLegApos");
            var txtarea2 = document.getElementById("txtaDatNavLegConsult");
            var txtarea3 = document.getElementById("txtaDatNavLegNota");
            var txtarea4 = document.getElementById("txtaDatNavTradu");

            if ((chkb1.checked && txtarea1.value != "") || (chkb2.checked && txtarea2.value != "") ||
                (chkb3.checked && txtarea3.value != "") || (chkb4.checked && txtarea4.value != "")) {

                toastr.success("Campos guardados", Message_Success);
            }
            else {
                toastr.warning("Seleccione al menos una casilla y rellene con instrucciones.", Message_Warning);
                return false;
            }
        }
    });

    /* INICIO SOCIEDADES */
    $("#chkInstSocCurrier").change(function () {
        if ($('#hiddenStep').val() == 'Entrega') {
            if ($('#chkInstSocCurrier').is(':checked')) {
                $('#divInstSocNumeroGuia').show(500);
            }
            else {
                $('#divInstSocNumeroGuia').hide(300);
            }
        }

        if ($('#chkInstSocCurrier').is(':checked') || $('#chkInstSocMensajeria').is(':checked')) {
            $('#divInstSocDireccionEnvio').show(500);
        }
        else {
            $('#divInstSocDireccionEnvio').hide(300);
            $('#txtInstSocDireccionEnvio').val('');
        }
    });

    $("#chkInstSocMensajeria").change(function () {
        if ($('#hiddenStep').val() == 'Entrega') {
            if ($('#chkInstSocMensajeria').is(':checked'))
                $('#divInstSocNumeroSolicitud').show(500);
            else
                $('#divInstSocNumeroSolicitud').hide(300);
        }

        if ($('#chkInstSocCurrier').is(':checked') || $('#chkInstSocMensajeria').is(':checked')) {
            $('#divInstSocDireccionEnvio').show(500);
        }
        else {
            $('#divInstSocDireccionEnvio').hide(300);
            $('#txtInstSocDireccionEnvio').val('');
        }
    });

    $("#rbSociedadExiste").click(function () {
        $('#btnDatSocConsultar').show(500);
        //$('#btnInstSocDistAgregar').hide(300);

        LimpiarFormularioSociedad();
        HabilitarFormularioSociedad(false);
        $('#txtDatSocCodSociedad').removeAttr("readonly");
    });

    $("#rbSociedadNueva").click(function () {
        $('#btnDatSocConsultar').hide(300);
        //$('#btnInstSocDistAgregar').show(500);

        LimpiarFormularioSociedad();
        HabilitarFormularioSociedad(true);
        $('#txtDatSocCodSociedad').attr("readonly", "readonly");

        $("#txtDatSocCodClienteAnterior").val($("#TempCodigoCliente").val());
        $("#txtDatSocClienteAnterior").val($("#NombreCodigoCliente").val());
    });
    
    $("#btnDatSocConsultar").click(function () {
        if ($('#txtDatSocCodSociedad').val() == "" && $('#txtDatSocSociedad').val() == "")
        {
            toastr.warning("Ingrese código o nombre de la sociedad.", Message_Warning);
            return;
        }
        
        var codigo = $('#txtDatSocCodSociedad').val();
        var nombre = $('#txtDatSocSociedad').val();
        LimpiarFormularioSociedad();

        if (codigo != "")
        {
            BuscarSociedades(codigo, "");
        }
        else if (nombre != "")
        {
            BuscarSociedades("", nombre);
        }
    });

    $("#btnDatSocActualizar").click(function () {
        if ($('#txtDatSocCodSociedad').val() == "" && $('#txtDatSocSociedad').val() == "") {
            toastr.warning("Ingrese código o nombre de la sociedad.", Message_Warning);
            return;
        }

        var codigo = $('#txtDatSocCodSociedad').val();
        var nombre = $('#txtDatSocSociedad').val();
        LimpiarFormularioSociedad();

        if (codigo != "") {
            BuscarSociedades(codigo, "");
        }
        else if (nombre != "") {
            BuscarSociedades("", nombre);
        }
    });

    $("#tbSociedadesEncontradas tbody td").live("dblclick", "td", function (event) {
        var data = $('#tbSociedadesEncontradas').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            GetSociedadById(data.CodSociedad);
            $('#modalSociedadesEncontradas').modal('hide');
        }
    });

    $("#btnCloseSociedadesEncontradas").click(function () {
        $('#modalSociedadesEncontradas').modal('hide');
    });
    /* FIN SOCIEDADES */

    /* INICIO FUNDACIONES */
    $("#chkInstFunCurrier").change(function () {
        if ($('#hiddenStep').val() == 'Entrega') {
            if ($('#chkInstFunCurrier').is(':checked')) {
                $('#divInstFunNumeroGuia').show(500);
            }
            else {
                $('#divInstFunNumeroGuia').hide(300);
            }
        }

        if ($('#chkInstFunCurrier').is(':checked') || $('#chkInstFunMensajeria').is(':checked')) {
            $('#divInstFunDireccionEnvio').show(500);
        }
        else {
            $('#divInstFunDireccionEnvio').hide(300);
            $('#txtInstFunDireccionEnvio').val('');
        }
    });

    $("#chkInstFunMensajeria").change(function () {
        if ($('#hiddenStep').val() == 'Entrega') {
            if ($('#chkInstFunMensajeria').is(':checked'))
                $('#divInstFunNumeroSolicitud').show(500);
            else
                $('#divInstFunNumeroSolicitud').hide(300);
        }

        if ($('#chkInstFunCurrier').is(':checked') || $('#chkInstFunMensajeria').is(':checked')) {
            $('#divInstFunDireccionEnvio').show(500);
        }
        else {
            $('#divInstFunDireccionEnvio').hide(300);
            $('#txtInstFunDireccionEnvio').val('');
        }
    });

    $("#rbFundacionExiste").click(function () {
        $('#btnDatFunConsultar').show(500);

        LimpiarFormularioFundacion();
        HabilitarFormularioFundacion(false);
        $('#txtDatFunCodNomFundacion').removeAttr("readonly");
    });

    $("#rbFundacionNueva").click(function () {
        $('#btnDatFunConsultar').hide(300);

        LimpiarFormularioFundacion();
        HabilitarFormularioFundacion(true);
        $('#txtDatFunCodNomFundacion').attr("readonly", "readonly");

        $("#txtDatFunCodNomCliente").val($("#TempCodigoCliente").val());
        $("#txtDatFunNomCliente").val($("#NombreCodigoCliente").val());
    });

    $("#btnDatFunConsultar").click(function () {
        if ($('#txtDatFunCodNomFundacion').val() == "" && $('#txtDatFunNomFundacion').val() == "") {
            toastr.warning("Ingrese código o nombre de la fundación.", Message_Warning);
            return;
        }

        var codigo = $('#txtDatFunCodNomFundacion').val();
        var nombre = $('#txtDatFunNomFundacion').val();
        LimpiarFormularioFundacion();

        if (codigo != "") {
            BuscarFundaciones(codigo, "");
        }
        else if (nombre != "") {
            BuscarFundaciones("", nombre);
        }
    });

    $("#btnDatSocActualizar").click(function () {
        if ($('#txtDatFunCodNomFundacion').val() == "" && $('#txtDatFunNomFundacion').val() == "") {
            toastr.warning("Ingrese código o nombre de la fundación.", Message_Warning);
            return;
        }

        var codigo = $('#txtDatFunCodNomFundacion').val();
        var nombre = $('#txtDatFunNomFundacion').val();
        LimpiarFormularioFundacion();

        if (codigo != "") {
            BuscarFundaciones(codigo, "");
        }
        else if (nombre != "") {
            BuscarFundaciones("", nombre);
        }
    });

    $("#tbFundacionesEncontradas tbody td").live("dblclick", "td", function (event) {
        var data = $('#tbFundacionesEncontradas').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            GetFundacionById(data.CodFundacion);
            $('#modalFundacionesEncontradas').modal('hide');
        }
    });

    $("#btnCloseFundacionesEncontradas").click(function () {
        $('#modalFundacionesEncontradas').modal('hide');
    });
    /* FIN FUNDACIONES */

    /* INICIO NAVES */
    $("#chkInstNavCurrier").change(function () {
        if ($('#hiddenStep').val() == 'Entrega') {
            if ($('#chkInstNavCurrier').is(':checked')) {
                $('#divInstNavNumeroGuia').show(500);
            }
            else {
                $('#divInstNavNumeroGuia').hide(300);
            }
        }

        if ($('#chkInstNavCurrier').is(':checked') || $('#chkInstNavMensajeria').is(':checked')) {
            $('#divInstNavDireccionEnvio').show(500);
        }
        else {
            $('#divInstNavDireccionEnvio').hide(300);
            $('#txtInstNavDireccionEnvio').val('');
        }
    });

    $("#chkInstNavMensajeria").change(function () {
        if ($('#hiddenStep').val() == 'Entrega') {
            if ($('#chkInstNavMensajeria').is(':checked'))
                $('#divInstNavNumeroSolicitud').show(500);
            else
                $('#divInstNavNumeroSolicitud').hide(300);
        }

        if ($('#chkInstNavCurrier').is(':checked') || $('#chkInstNavMensajeria').is(':checked')) {
            $('#divInstNavDireccionEnvio').show(500);
        }
        else {
            $('#divInstNavDireccionEnvio').hide(300);
            $('#txtInstNavDireccionEnvio').val('');
        }
    });

    $("#rbNaveExiste").click(function () {
        $('#btnDatNavConsultar').show(500);

        LimpiarFormularioNave();
        HabilitarFormularioNave(false);
        $('#txtDatNavCodNave').removeAttr("readonly");
    });

    $("#rbNaveNueva").click(function () {
        $('#btnDatNavConsultar').hide(300);

        LimpiarFormularioNave();
        HabilitarFormularioNave(true);
        $('#txtDatNavCodNave').attr("readonly", "readonly");

        $("#txtDatNavCodNomCliente").val($("#TempCodigoCliente").val());
        $("#txtDatNavNomCliente").val($("#NombreCodigoCliente").val());
    });

    $("#btnDatNavConsultar").click(function () {
        if ($('#txtDatNavCodNave').val() == "" && $('#txtDatNavNombreNave').val() == "") {
            toastr.warning("Ingrese código o nombre de la Nave.", Message_Warning);
            return;
        }

        var codigo = $('#txtDatNavCodNave').val();
        var nombre = $('#txtDatNavNombreNave').val();
        LimpiarFormularioNave();

        if (codigo != "") {
            BuscarNaves(codigo, "");
        }
        else if (nombre != "") {
            BuscarNaves("", nombre);
        }
    });

    $("#btnDatNavActualizar").click(function () {
        if ($('#txtDatNavCodNave').val() == "" && $('#txtDatNavNombreNave').val() == "") {
            toastr.warning("Ingrese código o nombre de la Nave.", Message_Warning);
            return;
        }

        var codigo = $('#txtDatNavCodNave').val();
        var nombre = $('#txtDatNavNombreNave').val();
        LimpiarFormularioNave();

        if (codigo != "") {
            BuscarNaves(codigo, "");
        }
        else if (nombre != "") {
            BuscarNaves("", nombre);
        }
    });

    $("#tbNavesEncontradas tbody td").live("dblclick", "td", function (event) {
        var data = $('#tbNavesEncontradas').DataTable().row($(this).parents('tr')).data();
        if (data != null) {
            GetNaveById(data.CodNave);
            $('#modalNavesEncontradas').modal('hide');
        }
    });

    $("#btnCloseNavesEncontradas").click(function () {
        $('#modalNavesEncontradas').modal('hide');
    });
    /* FIN NAVES */

    /* INICIO MIGRACIONES */
    UI.DataTable_Spanish("#tbMigSolicitantes", 0, "asc", false, false, true);

    $("#chkInstMigCurrier").change(function () {
        if ($('#hiddenStep').val() == 'Entrega') {
            if ($('#chkInstMigCurrier').is(':checked')) {
                $('#divInstMigNumeroGuia').show(500);
            }
            else {
                $('#divInstMigNumeroGuia').hide(300);
            }
        }

        if ($('#chkInstMigCurrier').is(':checked') || $('#chkInstMigMensajeria').is(':checked')) {
            $('#divInstMigDireccionEnvio').show(500);
        }
        else {
            $('#divInstMigDireccionEnvio').hide(300);
            $('#txtInstMigDireccionEnvio').val('');
        }
    });

    $("#chkInstMigMensajeria").change(function () {
        if ($('#hiddenStep').val() == 'Entrega') {
            if ($('#chkInstMigMensajeria').is(':checked'))
                $('#divInstMigNumeroSolicitud').show(500);
            else
                $('#divInstMigNumeroSolicitud').hide(300);
        }

        if ($('#chkInstMigCurrier').is(':checked') || $('#chkInstMigMensajeria').is(':checked')) {
            $('#divInstMigDireccionEnvio').show(500);
        }
        else {
            $('#divInstMigDireccionEnvio').hide(300);
            $('#txtInstMigDireccionEnvio').val('');
        }
    });

    $("#chkMigDepRepatriacion").on("change", function () {
        if ($(this).is(":checked")) {
            $("#txtMigDepRepatriacion").removeAttr("disabled");
        }
        else {
            $("#txtMigDepRepatriacion").attr("disabled", "disabled");
        }
    });

    $("#chkMigCambiosCatMig").on("change", function () {
        if ($(this).is(":checked")) {
            $("#txtMigCambioCateMigr").removeAttr("disabled");
        }
        else {
            $("#txtMigCambioCateMigr").attr("disabled", "disabled");
        }
    });
    
    $("#chkMigCarneTramite").on("change", function () {
        if ($(this).is(":checked")) {
            $("#txtMigCarneTramite").removeAttr("disabled");
        }
        else {
            $("#txtMigCarneTramite").attr("disabled", "disabled");
        }
    });

    $("#chkMigVisaMultiple").on("change", function () {
        if ($(this).is(":checked")) {
            $("#txtMigVisaMultiple").removeAttr("disabled");
        }
        else {
            $("#txtMigVisaMultiple").attr("disabled", "disabled");
        }
    });

    $("#ddlFormasMigration").on("change", function () {
        ObtenerChequeByFormasMigracion($(this).val());
        ObtenerTipoByFormasMigracion($(this).val());
        ObtenerRequisitosByFormasMigracion($(this).val());
    });

    $("#btnMigAgregarCheque").click(function () {
        $("#divMigCheques").removeClass("hidden");
        $("#txtMigChequesTitulo").removeClass("hidden");
        $("#btnMigAgregar").removeClass("hidden");
    });

    $("#btnMigAgregar").click(function () {
        var html =
        "<div class='row'>" +
            "<div class='col-md-10'>" +

                "<div class='input-group padding-bottom form-inline input-group-sm'>" +
                    "<span class='input-group-addon' style='width:331px'>" + $("#txtMigChequesTitulo").val() + "</span>" +
                    "<input type='number' class='form-control input-sm' aria-describedby='basic-addon1' onblur='CalcularTotalTramiteMigracion();' required />" +
                    "<input type='hidden' value='0'/>" +
                "</div>" +

            "</div>" +
            "<div class='col-md-2'>" +
                "<input type='checkbox' checked='true' onchange='chequeOnChange(this);' />"
            "</div>" +
        "</div>";

        $("#divOpcionCheque").append(html);
        $("#txtMigChequesTitulo").val("");
        $("#divMigCheques").addClass("hidden");
    });

    $("#btnMigAgregarSolicitantes").click(function () {
        $("#txtAddSolicitantesNombre").val('');
        $("#txtAddSolicitantesIdentificacion").val('');
        $("#txtAddSolicitantesTipo").val('');
        CleanToolTipError();
        $('#modalAddSolicitantes').modal('show');
    });

    $("#btnCloseAddSolicitantes").click(function () {
        $('#modalAddSolicitantes').modal('hide');
    });

    $("#btnCloseUpdSolicitantes").click(function () {
        $('#modalUpdSolicitantes').modal('hide');
    });

    $("#btnModalAgregarSolicitante").click(function () {
        if (!$("#formModalAddSolicitantes").valid()) {
            toastr.warning("Ingresar todos los campos requeridos.", Message_Warning);
            return false;
        }
        else {

            addGridSolicitantes($("#txtAddSolicitantesNombre").val(),
                                $("#txtAddSolicitantesIdentificacion").val(),
                                $("#ddlAddSolicitantesTipo option:selected" ).text(),
                                0,
                                0);

            $("#formModalAddSolicitantes").reset();
            $('#modalAddSolicitantes').modal('hide');
        }
    });

    $("#btnModalActualizarSolicitante").click(function () {

        if (!$("#formModalUpdSolicitantes").valid()) {
            toastr.warning("Ingresar todos los campos requeridos.", Message_Warning);
            return false;
        }
        else {
            var data = $('#tbMigSolicitantes').DataTable().row(trSolicitante).data();
            data[0] = $("#txtUpdSolicitantesNombre").val();
            data[1] = $("#txtUpdSolicitantesIdentificacion").val();
            data[2] = $("#ddlUpdSolicitantesTipo option:selected").text(), 
            $('#tbMigSolicitantes').DataTable().row(trSolicitante).data(data).draw();
            $("#formModalUpdSolicitantes").reset();
            $('#modalUpdSolicitantes').modal('hide');
        }
    });
    /* FIN MIGRACIONES */

    /* INICIO CHECK LIST CONTROL FUNDACIONES */
    if ($('#hiddenStep').val() == 'Control') {
        ObtenerCheckListControlFundaciones();
    }
    /* FIN CHECK LIST CONTROL FUNDACIONES */

    // Datos Corporate Incorporate

    ObtenerCorporateIncorporate();

    $("#btnActualizarDtlInstrucciones").click(function () {
        ObtenerCorporateIncorporateByCodigoSociedad($("#txtCodigoSociedad1").val());
    });


    $("#ddlDatMarTipoSolicitudMarca").on("change", function () {

        $("#divMarcas1").hide();
        $("#divMarcas2").hide();
        $("#divMarcas3").hide();
        $("#divMarcas" + $("#ddlDatMarTipoSolicitudMarca").val()).show();
    });

    //SOLO en las etapas RegistrarSolicitud y AnalisisLegal
    if ($("#hiddenStep").val() == "RegistrarSolicitud" || $("#hiddenStep").val() == "AnalisisLegal") {
        $('#tbInstrucciones').DataTable().column(0).visible(true);
        $('#tbInstrucciones tbody td:not(:first-child)').live('click', 'td', function (event) {
            var row = $('#tbInstrucciones').DataTable().row($(this).parents('tr')).data();
            $('#modalUpdateInstrucciones').modal('show');
            $("#txtModalUpdateInstruccion").val(row[2]);
            $("#hiddenId").val(row[1]);
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
        var inst = { "Instrucciones": $("#txtModalUpdateInstruccion").val(), "Incidente": $("#hiddenIncident").val(), "Proceso": "ServiciosLegales", "IdInstruccion": $("#hiddenId").val() };
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

this.CleanToolTipError = function () {
    $(".selectWarning").removeClass("selectWarning");
    $(".inputWarning").removeClass("inputWarning");
    $(".inputSuccess").removeClass("inputSuccess");
    $(".selectSuccess").removeClass("selectSuccess");
    $(".error").removeClass("error").tooltip("destroy");
}

function CargarFormularioPorArea(opcion)
{
    if (opcion == '1') {
        $('#divInstruccionSociedades').show(300);
        $('#divInstruccionFundaciones').hide();
        $('#divInstruccionMigracion').hide();
        $('#divInstruccionNaves').hide();
        $('#divInstruccionMarcas').hide();
    }
    else if (opcion == '2') {
        $('#divInstruccionSociedades').hide();
        $('#divInstruccionFundaciones').show(300);
        $('#divInstruccionMigracion').hide();
        $('#divInstruccionNaves').hide();
        $('#divInstruccionMarcas').hide();
    }
    else if (opcion == '3') {
        $('#divInstruccionSociedades').hide();
        $('#divInstruccionFundaciones').hide();
        $('#divInstruccionMigracion').show(300);
        $('#divInstruccionNaves').hide();
        $('#divInstruccionMarcas').hide();
    }
    else if (opcion == '5') {
        $('#divInstruccionSociedades').hide();
        $('#divInstruccionFundaciones').hide();
        $('#divInstruccionMigracion').hide();
        $('#divInstruccionNaves').show(300);
        $('#divInstruccionMarcas').hide();
    }
    else if (opcion == '6') {
        $('#divInstruccionSociedades').hide();
        $('#divInstruccionFundaciones').hide();
        $('#divInstruccionMigracion').hide();
        $('#divInstruccionNaves').hide();
        $('#divInstruccionMarcas').show(300);
    }
    else if (opcion == '0') {
        $('#divInstruccionSociedades').hide();
        $('#divInstruccionFundaciones').hide();
        $('#divInstruccionMigracion').hide();
        $('#divInstruccionNaves').hide();
        $('#divInstruccionMarcas').hide();
    }
}

function chequeOnChange(check)
{
    var text = $($(check).parent().parent().children()[0].children[0].children[1]);
    text.val('');

    if (check.checked)
    {
        text.removeAttr("disabled");
    }
    else
    {
        text.attr("disabled", "disabled");
    }
    CalcularTotalTramiteMigracion();
}

function addGridDistribucion(codigo, beneficiario, porcentaje, nAcciones) {
    $('#tbDistribucion').DataTable().row.add([codigo, beneficiario, porcentaje, nAcciones]).draw();
}

function addGridDignatarios(nombre, identificacion, titulo) {
    $('#tbDignatarios').DataTable().row.add([nombre, identificacion, titulo]).draw();
}

function addGridConsejos(nombre, identificacion, titulo) {
    $('#tbConsejoFundacional').DataTable().row.add([nombre, identificacion, titulo]).draw();
}

function addGridSolicitantes(nombre, identificacion, tipo, id, idDatosMigracion) {
    var divButtons = "<a class='btn btn-default btn-sm' style='margin-right: 5px;' onclick='javascript:MigracionModificar($(this));'>Modificar</a>" +
                     "<a class='btn btn-default btn-sm' onclick='javascript:MigracionEliminar($(this));'>Eliminar</a>" +
                     "<input type='hidden' value='" + id + "'\>" +
                     "<input type='hidden' value='" + idDatosMigracion + "'\>";
    $('#tbMigSolicitantes').DataTable().row.add([nombre, identificacion, tipo, divButtons]).draw();
}

function MigracionModificar(obj) {
    trSolicitante = obj.parents('tr');
    var row = $('#tbMigSolicitantes').DataTable().row(trSolicitante).data();
    
    CleanToolTipError();
    $("#txtUpdSolicitantesNombre").val(row[0]);
    $("#txtUpdSolicitantesIdentificacion").val(row[1]);

    $("#ddlUpdSolicitantesTipo").each(function () {
        $('option', this).each(function () {
            if ($(this).text().toUpperCase() == row[2]) {
                $(this).attr('selected', 'selected')
            };
        });
    });

    $('#modalUpdSolicitantes').modal('show');
}

function MigracionEliminar(obj) {
    if (confirm("¿Está seguro que desea Eliminar el registro?")) {
        $('#tbMigSolicitantes').DataTable().row(obj.parents('tr')).remove().draw();
    }
}

function deshabilitaTextAreaInstrOtrosDepto() {
    txtas = document.getElementsByClassName("txta-otros-depto");
    for (var i = 0; i < txtas.length; i++) {
        txtas[i].disabled = true;
    }
}

this.GetIdiomas = function (cod) {

    var uri = "api/SolicitudAPI/GetIdioma/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            idiomas = data
            //$("#ddlIdioma").append("<option value=\"" + k + "\">" + v + "</option>");
            $.each(data, function (key, item) {
                $("#ddlIdioma").append('<option value=' + item.CodIdioma + '>' + item.Descripcion + '</option>');
            })

            if (cod != undefined && cod > 0) {
                $("#ddlIdioma").val(cod);
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

this.GetArea = function (cod) {

    var uri = "api/SolicitudAPI/GetArea/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            areas = data
            //$("#ddlIdioma").append("<option value=\"" + k + "\">" + v + "</option>");
            $("#ddlArea").append("<option value=\"0\"></option>");
            $.each(data, function (key, item) {
                $("#ddlArea").append("<option value=\"" + item.CodArea + "\">" + item.Descripcion + "</option>");
            })

            if (cod != undefined && cod > 0) {
                $("#ddlArea").val(cod);
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

this.GetTramite = function (area, cod) {

    var uri = "api/SolicitudAPI/GetTramite?idArea=" + (area != undefined && area > 0 ? area : $("#ddlArea").val());

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            tramite = data
            $("#ddlTramite").append("<option></option>");
            $.each(data, function (key, item) {
                $("#ddlTramite").append("<option value=\"" + item.CodTramite + "\">" + item.Descripcion + "</option>");
            })

            if (cod != undefined && cod > 0) {
                $("#ddlTramite").val(cod);
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

this.GetProcedencia = function (cod) {

    var uri = "api/SolicitudAPI/GetProcedencia/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            idiomas = data
            $.each(data, function (key, item) {
                $("#ddlProcedencia").append('<option value=' + item.CodProcedencia + '>' + item.Descripcion + '</option>');
            })

            if (cod != undefined && cod > 0) {
                $("#ddlProcedencia").val(cod);
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

this.GetJurisdiccion = function (cod) {

    var uri = "api/SolicitudAPI/GetJurisdiccion";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            tramite = data
            $("#ddlJurisdiccion").append("<option></option>");
            $.each(data, function (key, item) {
                $("#ddlJurisdiccion").append("<option value=\"" + item.CodJurisdiccion + "\">" + item.Nombre + "</option>");
            })

            if (cod != undefined && cod > 0) {
                $("#ddlJurisdiccion").val(cod);
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

this.GetAbogados = function (idioma, abogado) {

    $("#ddlAbogado").empty();
    var uri = "api/SolicitudAPI/GetAbogado?idIdioma=" + (idioma != undefined && idioma > 0 ? idioma : $("#ddlIdioma").val());

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            abogados = data;
            $("#ddlAbogado").append("<option></option>");
            $.each(abogados, function (key, item) {
                $("#ddlAbogado").append("<option value=" + item.Usuario + ">" + item.Nombre + "</option>");
            });

            if (abogado != undefined) {
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

this.GetInstruccion = function () {

    var uri = "api/ProcesosLegalesApi/GetInstruccion?proceso=" + $("#hiddenProcess").val() + "&incidente=" + ($("#hiddenIncident").val() != 0 ? $("#hiddenIncident").val() : $("#hiddenTempIncident").val()) + "&tipo=2&solicitud=" + $("#hiddensol_co_solicitud").val();
    var responder = $("#hiddenProcess").val() == "ServiciosLegales" && ($("#hiddenStep").val() == "InstruccionAbogadoGestor" || $("#hiddenStep").val() == "EjecutarInstruccion") ? $("#ucBtnResponder").html() : "";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            var tableInstrucciones = $('#tbInstrucciones').DataTable();
            tableInstrucciones.clear().draw();

            $.each(data, function (key, item) {

                tableInstrucciones.row.add(["<a class='btn btn-success btn-xs'>Borrar</a>", item.IdInstruccion, decodeURIComponent(item.Instrucciones), decodeURIComponent(item.Respuesta), item.EjecutadoPor, responder.replace("_ID", item.IdInstruccion)]).draw();

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

this.GetTipoFacturacion = function (cod) {

    var uri = "api/SolicitudAPI/GetTiposFacturacion/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            tipoFacturaciones = data

            //Datos Facturacion Entrega Sociedades
            $("#ddlInstSocTipoFact").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlInstSocTipoFact").append('<option value=' + item.COD_FACTURACION + '>' + item.DESCRIPCION + '</option>');
            })
            if (cod != undefined && cod > 0) {
                $("#ddlInstSocTipoFact").val(cod);
            }

            //Datos Facturacion Entrega Fundaciones
            $("#ddlInstFunTipoFact").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlInstFunTipoFact").append('<option value=' + item.COD_FACTURACION + '>' + item.DESCRIPCION + '</option>');
            })
            if (cod != undefined && cod > 0) {
                $("#ddlInstFunTipoFact").val(cod);
            }

            //Datos Facturacion Entrega Migraciones
            $("#ddlInstMigTipoFact").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlInstMigTipoFact").append('<option value=' + item.COD_FACTURACION + '>' + item.DESCRIPCION + '</option>');
            })
            if (cod != undefined && cod > 0) {
                $("#ddlInstMigTipoFact").val(cod);
            }

            //Datos Facturacion Entrega Naves
            $("#ddlInstNavTipoFact").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlInstNavTipoFact").append('<option value=' + item.COD_FACTURACION + '>' + item.DESCRIPCION + '</option>');
            })
            if (cod != undefined && cod > 0) {
                $("#ddlInstNavTipoFact").val(cod);
            }

            //Datos Facturacion Marcas
            $("#ddlInstMarTipoFact").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlInstMarTipoFact").append('<option value=' + item.COD_FACTURACION + '>' + item.DESCRIPCION + '</option>');
            })
            if (cod != undefined && cod > 0) {
                $("#ddlInstMarTipoFact").val(cod);
            }

            //Abrir Asunto Sociedad
            $("#ddlDatSocTipoFacturacion").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlDatSocTipoFacturacion").append('<option value=' + item.COD_FACTURACION + '>' + item.DESCRIPCION + '</option>');
            })

            //Abrir Asunto Fundacion
            $("#ddlDatFunTipoFacturacion").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlDatFunTipoFacturacion").append('<option value=' + item.COD_FACTURACION + '>' + item.DESCRIPCION + '</option>');
            })

            //Abrir Asunto Marcas
            $("#ddlDatMarTipoFacturacion").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlDatMarTipoFacturacion").append('<option value=' + item.COD_FACTURACION + '>' + item.DESCRIPCION + '</option>');
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

this.GetTipoFundacion = function () {
    var uri = "api/DatosFundacion/GetTipoFundacion/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            tipo = data
            $("#ddlDatFunTipoFundacion").empty().append("<option value=''></option>");
            $.each(data, function (key, item) {
                $("#ddlDatFunTipoFundacion").append('<option value=' + item.CodTipoFundacion + '>' + item.Descripcion + '</option>');
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

this.GetPropositos = function () {

    var uri = "api/DatosSociedad/GetPropositos/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {
            propositos = data
            $.each(data, function (key, item) {
                $("#ddlDatSocPropositos").append('<option value=' + item.Codigo + '>' + item.Descripcion + '</option>');
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

this.GetRazones = function () {

    var uri = "api/DatosSociedad/GetRazones/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {
            razones = data
            $.each(data, function (key, item) {
                $("#ddlDatSocRazones").append('<option value=' + item.Codigo + '>' + item.Descripcion + '</option>');
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

this.RegistrarInstruccion = function (instrucciones) {
    var Instruccion = {
        Proceso: $("#hiddenProcess").val(),
        Incidente: ($("#hiddenIncident").val() != 0 ? $("#hiddenIncident").val() : $("#hiddenTempIncident").val()),
        Solicitud: ($("#hiddensol_co_solicitud").val()),
        Tipo: 2,
        Instrucciones: encodeURIComponent(instrucciones)
    }

    var uri = "api/ProcesosLegalesApi/RegistrarInstruccion";

    $.ajax({
        type: "POST",
        data: JSON.stringify(Instruccion    ),
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
                tableInstrucciones.row.add([null, null, instrucciones, null, null, ""]).draw();
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

this.RegistrarDatosFacturacion = function () {
    var datosFacturacion = GetDatosFacturacion();
    var uri = "api/SolicitudAPI/RegistrarDatosFacturacion";

    $.ajax({
        type: "POST",
        data: JSON.stringify(datosFacturacion),
        url: server + uri,
        contentType: "application/json",
        error: function (request, error) {
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            UI.ENDREQUEST();
        },
        success: function () {
            UI.ENDREQUEST();
        }
    });
}

this.GetDatosFacturacion = function () {
    var form = GetAreaSelected();

    var datosFacturacion = {
        INCIDENTE: $('#hiddenIncident').val() == '0' ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val(),
        COD_CLIENTE: $("#txtInst" + form + "CodCliente").val(),
        TIPO_FACTURACION: $("#ddlInst" + form + "TipoFact").val(),
        FORMA_PAGO: $("#rbInst" + form + "FormaPagoCre").is(':checked') ? 'CRE' : 'CON',
        PROFORMA: $("#rbInst" + form + "ProformaSi").is(':checked') ? 1 : 0,
        TIPO_FACTURA: $("#rbInst" + form + "TipoFacturaGlo").is(':checked') ? 'GLO' : 'DET',
        REFERENCIA: $("#txtInst" + form + "Referencia").val(),
        NOMBRE_FACTURA: $("#txtInst" + form + "NombreFactura").val(),
        DIRECCION_FACTURA: $("#txtInst" + form + "DireccionFactura").val(),
        LEYENDA_FACTURA: $("#txtInst" + form + "LeyendaFactura").val(),
        RESPONSABLE_NOTIF: $("#rbInst" + form + "RespNotiAbog").is(':checked') ? 'ABO' : 'ASI',
        FORMA_ENVIO_CURRIER: $("#chkInst" + form + "Currier").is(':checked') ? 'S' : 'N',
        FORMA_ENVIO_EMAIL: $("#chkInst" + form + "Email").is(':checked') ? 'S' : 'N',
        FORMA_ENVIO_PERSONAL: $("#chkInst" + form + "Personal").is(':checked') ? 'S' : 'N',
        FORMA_ENVIO_MENSAJERIA: $("#chkInst" + form + "Mensajeria").is(':checked') ? 'S' : 'N',
        //DIRECCION_ENVIO: ($('#chkInst' + form + 'Currier').is(':checked') ? '1' : '0') + ($('#chkInst' + form + 'Email').is(':checked') ? '1' : '0') + ($('#chkInst' + form + 'Personal').is(':checked') ? '1' : '0') + ($('#chkInst' + form + 'Mensajeria').is(':checked') ? '1' : '0'),
        DIRECCION_ENVIO: $("#txtInst" + form + "DireccionEnvio").val(),
        NUMERO_SOLICITUD: $("#txtInst" + form + "NroSolicitud").val(),
        NUMERO_GUIA: $("#txtInst" + form + "NroGuia").val(),
        MONEDA: $("#ddlInst" + form + "Moneda").val(),
        HONORARIOS: ($("#txtDat" + form + "Honorarios").length == 0 ? "" : $("#txtDat" + form + "Honorarios").val()),
        GASTOS: ($("#txtDat" + form + "Gastos").length == 0 ? "" : $("#txtDat" + form + "Gastos").val()),
        TOTAL: ($("#txtDat" + form + "Total").length == 0 ? "" : $("#txtDat" + form + "Total").val()),
        Solicitud: $('#hiddensol_co_solicitud').val()
    };

    return datosFacturacion;
}

this.LoadDireccionCliente = function (id) {
    var uri = "api/SolicitudAPI/GetDireccionClienteById?id=" + id;
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            var LibretaDirecciones = data;
            if (LibretaDirecciones != null) {
                var tablaLD = $('#tbLibretaDireccionModal').DataTable().clear().draw();              
                $.each(LibretaDirecciones, function (key, item) {
                    tablaLD.row.add([item.CodDireccion, item.Direccion]).draw();
                })
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

this.GuardarDatosSolicitud = function () {
    var datosSolicitud = GetDatosSolicitudData();

    var uri = "api/DatosSociedad/GuardarDatosSolicitud";

    $.ajax({
        type: "POST",
        data: JSON.stringify(datosSolicitud),
        url: server + uri,
        contentType: "application/json",
        error: function (request, error) {
            UI.ENDREQUEST();
        },
        success: function () {
            UI.ENDREQUEST();
        }
    });
}

this.GetDatosSolicitudData = function () {
    var datosSociedad = null,
        datosFundacion = null,
        datosNave = null,
        datosMigracion = null,
        datos = null,
        dignatarios = null,
        consejos = null,
        solicitantes = null,
        opciones = null,
        requisitos = null,
        datosMarcas = null;


    switch ($("#ddlArea").val()) {
        case '1': //SOCIEDADES
            datos = $("#tbDignatarios").dataTable().fnGetData();
            dignatarios = new Array();
            for (var i = 0; i < datos.length; i++)
                dignatarios.push({ Secuencia: String(i + 1), Nombre: datos[i][0], Identificacion: datos[i][1], Titulo: datos[i][2] });

            datosSociedad = {
                Incidente: ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()),
                SociedadExistente: ($('#rbSociedadExiste').is(':checked') ? "1" : ($('#rbSociedadNueva').is(':checked') ? "0" : "0")),
                CodSociedad: $('#txtDatSocCodSociedad').val(),
                NombreSociedad: $('#txtDatSocSociedad').val(),
                IdTipoCorporacion: $('#ddlDatSocTipoCorp').val(),
                IdClienteAnterior: $('#txtDatSocCodClienteAnterior').val(),
                NombreClienteAnterior: $('#txtDatSocClienteAnterior').val(),
                IdTipoCapital: $('#ddlDatSocTipoCap').val(),
                MontoCapital: $('#txtDatSocMontoCap').val(),
                NAcciones: $('#txtDatSocNumAcciones').val(),
                ValorAccion: $('#txtDatSocValorAccion').val(),
                IdTipoAcciones: $('#ddlDatSocTipoAccion').val(),
                IdTipoDirector: $('#ddlDatSocDignatarios').val(),
                Cantidad: $('#txtDatSocCant').val(),
                IdRazones: $('#ddlDatSocRazones').val(),
                IdPropositos: $('#ddlDatSocPropositos').val(),
                DetalleProposito: $('#txtaDatSocDetProposito').text(),
                ProveeDirectores: ($('#ddlDatSocProveeDirectores').is(':checked') ? '1' : ''),
                Tasa: $('#txtDatSocTasa').val(),
                AgenteResid: $('#txtDatSocAgenteResid').val(),
                DirectoresQA: $('#txtDatSocDirectoresQA').val(),
                Honorarios: string2float($('#txtDatSocHonorarios').val()),
                Gastos: string2float($('#txtDatSocGastos').val()),
                Total: string2float($('#txtDatSocTotal').val()),
                Dignatarios: dignatarios
            };
            break;
        case '2': //FUNDACIONES
            datos = $("#tbConsejoFundacional").dataTable().fnGetData();
            consejos = new Array();
            for (var i = 0; i < datos.length; i++)
                consejos.push({ Secuencia: String(i + 1), Nombre: datos[i][0], Identificacion: datos[i][1], Titulo: datos[i][2] });

            datosFundacion = {
                Incidente: ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()),
                FundacionExistente: ($('#rbFundacionExiste').is(':checked') ? "1" : ($('#rbFundacionNueva').is(':checked') ? "0" : "0")),
                CodFundacion: $('#txtDatFunCodNomFundacion').val(),
                NombreFundacion: $('#txtDatFunNomFundacion').val(),
                TipoFundacion: $('#ddlDatFunTipoFundacion').val(),
                CodCia: $('#hiddenDatFunCodCia').val(),
                DescripcionCia: $('#txtDatFunCodNomCompania').val(),
                Fundador: $('#txtDatFunNomFundador').val(),
                CodBajoLeyes: $('#hiddenDatFunDescBajoLeyes').val(),
                DescripcionBajoLeyes: $('#txtDatFunCodNomBajoLeyes').val(),
                IsPendienteCambioClt: $('#chkDatFunCodPendCambioClt').is(':checked'),
                PendienteCambioClt: $('#txtDatFunPendCambioClt').val(),
                CodigoCliente: $('#txtDatFunCodNomCliente').val(),
                NombreCliente: $('#txtDatFunNomCliente').val(),
                Duracion: $('#txtDatFunDuracion').val(),
                RespAnualidadCodigo: $('#txtDatFunCodNomRespAn').val(),
                RespAnualidadNombre: $('#txtDatFunNomRespAn').val(),
                NoReg: $('#txtDatFunNoReg').val(),
                NombreAnterior: $('#txtDatFunNombreAnt').val(),
                RUC: $('#txtDatFunRUC').val(),
                FechaInscripcion: $('#txtDatFunFecInscrip').dotNetFormat(),
                Idioma: $('#ddlDatFunIdioma').val(),
                Oficina: $('#txtDatFunCodNomOficina').val(),
                Patrimonio: $('#txtDatFunPatrimonio').val(),
                Estatus: $('#ddlDatFunEstatus').val(),
                PropositoFundacion: $('#txtaDatFunPropoFunda').text(),
                ProveeDirectores: ($('#chkClienteProveeAgentesResidentes').is(':checked') ? '1' : ''),
                Tasa: $('#txtDatFunTarifaAnualidad').val(),
                AgenteResid: $('#txtDatFunAgenteResid').val(),
                DirectoresQA: $('#txtDatFunDirectoresQA').val(),
                Honorarios: string2float($('#txtDatFunHonorarios').val()),
                Gastos: string2float($('#txtDatFunGastos').val()),
                Total: string2float($('#txtDatFunToatal').val()),
                Consejos: consejos
            }
            break;
        case '3': //MIGRACIONES
            datos = $("#tbMigSolicitantes").dataTable().fnGetData();
            solicitantes = new Array();
            for (var i = 0; i < datos.length; i++) {
                solicitantes.push({ Nombre: datos[i][0], Identificacion: datos[i][1], Tipo: datos[i][2], Id: parseInt($(datos[i][3])[2].value), IdDatosMigracion: parseInt($(datos[i][3])[3].value) });
            }

            datos = $("#divOpcionCheque").children();
            opciones = new Array();
            for (var i = 0; i < datos.length; i++) {
                if ($(datos[i].children[1].children[0]).prop('checked'))
                    opciones.push({ Nombre: datos[i].children[0].children[0].children[0].innerHTML, Valor: parseFloat(datos[i].children[0].children[0].children[1].value), Id: datos[i].children[0].children[0].children[2].value });
            }

            datos = $('#jqxgridRequisitos').jqxGrid('getrows');
            requisitos = new Array();

            if ($('#ddlFormasMigration').val() == '13') {
                for (var i = 0; i < datos.length; i++) {
                    requisitos.push({
                        IdRequisitoMigracion: datos[i].IdRequisitoMigracion,
                        Tipo: datos[i].Tipo,
                        Nombre: datos[i].Nombre,
                        Notarizado: datos[i].Notarizado ? "1" : "0",
                        Traduccion: datos[i].Traduccion ? "1" : "0",
                        Instruccion: datos[i].Instruccion,
                        Apostillado_Legalizado: datos[i].Apostillado_Legalizado ? "1" : "0",
                        IdDatosMigracion: datos[i].IdDatosMigracion
                    });
                }
            }
            else {
                for (var i = 0; i < datos.length; i++) {
                    requisitos.push({
                        IdRequisitoMigracion: datos[i].IdRequisitoMigracion,
                        Tipo: datos[i].Tipo,
                        Nombre: datos[i].Nombre,
                        Notarizado: datos[i].Notarizado,
                        Traduccion: datos[i].Traduccion ? "1" : "0",
                        Instruccion: datos[i].Instruccion,
                        Apostillado_Legalizado: datos[i].Apostillado_Legalizado,
                        IdDatosMigracion: datos[i].IdDatosMigracion
                    });
                }
            }            

            datosMigracion = {
                Incidente: ($("#hiddenIncident").val() == "0" ? $("#hiddenTempIncident").val() : $("#hiddenIncident").val()),
                DepositoRepatriacion: $("#txtMigDepRepatriacion").val(),
                CambioCatMigratoria: $("#txtMigCambioCateMigr").val(),
                CarneTramite: $("#txtMigCarneTramite").val(),
                VisaMultipleEntSal: $("#txtMigVisaMultiple").val(),
                Registro: $("#txtMigRegistro").val(),
                AplicaDependientes: ($("#rbMigTieneDependiente").is(':checked') ? "1" : $("#rbMigNoTieneDependiente").is(':checked') ? "0" : ""),
                FechaProgRegistro: $("#txtMigFechaProgRegistro").dotNetFormat(),
                FechaProgPresentacion: $("#txtMigFechaProgPresentacion").dotNetFormat(),
                Honorarios: $("#txtDatMigHonorarios").val(),
                Gastos: $("#txtDatMigGastos").val(), 
                Total: $("#txtDatMigTotal").val(),
                Solicitantes: solicitantes,
                OpcionesCheques: opciones,
                Requisitos: requisitos
            }
            break;
        case '5': //NAVES
            datosNave = {
                Incidente: ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()),
                NaveExistente: ($('#rbNaveExiste').is(':checked') ? "1" : ($('#rbNaveNueva').is(':checked') ? "0" : "0")),
                CodNave: $('#txtDatNavCodNave').val(),
                NombreNave: $('#txtDatNavNombreNave').val(),
                CodCia: $('#txtDatNavCodCia').val(),
                NombreCia: $('#txtDatNavNombreCia').val(),
                NombreAnterior: $('#txtDatNavNombreAnterior').val(),
                SocPropietaria: $('#txtDatNavSocPropietaria').val(),
                Corresponsal: $('#txtDatNavCorresponsal').val(),
                ClienteCorresponsal: $('#txtDatNavClienteCorresponsal').val(),
                Estado: $('#txtDatNavEstado').val(),
                StsConsolidado: $('#txtDatNavStsConsolidado').val(),
                PropNave: $('#txtDatNavPropNave').val(),
                SomosRL: $('#chkDatNavSomosRL').is(':checked'),
                CobrarRL: $('#chkDatNavCobrarRL').is(':checked'),
                CambioRL: $('#chkDatNavCambioRL').is(':checked'),
                NombreRL:  $('#txtDatNavNombreRL').val()
            }
            break;
        case '6': //MARCAS
            datosMarcas = {
                Incidente: ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()),
                TIPO_SOLICITUD_MARCA: $('#ddlDatMarTipoSolicitudMarca').val(),
                TIPO_REGISTRO: $('#ddlDatMarTipoRegistro').val(),
                PODER: ($('#ddlDatMarTipoSolicitudMarca').val() == '1' ? ($("#ckbDatMar1Poder").is(':checked') ? "1" : "0") : ($('#ddlDatMarTipoSolicitudMarca').val() == '3' ? ($("#ckbDatMar3Poder").is(':checked') ? "1" : "0") : null)),
                CHEQUE: ($('#ddlDatMarTipoSolicitudMarca').val() == '1' ? ($("#ckbDatMar1Cheque").is(':checked') ? "1" : "0") : ($('#ddlDatMarTipoSolicitudMarca').val() == '2' ? ($("#ckbDatMar2Cheque").is(':checked') ? "1" : "0") : ($('#ddlDatMarTipoSolicitudMarca').val() == '3' ? ($("#ckbDatMar3Cheque").is(':checked') ? "1" : "0") : null))),
                ETIQUETAS: ($('#ddlDatMarTipoSolicitudMarca').val() == '1' ? ($("#ckbDatMar1Etiquetas").is(':checked') ? "1" : "0") : null),
                DECLARACION: ($('#ddlDatMarTipoSolicitudMarca').val() == '1' ? ($("#ckbDatMar1Declaracion").is(':checked') ? "1" : "0") : null),
                FORMULARIO: ($('#ddlDatMarTipoSolicitudMarca').val() == '1' ? ($("#ckbDatMar1Formulario").is(':checked') ? "1" : "0") : ($('#ddlDatMarTipoSolicitudMarca').val() == '2' ? ($("#ckbDatMar2Formulario").is(':checked') ? "1" : "0") : null)),
                OTROS: ($('#ddlDatMarTipoSolicitudMarca').val() == '1' ? ($("#ckbDatMar1Otros").is(':checked') ? "1" : "0") : ($('#ddlDatMarTipoSolicitudMarca').val() == '2' ? ($("#ckbDatMar2Otros").is(':checked') ? "1" : "0") : ($('#ddlDatMarTipoSolicitudMarca').val() == '3' ? ($("#ckbDatMar3Otros").is(':checked') ? "1" : "0") : null))),
                ANEXOS: ($('#ddlDatMarTipoSolicitudMarca').val() == '2' ? ($("#ckbDatMar2Anexos").is(':checked') ? "1" : "0") : null),
                PETICION: ($('#ddlDatMarTipoSolicitudMarca').val() == '3' ? ($("#ckbDatMar3Peticion").is(':checked') ? "1" : "0") : null),
                NUMERO_REGISTRO: ($('#ddlDatMarTipoSolicitudMarca').val() == '3' ? $("#txtDatMar3NumeroRegistro").val() : null)
            }
            break;
        default:
            break;
    }

    var comunicacion = {
        Incidente_Relacionado: ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()),
        CodIdioma: $("#ddlIdioma").val(),
        CodProcedencia: $("#ddlProcedencia").val(),
        CodArea: $("#ddlArea").val(),
        CodJurisdiccion: $("#ddlJurisdiccion").val(),
        CodTramite: $("#ddlTramite").val(),
        Abogado_Asignado: $("#ddlAbogado").val(),
        CodFormasMig: $('#ddlFormasMigration').val()
    };

    var datosSolicitud = {
        comunicacion: comunicacion,
        datosSociedad: datosSociedad,
        datosFundacion: datosFundacion,
        datosNave: datosNave,
        datosMigracion: datosMigracion,
        datosMarcas: datosMarcas,
        InstruccionesEspecificas: ($('#chkTraduccion').is(':checked') ? '1' : '0') + ($('#chkAutenticacion').is(':checked') ? '1' : '0') + ($('#chkNotaria').is(':checked') ? '1' : '0') + ($('#chkApostilla').is(':checked') ? '1' : '0') + ($('#chkMINREX').is(':checked') ? '1' : '0') + ($('#chkConsulado').is(':checked') ? '1' : '0') + ($('#chkSeInscribe').is(':checked') ? '1' : '0') + ($('#chkAlteracionTurno').is(':checked') ? '1' : '0') + ($('#chkAperturaCuenta').is(':checked') ? '1' : '0'),
        Solicitud: $('#hiddensol_co_solicitud').val()
    };

    return datosSolicitud;
}

this.ObtenerDatosSolicitud = function () {
    var incidente = ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val());
    var etapa = $('#hiddenStep').val();
    var uri = "api/DatosSociedad/ObtenerDatosSolicitud?incidente=" + incidente + "&etapa=" + etapa + "&solicitud=" + $('#hiddensol_co_solicitud').val();
    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            if (data.datosSociedad != null)
            {
                $("#txtInstSocCodCliente").val(data.datosSociedad.CodigoCliente);
                $("#txtInstSocCliente").val(data.datosSociedad.NombreCliente);
                $("#TempCodigoCliente").val(data.datosSociedad.CodigoCliente);
                $("#NombreCodigoCliente").val(data.datosSociedad.NombreCliente);
                CargarSociedad(data.datosSociedad);
            }

            if (data.datosFundacion != null) {
                $("#txtInstFunCodCliente").val(data.datosFundacion.CodigoCliente);
                $("#txtInstFunCliente").val(data.datosFundacion.NombreCliente);
                $("#TempCodigoCliente").val(data.datosFundacion.CodigoCliente);
                $("#NombreCodigoCliente").val(data.datosFundacion.NombreCliente);
                $("#txtCLCodigoFundacion").val(data.datosFundacion.CodFundacion);
                CargarFundacion(data.datosFundacion);
            }

            if (data.datosNave != null) {
                $("#txtInstNavCodCliente").val(data.datosNave.CodigoCliente);
                $("#txtInstNavCliente").val(data.datosNave.NombreCliente);
                $("#TempCodigoCliente").val(data.datosNave.CodigoCliente);
                $("#NombreCodigoCliente").val(data.datosNave.NombreCliente);
                CargarNave(data.datosNave);
            }

            if (data.datosMigracion != null) {
                $("#txtInstMigCodCliente").val(data.datosMigracion.CodigoCliente);
                $("#txtInstMigCliente").val(data.datosMigracion.NombreCliente);
                $("#TempCodigoCliente").val(data.datosMigracion.CodigoCliente);
                $("#NombreCodigoCliente").val(data.datosMigracion.NombreCliente);
                CargarMigracion(data.datosMigracion);
            }

            if (data.datosMarcas != null) {
                CargarDatosMarcas(data.datosMarcas);
            }

            if (data.InstruccionesEspecificas == null)
                data.InstruccionesEspecificas = "";

            if (data.InstruccionesEspecificas.charAt(0) == "1")
                $("#chkTraduccion").prop('checked', true);
            if (data.InstruccionesEspecificas.charAt(1) == "1")
                $("#chkAutenticacion").prop('checked', true);
            if (data.InstruccionesEspecificas.charAt(2) == "1")
                $("#chkNotaria").prop('checked', true);
            if (data.InstruccionesEspecificas.charAt(3) == "1")
                $("#chkApostilla").prop('checked', true);
            if (data.InstruccionesEspecificas.charAt(4) == "1")
                $("#chkMINREX").prop('checked', true);
            if (data.InstruccionesEspecificas.charAt(5) == "1")
                $("#chkConsulado").prop('checked', true);
            if (data.InstruccionesEspecificas.charAt(6) == "1")
                $("#chkSeInscribe").prop('checked', true);
            if (data.InstruccionesEspecificas.charAt(7) == "1")
                $("#chkAlteracionTurno").prop('checked', true);
            if (data.InstruccionesEspecificas.charAt(8) == "1")
                $("#chkAperturaCuenta").prop('checked', true);

            CargarListas(data);

            if (data.comunicacion != null) {
                CargarComunicacion(data.comunicacion);
            }

            ObtenerDatosFacturacion();

            if ($('#hiddenStep').val() == 'EjecutarInstruccion' || $('#hiddenStep').val() == 'Monitoreo') {
                ObtenerDatosAsunto();
            }
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.ObtenerDatosFacturacion = function () {
    var form = GetAreaSelected();

    var uri = "api/SolicitudAPI/GetDatosFacturacion?incidente=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()) +
               "&solicitud=" + $('#hiddensol_co_solicitud').val();
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            $("#txtInst" + form + "CodCliente").val(data.COD_CLIENTE);
            $("#ddlInst" + form + "TipoFact").val(data.TIPO_FACTURA);
            if (data.FORMA_PAGO == 'CRE' ? $("#rbInst" + form + "FormaPagoCre").prop('checked', 'checked') : $("#rbInst" + form + "FormaPagoCon").prop('checked', 'checked'));
            if (data.PROFORMA == 1 ? $("#rbInst" + form + "ProformaSi").prop('checked', 'checked') : $("#rbInst" + form + "ProformaNo").prop('checked', 'checked'));
            if (data.TIPO_FACTURA == 'GLO' ? $("#rbInst" + form + "TipoFacturaGlo").prop('checked', 'checked') : $("#rbInst" + form + "TipoFacturaDet").prop('checked', 'checked'));
            if (data.RESPONSABLE_NOTIF == 'ABO' ? $("#rbInst" + form + "RespNotiAbog").prop('checked', 'checked') : $("#rbInst" + form + "RespNotiAsistLeg").prop('checked', 'checked'));
                            
            $("#ddlInst" + form + "TipoFact option").each(function () {
                if ($(this).val() == data.TIPO_FACTURACION) {
                    $(this).attr('selected', '"selected"');
                }
            });

            $("#txtInst" + form + "Referencia").val(data.REFERENCIA)
            $("#txtInst" + form + "NombreFactura").val(data.NOMBRE_FACTURA)
            $("#txtInst" + form + "DireccionFactura").val(data.DIRECCION_FACTURA)
            $("#txtInst" + form + "LeyendaFactura").val(data.LEYENDA_FACTURA)
            $("#chkInst" + form + "Currier").prop('checked', data.FORMA_ENVIO_CURRIER == 'S');
            $("#chkInst" + form + "Email").prop('checked', data.FORMA_ENVIO_EMAIL == 'S');
            $("#chkInst" + form + "Personal").prop('checked', data.FORMA_ENVIO_PERSONAL == 'S');
            $("#chkInst" + form + "Mensajeria").prop('checked', data.FORMA_ENVIO_MENSAJERIA == 'S');
            $("#txtInst" + form + "DireccionEnvio").val(data.DIRECCION_ENVIO);
            /*if (data.DIRECCION_ENVIO == null)
                data.DIRECCION_ENVIO = "";

            if (data.DIRECCION_ENVIO.charAt(0) == "1")
                $('#chkInst' + form + 'Currier').prop('checked', true);
            if (data.DIRECCION_ENVIO.charAt(1) == "1")
                $('#chkInst' + form + 'Email').prop('checked', true);
            if (data.DIRECCION_ENVIO.charAt(2) == "1")
                $('#chkInst' + form + 'Personal').prop('checked', true);
            if (data.DIRECCION_ENVIO.charAt(3) == "1")
                $('#chkInst' + form + 'Mensajeria').prop('checked', true);*/

            $("#txtInst" + form + "NroSolicitud").val(data.NUMERO_SOLICITUD == 0 ? "" : data.NUMERO_SOLICITUD);
            $("#txtInst" + form + "NroGuia").val(data.NUMERO_GUIA == 0 ? "" : data.NUMERO_GUIA);
            $("#ddlInst" + form + "Moneda").val(data.MONEDA);

            if ($("#txtDat" + form + "Honorarios").length > 0)
                $("#txtDat" + form + "Honorarios").val(data.HONORARIOS);

            if ($("#txtDat" + form + "Gastos").length > 0)
                $("#txtDat" + form + "Gastos").val(data.GASTOS);

            if ($("#txtDat" + form + "Total").length > 0)
                $("#txtDat" + form + "Total").val(data.TOTAL);
                
            if ($("#chkInst" + form + "Mensajeria").is(':checked') || $("#chkInst" + form + "Currier").is(':checked')) {
                $("#divInst" + form + "DireccionEnvio").show(500);
            }
            else {
                $("#divInst" + form + "DireccionEnvio").hide(300);
            }

            if ($('#hiddenStep').val() == 'Entrega') {
                if ($("#chkInst" + form + "Currier").is(':checked')) {
                    $("#divInst" + form + "NumeroGuia").show(500);
                }
                else {
                    $("#divInst" + form + "NumeroGuia").hide(300);
                }
                if ($("#chkInst" + form + "Mensajeria").is(':checked'))
                    $("#divInst" + form + "NumeroSolicitud").show(500);
                else
                    $("#divInst" + form + "NumeroSolicitud").hide(300);
            }
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.ObtenerDatosAsunto = function () {
    var uri = "api/SolicitudAPI/ObtenerDatosAsunto?incidente=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()) + 
              "&solicitud=" + $('#hiddensol_co_solicitud').val();
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            $("#txtDat" + GetAreaSelected() + "Asunto").val(data.NroAsunto);

            CleanDatosAsunto();
            SetDatosAsunto(data);
        }
    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.CleanDatosAsunto = function () {
    var form = GetAreaSelected();

    if (form == 'Mig')
    {
        $("#txtDatMigAsunto").val('');
        $("#txtDatMigCodCompany").val('');
        $("#txtDatMigCompany").val('');
        $("#txtDatMigSeccion").val('');
        $("#txtDatMigPropiedadIntelectual").val('');
        $("#txtDatMigNombreFact").val('');
        $("#txtDatMigJurisdiccion").val('');
        $("#txtDatMigMateria").val('');
        $("#txtDatMigCorresponsal").val('');
        $("#txtDatMigDir").val('');
        $("#txtDatMigReferencia").val('');
        $("#txtDatMigFechaApertura").val('');
        $("#txtDatMigAbogado").val('');
        $("#txtDatMigSecretaria").val('');
        $("#txtDatMigOficina").val('');
        $("#txtDatMigObservaciones").text('');
        $("#txtDatMigEstCont").val('');
    }
    else if (form != 'Nav') {
        $("#txtDat" + form + "EstadoAsunto").val('');
        $("#txtDat" + form + "CodOficina").val('');
        $("#txtDat" + form + "CodCompany").val('');
        $("#txtDat" + form + "Company").val('');
        $("#txtDat" + form + "AsuntoCodSociedad").val('');
        $("#txtDat" + form + "CodigoSociedad").val('');
        $("#txtDat" + form + "Aplicacion").val('');
        $("#txtDat" + form + "FechaApertura").val('');
        $("#txtDat" + form + "CodAsistente").val('');
        $("#txtDat" + form + "Asistente").val('');
        $("#txtDat" + form + "AsuntoSociedad").val('');
        $("#txtDat" + form + "AsuntoCodCliente").val('');
        $("#txtDat" + form + "AsuntoCliente").val('');
        $("#ddlDat" + form + "TipoFacturacion").val('');
        $("#tbDat" + form + "EstadoSolicitudes").DataTable().clear().draw();
    }
    else {
        $("#txtDatNavCodCompany").val('');
        $("#txtDatNavCompany").val('');
        $("#txtDatNavJurisdiccion").val('');
        $("#txtDatNavCodJurisdiccion").val('');
        $("#txtDatNavNave").val('');
        $("#txtDatNavDescNave").val('');
        $("#txtDatNavOficina").val('');
        $("#txtDatNavMateria").val('');
        $("#txtDatNavResponsable").val('');
        $("#txtDatNavDireccion").val('');
        $("#txtDatNavReferencia").val('');
        $("#txtDatNavReferidoPor").val('');
        $("#txtDatNavNombreFactura").val('');
        $("#txtDatNavSecretaria").val('');
        $("#txtDatNavFechaApertura").val('');
        $("#txtDatNavEstCont").val('');
        //$("#txtDatNavHonCar").val('');
        //$("#txtDatNavHonFact").val('');
        //$("#txtDatNavAcuHon").val('');
        //$("#txtDatNavGastFact").val('');
        //$("#txtDatNavCarCob").val('');
        //$("#txtDatNavTotFact").val('');
        //$("#txtDatNavAdelantos").val('');
        //$("#txtDatNavTotPendFact").val('');
        //$("#txtDatNavGastCar").val('');
        $("#tbDatNavEstadoSolicitudes").DataTable().clear().draw();
    }    
}

this.SetDatosAsunto = function (data) {
    var form = GetAreaSelected();

    if (form == 'Mig') {
        $("#txtDatMigAsunto").val(data.NroAsunto);
        $("#txtDatMigCodCompany").val(data.CodCompany);
        $("#txtDatMigCompany").val(data.Company);
        $("#txtDatMigSeccion").val(data.Seccion);
        $("#txtDatMigPropiedadIntelectual").val(data.PropiedadIntelectual);
        $("#txtDatMigNombreFact").val(data.NombreFactura);
        $("#txtDatMigJurisdiccion").val(data.DescJurisdiccion);
        $("#txtDatMigMateria").val(data.DescMateria);
        $("#txtDatMigCorresponsal").val(data.Corresponsal);
        $("#txtDatMigDir").val(data.Direccion);
        $("#txtDatMigReferencia").val(data.Referencia);
        $("#txtDatMigFechaApertura").val(data.FechaApertura);
        $("#txtDatMigAbogado").val(data.Abogado);
        $("#txtDatMigSecretaria").val(data.Secretaria);
        $("#txtDatMigOficina").val(data.Oficina);
        $("#txtDatMigObservaciones").text(data.Observaciones);
        $("#txtDatMigEstCont").val(data.NombreEstado);
    }
    else if (form != 'Nav') {
        $("#txtDat" + form + "Asunto").val(data.NroAsunto);
        $("#txtDat" + form + "EstadoAsunto").val(data.NombreEstado);
        $("#txtDat" + form + "CodOficina").val(data.Oficina);
        $("#txtDat" + form + "CodCompany").val(data.CodCompany);
        $("#txtDat" + form + "Company").val(data.Company);
        $("#txtDat" + form + "AsuntoCodSociedad").val(data.CodSociedad);
        $("#txtDat" + form + "CodigoSociedad").val(data.CodSociedad);
        $("#txtDat" + form + "Aplicacion").val(data.Aplicacion);
        $("#txtDat" + form + "FechaApertura").dateFormatNoTime(data.FechaApertura);
        $("#txtDat" + form + "CodAsistente").val(data.CodAsistente);
        $("#txtDat" + form + "Asistente").val(data.Asistente);
        $("#txtDat" + form + "AsuntoSociedad").val(data.SociedadFundac);
        $("#txtDat" + form + "AsuntoCodCliente").val(data.CodCliente);
        $("#txtDat" + form + "AsuntoCliente").val(data.NombreCliente);
        $("#ddlDat" + form + "TipoFacturacion").val(data.TipoFactura);
    }
    else {
        $("#txtDat" + form + "Asunto").val(data.NroAsunto);
        $("#txtDatNavCodCompany").val(data.CodCompany);
        $("#txtDatNavCompany").val(data.Company);
        $("#txtDatNavJurisdiccion").val(data.DescJurisdiccion);
        $("#txtDatNavCodJurisdiccion").val(data.CodJurisdiccion);
        $("#txtDatNavNave").val(data.CodNave);
        $("#txtDatNavDescNave").val(data.DescNave);
        $("#txtDatNavOficina").val(data.Oficina);
        $("#txtDatNavMateria").val(data.CodMateria);
        $("#txtDatNavResponsable").val(data.Responsable);
        $("#txtDatNavDireccion").val(data.Direccion);
        $("#txtDatNavReferencia").val(data.Referencia);
        $("#txtDatNavReferidoPor").val(data.ReferidoPor);
        $("#txtDatNavNombreFactura").val(data.NombreFactura);
        $("#txtDatNavSecretaria").val(data.Secretaria);
        $("#txtDatNavFechaApertura").dateFormatNoTime(data.FechaApertura);
        var stat = data.Estado.substring(0, 1) == "E" ? "X": data.Estado.substring(0, 1);
        //$("#txtDatNavEstCont").val(data.Estado.substring(0,1));
        $("#txtDatNavEstCont").val(stat);
        $("#tbDatNavEstadoSolicitudes").DataTable().clear().draw();
    }

    var EstadoSolicitudes = data.EstadoSolicitudes;
    if (EstadoSolicitudes != null) {
        var tablaLD = $("#tbDat" + form + "EstadoSolicitudes").DataTable().clear().draw();
        var columnDefinition = [
                { "data": "Tipo" },
                {
                    "aTargets": [1],
                    "data": "FechaSolicitud",
                    "mRender": function (data, type, full) {
                        return DateFormatShort(data);
                    }
                },
                { "data": "Asignado" },
                { "data": "Estado" }
        ];

        $("#tbDat" + form + "EstadoSolicitudes").TableInit(0, true, true, false, true, columnDefinition, EstadoSolicitudes);
    }

    BlockDataAsunto();
}

this.GuardarTab = function () {

    var formDatosSolicitud = $("#formDatosSolicitud");
          formDatosSolicitud.validate();

        if (!formDatosSolicitud.valid()) {
            toastr.warning("Ingresar todos los campos que son requeridos por el formulario Datos de la Solicitud", Message_Warning);
            return false;
        }

        //CAMPOS REQUERIDOS - DATOS SOLICITUD
        switch ($("#ddlArea").val()) {
            case '1': //SOCIEDADES

                if ($('#rbSociedadNueva').is(':checked') == false && $('#rbSociedadExiste').is(':checked') == false) {
                    toastr.warning("Ingresar todos los campos que son requeridos por el formulario Datos de la Sociedad", Message_Warning);
                    return false;
                }

                if ($('#rbSociedadNueva').is(':checked')) {
                    var formInstSocDatosSociedad = $("#formInstSocDatosSociedad");
                    formInstSocDatosSociedad.validate();

                    if (!formInstSocDatosSociedad.valid()) {
                        toastr.warning("Ingresar todos los campos que son requeridos por el formulario Datos de la Sociedad", Message_Warning);
                        return false;
                    }
                }
                else if ($('#rbSociedadExiste').is(':checked')) {
                    if ($('#txtDatSocCodSociedad').val() == '') {
                        toastr.warning("El campo Código de Sociedad es requerido.", Message_Warning);
                        return false;
                    }
                    if ($('#txtDatSocSociedad').val() == '') {
                        toastr.warning("El campo Nombre Sociedad es requerido.", Message_Warning);
                        return false;
                    }
                }
                else if ($('#hiddenStep').val() == 'AnalisisLegal') {
                    toastr.warning("Ingresar todos los campos que son requeridos por el formulario Datos de la Sociedad", Message_Warning);
                    return false;
                }
                break;
            case '2': //FUNDACIONES
                if ($('#rbFundacionNueva').is(':checked')) {
                    var formDatosFundacion = $("#formInstFunDatosFundacion");
                    formDatosFundacion.validate();

                    if (!formDatosFundacion.valid()) {
                        toastr.warning("Ingresar todos los campos que son requeridos por el formulario Datos de la Fundación", Message_Warning);
                        return false;
                    }
                }
                else if ($('#rbFundacionExiste').is(':checked')) {
                    if ($('#txtDatFunCodNomFundacion').val() == '') {
                        toastr.warning("El campo Código Fundación es requerido.", Message_Warning);
                        return false;
                    }
                    if ($('#txtDatFunNomFundacion').val() == '') {
                        toastr.warning("El campo Nombre Fundación es requerido.", Message_Warning);
                        return false;
                    }
                }
                else if ($('#hiddenStep').val() == 'AnalisisLegal') {
                    toastr.warning("Ingresar todos los campos que son requeridos por el formulario Datos de la Fundación", Message_Warning);
                    return false;
                }
                break;
            case '3': //MIGRACIONES
                var formInstMigDatosMigracion = $("#formInstMigDatosMigracion");
                formInstMigDatosMigracion.validate();

                if (!formInstMigDatosMigracion.valid()) {
                    toastr.warning("Ingresar todos los campos que son requeridos por el formulario Datos de Migración", Message_Warning);
                    return false;
                }

                if ($('#ddlFormasMigration').val() == '') {
                    toastr.warning("El campo Formas de Migración es requerido.", Message_Warning);
                    return false;
                }

                break;
            case '5': //NAVES
                if ($('#rbNaveNueva').is(':checked')) {
                    var formInstNavDatosNaves = $("#formInstNavDatosNaves");
                    formInstNavDatosNaves.validate();

                    if (!formInstNavDatosNaves.valid()) {
                        toastr.warning("Ingresar todos los campos que son requeridos por el formulario Datos de Naves", Message_Warning);
                        return false;
                    }
                }
                else if ($('#rbNaveExiste').is(':checked')) {
                    if ($('#txtDatNavCodNave').val() == '') {
                        toastr.warning("El campo Código de Nave es requerido.", Message_Warning);
                        return false;
                    }
                    if ($('#txtDatNavNombreNave').val() == '') {
                        toastr.warning("El campo Nombre de la Nave es requerido.", Message_Warning);
                        return false;
                    }
                }
                else if ($('#hiddenStep').val() == 'AnalisisLegal') {
                    toastr.warning("Ingresar todos los campos que son requeridos por el formulario Datos de Naves", Message_Warning);
                    return false;
                }
                break;
            case '6': //MARCAS
                var formInstMarDatosMarcas = $("#formInstMarDatosMarcas");
                formInstMarDatosMarcas.validate();

                if (!formInstMarDatosMarcas.valid()) {
                    toastr.warning("Ingresar todos los campos que son requeridos por el formulario Datos de Marcas", Message_Warning);
                    return false;
                }
                break;
            default:
                break;
        }

        //CAMPOS REQUERIDOS - DATOS FACTURACION Y ENTREGA 
        var form = GetAreaSelected();
        var formInstDatosFactEntrega = $("#formInst" + form + "DatosFactEntrega");
        formInstDatosFactEntrega.validate();

        if (!formInstDatosFactEntrega.valid()) {
            toastr.warning("Hay campos que son requeridos por el formulario Datos de Facturación y Entrega", Message_Warning);
            return false;
        }

        if (($("#divInst" + form + "NumeroSolicitud:visible").length > 0 && $("#txtInst" + form + "NroSolicitud").val() == "") ||
            ($("#divInst" + form + "NumeroGuia:visible").length > 0 && $("#txtInst" + form + "NroGuia").val() == "") ||
            ($("#divInst" + form + "DireccionEnvio:visible").length > 0 && $("#txtInst" + form + "DireccionEnvio").val() == "")) {
            toastr.warning("Hay campos que son requeridos por el formulario Datos de Facturación y Entrega", Message_Warning);
            return false;
        }

        if (!$("#chkInst" + form + "Currier").is(":checked") &&
            !$("#chkInst" + form + "Email").is(":checked") &&
            !$("#chkInst" + form + "Personal").is(":checked") &&
            !$("#chkInst" + form + "Mensajeria").is(":checked")) {
            toastr.warning("Debe seleccionar al menos una opción de Forma de Envío del Paquete en el formulario Datos de Facturación y Entrega", Message_Warning);
            return false;
        }

        //CAMPOS REQUERIDOS - ASUNTO
        if ($('#hiddenStep').val() == 'EjecutarInstruccion') {
            var formAsunto = $("#formDat" + form + "Asunto");
            formAsunto.validate();

            if (!formAsunto.valid()) {
                toastr.warning("Hay campos que son requeridos por el formulario Abrir Asunto", Message_Warning);
                return false;
            }
        }

        LoadMessage();

    var datosSolicitud = GetDatosSolicitudData();
    var uri = "api/DatosSociedad/GuardarDatosSolicitud";

    $.ajax({
        type: "POST",
        data: JSON.stringify(datosSolicitud),
        url: server + uri,
        async: false,
        contentType: "application/json",
        error: function (request, error) {
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            UI.ENDREQUEST();
            return false;
        },
        success: function () {
            var datosFacturacion = GetDatosFacturacion();
            var uri = "api/SolicitudAPI/RegistrarDatosFacturacion";

            $.ajax({
                type: "POST",
                data: JSON.stringify(datosFacturacion),
                async: false,
                url: server + uri,
                contentType: "application/json",
                error: function (request, error) {
                    toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
                    UI.ENDREQUEST();
                    return false;
                },
                success: function () {
                    if ($('#hiddenStep').val() == 'EjecutarInstruccion') {

                        var asunto = GetDatosAsunto();
                        var uri = "api/SolicitudAPI/RegistrarAsunto";

                        $.ajax({
                            type: "POST",
                            data: JSON.stringify(asunto),
                            async: false,
                            url: server + uri,
                            contentType: "application/json",
                            error: function (request, error) {
                                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
                                UI.ENDREQUEST();
                                return false;
                            },
                            success: function () {
                                UI.ENDREQUEST();
                                if (enviar) {
                                    enviar = false;
                                    EnviarIncidente();
                                }                                    
                            }
                        });
                    }
                    else {
                        UI.ENDREQUEST();
                        if (enviar) {
                            enviar = false;
                            EnviarIncidente();
                        }
                            
                    }

                    return true;
                }
            });
        }
    });
}

this.GuardarCheckListControl = function () {
    
    var formCheckListControlFundacion = $("#formCheckListControlFundacion");
       formCheckListControlFundacion.validate();

    //if (!formCheckListControlFundacion.valid()) {
    //        toastr.warning("Ingresar todos los campos que son requeridos por el formulario Check List Control", Message_Warning);
    //        return false;  
    //}
     
      if ($('#txtComentarioControl').val() == '') {
          toastr.warning("El campo Comentarios de Control es Requerido .", Message_Warning);
          $("#btnEnviar").prop("disabled", false);

         return false;
      }

    var checkListControlFundacion = GetCheckListControlFundacion();
    var uri = "api/DatosFundacion/GuardarCheckListControl";

    $.ajax({
        type: "POST",
        data: JSON.stringify(checkListControlFundacion),
        async: false,
        url: server + uri,
        contentType: "application/json",
        error: function (request, error) {
            $("#btnEnviar").prop('disabled', false);
            toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            UI.ENDREQUEST();
        },
        success: function () {                                       
            var uri = "api/datossociedad/GuardarDatosEtapaControl?incidente=" + ($('#hiddenIncident').val() == "0"?$('#hiddenTempIncident').val():$('#hiddenIncident').val()) +
		   "&solicitud=" + $('#hiddensol_co_solicitud').val() +
		   "&comentario=" + $('#txtComentarioControl').val();

            $.getJSON(server + uri).done(function (data) {
                 if (enviar) {
                    enviar = false;
                    EnviarIncidente();
                    }        
            }).fail(function (jqxhr, textStatus, error) {
                UI.ENDREQUEST();
                toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }).then(function (value) {
                UI.ENDREQUEST();
            });                                                   
        }
    });
}

this.GetCheckListControlFundacion = function () {
    var data = {
        Incidente: $('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val(),
        CodigoFundacion : $("#txtCLCodigoFundacion").val(),
        ActaFundacional : $("#chkbCLActaFundacional").is(":checked") ? "1" : "0",
        Reglamentos : $("#chkbCLReglamentos").is(":checked") ? "1" : "0",
        DesignacionProtector : $("#chkbCLDesignacionProtector").is(":checked") ? "1" : "0",
        PoderGenEsp : $("#chkbCLPoderGenEsp").is(":checked") ? "1" : "0",
        PINombrePrimerBen : $("#chkbCLPINombrePrimerBen").is(":checked") ? "1" : "0",
        PruebaDomPrimerBen : $("#chkbCLPruebaDomPrimerBen").is(":checked") ? "1" : "0",
        CopiaPasIdPrimerBen : $("#chkbCLCopiaPasIdPrimerBen").is(":checked") ? "1" : "0",
        CartaRefBancaria : $("#chkbCLCartaRefBancaria").is(":checked") ? "1" : "0",
        CartaRefProfesional : $("#chkbCLCartaRefProfesional").is(":checked") ? "1" : "0",
        AccountingRecordLocation : $("#txtaCLAccountingRecordLocation").val(),
        EPINombrePrimerBen : $("#chkbCLEPINombrePrimerBen").is(":checked") ? "1" : "0",
        DireccionPrimerBen: $("#chkbCLDireccionPrimerBen").is(":checked") ? "1" : "0",
        VuelveAlPrincipio: $("#chkDevolver").is(':checked') ? "1" : "0",
        Solicitud: $('#hiddensol_co_solicitud').val()
    }

    return data;
}

this.GetAreaSelected = function () {
    switch ($("#ddlArea").val()) {
        case '1': 
            return "Soc";
        case '2': 
            return "Fun";
        case '3': 
            return "Mig";
        case '5': 
            return "Nav";
        case '6':
            return "Mar";
        default:
            return "Soc";
    }
}

this.EnviarIncidente = function () {
    if ($('#hiddenIncident').val() == "0") {
        CrearServiciosLegales(); //Crea el Incidente
    }
    else {
        CompletarTarea(); //Completa la etapa y pasa a la siguiente, el incidente ya existe
    }
}

this.CompletarTarea = function () {
    var notificaCliente = ($('#hiddenStep').val() == 'EjecutarInstruccion' ? ($("#rbInst" + GetAreaSelected() + "RespNotiAbog").is(':checked') ? 'ABO' : 'ASI') : "");
    var codAreaLocal = ($('#hiddenStep').val() == 'RegistrarSolicitud' || $('#hiddenStep').val() == 'AnalisisLegal') ? $("#ddlArea").val() : "";

    var uri = "api/ProcesosLegalesApi/CompletarTarea?proceso=" + $("#hiddenProcess").val()
                                            + "&tempIncident=" + (($("#hiddenIncident").val() == "" || $("#hiddenIncident").val() == "0") ? $("#hiddenTempIncident").val() : $("#hiddenIncident").val())
                                            + "&userID=" + $("#hiddenUserID").val()
                                            + "&taskID=" + $("#hiddenTaskId").val()
                                            + "&codAreaLegal=" + codAreaLocal
                                            + "&notificaCliente=" + notificaCliente
                                            + "&step=" + $("#hiddenStep").val()
                                            + "&codJurisdiccion=" + ($("#ddlJurisdiccion").length == 0 ? "" : $("#ddlJurisdiccion").val())
                                            + "&codProcedencia=" + ($("#ddlProcedencia").length == 0 ? "" : $("#ddlProcedencia").val());
    LoadMessage();
    $.getJSON(server + uri).done(function (data) {
        if (data > 0) {
            toastr.options.onHidden = function () { window.close(); };
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

this.GetDatosAsunto = function () {
    var asunto = {
        Incidente : $('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val(),
        NroAsunto: $("#txtDat" + GetAreaSelected() + "Asunto").val(),
        Solicitud: $('#hiddensol_co_solicitud').val()
    }

    return asunto;
}

this.LoadDatosAsunto = function (id) {
    var uri = "api/SolicitudAPI/GetAsuntoById?id=" + id;

    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            CleanDatosAsunto();
            SetDatosAsunto(data);
        }
        else {
            toastr.warning("El Nro Asunto ingresado No existe en GENSYS.", Message_Warning);
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.BuscarAsuntos = function (id) {
    var uri = "api/SolicitudAPI/BuscarAsuntosById?id=" + id;

    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            asuntos = data

            var columnDefinition = [
                { "data": "NroAsunto" },
                { "data": "NombreCliente" },
                { "data": "Estado" }
            ];
            var area = GetAreaSelected();
            $("#tb" + area + "AsuntosEncontrados").TableInitPag(0, true, true, 5, true, true, columnDefinition, asuntos);
            var tbAsuntosEncontrados = $("#tb" + area + "AsuntosEncontrados").DataTable();
            $("#modal" + area + "AsuntosEncontrados").modal('show');
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

this.LoadDatosAsuntoNaves = function (id) {
    var uri = "api/SolicitudAPI/GetAsuntoNavesById?id=" + id;
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            CleanDatosAsunto();
            SetDatosAsunto(data);
        }
        else {
            toastr.warning("El Nro Asunto ingresado No existe en GENSYS.", Message_Warning);
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.BlockDataAsunto = function () {
    form = GetAreaSelected();

    if (form == 'Mig') {
        //$("#txtDatMigAsunto").attr("disabled", "disabled");
        $("#txtDatMigCodCompany").attr("disabled", "disabled");
        $("#txtDatMigCompany").attr("disabled", "disabled");
        $("#txtDatMigSeccion").attr("disabled", "disabled");
        $("#txtDatMigPropiedadIntelectual").attr("disabled", "disabled");
        $("#txtDatMigNombreFact").attr("disabled", "disabled");
        $("#txtDatMigJurisdiccion").attr("disabled", "disabled");
        $("#txtDatMigMateria").attr("disabled", "disabled");
        $("#txtDatMigCorresponsal").attr("disabled", "disabled");
        $("#txtDatMigDir").attr("disabled", "disabled");
        $("#txtDatMigReferencia").attr("disabled", "disabled");
        $("#txtDatMigFechaApertura").attr("disabled", "disabled");
        $("#txtDatMigAbogado").attr("disabled", "disabled");
        $("#txtDatMigSecretaria").attr("disabled", "disabled");
        $("#txtDatMigOficina").attr("disabled", "disabled");
        $("#txtDatMigObservaciones").attr("disabled", "disabled");
        $("#txtDatMigEstCont").attr("disabled", "disabled");
    }
    if (form != 'Nav') {
        $("#txtDat" + form + "EstadoAsunto").attr("disabled", "disabled");
        $("#txtDat" + form + "CodOficina").attr("disabled", "disabled");
        $("#txtDat" + form + "Oficina").attr("disabled", "disabled");
        $("#txtDat" + form + "CodCompany").attr("disabled", "disabled");
        $("#txtDat" + form + "Company").attr("disabled", "disabled");
        $("#txtDat" + form + "AsuntoCodSociedad").attr("disabled", "disabled");
        $("#txtDat" + form + "CodigoSociedad").attr("disabled", "disabled");
        $("#txtDat" + form + "Aplicacion").attr("disabled", "disabled");
        $("#txtDat" + form + "FechaApertura").attr("disabled", "disabled");
        $("#txtDat" + form + "CodAsistente").attr("disabled", "disabled");
        $("#txtDat" + form + "Asistente").attr("disabled", "disabled");
        $("#txtDat" + form + "AsuntoSociedad").attr("disabled", "disabled");
        $("#txtDat" + form + "AsuntoCodCliente").attr("disabled", "disabled");
        $("#txtDat" + form + "AsuntoCliente").attr("disabled", "disabled");
        $("#ddlDat" + form + "TipoFacturacion").attr("disabled", "disabled");
    }
    else {
        $("#txtDatNavCodCompany").attr("disabled", "disabled");
        $("#txtDatNavCompany").attr("disabled", "disabled");
        $("#txtDatNavJurisdiccion").attr("disabled", "disabled");
        $("#txtDatNavCodJurisdiccion").attr("disabled", "disabled");
        $("#txtDatNavNave").attr("disabled", "disabled");
        $("#txtDatNavDescNave").attr("disabled", "disabled");
        $("#txtDatNavOficina").attr("disabled", "disabled");
        $("#txtDatNavMateria").attr("disabled", "disabled");
        $("#txtDatNavResponsable").attr("disabled", "disabled");
        $("#txtDatNavDireccion").attr("disabled", "disabled");
        $("#txtDatNavReferencia").attr("disabled", "disabled");
        $("#txtDatNavReferidoPor").attr("disabled", "disabled");
        $("#txtDatNavNombreFactura").attr("disabled", "disabled");
        $("#txtDatNavSecretaria").attr("disabled", "disabled");
        $("#txtDatNavFechaApertura").attr("disabled", "disabled");
        $("#txtDatNavEstCont").attr("disabled", "disabled");
        //$("#txtDatNavHonCar").attr("disabled", "disabled");
        //$("#txtDatNavHonFact").attr("disabled", "disabled");
        //$("#txtDatNavAcuHon").attr("disabled", "disabled");
        //$("#txtDatNavGastFact").attr("disabled", "disabled");
        //$("#txtDatNavCarCob").attr("disabled", "disabled");
        //$("#txtDatNavTotFact").attr("disabled", "disabled");
        //$("#txtDatNavAdelantos").attr("disabled", "disabled");
        //$("#txtDatNavTotPendFact").attr("disabled", "disabled");
        //$("#txtDatNavGastCar").attr("disabled", "disabled");
    }    
}

this.BuscarSociedades = function (id, nombre) {

    var uri;
    if (id != "")
        uri = "api/DatosSociedad/BuscarSociedadesById?id=" + id;
    else
        uri = "api/DatosSociedad/BuscarSociedadesByName?nombre=" + nombre;

    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            sociedades = data

            var columnDefinition = [
                { "data": "CodSociedad" },
                { "data": "NombreSociedad" }
            ];

            $('#tbSociedadesEncontradas').TableInit(0, true, true, false, true, columnDefinition, sociedades);
            var tbSociedadesEncontradas = $('#tbSociedadesEncontradas').DataTable();
            $('#modalSociedadesEncontradas').modal('show');
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

this.GetSociedadById = function (id) {
    var uri = "api/DatosSociedad/GetSociedadById?id=" + id;

    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            CargarSociedad(data);
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.BuscarFundaciones = function (id, nombre) {
    var uri;
    if (id != "")
        uri = "api/DatosFundacion/BuscarFundacionesById?id=" + id;
    else
        uri = "api/DatosFundacion/BuscarFundacionesByName?nombre=" + nombre;

    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            fundaciones = data

            var columnDefinition = [
                { "data": "CodFundacion" },
                { "data": "NombreFundacion" }
            ];

            $('#tbFundacionesEncontradas').TableInit(0, true, true, false, true, columnDefinition, fundaciones);
            var tbFundacionesEncontradas = $('#tbFundacionesEncontradas').DataTable();
            $('#modalFundacionesEncontradas').modal('show');
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

this.GetFundacionById = function (id) {
    var uri = "api/DatosFundacion/GetFundacionById?id=" + id;

    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            CargarFundacion(data);
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.BuscarNaves = function (id, nombre) {
    var uri;
    if (id != "")
        uri = "api/DatosNave/BuscarNavesById?id=" + id;
    else
        uri = "api/DatosNave/BuscarNavesByName?nombre=" + nombre;

    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            naves = data

            var columnDefinition = [
                { "data": "CodNave" },
                { "data": "NombreNave" }
            ];

            $('#tbNavesEncontradas').TableInit(0, true, true, false, true, columnDefinition, naves);
            var tbNavesEncontradas = $('#tbNavesEncontradas').DataTable();
            $('#modalNavesEncontradas').modal('show');
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

this.GetNaveById = function (id) {
    var uri = "api/DatosNave/GetNaveById?id=" + id;

    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            CargarNave(data);
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.CargarSociedad = function (data) {

    if (data.TiposCorporacion != null)
    {
        $("#ddlDatSocTipoCorp").empty();
        $.each(data.TiposCorporacion, function (key, item) {
            $("#ddlDatSocTipoCorp").append('<option value=' + item.Key + '>' + item.Value + '</option>');
        });
    }

    if (data.TiposAcciones != null)
    {
        $("#ddlDatSocTipoAccion").empty();
        $.each(data.TiposAcciones, function (key, item) {
            $("#ddlDatSocTipoAccion").append('<option value=' + item.Key + '>' + item.Value + '</option>');
        });
    }

    if (data.SociedadExistente == "1")
    {
        $("#rbSociedadExiste").prop('checked', true);

        $('#btnDatSocConsultar').show(500);
        HabilitarFormularioSociedad(false);
        $('#txtDatSocCodSociedad').removeAttr("readonly");
    }
    else if (data.SociedadExistente == "0")
    {
        $("#rbSociedadNueva").prop('checked', true);

        $('#btnDatSocConsultar').hide(300);
        HabilitarFormularioSociedad(true);
        $('#txtDatSocCodSociedad').attr("readonly", "readonly");
    }

    $('#txtDatSocCodSociedad').val(data.CodSociedad);
    $('#txtDatSocSociedad').val(data.NombreSociedad);
    $('#ddlDatSocTipoCorp').val(data.IdTipoCorporacion);
    $('#txtDatSocCodClienteAnterior').val(data.IdClienteAnterior);
    $('#txtDatSocClienteAnterior').val(data.NombreClienteAnterior);
    //$('#ddlDatSocTipoCap').val(data.IdTipoCapital);
    $('#txtDatSocMontoCap').val(data.MontoCapital);
    $('#txtDatSocNumAcciones').val(data.NAcciones);
    $('#txtDatSocValorAccion').val(data.ValorAccion);
    $('#ddlDatSocTipoAccion').val(data.IdTipoAcciones);
    //$('#ddlDatSocDignatarios').val(data.IdTipoDirector);
    $('#txtDatSocCant').val(data.Cantidad);

    $('#ddlDatSocRazones').val(data.IdRazones);
    $('#ddlDatSocPropositos').val(data.IdPropositos);
    $('#txtaDatSocDetProposito').text(data.DetalleProposito);
    if (data.ProveeDirectores == "1")
        $("#ddlDatSocProveeDirectores").prop('checked', true);
    $('#txtDatSocTasa').val(data.Tasa);
    $('#txtDatSocAgenteResid').val(data.AgenteResid);
    $('#txtDatSocDirectoresQA').val(data.DirectoresQA);
    $('#txtDatSocHonorarios').val(data.Honorarios);
    $('#txtDatSocGastos').val(data.Gastos);
    $('#txtDatSocTotal').val(formatToCurrency(data.Total));

    var table = $("#tbDignatarios").DataTable();
    table.clear().draw();
    if (data.Dignatarios != null) {
        $.each(data.Dignatarios, function (key, item) {
            table.row.add([item.Nombre, item.Identificacion, item.Titulo]).draw();
        });
    }

    var table = $("#tbAnalisis").DataTable();
    table.clear().draw();
    if (data.AnalisisAntiguedad != null) {
        $.each(data.AnalisisAntiguedad, function (key, item) {
            table.row.add([item.Descripcion, item.SaldoCorriente, item.Saldo30, item.Saldo60, item.Saldo90, item.Saldo120, item.Saldo360, item.SaldoMas360, item.SaldoTotal]).draw();
        });
    }
}

this.CargarFundacion = function (data) {

    if (data.FundacionExistente == "1") {
        $("#rbFundacionExiste").prop('checked', true);

        $('#btnDatFunConsultar').show(500);
        HabilitarFormularioFundacion(false);
        $('#txtDatFunCodNomFundacion').removeAttr("readonly");
    }
    else if (data.FundacionExistente == "0") {
        $("#rbFundacionNueva").prop('checked', true);

        $('#btnDatFunConsultar').hide(300);
        HabilitarFormularioFundacion(true);
        $('#txtDatFunCodNomFundacion').attr("readonly", "readonly");
    }

    $('#txtDatFunCodNomFundacion').val(data.CodFundacion);
    $('#txtDatFunNomFundacion').val(data.NombreFundacion);
    $('#ddlDatFunTipoFundacion').val(data.TipoFundacion);
    $('#txtDatFunCodNomCompania').val(data.DescripcionCia);
    $('#hiddenDatFunCodCia').val(data.CodCia);
    $('#txtDatFunNomFundador').val(data.Fundador);
    $('#txtDatFunCodNomBajoLeyes').val(data.DescripcionBajoLeyes);
    $('#hiddenDatFunDescBajoLeyes').val(data.CodBajoLeyes);    
    $('#txtDatFunCodNomCliente').val(data.CodigoCliente);
    $('#txtDatFunNomCliente').val(data.NombreCliente);
    $('#chkDatFunCodPendCambioClt').prop('checked', data.IsPendienteCambioClt);
    $('#txtDatFunPendCambioClt').val(data.PendienteCambioClt);
    $('#txtDatFunCodNomRespAn').val(data.RespAnualidadCodigo);
    $('#txtDatFunNomRespAn').val(data.RespAnualidadNombre);
    $('#txtDatFunDuracion').val(data.Duracion);
    $('#txtDatFunNombreAnt').val(data.NombreAnterior);
    $('#txtDatFunNoReg').val(data.NoReg);
    if (data.FechaInscripcion != "0001-01-01T00:00:00")
        $('#txtDatFunFecInscrip').dateFormatNoTime(data.FechaInscripcion);
    $('#txtDatFunRUC').val(data.RUC);
    $('#txtDatFunCodNomOficina').val(data.Oficina);
    $('#ddlDatFunIdioma').val(data.Idioma);
    $('#ddlDatFunEstatus').val(data.Estatus);
    $('#txtDatFunPatrimonio').val(data.Patrimonio);
    $('#txtaDatFunPropoFunda').text(data.PropositoFundacion);
    $('#chkClienteProveeAgentesResidentes').prop('checked', data.ProveeDirectores);
    $('#txtDatFunTarifaAnualidad').val(data.Tasa);
    $('#txtDatFunAgenteResid').val(data.AgenteResid);
    $('#txtDatFunDirectoresQA').val(data.DirectoresQA);
    $('#txtDatFunHonorarios').val(data.Honorarios);
    $('#txtDatFunGastos').val(data.Gastos);
    $('#txtDatFunToatal').val(formatToCurrency(data.Total));

    var table = $("#tbConsejoFundacional").DataTable();
    table.clear().draw();
    if (data.Consejos != null) {
        $.each(data.Consejos, function (key, item) {
            table.row.add([item.Nombre, item.Identificacion, item.Titulo]).draw();
        });
    }

    var table = $("#tbFunAnalisis").DataTable();
    table.clear().draw();
    if (data.AnalisisAntiguedad != null) {
        $.each(data.AnalisisAntiguedad, function (key, item) {
            table.row.add([item.Descripcion, item.SaldoCorriente, item.Saldo30, item.Saldo60, item.Saldo90, item.Saldo120, item.Saldo360, item.SaldoMas360, item.SaldoTotal]).draw();
        });
    }
}

this.CargarNave = function (data) {
    if (data.NaveExistente == "1") {
        $("#rbNaveExiste").prop('checked', true);

        $('#btnDatNavConsultar').show(500);
        HabilitarFormularioNave(false);
        $('#txtDatNavCodNave').removeAttr("readonly");
    }
    else if (data.NaveExistente == "0") {
        $("#rbNaveNueva").prop('checked', true);

        $('#btnDatNavConsultar').hide(300);
        HabilitarFormularioNave(true);
        $('#txtDatNavCodNave').attr("readonly", "readonly");
    }

    $('#txtDatNavCodNave').val(data.CodNave);
    $('#txtDatNavNombreNave').val(data.NombreNave);
    $('#txtDatNavCodCia').val(data.CodCia);
    $('#txtDatNavNombreCia').val(data.NombreCia);
    $('#txtDatNavNombreAnterior').val(data.NombreAnterior);
    $('#txtDatNavSocPropietaria').val(data.SocPropietaria);
    $('#txtDatNavCorresponsal').val(data.Corresponsal);
    $('#txtDatNavNombreCorresponsal').val(data.CorresponsalNombre);
    $('#txtDatNavClienteCorresponsal').val(data.ClienteCorresponsal);
    $('#txtDatNavNombreClienteCorresponsal').val(data.ClienteCorresponsalNombre);
    $('#txtDatNavEstado').val(devuelveEstado(data.Estado));
    $('#txtDatNavStsConsolidado').val(data.StsConsolidado);
    $('#txtDatNavPropNave').val(data.PropNave);
    $('#chkDatNavSomosRL').prop('checked', data.SomosRL);
    $('#chkDatNavCobrarRL').prop('checked', data.CobrarRL);
    $('#chkDatNavCambioRL').prop('checked', data.CambioRL);
    $('#txtDatNavNombreRL').val(data.NombreRL);
}

this.devuelveEstado = function (letra) {
    if (letra === null || letra === "A")
        return "Abierto"
    else if( letra === "P")
        return "Pendiente"
    else if( letra === "T")
        return "Terminado"
    else if (letra === "X")
        return "Eliminado"
    else if (letra === "I")
        return "Incompleto"
}

this.CargarMigracion = function (data) {
    $('#txtMigDepRepatriacion').val(data.DepositoRepatriacion);
    $('#txtMigCambioCateMigr').val(data.CambioCatMigratoria);
    $('#txtMigCarneTramite').val(data.CarneTramite);
    $('#txtMigVisaMultiple').val(data.VisaMultipleEntSal);
    $('#txtMigRegistro').val(data.Registro);
    $('#rbMigTieneDependiente').prop('checked', data.AplicaDependientes == "1");
    $('#rbMigNoTieneDependiente').prop('checked', data.AplicaDependientes == "0");
    $('#txtMigFechaProgRegistro').dateFormatNoTime(data.FechaProgRegistro);
    $('#txtMigFechaProgPresentacion').dateFormatNoTime(data.FechaProgPresentacion);
    $('#txtDatMigHonorarios').val(data.Honorarios);
    $('#txtDatMigGastos').val(data.Gastos);
    $('#txtDatMigTotal').val(data.Total);

    if (data.OpcionesCheques != null) {
        $.each(data.OpcionesCheques, function (key, item) {
            var html =
            "<div class='row'>" +
                "<div class='col-md-10'>" +

                    "<div class='input-group padding-bottom form-inline input-group-sm'>" +
                        "<span class='input-group-addon' style='width:220px'>" + item.Nombre + "</span>" +
                        "<input type='number' class='form-control input-sm' aria-describedby='basic-addon1' onblur='CalcularTotalTramiteMigracion();' required value='" + item.Valor + "' />" +
                        "<input type='hidden' value='" + item.Id + "' />" +
                    "</div>" +

                "</div>" +
                "<div class='col-md-2'>" +
                    "<input type='checkbox' checked='true' onchange='chequeOnChange(this);' />"
                "</div>" +
            "</div>";

            $("#divOpcionCheque").append(html);
        });
    }

    var table = $("#tbMigSolicitantes").DataTable();
    table.clear().draw();
    if (data.Solicitantes != null) {
        $.each(data.Solicitantes, function (key, item) {
            addGridSolicitantes(item.Nombre,
                                item.Identificacion,
                                item.Tipo,
                                item.Id,
                                item.IdDatosMigracion);
        });
    }

    setTimeout(function () {
        SetRequisitos(data.Requisitos, $("#ddlFormasMigration").val());
    }, 1000);

    CalcularTotalTramiteMigracion();
}

this.CargarListas = function (data) {

    $("#ddlIdioma").empty();
    $("#ddlIdioma").append('<option></option>');
    if (data.idiomas != null) {
        $.each(data.idiomas, function (key, item) {
            $("#ddlIdioma").append('<option value=' + item.CodIdioma + '>' + item.Descripcion + '</option>');
        });
    }

    $("#ddlProcedencia").empty();
    $("#ddlProcedencia").append('<option></option>');
    if (data.procedencias != null) {
        $.each(data.procedencias, function (key, item) {
            $("#ddlProcedencia").append('<option value=' + item.CodProcedencia + '>' + item.Descripcion + '</option>');
        });
    }

    $("#ddlArea").empty();
    $("#ddlArea").append('<option></option>');
    if (data.areas != null) {
        $.each(data.areas, function (key, item) {
            $("#ddlArea").append("<option value=\"" + item.CodArea + "\">" + item.Descripcion + "</option>");
        });
    }

    $("#ddlJurisdiccion").empty();
    $("#ddlJurisdiccion").append('<option></option>');
    if (data.jurisdicciones != null) {
        $.each(data.jurisdicciones, function (key, item) {
            $("#ddlJurisdiccion").append("<option value=\"" + item.CodJurisdiccion + "\">" + item.Nombre + "</option>");
        });
    }

    $("#ddlAbogado").empty();
    $("#ddlAbogado").append('<option></option>');
    if (data.abogados != null) {
        $.each(data.abogados, function (key, item) {
            $("#ddlAbogado").append("<option value=" + item.Usuario + ">" + item.Nombre + "</option>");
        });
    }

    $("#ddlTramite").empty();
    $("#ddlTramite").append('<option></option>');
    if (data.tramites != null) {
        $.each(data.tramites, function (key, item) {
            $("#ddlTramite").append("<option value=\"" + item.CodTramite + "\">" + item.Descripcion + "</option>");
        });
    }

    $("#ddlFormasMigration").empty();
    $("#ddlFormasMigration").append('<option></option>');
    if (data.formasMigracion != null) {
        $.each(data.formasMigracion, function (key, item) {
            $("#ddlFormasMigration").append("<option value=\"" + item.CodFormasMigracion + "\">" + item.Descripcion + "</option>");
        });
    }
}

this.CargarComunicacion = function (data) {

    $("#txtFechaSolicitud").val(data.Fecha);
    $('#ddlIdioma').val(data.CodIdioma);
    $('#ddlProcedencia').val(data.CodProcedencia);
    $('#ddlArea').val(data.CodArea);
    $('#ddlJurisdiccion').val(data.CodJurisdiccion);
    $('#ddlAbogado').val(data.Abogado_Asignado);
    $('#ddlTramite').val(data.CodTramite);
    $('#ddlFormasMigration').val(data.CodFormasMig);
    
    if ($("#ddlArea").val() == "3")
        $("#formasMigracion").removeClass("hidden");
    else
        $("#formasMigracion").addClass("hidden");

    if (data.CodFormasMig > 0)
        ObtenerTipoByFormasMigracion(data.CodFormasMig);

    CargarFormularioPorArea(data.CodArea);
}

this.LimpiarFormularioFundacion = function () {

    $('#txtDatFunCodNomFundacion').val("");
    $('#txtDatFunNomFundacion').val("");
    $('#ddlDatFunTipoFundacion').val("");
    $('#txtDatFunCodNomCompania').val("");
    $('#txtDatFunNomFundador').val("");
    $('#txtDatFunCodNomBajoLeyes').val("");
    $('#txtDatFunCodNomCliente').val("");
    $('#txtDatFunNomCliente').val("");
    $('#chkDatFunCodPendCambioClt').prop('checked', false);
    $('#txtDatFunPendCambioClt').val("");
    $('#txtDatFunCodNomRespAn').val("");
    $('#txtDatFunNomRespAn').val("");
    $('#txtDatFunDuracion').val("");
    $('#txtDatFunNombreAnt').val("");
    $('#txtDatFunNoReg').val("");
    $('#txtDatFunFecInscrip').val("");
    $('#txtDatFunRUC').val("");
    $('#txtDatFunCodNomOficina').val("");
    $('#ddlDatFunIdioma').val("");
    $('#ddlDatFunEstatus').val("");
    $('#txtDatFunPatrimonio').val("");
    $('#txtaDatFunPropoFunda').text("");
    $('#chkClienteProveeAgentesResidentes').prop('checked', false);
    $('#txtDatFunTarifaAnualidad').val("");
    $('#txtDatFunAgenteResid').val("");
    $('#txtDatFunDirectoresQA').val("");
    $('#txtDatFunHonorarios').val("");
    $('#txtDatFunGastos').val("");
    $('#txtDatFunToatal').val("");
    $("#tbFunAnalisis").DataTable().clear().draw();
    $("#tbConsejoFundacional").DataTable().clear().draw();
}

this.LimpiarFormularioSociedad = function () {

    $('#txtDatSocCodSociedad').val("");
    $('#txtDatSocSociedad').val("");
    $('#ddlDatSocTipoCorp').val("");
    $('#txtDatSocCodClienteAnterior').val("");
    $('#txtDatSocClienteAnterior').val("");
    //$('#ddlDatSocTipoCap').val("");
    $('#txtDatSocMontoCap').val("");
    $('#txtDatSocNumAcciones').val("");
    $('#txtDatSocValorAccion').val("");
    $('#ddlDatSocTipoAccion').val("");
    //$('#ddlDatSocDignatarios').val("");
    $('#txtDatSocCant').val("");
    $('#ddlDatSocRazones').val("");
    $('#ddlDatSocPropositos').val("");
    $('#txtaDatSocDetProposito').text("");
    $("#ddlDatSocProveeDirectores").prop('checked', false);
    $('#txtDatSocTasa').val("");
    $('#txtDatSocAgenteResid').val("");
    $('#txtDatSocDirectoresQA').val("");
    $('#txtDatSocHonorarios').val("");
    $('#txtDatSocGastos').val("");
    $('#txtDatSocTotal').val("");
    $("#tbDignatarios").DataTable().clear().draw();
    $("#tbAnalisis").DataTable().clear().draw();
}

this.LimpiarFormularioNave = function () {
    $('#txtDatNavCodNave').val("");
    $('#txtDatNavNombreNave').val("");
    $('#txtDatNavCodCia').val("");
    $('#txtDatNavNombreCia').val("");
    $('#txtDatNavNombreAnterior').val("");
    $('#txtDatNavSocPropietaria').val("");
    $('#txtDatNavCorresponsal').val("");
    $('#txtDatNavClienteCorresponsal').val("");
    $('#txtDatNavEstado').val("");
    $('#txtDatNavStsConsolidado').val("");
    $('#txtDatNavPropNave').val("");
    $('#chkDatNavSomosRL').prop('checked', false);
    $('#chkDatNavCobrarRL').prop('checked', false);
    $('#chkDatNavCambioRL').prop('checked', false);
    $('#txtDatNavNombreRL').val("");
}

this.HabilitarFormularioFundacion = function (habilitado) {
    if (habilitado) {
        //$('#txtDatFunCodNomFundacion').removeAttr("disabled");
        //$('#txtDatFunNomFundacion').removeAttr("disabled");
        $('#ddlDatFunTipoFundacion').removeAttr("disabled");
        $('#txtDatFunCodNomCompania').removeAttr("disabled");
        $('#txtDatFunNomFundador').removeAttr("disabled");
        $('#txtDatFunCodNomBajoLeyes').removeAttr("disabled");
        $('#txtDatFunCodNomCliente').removeAttr("disabled");
        $('#txtDatFunNomCliente').removeAttr("disabled");
        $('#chkDatFunCodPendCambioClt').removeAttr("disabled");
        $('#txtDatFunPendCambioClt').removeAttr("disabled");
        $('#txtDatFunCodNomRespAn').removeAttr("disabled");
        $('#txtDatFunNomRespAn').removeAttr("disabled");
        $('#txtDatFunDuracion').removeAttr("disabled");
        $('#txtDatFunNombreAnt').removeAttr("disabled");
        $('#txtDatFunNoReg').removeAttr("disabled");
        $('#txtDatFunFecInscrip').removeAttr("disabled");
        $('#txtDatFunRUC').removeAttr("disabled");
        $('#txtDatFunCodNomOficina').removeAttr("disabled");
        $('#ddlDatFunIdioma').removeAttr("disabled");
        $('#ddlDatFunEstatus').removeAttr("disabled");
        $('#txtDatFunPatrimonio').removeAttr("disabled");
        $('#txtaDatFunPropoFunda').removeAttr("disabled");
        $('#chkClienteProveeAgentesResidentes').removeAttr("disabled");
        $('#txtDatFunTarifaAnualidad').removeAttr("disabled");
        $('#txtDatFunAgenteResid').removeAttr("disabled");
        $('#txtDatFunDirectoresQA').removeAttr("disabled");
        //$('#txtDatFunHonorarios').removeAttr("disabled");
        //$('#txtDatFunGastos').removeAttr("disabled");
        //$('#txtDatFunToatal').removeAttr("disabled");
        $('#btnConsejoFundacionalAgregar').show(500);
    }
    else {
        //$('#txtDatFunCodNomFundacion').attr("disabled", "disabled");
        //$('#txtDatFunNomFundacion').attr("disabled", "disabled");
        $('#ddlDatFunTipoFundacion').attr("disabled", "disabled");
        $('#txtDatFunCodNomCompania').attr("disabled", "disabled");
        $('#txtDatFunNomFundador').attr("disabled", "disabled");
        $('#txtDatFunCodNomBajoLeyes').attr("disabled", "disabled");
        $('#txtDatFunCodNomCliente').attr("disabled", "disabled");
        $('#txtDatFunNomCliente').attr("disabled", "disabled");
        $('#chkDatFunCodPendCambioClt').attr("disabled", "disabled");
        $('#txtDatFunPendCambioClt').attr("disabled", "disabled");
        $('#txtDatFunCodNomRespAn').attr("disabled", "disabled");
        $('#txtDatFunNomRespAn').attr("disabled", "disabled");
        $('#txtDatFunDuracion').attr("disabled", "disabled");
        $('#txtDatFunNombreAnt').attr("disabled", "disabled");
        $('#txtDatFunNoReg').attr("disabled", "disabled");
        $('#txtDatFunFecInscrip').attr("disabled", "disabled");
        $('#txtDatFunRUC').attr("disabled", "disabled");
        $('#txtDatFunCodNomOficina').attr("disabled", "disabled");
        $('#ddlDatFunIdioma').attr("disabled", "disabled");
        $('#ddlDatFunEstatus').attr("disabled", "disabled");
        $('#txtDatFunPatrimonio').attr("disabled", "disabled");
        $('#txtaDatFunPropoFunda').attr("disabled", "disabled");
        $('#chkClienteProveeAgentesResidentes').attr("disabled", "disabled");
        $('#txtDatFunTarifaAnualidad').attr("disabled", "disabled");
        $('#txtDatFunAgenteResid').attr("disabled", "disabled");
        $('#txtDatFunDirectoresQA').attr("disabled", "disabled");
        //$('#txtDatFunHonorarios').attr("disabled", "disabled");
        //$('#txtDatFunGastos').attr("disabled", "disabled");
        //$('#txtDatFunToatal').attr("disabled", "disabled");
        $('#btnConsejoFundacionalAgregar').hide(300);
    }
}

this.HabilitarFormularioSociedad = function (habilitado) {

    if (habilitado)
    {
        $('#ddlDatSocTipoCorp').removeAttr("disabled");
        $('#txtDatSocCodClienteAnterior').removeAttr("disabled");
        $('#txtDatSocClienteAnterior').removeAttr("disabled");
        //$('#ddlDatSocTipoCap').removeAttr("disabled");
        $('#txtDatSocMontoCap').removeAttr("disabled");
        $('#txtDatSocNumAcciones').removeAttr("disabled");
        $('#txtDatSocValorAccion').removeAttr("disabled");
        $('#ddlDatSocTipoAccion').removeAttr("disabled");
        //$('#ddlDatSocDignatarios').removeAttr("disabled");
        $('#txtDatSocCant').removeAttr("disabled");
        $('#ddlDatSocRazones').removeAttr("disabled");
        $('#ddlDatSocPropositos').removeAttr("disabled");
        $('#txtaDatSocDetProposito').removeAttr("disabled");
        $("#ddlDatSocProveeDirectores").removeAttr("disabled");
        $('#txtDatSocTasa').removeAttr("disabled");
        $('#txtDatSocAgenteResid').removeAttr("disabled");
        $('#txtDatSocDirectoresQA').removeAttr("disabled");
        $('#btnDignatarioAgregar').show(500);
    }
    else
    {
        $('#ddlDatSocTipoCorp').attr("disabled", "");
        $('#txtDatSocCodClienteAnterior').attr("disabled", "");
        $('#txtDatSocClienteAnterior').attr("disabled", "");
        //$('#ddlDatSocTipoCap').attr("disabled", "");
        $('#txtDatSocMontoCap').attr("disabled", "");
        $('#txtDatSocNumAcciones').attr("disabled", "");
        $('#txtDatSocValorAccion').attr("disabled", "");
        $('#ddlDatSocTipoAccion').attr("disabled", "");
        //$('#ddlDatSocDignatarios').attr("disabled", "");
        $('#txtDatSocCant').attr("disabled", "");
        $('#ddlDatSocRazones').attr("disabled", "");
        $('#ddlDatSocPropositos').attr("disabled", "");
        $('#txtaDatSocDetProposito').attr("disabled", "");
        $("#ddlDatSocProveeDirectores").attr("disabled", "");
        $('#txtDatSocTasa').attr("disabled", "");
        $('#txtDatSocAgenteResid').attr("disabled", "");
        $('#txtDatSocDirectoresQA').attr("disabled", "");
        $('#btnDignatarioAgregar').hide(300);
    }
}

this.HabilitarFormularioNave = function (habilitado) {
    if (habilitado) {
        $('#txtDatNavCodCia').removeAttr("disabled");
        $('#txtDatNavNombreCia').removeAttr("disabled");
        $('#txtDatNavNombreAnterior').removeAttr("disabled");
        $('#txtDatNavSocPropietaria').removeAttr("disabled");
        $('#txtDatNavCorresponsal').removeAttr("disabled");
        $('#txtDatNavClienteCorresponsal').removeAttr("disabled");
        $('#txtDatNavEstado').removeAttr("disabled");
        $('#txtDatNavStsConsolidado').removeAttr("disabled");
        $('#txtDatNavPropNave').removeAttr("disabled");
        $('#chkDatNavSomosRL').removeAttr("disabled");
        $('#chkDatNavCobrarRL').removeAttr("disabled");
        $('#chkDatNavCambioRL').removeAttr("disabled");
        $('#txtDatNavNombreRL').removeAttr("disabled");

    }
    else {
        $('#txtDatNavCodCia').attr("disabled", "");
        $('#txtDatNavNombreCia').attr("disabled", "");
        $('#txtDatNavNombreAnterior').attr("disabled", "");
        $('#txtDatNavSocPropietaria').attr("disabled", "");
        $('#txtDatNavCorresponsal').attr("disabled", "");
        $('#txtDatNavClienteCorresponsal').attr("disabled", "");
        $('#txtDatNavEstado').attr("disabled", "");
        $('#txtDatNavStsConsolidado').attr("disabled", "");
        $('#txtDatNavPropNave').attr("disabled", "");
        $('#chkDatNavSomosRL').attr("disabled", "");
        $('#chkDatNavCobrarRL').attr("disabled", "");
        $('#chkDatNavCambioRL').attr("disabled", "");
        $('#txtDatNavNombreRL').attr("disabled", "");
    }
}

this.CalcularTotalTramite = function () {
    var total = 0;

    if ($("#txtDatSocHonorarios").val() != "")
        total += string2float($("#txtDatSocHonorarios").val());

    if ($("#txtDatSocGastos").val() != "")
        total += string2float($("#txtDatSocGastos").val());

    if ($("#txtDatSocHonorarios").val() == '' && $("#txtDatSocGastos").val() == '') {
        $("#txtDatSocTotal").val("");
    }
    else {
        total = total.toFixed(2);
        $("#txtDatSocTotal").val(formatToCurrency(total));
    }    
}

this.CalcularTotalTramiteFundacion = function () {
    var total = 0;

    if ($("#txtDatFunHonorarios").val() != "")
        total += string2float($("#txtDatFunHonorarios").val());

    if ($("#txtDatFunGastos").val() != "")
        total += string2float($("#txtDatFunGastos").val());

    if ($("#txtDatFunHonorarios").val() == '' && $("#txtDatFunGastos").val() == '') {
        $("#txtDatFunToatal").val("");
    }
    else {
        total = total.toFixed(2);
        $("#txtDatFunToatal").val(formatToCurrency(total));
    }
}

this.CalcularTotalTramiteMigracion = function () {

    if ($("#txtDatMigTotal").length == 0)
        return;

    var desembolso = 0;
    
    if ($("#txtMigRegistro").val() != "")
        desembolso += string2float($("#txtMigRegistro").val());

    if (document.getElementById("chkMigDepRepatriacion").checked && $("#txtMigDepRepatriacion").val() != "")
        desembolso += string2float($("#txtMigDepRepatriacion").val());

    if (document.getElementById("chkMigCambiosCatMig").checked && $("#txtMigCambioCateMigr").val() != "")
        desembolso += string2float($("#txtMigCambioCateMigr").val());

    if (document.getElementById("chkMigCarneTramite").checked && $("#txtMigCarneTramite").val() != "")
        desembolso += string2float($("#txtMigCarneTramite").val());

    if (document.getElementById("chkMigVisaMultiple").checked && $("#txtMigVisaMultiple").val() != "")
        desembolso += string2float($("#txtMigVisaMultiple").val());

    var datos = $("#divOpcionCheque").children();
    for (var i = 0; i < datos.length; i++) {
        if ($(datos[i].children[1].children[0]).prop('checked') && datos[i].children[0].children[0].children[1].value != "")
            desembolso += string2float(datos[i].children[0].children[0].children[1].value);
    }

    if (desembolso == 0) {
        $("#txtDatMigDesembolso").val("");
    }
    else {
        $("#txtDatMigDesembolso").val(formatToCurrency(desembolso.toFixed(2)));
    }

    var total = 0
    total += desembolso;
    if ($("#txtDatMigHonorarios").val() != "")
        total += string2float($("#txtDatMigHonorarios").val());

    if ($("#txtDatMigGastos").val() != "")
        total += string2float($("#txtDatMigGastos").val());

    if (total == 0) {
        $("#txtDatMigTotal").val("");
    }
    else {
        $("#txtDatMigTotal").val(formatToCurrency(total.toFixed(2)));
    }
}

this.CrearServiciosLegales = function () {
    var notificaCliente = ($('#hiddenStep').val() == 'EjecutarInstruccion' ? ($("#rbInst" + GetAreaSelected() + "RespNotiAbog").is(':checked') ? 'ABO' : 'ASI') : "");
    var codAreaLocal = ($('#hiddenStep').val() == 'RegistrarSolicitud' || $('#hiddenStep').val() == 'AnalisisLegal') ? $("#ddlArea").val() : "";
    var uri = "api/ProcesosLegalesApi/CrearServiciosLegales?proceso=" + $("#hiddenProcess").val()
                                            + "&tempIncident=" + $("#hiddenTempIncident").val()
                                            + "&userID=" + $("#hiddenUserID").val()
                                            + "&taskID=" + $("#hiddenTaskId").val()
                                            + "&codAreaLegal=" + codAreaLocal
                                            + "&notificaCliente=" + notificaCliente
                                            + "&codJurisdiccion=" + ($("#ddlJurisdiccion").length == 0 ? "" : $("#ddlJurisdiccion").val())
                                            + "&codProcedencia=" + ($("#ddlProcedencia").length == 0 ? "" : $("#ddlProcedencia").val()
                                            + "&nombreCliente=" + $("#NombreCodigoCliente").val()
                                            + "&codSolicitud=" + $("#hiddensol_co_solicitud").val()
                                            );
    LoadMessage();
    $.getJSON(server + uri).done(function (data) {

        if (data > 0) {
            toastr.options.onHidden = function () { window.close(); };
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

this.ObtenerCorporateIncorporate = function () {
    var uri = "api/DatosSociedad/ObtenerCorporateIncorporate?incidente=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()) +
              "&solicitud=" + $("#hiddensol_co_solicitud").val() ;
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            SetDataCorporatateIncorporate(data);
            BlockDataCorporateIncorporate();
            $("#txtCodigoSociedad1").val(data.CodigoSociedad);
            $("#txtCodigoSociedad1").attr("disabled", "disabled");
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.ObtenerCorporateIncorporateByCodigoSociedad = function (codigoSociedad) {
    var uri = "api/DatosSociedad/ObtenerCorporateIncorporateByCodigoSociedad?codigoSociedad=" + codigoSociedad;
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            SetDataCorporatateIncorporate(data);
            BlockDataCorporateIncorporate();
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.SetDataCorporatateIncorporate = function (data) {
    $("#chkMandAA").prop('checked', data.MandAA);
    $("#chkAppointmentFirstDirectors").prop('checked', data.AppointmentFirstDirectors);
    $("#chkCOI").prop('checked', data.COI);
    $("#chkSealImpression").prop('checked', data.SealImpression);
    $("#chkLetterImpression").prop('checked', data.LetterImpression);
    $("#chkResolutionShares").prop('checked', data.ResolutionShares);
    $("#chkShareCertificate").prop('checked', data.ShareCertificate);
    $("#chkRegisterDirectors").prop('checked', data.RegisterDirectors);
    $("#chkRegisterMembers").prop('checked', data.RegisterMembers);
    $("#chkAccountingRecordSolution").prop('checked', data.AccountingRecordSolution);
    $("#chkCompanyApplicationForm").prop('checked', data.CompanyApplicationForm);
    $("#chkResolutionNewDirectors").prop('checked', data.ResolutionNewDirectors);
    $("#chkResolutionNewOfficer").prop('checked', data.ResolutionNewOfficer);
    $("#chkResolutionNewMembers").prop('checked', data.ResolutionNewMembers);
    $("#chkLetterResignation").prop('checked', data.LetterResignation);
    $("#chkNewShareCertificate").prop('checked', data.NewShareCertificate);
    $("#chkNewDirectorConsent").prop('checked', data.NewDirectorConsent);
    $("#chkPowerAttorney").prop('checked', data.PowerAttorney);
    $("#chkInstrumentTransfer").prop('checked', data.InstrumentTransfer);
    $("#chkPTrustAgreement").prop('checked', data.PTrustAgreement);
    $("#txtaAccountingRecordLocation").val(data.AccountingRecordLocation);
}

this.BlockDataCorporateIncorporate = function () {
    $("#chkMandAA").attr("disabled", "disabled");
    $("#chkAppointmentFirstDirectors").attr("disabled", "disabled");
    $("#chkCOI").attr("disabled", "disabled");
    $("#chkSealImpression").attr("disabled", "disabled");
    $("#chkLetterImpression").attr("disabled", "disabled");
    $("#chkResolutionShares").attr("disabled", "disabled");
    $("#chkShareCertificate").attr("disabled", "disabled");
    $("#chkRegisterDirectors").attr("disabled", "disabled");
    $("#chkRegisterMembers").attr("disabled", "disabled");
    $("#chkAccountingRecordSolution").attr("disabled", "disabled");
    $("#chkCompanyApplicationForm").attr("disabled", "disabled");
    $("#chkResolutionNewDirectors").attr("disabled", "disabled");
    $("#chkResolutionNewOfficer").attr("disabled", "disabled");
    $("#chkResolutionNewMembers").attr("disabled", "disabled");
    $("#chkLetterResignation").attr("disabled", "disabled");
    $("#chkNewShareCertificate").attr("disabled", "disabled");
    $("#chkNewDirectorConsent").attr("disabled", "disabled");
    $("#chkPowerAttorney").attr("disabled", "disabled");
    $("#chkInstrumentTransfer").attr("disabled", "disabled");
    $("#chkPTrustAgreement").attr("disabled", "disabled");
    $("#txtaAccountingRecordLocation").attr("disabled", "disabled");
}

this.ObtenerDatosCliente = function () {
    //$("#btnEnviar").attr("disabled", "disabled");
    var uri = "api/SolicitudAPI/ObtenerDatosCliente?incidente=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val())
            + "&solicitud=" + $("#hiddensol_co_solicitud").val();
    $.getJSON(server + uri).done(function (data) {
        var form = GetAreaSelected();

        if (data != null) {
            $("#txtInst" + form + "CodCliente").val(datosCliente.Cod);
            $("#txtInst" + form + "Cliente").val(datosCliente.Nombre);
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.ObtenerCheckListControlFundaciones = function () {
    var uri = "api/DatosFundacion/ObtenerCheckListControl?incidente=" + ($('#hiddenIncident').val() == "0" ? $('#hiddenTempIncident').val() : $('#hiddenIncident').val()) + "&solicitud=" + $('#hiddensol_co_solicitud').val();
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            SetCheckListControlFundaciones(data);
            BlockCheckListControlFundaciones();
        }

    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.SetCheckListControlFundaciones = function (data) {
    $("#txtCLCodigoFundacion").val(data.CodigoFundacion),
    $("#chkbCLActaFundacional").prop('checked', data.ActaFundacional == "1");
    $("#chkbCLReglamentos").prop('checked', data.Reglamentos == "1");
    $("#chkbCLDesignacionProtector").prop('checked', data.DesignacionProtector == "1");
    $("#chkbCLPoderGenEsp").prop('checked', data.PoderGenEsp == "1");
    $("#chkbCLPINombrePrimerBen").prop('checked', data.PINombrePrimerBen == "1");
    $("#chkbCLPruebaDomPrimerBen").prop('checked', data.PruebaDomPrimerBen == "1");
    $("#chkbCLCopiaPasIdPrimerBen").prop('checked', data.CopiaPasIdPrimerBen == "1");
    $("#chkbCLCartaRefBancaria").prop('checked', data.CartaRefBancaria == "1");
    $("#chkbCLCartaRefProfesional").prop('checked', data.CartaRefProfesional == "1");
    $("#txtaCLAccountingRecordLocation").val(data.AccountingRecordLocation),
    $("#chkbCLEPINombrePrimerBen").prop('checked', data.EPINombrePrimerBen == "1");
    $("#chkbCLDireccionPrimerBen").prop('checked', data.DireccionPrimerBen == "1");
}

this.BlockCheckListControlFundaciones = function () {
    $("#txtCLCodigoFundacion").attr('disabled', 'disabled');
    $("#chkbCLActaFundacional").attr('disabled', 'disabled');
    $("#chkbCLReglamentos").attr('disabled', 'disabled');
    $("#chkbCLDesignacionProtector").attr('disabled', 'disabled');
    $("#chkbCLPoderGenEsp").attr('disabled', 'disabled');
    $("#chkbCLPINombrePrimerBen").attr('disabled', 'disabled');
    $("#chkbCLPruebaDomPrimerBen").attr('disabled', 'disabled');
    $("#chkbCLCopiaPasIdPrimerBen").attr('disabled', 'disabled');
    $("#chkbCLCartaRefBancaria").attr('disabled', 'disabled');
    $("#chkbCLCartaRefProfesional").attr('disabled', 'disabled');
    $("#txtaCLAccountingRecordLocation").attr('disabled', 'disabled');
    $("#chkbCLEPINombrePrimerBen").attr('disabled', 'disabled');
    $("#chkbCLDireccionPrimerBen").attr('disabled', 'disabled');
}

this.ObtenerChequeByFormasMigracion = function (id) {
    var uri = "api/DatosMigracion/ObtenerChequeByFormasMigracion?id=" + id;
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            LimpiarCheque();
            SetCheque(data);
            BlockCheque();
            if ($("#ddlFormasMigration :selected").text() == "ACUERDO DE MARRAKECH" || $("#ddlFormasMigration :selected").text() == "PERMISO DE RESIDENCIA 10%") {
                $("#prorroga").removeClass("hidden");
            }
            else {
                $("#prorroga").addClass("hidden");
            }
        }
    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.ObtenerTipoByFormasMigracion = function (id) {
    var uri = "api/DatosMigracion/ObtenerTipoByFormasMigracion?id=" + id;
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            SetTipos(data);
        }
    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.ObtenerRequisitosByFormasMigracion = function (id) {
    var uri = "api/DatosMigracion/ObtenerRequisitosByFormasMigracion?id=" + id;
    $.getJSON(server + uri).done(function (data) {
        if (data != null) {
            SetRequisitos(data, id);
        }
    }).fail(function (jqxhr, textStatus, error) {

        UI.ENDREQUEST();
        toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);

    }).then(function (value) {

        UI.ENDREQUEST();

    });
}

this.SetRequisitos = function (data, CodFormasMig) {
    if (CodFormasMig == '13') {//Opcion: Otros
        var generaterow = function (i) {
            var row = {};
            row["IdRequisitoMigracion"] = 0;
            row["IdDatosMigracion"] = 0;
            row["Tipo"] = "";
            row["Nombre"] = "";
            row["Instruccion"] = "";
            row["Notarizado"] = false;
            row["Traduccion"] = false;
            row["Apostillado_Legalizado"] = false;
            return row;
        }
        var source =
        {
            localdata: data,
            datatype: "local",
            datafields:
            [
                { name: 'IdRequisitoMigracion', type: 'number' },
                { name: 'Tipo', type: 'string' },
                { name: 'Nombre', type: 'string' },
                { name: 'Instruccion', type: 'string' },
                { name: 'Notarizado', type: 'bool' },
                { name: 'Traduccion', type: 'bool' },
                { name: 'Apostillado_Legalizado', type: 'bool' },
                { name: 'IdDatosMigracion', type: 'number' },
            ],
            addrow: function (rowid, rowdata, position, commit) {
                commit(true);
            },
            deleterow: function (rowid, commit) {
                commit(true);
            },
            updaterow: function (rowid, newdata, commit) {
                commit(true);
            }
        };
        var dataAdapter = new $.jqx.dataAdapter(source, {
            loadComplete: function (data) { },
            loadError: function (xhr, status, error) { }
        });
        $("#jqxgridRequisitos").jqxGrid(
        {
            width: "100%",
            autoheight: true,
            theme: "energyblue",
            source: dataAdapter,
            columnsresize: true,
            groupable: true,
            groupsexpandedbydefault: true,
            editable: true,
            //localization: getLocalization('es'),
            selectionmode: 'singlecell',
            showtoolbar: true,
            rendertoolbar: function (toolbar) {
                var me = this;
                toolbar[0].style.visibility = '';
                var container = $("<div style='margin: 5px;'></div>");
                toolbar.append(container);
                container.append('<input id="addrowbutton" type="button" value="Agregar" />');
                container.append('<input style="margin-left: 5px;" id="deleterowbutton" type="button" value="Eliminar" />');
                $("#addrowbutton").jqxButton();
                $("#deleterowbutton").jqxButton();
                // create new row.
                $("#addrowbutton").on('click', function () {
                    var datarow = generaterow();
                    var commit = $("#jqxgridRequisitos").jqxGrid('addrow', null, datarow);
                });
                // delete row.
                $("#deleterowbutton").on('click', function () {
                    var cell = $("#jqxgridRequisitos").jqxGrid('getselectedcells');
                    if (cell.length > 0) {
                        var datarow = $("#jqxgridRequisitos").jqxGrid('getrowdata', cell[0].rowindex);
                        if (confirm("¿Está seguro que desea Eliminar el registro? \n Tipo: " + datarow.Tipo + "\n Nombre: " + datarow.Nombre)) {
                            var selectedrowindex = cell[0].rowindex;
                            var rowscount = $("#jqxgridRequisitos").jqxGrid('getdatainformation').rowscount;
                            if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                                var id = $("#jqxgridRequisitos").jqxGrid('getrowid', selectedrowindex);
                                var commit = $("#jqxgridRequisitos").jqxGrid('deleterow', id);
                                $("#jqxgridRequisitos").jqxGrid('endupdate');
                            }
                        }                        
                    }                    
                });
            },
            columns: [
              { text: 'Tipo', datafield: 'Tipo', width: '35%' },
              { text: 'Trámite', datafield: 'Nombre', width: '35%' },
              { text: 'Instrucción', datafield: 'Instruccion', width: '10%' },
              { text: 'Notarizado', datafield: 'Notarizado', width: '6%', columntype: 'checkbox' },
              { text: 'Traducción', datafield: 'Traduccion', width: '6%', columntype: 'checkbox' },
              { text: 'Apostillado o Legalizado', datafield: 'Apostillado_Legalizado', width: '8%', columntype: 'checkbox' }
            ]
        });
    }
    else {
        var source =
        {
            localdata: data,
            datatype: "array"
        };
        var dataAdapter = new $.jqx.dataAdapter(source, {
            loadComplete: function (data) { },
            loadError: function (xhr, status, error) { }
        });

        $("#jqxgridRequisitos").jqxGrid(
        {
            width: "100%",
            autoheight: true,
            theme: "energyblue",
            source: dataAdapter,
            columnsresize: true,
            groupable: true,
            groupsexpandedbydefault: true,
            editable: true,
            //localization: getLocalization('es'),
            selectionmode: 'singlecell',
            showtoolbar: false,
            columns: [
              {
                  text: 'Tipo', datafield: 'Tipo', width: '35%', cellbeginedit: function (row) { return false; }
              },
              {
                  text: 'Trámite', datafield: 'Nombre', width: '35%', cellbeginedit: function (row) { return false; }
              },
              {
                  text: 'Instrucción', datafield: 'Instruccion', width: '10%'
              },
              {
                  text: 'Notarizado', datafield: 'Notarizado', width: '6%', columntype: 'checkbox', cellbeginedit: function (row) { return false; }
              },
              {
                  text: 'Traducción', datafield: 'Traduccion', width: '6%', columntype: 'checkbox'
              },
              {
                  text: 'Apostillado o Legalizado', datafield: 'Apostillado_Legalizado', width: '8%', columntype: 'checkbox', cellbeginedit: function (row) { return false; }
              }
            ],
            groups: ['Tipo']
        });
    }    
}

this.SetTipos = function (data) {
    $("#ddlAddSolicitantesTipo").empty();
    $("#ddlAddSolicitantesTipo").append('<option></option>');
    if (data != null) {
        $.each(data, function (key, item) {
            $("#ddlAddSolicitantesTipo").append('<option value=' + item.Id + '>' + item.Descripcion + '</option>');
        });
    }

    $("#ddlUpdSolicitantesTipo").empty();
    $("#ddlUpdSolicitantesTipo").append('<option></option>');
    if (data != null) {
        $.each(data, function (key, item) {
            $("#ddlUpdSolicitantesTipo").append('<option value=' + item.Id + '>' + item.Descripcion + '</option>');
        });
    }
}

this.LimpiarCheque = function () {
    $("#txtMigDepRepatriacion").val("");
    $("#txtMigCambioCateMigr").val("");
    $("#txtMigCarneTramite").val("");
    $("#txtMigVisaMultiple").val("");
    $("#txtMigRegistro").val("");
}

this.SetCheque = function (cheque) {
    $("#txtMigDepRepatriacion").val(cheque.DepositoRepatriacion);
    $("#txtMigCambioCateMigr").val(cheque.CambioCatMigratoria);
    $("#txtMigCarneTramite").val(cheque.CarneTramite);
    $("#txtMigVisaMultiple").val(cheque.VisaMultipleEntSal);
    $("#txtMigRegistro").val(cheque.Registro);
}

this.BlockCheque = function () {
    $("#txtMigDepRepatriacion").attr("disabled", "disabled");
    $("#txtMigCambioCateMigr").attr("disabled", "disabled");
    $("#txtMigCarneTramite").attr("disabled", "disabled");
    $("#txtMigVisaMultiple").attr("disabled", "disabled");
    //$("#txtMigRegistro").attr("disabled", "disabled");
    $("#chkMigDepRepatriacion").prop('checked', false);
    $("#chkMigCambiosCatMig").prop('checked', false);
    $("#chkMigCarneTramite").prop('checked', false);
    $("#chkMigVisaMultiple").prop('checked', false);
}

function string2float(string) {
    return parseFloat(string.replace(/\,/g, ''));
}

function formatToCurrency(monto) {
    if (monto != null) {
        var array = monto.split('.');
        if (array.length == 1) {
            monto = monto * 100;
        } else if (array.length == 2) {
            if (array[1].length == 1)
                monto = monto * 100;
        }

        //Cambiar al formato moneda con 2 decimales del total
        var value = String(monto).replace(/\./g, '');
        if (value.length == 2) value = '0' + value;
        if (value.length == 1) value = '00' + value;

        var formatted = '';
        for (var i = 0; i < value.length; i++) {
            var sep = '';
            if (i == 2) sep = '.';
            if (i > 3 && (i + 1) % 3 == 0) sep = ',';
            formatted = value.substring(value.length - 1 - i, value.length - i) + sep + formatted;
        }

        return formatted;
    }
    else {
        return "";
    }   
}



this.GetTipoSolicitudMarca = function (cod) {

    var uri = "api/SolicitudAPI/GetTipoSolicitudMarca/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            $("#ddlDatMarTipoSolicitudMarca").html("<option></option>");
            $.each(data, function (key, item) {
                $("#ddlDatMarTipoSolicitudMarca").append('<option value=' + item.CodTipoSolicitudMarca + '>' + item.Descripcion + '</option>');
            })

            if (cod != undefined && cod > 0) {
                $("#ddlDatMarTipoSolicitudMarca").val(cod);
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

this.GetTipoRegistro = function (cod) {

    var uri = "api/SolicitudAPI/GetTipoRegistro/";

    $.getJSON(server + uri).done(function (data) {

        if (data != null) {

            $("#ddlDatMarTipoRegistro").html("<option></option>");
            $.each(data, function (key, item) {
                $("#ddlDatMarTipoRegistro").append('<option value=' + item.CodTipoRegistro + '>' + item.Descripcion + '</option>');
            })

            if (cod != undefined && cod > 0) {
                $("#ddlDatMarTipoRegistro").val(cod);
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

this.CargarDatosMarcas = function (datos) {

    $('#ddlDatMarTipoSolicitudMarca').val(datos.TIPO_SOLICITUD_MARCA);
    $('#ddlDatMarTipoRegistro').val(datos.TIPO_REGISTRO);

    if (datos.TIPO_SOLICITUD_MARCA == '1') {
        $('#ckbDatMar1Poder').prop('checked', datos.PODER == "1");
        $('#ckbDatMar1Cheque').prop('checked', datos.CHEQUE == "1");
        $('#ckbDatMar1Etiquetas').prop('checked', datos.ETIQUETAS == "1");
        $('#ckbDatMar1Declaracion').prop('checked', datos.DECLARACION == "1");
        $('#ckbDatMar1Formulario').prop('checked', datos.FORMULARIO == "1");
        $('#ckbDatMar1Otros').prop('checked', datos.OTROS == "1");
    }
    else if (datos.TIPO_SOLICITUD_MARCA == '2') {
        $('#ckbDatMar2Formulario').prop('checked', datos.FORMULARIO == "1");
        $('#ckbDatMar2Cheque').prop('checked', datos.CHEQUE == "1");
        $('#ckbDatMar2Anexos').prop('checked', datos.ANEXOS == "1");
        $('#ckbDatMar2Otros').prop('checked', datos.OTROS == "1");
    }
    else if (datos.TIPO_SOLICITUD_MARCA == '3') {
        $('#txtDatMar3NumeroRegistro').val(datos.NUMERO_REGISTRO);
        $('#ckbDatMar3Poder').prop('checked', datos.PODER == "1");
        $('#ckbDatMar3Cheque').prop('checked', datos.CHEQUE == "1");
        $('#ckbDatMar3Peticion').prop('checked', datos.PETICION == "1");
        $('#ckbDatMar3Otros').prop('checked', datos.OTROS == "1");
    }

    $("#divMarcas1").hide();
    $("#divMarcas2").hide();
    $("#divMarcas3").hide();
    $("#divMarcas" + $("#ddlDatMarTipoSolicitudMarca").val()).show();
}

this.CalcularTotalMarcas = function () {
    var total = 0;

    if ($("#txtDatMarHonorarios").val() != "")
        total += string2float($("#txtDatMarHonorarios").val());

    if ($("#txtDatMarGastos").val() != "")
        total += string2float($("#txtDatMarGastos").val());

    if ($("#txtDatMarHonorarios").val() == '' && $("#txtDatMarGastos").val() == '') {
        $("#txtDatMarTotal").val("");
    }
    else {
        total = total.toFixed(2);
        $("#txtDatMarTotal").val(formatToCurrency(total));
    }
}

