<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TareaMensajeria.aspx.cs" Inherits="ULA.Quijano.Internet.Mensajeria.TareaMensajeria" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <title></title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <link href="../jquery-ui/jquery-ui.css" rel="stylesheet" media="screen" />
    <link href="../bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />
    <link href="../Css/TareaMensajeria.css" rel="stylesheet" media="screen" />
</head>
<body>
    <form id="form1" runat="server" role="form">
        <div class="container-fluid">

            <div class="text-center barraTitulo">
                <img src="../Images/qalogoeng.png" />
            </div>

            <div id="divSolicitud" runat="server" class="panel panel-info">

                <div class="panel-heading">
                    <h3 id="txtTitulo" runat="server" class="panel-title text-center"></h3>
                </div>

                <div class="panel-body">

                    <div id="divPasante" runat="server" visible="false">

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="input-group">
                                    <span class="input-group-addon">Solicitador por:</span>
                                    <input id="TxtSolicitadoPor" name="TxtSolicitadoPor" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="input-group">
                                    <span class="input-group-addon">Entidad:</span>
                                    <input id="txtEntidad" name="txtEntidad" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="input-group">
                                    <span class="input-group-addon">Dirección:</span>
                                    <input id="txtDireccion" name="txtDireccion" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="input-group">
                                    <span class="input-group-addon">Trámite:</span>
                                    <input id="txtTramite" name="txtTramite" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <span class="input-group-addon">Detalles:</span>
                            </div>
                            <div class="col-xs-12">
                                <textarea id="txtDetalles" name="txtaDetalles" class="form-control" rows="3" runat="server" readonly></textarea>
                            </div>
                        </div>

                    </div>

                    <div id="divMensajero" runat="server" visible="false">

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="input-group">
                                    <span class="input-group-addon">Solicitador por:</span>
                                    <input id="TxtSolicitadoPor2" name="TxtSolicitadoPor2" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="input-group">
                                    <span class="input-group-addon">Entidad:</span>
                                    <input id="txtEntidad2" name="txtEntidad2" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12 col-sm-6">
                                <div class="input-group">
                                    <span class="input-group-addon">Para:</span>
                                    <input id="txtPara" name="txtPara" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6">
                                <div class="input-group">
                                    <span class="input-group-addon">De:</span>
                                    <input id="txtDe" name="txtDe" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="input-group">
                                    <span class="input-group-addon">Dirección:</span>
                                    <input id="txtDireccion2" name="txtDireccion2" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12 col-sm-6">
                                <div class="input-group">
                                    <span class="input-group-addon">Piso:</span>
                                    <input id="txtPiso" name="txtPiso" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                           <div class="col-xs-12 col-sm-6">
                                <div class="input-group">
                                    <span class="input-group-addon">Calle:</span>
                                    <input id="txtCalle" name="txtCalle" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <span class="input-group-addon">Referencias del lugar de envío:</span>
                            </div>
                            <div class="col-xs-12">
                                <textarea id="txtReferenciaLugarEnvio" name="txtReferenciaLugarEnvio" class="form-control" rows="3" runat="server" readonly></textarea>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12 col-sm-6">
                                <div class="input-group">
                                    <span class="input-group-addon">Horarios:</span>
                                    <input id="txtHorarios" name="txtHorarios" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                           <div class="col-xs-12 col-sm-6">
                                <div class="input-group">
                                    <span class="input-group-addon">Teléfonos:</span>
                                    <input id="txtTelefonos" name="txtTelefonos" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12 col-sm-7">
                                <div class="input-group">
                                    <span class="input-group-addon">Asunto:</span>
                                    <input id="txtAsunto" name="txtAsunto" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                           <div class="col-xs-12 col-sm-5">
                                <div class="input-group">
                                    <span class="input-group-addon">Acciones:</span>
                                    <input id="txtAcciones" name="txtAcciones" type="text" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>

                    </div>

                    <div id="divAdjuntos" runat="server">
                        <div class="row">
                            <div class="col-xs-12">
                                <span class="input-group-addon">Adjuntos:</span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="container-fluid" >

                                    <asp:Repeater ID="RptAdjuntos" runat="server">
                                        <ItemTemplate>
                                            <div class="col-xs-6 col-sm-4 col-md-3 text-center">
                                                <div class="thumbnail" style="cursor:pointer; margin-bottom: 5px;" onclick="CargarImagen('<%# Container.DataItem %>', 0);">
                                                    <div class="center-block">
                                                        <img src="../Images/picture.png" />
                                                    </div>
                                                    <h6><%# Container.DataItem %></h6>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                </div>
                            </div>
                        </div>
                    </div>

                    <hr />

                    <div class="row">
                        <div class="col-xs-12  col-sm-6">
                            <div class="input-group">
                                <span class="input-group-addon">Estado:</span>
                                <input id="txtEstado" name="txtEstado" type="text" class="form-control" runat="server" readonly />
                            </div>
                        </div>
                        <div class="col-xs-12  col-sm-6">
                            <div class="input-group">
                                <span class="input-group-addon">Responsable:</span>
                                <input id="txtResponsable" name="txtResponsable" type="text" class="form-control" runat="server" readonly />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-xs-12">
                            <span class="input-group-addon">Instrucciones:</span>
                        </div>
                        <div class="col-xs-12">
                            <textarea id="txtComentario" name="txtComentario" class="form-control" rows="3" runat="server" readonly></textarea>
                        </div>
                    </div>

                </div>

            </div>

            <div id="divCompletarTarea" runat="server" class="panel panel-info">

                <div class="panel-heading">
                    <h3 class="panel-title text-center">Completar tarea</h3>
                </div>
                <div class="panel-body">

                    <div id="divEnviar" runat="server">

                        <div class="row">
                            <div class="col-xs-12 col-xs-offset-0 col-sm-6 col-sm-offset-3">
                                <div class="input-group">
                                    <span class="input-group-addon">Asignar Estado:</span>
                                    <select id="ddlEstado" name="ddlEstado" runat="server" class="form-control" aria-label="..." required>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12 col-xs-offset-0 col-sm-6 col-sm-offset-3">
                                <div class="input-group">
                                    <span class="input-group-addon">Imagenes:</span>
                                    <div class="form-control" style="height:200px">

                                        <div runat="server" class="container-fluid" style="height:150px; overflow:auto; padding: 0px;">
                                            <asp:Repeater ID="RptFiles" runat="server">
                                                <ItemTemplate>
                                                    <div class="col-xs-12 text-center" style="padding: 0px;">
                                                        <div class="thumbnail" style="cursor:pointer; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;" onclick="CargarImagen('<%# Container.DataItem %>', 1);" >
                                                            <img src="../Images/picture.png" style="display:inline" /> <span style="font-size:small;"><%# Container.DataItem %></span>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>

                                        <span id="btnCapturar" class="btn btn-default btn-file">
                                            Capturar
                                            <asp:FileUpload ID="FupImagen" name="FupImagen" runat="server" accept="image/*" />
                                            <asp:Button runat="server" ID="BtnSubirImagen" OnClick="BtnSubirImagen_Click" CausesValidation="false" style="display:none" />
                                        </span>

                                        <div id="divUpload" class="progress" style="display:none">
                                          <div id="uploadProgressBar" class="progress-bar progress-bar-success" role="progressbar"
                                               aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"
                                               style="width: 0%">
                                          </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" id="divFechaSeguimiento">
                            <div class="col-xs-12 col-xs-offset-0 col-sm-6 col-sm-offset-3">
                                <div class="input-group">
                                    <span class="input-group-addon">Fecha de seguimiento:</span>
                                    <input id="TxtFechaSeguimiento" name="TxtFechaSeguimiento" type="text" class="form-control" aria-label="..." onkeypress="return false;" runat="server"/>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12 col-xs-offset-0 col-sm-6 col-sm-offset-3">
                                <div class="input-group">
                                    <span class="input-group-addon">Comentarios:</span>
                                    <textarea id="txtComentariosResolucion" name="txtComentariosResolucion" class="form-control" rows="3" runat="server" maxlength="500" required></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="col-xs-4 col-xs-offset-8 col-sm-2 col-sm-offset-10">
                            <asp:Button ID="BtnEnviar" runat="server" Text="Enviar" OnClick="BtnEnviar_Click" CssClass="btn btn-success btn-block" />
                        </div>
                    </div>

                    <div id="divEnviado" runat="server" class="row" visible="false">
                        <div class="col-xs-12">
                            <div class="alert alert-success">¡El estado de la tarea fue actualizado exitosamente!</div>
                        </div>
                    </div>

                </div>
            </div>

            <div id="LblError" runat="server" class="alert alert-danger" visible="false">
                <button type="button" class="close" aria-hidden="true">&times;</button>
            </div>

            <div id="VisorImagen" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                             <img id="Imagen" class="img-responsive" style="display:block; margin-left:auto; margin-right:auto;" />
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </form>

    <script src="../Scripts/jquery.min.js"></script>
    <script src="../jquery-ui/jquery-ui.js"></script>
    <script src="../bootstrap/js/bootstrap.min.js"></script>

    <script>
        $.datepicker.regional['es'] = {
            closeText: 'Cerrar',
            prevText: '<Ant',
            nextText: 'Sig>',
            currentText: 'Hoy',
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            weekHeader: 'Sm',
            dateFormat: 'dd/mm/yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['es']);

        $('#divFechaSeguimiento').hide();
        $("#TxtFechaSeguimiento").datepicker( { minDate: 0 });
        $("#ddlEstado").change(function () {
            $('#TxtFechaSeguimiento').val("");
            if ($("#ddlEstado").val() == "4") {
                $("#divFechaSeguimiento").show(500);
                $("#TxtFechaSeguimiento").attr("required", true);
            }
            else {
                $("#divFechaSeguimiento").hide();
                $("#TxtFechaSeguimiento").removeAttr("required");
            }
        });

        $(document).on('change', '.btn-file :file', function () {

            $("#uploadProgressBar").attr("style", "width: 0%")
            $("#btnCapturar").hide();
            $("#divUpload").show();
            $("#<%: BtnEnviar.ClientID %>").prop("disabled", true);

            var xhr = new XMLHttpRequest();
            var data = new FormData();
            var files = $("#<%: FupImagen.ClientID %>").get(0).files;
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            xhr.upload.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var progress = Math.round(evt.loaded * 100 / evt.total);
                    $("#uploadProgressBar").attr("style", "width: " + progress + "%")
                }
            }, false);
            xhr.onreadystatechange = function (evt) {
                if (xhr.readyState == 4) {
                    $("#FupImagen").val("");
                    __doPostBack('<%: BtnSubirImagen.ClientID %>', '');
                }
            };

            xhr.open("POST", "UploadHandler.ashx", true);
            xhr.send(data);
        });

        function CargarImagen(imagen, resolucion) {
            $('#Imagen').attr('src', '../Images/loading.gif');
            $('#VisorImagen').modal('show');
            $('#Imagen').attr('src', 'Imagen.aspx?i=' + imagen + '&r=' + resolucion);
        }

    </script>

</body>
</html>
